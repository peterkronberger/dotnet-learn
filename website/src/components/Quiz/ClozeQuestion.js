import React, {useState} from 'react';
import clsx from 'clsx';
import Translate, {translate} from '@docusaurus/Translate';
import Feedback from './Feedback';
import {renderInline} from './QuizText';
import {getExample} from '../../data/quiz/examples';
import {splitCloze, normalizeBlank, clozeCode} from '../../data/quiz/cloze';
import styles from './styles.module.css';

export {splitCloze};

const t = (id, message, values) => translate({id, message}, values);

export default function ClozeQuestion({question, onResult}) {
  const example = getExample(question.example);
  if (!example) {
    throw new Error(`Quiz "${question.id}": example "${question.example}" or its .out.txt not found`);
  }
  const blanks = question.blanks;
  const parts = splitCloze(clozeCode(example.code), blanks);
  const [values, setValues] = useState(() => blanks.map(() => ''));
  const [marks, setMarks] = useState(null); // per blank: true/false once checked
  const [failedTries, setFailedTries] = useState(0);
  const [showSolution, setShowSolution] = useState(false);
  const checked = marks !== null;
  const result = checked ? (marks.every(Boolean) ? 'correct' : 'wrong') : null;

  const label = (n) => t('quiz.cloze.blankLabel', 'Lücke {n} von {total}', {n: n + 1, total: blanks.length});

  function check(event) {
    event.preventDefault();
    if (values.every((v) => v.trim() === '')) return;
    const next = blanks.map((blank, i) =>
      (blank.accept ?? [blank.text]).map(normalizeBlank).includes(normalizeBlank(values[i])),
    );
    setMarks(next);
    if (!next.every(Boolean)) setFailedTries(failedTries + 1);
    onResult(next.every(Boolean) ? 'correct' : 'wrong');
  }

  const notes = [];
  const misconceptions = [];
  if (checked) {
    blanks.forEach((blank, i) => {
      if (marks[i]) return;
      const wrong = (blank.wrongAnswers ?? []).find((w) => normalizeBlank(w.text) === normalizeBlank(values[i]));
      if (wrong && wrong.feedback) notes.push(`${label(i)}: ${wrong.feedback}`);
      else if (blank.hint) notes.push(`${label(i)}: ${blank.hint}`);
      if (wrong && wrong.misconception) misconceptions.push(wrong.misconception);
    });
  }

  return (
    <form className={styles.quiz} onSubmit={check}>
      <p className={styles.question}>{renderInline(question.question)}</p>
      <pre className={styles.clozeCode}>
        <code>
          {parts.map((part, i) => {
            if (part.kind === 'code') return <React.Fragment key={i}>{part.text}</React.Fragment>;
            const n = part.index;
            return (
              <React.Fragment key={i}>
                <input
                  type="text"
                  className={clsx(styles.clozeInput, checked && (marks[n] ? styles.clozeOk : styles.clozeBad))}
                  size={Math.max(3, blanks[n].text.length)}
                  value={values[n]}
                  readOnly={checked}
                  autoComplete="off"
                  autoCapitalize="off"
                  spellCheck={false}
                  aria-label={label(n)}
                  onChange={(e) => setValues(values.map((v, j) => (j === n ? e.target.value : v)))}
                />
                {checked && (
                  <span className={clsx(styles.mark, marks[n] ? styles.markOk : styles.markBad)}>
                    {marks[n] ? ' ✓ Stimmt' : ' ✗ Stimmt nicht'}
                  </span>
                )}
              </React.Fragment>
            );
          })}
        </code>
      </pre>

      <div className={styles.actions}>
        {!checked && (
          <button type="submit" className="button button--primary">
            <Translate id="quiz.check">Antwort prüfen</Translate>
          </button>
        )}
        {checked && result !== 'correct' && (
          <button type="button" className="button button--secondary" onClick={() => setMarks(null)}>
            <Translate id="quiz.retry">Nochmal versuchen</Translate>
          </button>
        )}
      </div>
      {checked && result === 'wrong' && failedTries >= 2 && (
        <div className={styles.actions}>
          <button
            type="button"
            className="button button--secondary"
            aria-expanded={showSolution}
            onClick={() => setShowSolution(!showSolution)}>
            {showSolution ? (
              <Translate id="quiz.cloze.hideSolution">Lösung ausblenden</Translate>
            ) : (
              <Translate id="quiz.cloze.showSolution">Lösung anzeigen</Translate>
            )}
          </button>
        </div>
      )}
      {showSolution && (
        <div role="region" aria-label={t('quiz.cloze.solutionLabel', 'Lösung')}>
          <ul>
            {blanks.map((blank, i) => (
              <li key={i}>
                {label(i)}: <code>{blank.text}</code>
              </li>
            ))}
          </ul>
        </div>
      )}
      <Feedback result={result} notes={notes} explanation={question.explanation} misconceptions={misconceptions} />
    </form>
  );
}
