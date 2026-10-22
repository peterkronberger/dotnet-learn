import React from 'react';
import Layout from '@theme/Layout';
import CSharpRunner from '@site/src/components/CSharpRunner';

const STARTER = `// Sage voraus, was ausgegeben wird - und führe es dann aus.
var zahlen = new[] { 3, 1, 4, 1, 5 };
Console.WriteLine(zahlen.Sum());
Console.WriteLine(zahlen.Max());`;

export default function Playground() {
  return (
    <Layout title="Playground" description="C# direkt im Browser schreiben und ausführen">
      <main className="container margin-vert--lg">
        <h1>C#-Playground</h1>
        <p>
          Schreibe C#-Code und führe ihn sofort aus. Es reichen Anweisungen ohne <code>Main</code>{' '}
          (Top-Level-Statements). Häufige Namespaces wie <code>System</code>,{' '}
          <code>System.Linq</code> und <code>System.Collections.Generic</code> sind bereits eingebunden.
        </p>
        <CSharpRunner code={STARTER} height={640} />
      </main>
    </Layout>
  );
}
