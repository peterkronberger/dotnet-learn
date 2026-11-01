import React, {useId, useState} from 'react';
import clsx from 'clsx';
import CodeBlock from '@theme/CodeBlock';
import Translate from '@docusaurus/Translate';
import Feedback from './Feedback';
import DeferredActions, {useEditFocus} from './DeferredActions';
import {renderInline} from './QuizText';
import {getExample} from '../../data/quiz/examples';
import styles from './styles.module.css';

// CRLF -> LF, trailing spaces per line and trailing blank lines removed.
export function normalizeOutput(text) {
  return String(text)
    .replace(/\r\n?/g, '\n')
    .split('\n')
    .map((line) => line.replace(/\s+$/, ''))
    .join('\n')
    .replace(/\n+$/, '');
}

export default function OutputQuestion({question, onResult, deferred = false}) {
  const inputId = useId();
  const example = getExample(question.example);
  if (!example) {
    throw new Error(`Quiz "${question.id}": example "${question.example}" or its .out.txt not found`);
  }
  const [answer, setAnswer] = useState('');
  const [result, setResult] = useState(null);
  const checked = result !== null;
  const reveal = checked && !deferred;
  const {formRef, requestFocus} = useEditFocus();

  function check(event) {
    event.preventDefault();
    const ok = normalizeOutput(answer) === normalizeOutput(example.expected);
    const next = ok ? 'correct' : 'wrong';
    setResult(next);
    onResult(next);
  }

  function retry() {
    setResult(null);
  }

  function edit() {
    requestFocus();
    setResult(null);
    onResult(null);
  }

  const given = normalizeOutput(answer).split('\n');
  const actual = normalizeOutput(example.expected).split('\n');
  const rows = Math.max(given.length, actual.length);

  return (
    <form className={styles.quiz} onSubmit={check} ref={formRef}>
      <p className={styles.question}>{renderInline(question.question)}</p>
      <CodeBlock language="csharp">{example.code.replace(/\n+$/, '')}</CodeBlock>
      <label htmlFor={inputId}>
        <Translate id="quiz.outputLabel">Deine Vorhersage der Ausgabe (eine Zeile pro Ausgabezeile)</Translate>
      </label>
      <textarea
        id={inputId}
        className={styles.textarea}
        rows={Math.max(3, actual.length + 1)}
        value={answer}
        readOnly={checked}
        onChange={(e) => setAnswer(e.target.value)}
      />
      {deferred ? (
        <DeferredActions saved={checked} canSave={answer.trim() !== ''} onEdit={edit} />
      ) : (
      <div className={styles.actions}>
        {!checked && (
          <button type="submit" className="button button--primary" disabled={answer.trim() === ''}>
            <Translate id="quiz.check">Antwort prüfen</Translate>
          </button>
        )}
        {checked && result !== 'correct' && (
          <button type="button" className="button button--secondary" onClick={retry}>
            <Translate id="quiz.retry">Nochmal versuchen</Translate>
          </button>
        )}
      </div>
      )}
      {reveal && result === 'wrong' && (
        <table className={styles.compare}>
          <thead>
            <tr>
              <th><Translate id="quiz.yourAnswer">Deine Antwort</Translate></th>
              <th><Translate id="quiz.actualOutput">Tatsächliche Ausgabe</Translate></th>
              <th><span className="sr-only"><Translate id="quiz.lineResult">Ergebnis</Translate></span></th>
            </tr>
          </thead>
          <tbody>
            {Array.from({length: rows}, (_, i) => {
              const same = given[i] === actual[i];
              return (
                <tr key={i} className={clsx(same ? styles.optionCorrect : styles.optionWrong)}>
                  <td><code>{given[i] ?? ''}</code></td>
                  <td><code>{actual[i] ?? ''}</code></td>
                  <td>{same ? '✓ Stimmt' : `✗ Stimmt nicht – erwartet war ${actual[i] ?? '(keine Zeile)'}`}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      )}
      <Feedback result={reveal ? result : null} explanation={question.explanation} />
    </form>
  );
}
