import React from 'react';
import Layout from '@theme/Layout';
import CSharpRunner from '@site/src/components/CSharpRunner';
import predictDemo from '!!raw-loader!@site/examples/s7/predict-demo.cs';
import predictDemoOutput from '!!raw-loader!@site/examples/s7/predict-demo.out.txt';
import palindromeTask from '!!raw-loader!@site/examples/s7/palindrome-task.cs';
import palindromeTests from '!!raw-loader!@site/examples/s7/palindrome-task.tests.snippet.cs';

// Technische Demo für Spike S7 (docs-dev/adr/0006-runner-messages.md), kein Lerninhalt.
export default function RunnerDemo() {
  return (
    <Layout title="Runner-Demo (S7)" description="Technische Demo: Runner meldet Ergebnisse an die Seite">
      <main className="container margin-vert--lg">
        <h1>Runner-Demo (Spike S7)</h1>
        <h2>Vorhersagefeld</h2>
        <p>Tippe die Ausgabe jeder Zeile, führe den Code aus und vergleiche.</p>
        <CSharpRunner id="s7-predict" code={predictDemo} predict expected={predictDemoOutput} height={300} />
        <h2>Geprüfte Aufgabe</h2>
        <p>
          Implementiere <code>IsPalindrome</code>. Zwei Tests sind sichtbar, zwei versteckt.
        </p>
        <CSharpRunner id="s7-task" code={palindromeTask} tests={palindromeTests} height={360} />
      </main>
    </Layout>
  );
}
