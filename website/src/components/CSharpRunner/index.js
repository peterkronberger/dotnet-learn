import React, {useCallback, useEffect, useRef} from 'react';
import clsx from 'clsx';
import useBaseUrl from '@docusaurus/useBaseUrl';
import {useColorMode} from '@docusaurus/theme-common';
import styles from './styles.module.css';

// Base64url (UTF-8), damit beliebiger Code im URL-Fragment übergeben werden kann.
// Das Fragment wird nie an den Server gesendet.
function encodeCode(code) {
  const bytes = new TextEncoder().encode(code);
  let binary = '';
  bytes.forEach((byte) => {
    binary += String.fromCharCode(byte);
  });
  return btoa(binary).replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');
}

/**
 * Bettet den C#-Playground (Blazor WebAssembly + Roslyn, liegt unter /runner/)
 * als iframe ein und füllt den Editor mit `code`.
 *
 * Props:
 *  - code:   Startcode (String)
 *  - height: Höhe in Pixeln (Standard 460)
 */
export default function CSharpRunner({code = '', height = 460}) {
  const runnerUrl = useBaseUrl('/runner/');
  const {colorMode} = useColorMode();
  const frame = useRef(null);

  const syncTheme = useCallback(() => {
    frame.current?.contentWindow?.postMessage({type: 'theme', value: colorMode}, window.location.origin);
  }, [colorMode]);

  useEffect(() => {
    syncTheme();
  }, [syncTheme]);

  const src = code ? `${runnerUrl}#code=${encodeCode(code)}` : runnerUrl;

  return (
    <div className={clsx(styles.wrapper)}>
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
    </div>
  );
}
