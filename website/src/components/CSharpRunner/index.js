import React, {useCallback, useEffect, useId, useRef, useState} from 'react';
import clsx from 'clsx';
import useBaseUrl from '@docusaurus/useBaseUrl';
import {useColorMode} from '@docusaurus/theme-common';
import Translate from '@docusaurus/Translate';
import styles from './styles.module.css';

// Base64url (UTF-8), so that any code can be passed in the URL fragment.
// The fragment is never sent to the server.
function encodeCode(code) {
  const bytes = new TextEncoder().encode(code);
  let binary = '';
  bytes.forEach((byte) => {
    binary += String.fromCharCode(byte);
  });
  return btoa(binary).replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');
}

function splitLines(text) {
  return text.replace(/\r\n/g, '\n').replace(/\n+$/, '').split('\n');
}

// Height estimate before the runner has loaded (lazy iframe, WebAssembly start).
// ESTIMATE_LINE_PX: editor font 0.92rem x line height 1.55 at a 16px root = 22.8px.
// ESTIMATE_CHROME_PX: toolbar, editor padding, hint, gaps.
// Values from the design of #129; not yet re-measured on /dev/runner-demo at 1280px.
const ESTIMATE_LINE_PX = 23;
const ESTIMATE_CHROME_PX = 190;
const DEFAULT_SAMPLE_LINES = 12;

// ESTIMATE_STDIN_CHROME_PX: label, hint and gaps of the input field.
const ESTIMATE_STDIN_CHROME_PX = 80;

function estimateHeight(code, stdin) {
  const lineCount = code ? splitLines(code).length : DEFAULT_SAMPLE_LINES;
  let height = ESTIMATE_CHROME_PX + ESTIMATE_LINE_PX * Math.max(lineCount, 3);
  if (typeof stdin === 'string') {
    const stdinRows = Math.max(2, stdin.replace(/\r\n/g, '\n').split('\n').length + 1);
    height += ESTIMATE_STDIN_CHROME_PX + ESTIMATE_LINE_PX * stdinRows;
  }
  return height;
}

// Loading and unloading of the iframe (each one runs its own WebAssembly runtime).
// LOAD_MARGIN: the runner loads when its placeholder is closer than this to the viewport.
// UNLOAD_MARGIN: a loaded runner farther away than this (in viewport heights) is unloaded.
// The unload distance is an assumption (about 3 viewport heights) and can be adjusted.
const LOAD_MARGIN = '1250px 0px 1250px 0px';
const UNLOAD_MARGIN = '300% 0px 300% 0px';

function buildSrc(runnerUrl, instanceId, {code, tests, trace, stdin, keptCode, keptStdin}) {
  if (!code) { return runnerUrl; }
  let fragment = `#code=${encodeCode(code)}&id=${encodeURIComponent(instanceId)}`;
  if (tests) { fragment += `&tests=${encodeCode(tests)}`; }
  if (trace && !tests) { fragment += '&trace=1'; }
  if (typeof stdin === 'string') { fragment += `&stdin=${encodeCode(stdin)}`; }
  // State typed by the learner: the runner starts with it; "Zurücksetzen" still restores `code`/`stdin`.
  if (typeof keptCode === 'string' && keptCode !== code) { fragment += `&cur=${encodeCode(keptCode)}`; }
  if (typeof stdin === 'string' && typeof keptStdin === 'string' && keptStdin !== stdin) {
    fragment += `&curin=${encodeCode(keptStdin)}`;
  }
  return `${runnerUrl}${fragment}`;
}

/**
 * Embeds the C# playground (Blazor WebAssembly + Roslyn, served under /runner/)
 * as an iframe and fills the editor with `code`.
 *
 * Props:
 *  - code:     start code (string)
 *  - minHeight: minimum height in pixels, only for the full-page playground (default 0)
 *  - id:       stable key of this instance (default: automatic)
 *  - predict:  input field per output line; after running, ✓/✗ per line (needs `expected`)
 *  - expected: expected output, only sets the number of fields; the check uses the real output
 *  - tests:    C# check code (see concept/components.md); the runner then runs the tests
 *  - stdin:    console input (string); shows an input field, each line is read by one Console.ReadLine()
 *  - trace:    turn on the step view in the runner (variables, heap model)
 *  - onResult: callback with the `run-result` message of the runner
 *
 * The runner reports `{type: 'run-result', id, success, output, diagnostics, tests?}` by postMessage.
 * It also reports `{type: 'resize', id, height}` (content height in CSS px); the iframe height follows it.
 * Until the first report, the height is estimated from the line count of `code`.
 * It also reports `{type: 'state', id, code, stdin, running}` (typed code and input, debounced).
 * A loaded runner that is far outside the viewport is unloaded (iframe removed, placeholder with the last
 * height); when it comes near again, it loads with the last `state` (fragment keys `cur`, `curin`).
 * A runner with the keyboard focus or a running program is never unloaded.
 * Both sides check the origin; the page also checks the sender (iframe) and `id`.
 */
export default function CSharpRunner({
  code = '',
  minHeight = 0,
  id,
  predict = false,
  expected = '',
  tests,
  trace = false,
  stdin,
  onResult,
}) {
  const runnerUrl = useBaseUrl('/runner/');
  const {colorMode} = useColorMode();
  const frame = useRef(null);
  const autoId = useId();
  const instanceId = id ?? autoId;
  const [result, setResult] = useState(null);
  const [frameHeight, setFrameHeight] = useState(null);
  const [guesses, setGuesses] = useState({});
  const holder = useRef(null);
  const [src, setSrc] = useState(null); // null: not loaded (placeholder)
  const keptState = useRef({code: null, stdin: null, running: false});
  const nearRef = useRef(false);
  const farRef = useRef(false);
  const propsRef = useRef(null);
  propsRef.current = {code, tests, trace, stdin};
  const predictionLines = predict ? splitLines(expected) : [];

  const syncTheme = useCallback(() => {
    frame.current?.contentWindow?.postMessage({type: 'theme', value: colorMode}, window.location.origin);
  }, [colorMode]);

  useEffect(() => {
    syncTheme();
  }, [syncTheme]);

  // Load when near, unload when far away (unless the runner has focus or is running a program).
  useEffect(() => {
    const holderEl = holder.current;
    if (!holderEl || typeof IntersectionObserver === 'undefined') {
      setSrc(buildSrc(runnerUrl, instanceId, propsRef.current));
      return undefined;
    }
    function load() {
      setSrc((current) => current ?? buildSrc(runnerUrl, instanceId, {
        ...propsRef.current,
        keptCode: keptState.current.code,
        keptStdin: keptState.current.stdin,
      }));
    }
    function tryUnload() {
      if (nearRef.current || !farRef.current) { return; }
      const el = frame.current;
      if (!el) { return; }
      if (keptState.current.running || document.activeElement === el) { return; }
      setSrc(null);
    }
    const nearObserver = new IntersectionObserver((entries) => {
      nearRef.current = entries[entries.length - 1].isIntersecting;
      if (nearRef.current) { load(); }
    }, {rootMargin: LOAD_MARGIN});
    const farObserver = new IntersectionObserver((entries) => {
      // `farRef` is true while the placeholder is outside the large unload margin.
      farRef.current = !entries[entries.length - 1].isIntersecting;
      tryUnload();
    }, {rootMargin: UNLOAD_MARGIN});
    nearObserver.observe(holderEl);
    farObserver.observe(holderEl);
    // A deferred unload (focus or running program) is checked again when this may have changed.
    const recheck = () => setTimeout(tryUnload, 0);
    document.addEventListener('focusin', recheck);
    window.addEventListener('focus', recheck);
    window.addEventListener('message', recheck);
    return () => {
      nearObserver.disconnect();
      farObserver.disconnect();
      document.removeEventListener('focusin', recheck);
      window.removeEventListener('focus', recheck);
      window.removeEventListener('message', recheck);
    };
  }, [runnerUrl, instanceId]);

  useEffect(() => {
    function onMessage(event) {
      if (event.origin !== window.location.origin) { return; }
      if (event.source !== frame.current?.contentWindow) { return; }
      const data = event.data;
      if (!data || data.id !== instanceId) { return; }
      if (data.type === 'resize' && typeof data.height === 'number' && data.height > 0) {
        const el = frame.current;
        const border = (el && el.offsetHeight - el.clientHeight) || 2;
        setFrameHeight(Math.ceil(data.height) + border);
        return;
      }
      if (data.type === 'state') {
        keptState.current = {
          code: typeof data.code === 'string' ? data.code : null,
          stdin: typeof data.stdin === 'string' ? data.stdin : null,
          running: data.running === true,
        };
        return;
      }
      if (data.type !== 'run-result') { return; }
      setResult(data);
      onResult?.(data);
    }
    window.addEventListener('message', onMessage);
    return () => window.removeEventListener('message', onMessage);
  }, [instanceId, onResult]);

  const actualLines = result?.success ? splitLines(result.output ?? '') : null;

  return (
    <div className={clsx(styles.wrapper)}>
      {predictionLines.length > 0 && (
        <fieldset className={styles.predict}>
          <legend>
            <Translate id="csharpRunner.predict.legend">Dein Tipp für jede Ausgabezeile</Translate>
          </legend>
          {predictionLines.map((_, index) => {
            const inputId = `${instanceId}-guess-${index}`;
            return (
              <div key={index} className={styles.guessRow}>
                <label htmlFor={inputId}>
                  <Translate id="csharpRunner.predict.line" values={{n: index + 1}}>{'Zeile {n}'}</Translate>
                </label>
                <input
                  id={inputId}
                  type="text"
                  className={styles.guessInput}
                  value={guesses[index] ?? ''}
                  spellCheck={false}
                  autoComplete="off"
                  onChange={(e) => setGuesses({...guesses, [index]: e.target.value})}
                />
              </div>
            );
          })}
        </fieldset>
      )}
      <div
        ref={holder}
        className={styles.holder}
        style={{height: `${Math.max(minHeight, frameHeight ?? estimateHeight(code, stdin))}px`}}>
        {src !== null ? (
          <iframe
            ref={frame}
            className={styles.frame}
            src={src}
            title="Interaktiver C#-Playground"
            onLoad={syncTheme}
          />
        ) : (
          <p className={styles.placeholder}>
            <Translate id="csharpRunner.placeholder">
              Der Playground wird wieder geladen, sobald er in den sichtbaren Bereich kommt. Dein Code bleibt erhalten.
            </Translate>
          </p>
        )}
      </div>
      <p className={styles.note}>
        Der Code wird in deinem Browser kompiliert und ausgeführt. Der erste Start lädt den Compiler
        und dauert einige Sekunden.
      </p>
      <div aria-live="polite" className={styles.feedback}>
        {actualLines !== null && predictionLines.length > 0 && (
          <ul className={styles.list} aria-label="Prüfung deiner Vorhersage">
            {predictionLines.map((_, index) => {
              const actual = (actualLines[index] ?? '').trimEnd();
              const guess = (guesses[index] ?? '').trimEnd();
              if (guess.trim() === '') {
                return (
                  <li key={index}>
                    <Translate id="csharpRunner.predict.none" values={{n: index + 1, actual}}>
                      {'Zeile {n}: kein Tipp eingegeben. Ausgabe: {actual}'}
                    </Translate>
                  </li>
                );
              }
              return guess === actual ? (
                <li key={index}>
                  <span aria-hidden="true">✓ </span>
                  <Translate id="csharpRunner.predict.ok" values={{n: index + 1}}>{'Zeile {n}: Stimmt.'}</Translate>
                </li>
              ) : (
                <li key={index}>
                  <span aria-hidden="true">✗ </span>
                  <Translate id="csharpRunner.predict.wrong" values={{n: index + 1, actual}}>
                    {'Zeile {n}: Stimmt nicht – erwartet war {actual}'}
                  </Translate>
                </li>
              );
            })}
          </ul>
        )}
        {result?.tests && (
          <>
            <p>
              <Translate
                id="csharpRunner.tests.summary"
                values={{passed: result.tests.filter((t) => t.passed).length, total: result.tests.length}}>
                {'Tests: {passed} von {total} bestanden'}
              </Translate>
            </p>
            <ul className={styles.list} aria-label="Testergebnisse">
              {result.tests.map((t, index) => (
                <li key={index}>
                  <span aria-hidden="true">{t.passed ? '✓ ' : '✗ '}</span>
                  <strong>{t.name}</strong>
                  {': '}
                  {t.passed ? (
                    <Translate id="csharpRunner.tests.passed">bestanden</Translate>
                  ) : (
                    <Translate id="csharpRunner.tests.failed">nicht bestanden</Translate>
                  )}
                  {t.feedback ? ` – ${t.feedback}` : ''}
                </li>
              ))}
            </ul>
          </>
        )}
        {result && !result.success && (
          <p>
            <Translate id="csharpRunner.compileFailed">
              Der Code lässt sich nicht kompilieren; Details stehen im Runner.
            </Translate>
          </p>
        )}
      </div>
    </div>
  );
}
