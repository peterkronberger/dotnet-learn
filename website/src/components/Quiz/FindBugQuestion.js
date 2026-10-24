import React, {useId, useState} from 'react';
import clsx from 'clsx';
import Translate, {translate} from '@docusaurus/Translate';
import Feedback from './Feedback';
import {renderInline} from './QuizText';
import {getExample} from '../../data/quiz/examples';
import {findBugLines} from '../../data/quiz/findBug';
import styles from './styles.module.css';

const t = (id, message, values) => translate({id, message}, values);

export default function FindBugQuestion({question, onResult}) {
  const example = getExample(question.example);
  if (!example) {
    throw new Error(`Quiz "${question.id}": example "${question.example}" not found`);
  }
  const lines = findBugLines(example.code);
  const bugLines = question.bugLines;
  const single = bugLines.length === 1;
  const groupName = useId();
  const [selected, setSelected] = useState([]);
  const [result, setResult] = useState(null);
  const checked = result !== null;

  function toggle(n) {
    setSelected((prev) => (single ? [n] : prev.includes(n) ? prev.filter((i) => i !== n) : [...prev, n]));
  }

  function check(event) {
    event.preventDefault();
    if (selected.length === 0) return;
    const ok = selected.length === bugLines.length && bugLines.every((n) => selected.includes(n));
    const next = ok ? 'correct' : 'wrong';
    setResult(next);
    onResult(next);
  }

  function retry() {
    setSelected([]);
    setResult(null);
  }

  const lineFeedback = question.lineFeedback ?? {};
  const sortedSelected = [...selected].sort((a, b) => a - b);
  const notes = checked
    ? sortedSelected
        .filter((n) => lineFeedback[n] && lineFeedback[n].feedback)
        .map((n) => `${t('quiz.findBug.line', 'Zeile {n}', {n})}: ${lineFeedback[n].feedback}`)
    : [];
  const misconceptions = checked
    ? sortedSelected.map((n) => lineFeedback[n] && lineFeedback[n].misconception).filter(Boolean)
    : [];
  const missed = checked ? bugLines.filter((n) => !selected.includes(n)) : [];
  if (missed.length > 0 && !single) {
    notes.push(
      t('quiz.findBug.missed', 'Noch nicht gefunden: {lines}', {
        lines: missed.map((n) => t('quiz.findBug.line', 'Zeile {n}', {n})).join(', '),
      }),
    );
  }

  return (
    <form className={styles.quiz} onSubmit={check}>
      <fieldset className={styles.fieldset} disabled={checked}>
        <legend className={styles.question}>{renderInline(question.question)}</legend>
        {!single && (
          <p className={styles.hint}>
            <Translate id="quiz.findBug.multiHint">Mehrere Zeilen können fehlerhaft sein.</Translate>
          </p>
        )}
        <div className={styles.bugCode}>
          {lines.map((code, index) => {
            const n = index + 1;
            const isSelected = selected.includes(n);
            const isBug = bugLines.includes(n);
            return (
              <label
                key={n}
                className={clsx(
                  styles.bugLine,
                  checked && isBug && styles.optionCorrect,
                  checked && isSelected && !isBug && styles.optionWrong,
                )}>
                <input
                  type={single ? 'radio' : 'checkbox'}
                  name={groupName}
                  checked={isSelected}
                  aria-label={t('quiz.findBug.lineLabel', 'Zeile {n}: {code}', {n, code})}
                  onChange={() => toggle(n)}
                />
                <span className={styles.bugNumber} aria-hidden="true">
                  {n}
                </span>
                <code className={styles.bugText} aria-hidden="true">
                  {code || ' '}
                </code>
                {checked && isBug && (
                  <span className={clsx(styles.mark, styles.markOk)}>
                    {' '}
                    ✓ <Translate id="quiz.findBug.isBug">Fehlerhaft</Translate>
                  </span>
                )}
                {checked && isSelected && !isBug && (
                  <span className={clsx(styles.mark, styles.markBad)}>
                    {' '}
                    ✗ <Translate id="quiz.findBug.noBug">Kein Fehler</Translate>
                  </span>
                )}
              </label>
            );
          })}
        </div>
      </fieldset>
      <div className={styles.actions}>
        {!checked && (
          <button type="submit" className="button button--primary" disabled={selected.length === 0}>
            <Translate id="quiz.check">Antwort prüfen</Translate>
          </button>
        )}
        {checked && result !== 'correct' && (
          <button type="button" className="button button--secondary" onClick={retry}>
            <Translate id="quiz.retry">Nochmal versuchen</Translate>
          </button>
        )}
      </div>
      <Feedback result={result} notes={notes} explanation={question.explanation} misconceptions={misconceptions} />
    </form>
  );
}
