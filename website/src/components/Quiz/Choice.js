import React, {useId, useState} from 'react';
import clsx from 'clsx';
import Translate from '@docusaurus/Translate';
import Feedback from './Feedback';
import {renderInline} from './QuizText';
import styles from './styles.module.css';

// Shared implementation of `single` (radio group) and `multi` (checkboxes).
export default function Choice({question, multi, onResult}) {
  const groupName = useId();
  const [selected, setSelected] = useState([]);
  const [result, setResult] = useState(null);
  const checked = result !== null;
  const options = question.options;

  function toggle(index) {
    setSelected((prev) =>
      multi ? (prev.includes(index) ? prev.filter((i) => i !== index) : [...prev, index]) : [index],
    );
  }

  function check(event) {
    event.preventDefault();
    if (selected.length === 0) return;
    const ok = options.every((o, i) => Boolean(o.correct) === selected.includes(i));
    const next = ok ? 'correct' : 'wrong';
    setResult(next);
    onResult(next);
  }

  function retry() {
    setSelected([]);
    setResult(null);
  }

  // Feedback of wrongly set options (and missing correct ones for multi).
  const notes = checked
    ? options
        .filter((o, i) => Boolean(o.correct) !== selected.includes(i))
        .map((o) => o.feedback)
        .filter(Boolean)
    : [];
  const misconceptions = checked
    ? selected.map((i) => options[i].misconception).filter(Boolean)
    : [];

  return (
    <form className={styles.quiz} onSubmit={check}>
      <fieldset className={styles.fieldset} disabled={checked}>
        <legend className={styles.question}>{renderInline(question.question)}</legend>
        {options.map((option, index) => {
          const isSelected = selected.includes(index);
          const isCorrect = Boolean(option.correct);
          return (
            <label
              key={index}
              className={clsx(
                styles.option,
                checked && isCorrect && styles.optionCorrect,
                checked && isSelected && !isCorrect && styles.optionWrong,
              )}>
              <input
                type={multi ? 'checkbox' : 'radio'}
                name={groupName}
                checked={isSelected}
                onChange={() => toggle(index)}
              />
              <span>
                {renderInline(option.text)}
                {checked && isCorrect && <span className={styles.mark}> ✓</span>}
                {checked && isSelected && !isCorrect && <span className={styles.mark}> ✗</span>}
              </span>
            </label>
          );
        })}
      </fieldset>
      {multi && !checked && (
        <p className={styles.hint}>
          <Translate id="quiz.multiHint">Mehrere Antworten können richtig sein.</Translate>
        </p>
      )}
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
      <Feedback
        result={result}
        notes={notes}
        explanation={question.explanation}
        misconceptions={misconceptions}
      />
    </form>
  );
}
