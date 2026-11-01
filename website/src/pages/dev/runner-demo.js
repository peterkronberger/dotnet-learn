import React from 'react';
import Layout from '@theme/Layout';
import DevNav from '@site/src/components/DevNav';
import CSharpRunner from '@site/src/components/CSharpRunner';
import predictDemo from '!!raw-loader!@site/examples/dev/predict-demo.cs';
import predictDemoOutput from '!!raw-loader!@site/examples/dev/predict-demo.out.txt';
import palindromeTask from '!!raw-loader!@site/examples/dev/palindrome-task.cs';
import palindromeTests from '!!raw-loader!@site/examples/dev/palindrome-task.tests.snippet.cs';
import loopTrace from '!!raw-loader!@site/examples/dev/loop-trace.cs';
import readName from '!!raw-loader!@site/examples/dev/read-name.cs';
import readNameInput from '!!raw-loader!@site/examples/dev/read-name.in.txt';
import referenceTypes from '!!raw-loader!@site/examples/dev/reference-types.cs';

// Technical demo for spike S7, not learning content.
export default function RunnerDemo() {
  return (
    <Layout title="Runner-Demo (S7)" description="Technische Demo: Runner meldet Ergebnisse an die Seite">
      <main className="container margin-vert--lg">
        <DevNav />
        <h1>Runner-Demo (Spike S7)</h1>
        <h2>Vorhersagefeld</h2>
        <p>Tippe die Ausgabe jeder Zeile, führe den Code aus und vergleiche.</p>
        <CSharpRunner id="s7-predict" code={predictDemo} predict expected={predictDemoOutput} />
        <h2>Geprüfte Aufgabe</h2>
        <p>
          Implementiere <code>IsPalindrome</code>. Zwei Tests sind sichtbar, zwei versteckt.
        </p>
        <CSharpRunner id="s7-task" code={palindromeTask} tests={palindromeTests} />
        <h2>Schrittansicht (Spike S5)</h2>
        <p>
          Mit <code>trace</code> zeigt der Runner nach dem Ausführen Zeile, Variablen und Ausgabe pro Schritt.
        </p>
        <CSharpRunner id="s5-loop" code={loopTrace} trace />
        <p>Wert- und Referenztypen: zwei Variablen können auf dasselbe Objekt zeigen.</p>
        <CSharpRunner id="s5-refs" code={referenceTypes} trace />
        <h2>Konsoleneingabe (stdin)</h2>
        <p>
          Mit <code>stdin</code> zeigt der Runner ein Eingabefeld. Jede Zeile liest ein{' '}
          <code>Console.ReadLine()</code>; nach der letzten Zeile liefert es <code>null</code>.
        </p>
        <CSharpRunner id="stdin-demo" code={readName} stdin={readNameInput} />
        <h2>Entladen weit entfernter Runner</h2>
        <p>
          Runner, die weit außerhalb des Fensters liegen (etwa drei Bildschirmhöhen), werden entladen und beim
          Zurückscrollen wieder geladen. Dein getippter Code und die Eingabe bleiben erhalten, die Ausgabe nicht.
          Ändere den Code im ersten Runner, scrolle weit nach unten und wieder zurück.
        </p>
        {Array.from({length: 12}, (_, index) => (
          <React.Fragment key={index}>
            <p>Runner {index + 1} von 12</p>
            <CSharpRunner id={`unload-demo-${index}`} code={loopTrace} />
          </React.Fragment>
        ))}
      </main>
    </Layout>
  );
}
