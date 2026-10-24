import React from 'react';
import Layout from '@theme/Layout';
import CSharpRunner from '@site/src/components/CSharpRunner';
import predictDemo from '!!raw-loader!@site/examples/s7/predict-demo.cs';
import predictDemoOutput from '!!raw-loader!@site/examples/s7/predict-demo.out.txt';
import palindromeTask from '!!raw-loader!@site/examples/s7/palindrome-task.cs';
import palindromeTests from '!!raw-loader!@site/examples/s7/palindrome-task.tests.snippet.cs';
import loopTrace from '!!raw-loader!@site/examples/s5/loop-trace.cs';
import referenceTypes from '!!raw-loader!@site/examples/s5/reference-types.cs';

// Technical demo for spike S7, not learning content.
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
        <h2>Schrittansicht (Spike S5)</h2>
        <p>
          Mit <code>trace</code> zeigt der Runner nach dem Ausführen Zeile, Variablen und Ausgabe pro Schritt.
        </p>
        <CSharpRunner id="s5-loop" code={loopTrace} trace height={900} />
        <p>Wert- und Referenztypen: zwei Variablen können auf dasselbe Objekt zeigen.</p>
        <CSharpRunner id="s5-refs" code={referenceTypes} trace height={1000} />
      </main>
    </Layout>
  );
}
