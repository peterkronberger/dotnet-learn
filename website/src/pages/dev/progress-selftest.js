import React, { useEffect, useState } from 'react';
import Layout from '@theme/Layout';
import Head from '@docusaurus/Head';
import DevNav from '@site/src/components/DevNav';
import { runProgressSelfTests } from '@site/src/lib/progress/selftest';

// Developer page: runs the progress library self-tests in the browser (memory storage only).
export default function ProgressSelftest() {
  const [results, setResults] = useState(null);

  useEffect(() => {
    setResults(runProgressSelfTests());
  }, []);

  const passed = results ? results.filter((r) => r.passed).length : 0;
  const status = results ? (passed === results.length ? 'passed' : 'failed') : 'running';

  return (
    <Layout title="Selbsttest: Lernfortschritt">
      <Head>
        <meta name="robots" content="noindex" />
      </Head>
      <main className="container margin-vert--lg" data-selftest-status={status}>
        <DevNav />
        <h1>Selbsttest: Lernfortschritt</h1>
        <p aria-live="polite">
          {results ? `${passed} von ${results.length} Tests bestanden` : 'Tests laufen …'}
        </p>
        {results && (
          <table>
            <thead>
              <tr>
                <th>Test</th>
                <th>Ergebnis</th>
                <th>Detail</th>
              </tr>
            </thead>
            <tbody>
              {results.map((r) => (
                <tr key={r.name}>
                  <td>{r.name}</td>
                  <td>{r.passed ? '✓ bestanden' : '✗ fehlgeschlagen'}</td>
                  <td>{r.detail}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </main>
    </Layout>
  );
}
