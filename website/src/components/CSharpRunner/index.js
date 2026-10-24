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

/**
 * Embeds the C# playground (Blazor WebAssembly + Roslyn, served under /runner/)
 * as an iframe and fills the editor with `code`.
 *
 * Props:
 *  - code:     start code (string)
 *  - height:   height in pixels (default 460)
 *  - id:       stable key of this instance (default: automatic)
 *  - predict:  input field per output line; after running, ✓/✗ per line (needs `expected`)
 *  - expected: expected output, only sets the number of fields; the check uses the real output
 *  - tests:    C# check code (see concept/components.md); the runner then runs the tests
 *  - trace:    turn on the step view in the runner (variables, heap model)
 *  - onResult: callback with the `run-result` message of the runner
 *
 * The runner reports `{type: 'run-result', id, success, output, diagnostics, tests?}` by postMessage.
 * Both sides check the origin; the page also checks the sender (iframe) and `id`.
 */
export default function CSharpRunner({
  code = '',
  height = 460,
  id,
  predict = false,
  expected = '',
  tests,
  trace = false,
  onResult,
}) {
  const runnerUrl = useBaseUrl('/runner/');
  const {colorMode} = useColorMode();
  const frame = useRef(null);
  const autoId = useId();
  const instanceId = id ?? autoId;
  const [result, setResult] = useState(null);
  const [guesses, setGuesses] = useState({});
  const predictionLines = predict ? splitLines(expected) : [];

  const syncTheme = useCallback(() => {
    frame.current?.contentWindow?.postMessage({type: 'theme', value: colorMode}, window.location.origin);
  }, [colorMode]);

  useEffect(() => {
    syncTheme();
  }, [syncTheme]);

  useEffect(() => {
    function onMessage(event) {
      if (event.origin !== window.location.origin) { return; }
      if (event.source !== frame.current?.contentWindow) { return; }
      const data = event.data;
      if (!data || data.type !== 'run-result' || data.id !== instanceId) { return; }
      setResult(data);
      onResult?.(data);
    }
    window.addEventListener('message', onMessage);
    return () => window.removeEventListener('message', onMessage);
  }, [instanceId, onResult]);

  let fragment = '';
  if (code) {
    fragment = `#code=${encodeCode(code)}&id=${encodeURIComponent(instanceId)}`;
    if (tests) { fragment += `&tests=${encodeCode(tests)}`; }
    if (trace && !tests) { fragment += '&trace=1'; }
  }
  const src = `${runnerUrl}${fragment}`;

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
      <iframe
        ref={frame}
        className={styles.frame}
        src={src}
        title="Interaktiver C#-Playground"
        loading="lazy"
        height={height}
        onLoad={syncTheme}
      />
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
