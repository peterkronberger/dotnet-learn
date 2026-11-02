import React from 'react';
import CodeBlock from '@theme/CodeBlock';
import {parseFolds} from '../CSharpRunner/folds';

/**
 * Shows a C# file with its fold markers (// @fold-start, // @fold-end) applied:
 * visible parts are code blocks, each fold is a collapsed <details> with the hidden code.
 * Without folds it renders exactly one <CodeBlock>.
 *
 * Props:
 *  - code:  C# code (string, from raw-loader)
 *  - title: shown on the first code block
 */
export default function FoldedCode({code, title}) {
  const {parts, hasFolds} = parseFolds(code);
  if (!hasFolds) {
    return <CodeBlock language="csharp" title={title}>{code}</CodeBlock>;
  }
  let foldNumber = 0;
  return (
    <>
      {parts.map((part, index) => {
        const blockTitle = index === 0 ? title : undefined;
        if (part.kind === 'visible') {
          return <CodeBlock key={index} language="csharp" title={blockTitle}>{part.code}</CodeBlock>;
        }
        foldNumber += 1;
        return (
          <details key={index} className="margin-bottom--md">
            <summary>Block {foldNumber}: {part.lineCount} unveränderte Zeilen anzeigen</summary>
            <CodeBlock language="csharp">{part.code}</CodeBlock>
          </details>
        );
      })}
    </>
  );
}
