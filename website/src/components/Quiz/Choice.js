import React, {useId, useState} from 'react';
import clsx from 'clsx';
import Translate from '@docusaurus/Translate';
import {useShuffledOrder} from './shuffle';
import Feedback from './Feedback';
import DeferredActions, {useEditFocus} from './DeferredActions';
import {renderInline} from './QuizText';
import styles from './styles.module.css';

// Shared implementation of `single` (radio group) and `multi` (checkboxes).
export default function Choice({question, multi, onResult, deferred = false}) {
  const groupName = useId();
  const [selected, setSelected] = useState([]);
  const [result, setResult] = useState(null);
  const checked = result !== null;
  const reveal = checked && !deferred;
  const {formRef, requestFocus} = useEditFocus();
  const options = question.options;
  const pinnedLast = options.flatMap((o, i) => (o.pin === 'last' ? [i] : []));
  const {order, ready, reshuffle} = useShuffledOrder(options.length, {pinnedLast});

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
    reshuffle();
  }

  function edit() {
    requestFocus();
    setResult(null);
    onResult(null);
  }

  // Feedback of wrongly set options (and missing correct ones for multi).
  const notes = reveal
    ? options
        .filter((o, i) => Boolean(o.correct) !== selected.includes(i))
        .map((o) => o.feedback)
        .filter(Boolean)
    : [];
  const misconceptions = reveal
    ? selected.map((i) => options[i].misconception).filter(Boolean)
    : [];

  return (
    <form className={styles.quiz} onSubmit={check} ref={formRef}>
      <fieldset className={styles.fieldset} disabled={checked}>
        <legend className={styles.question}>{renderInline(question.question)}</legend>
        <div className={clsx(!ready && styles.shuffling)}>
        {order.map((index) => {
          const option = options[index];
          const isSelected = selected.includes(index);
          const isCorrect = Boolean(option.correct);
          return (
            <label
              key={index}
              className={clsx(
                styles.option,
                reveal && isCorrect && styles.optionCorrect,
                reveal && isSelected && !isCorrect && styles.optionWrong,
              )}>
              <input
                type={multi ? 'checkbox' : 'radio'}
                name={groupName}
                checked={isSelected}
                onChange={() => toggle(index)}
              />
              <span>
                {renderInline(option.text)}
                {reveal && isCorrect && <span className={styles.mark}> ✓</span>}
                {reveal && isSelected && !isCorrect && <span className={styles.mark}> ✗</span>}
              </span>
            </label>
          );
        })}
        </div>
      </fieldset>
      {multi && !checked && (
        <p className={styles.hint}>
          <Translate id="quiz.multiHint">Mehrere Antworten können richtig sein.</Translate>
        </p>
      )}
      {deferred ? (
        <DeferredActions saved={checked} canSave={selected.length > 0} onEdit={edit} />
      ) : (
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
      )}
      <Feedback
        result={reveal ? result : null}
        notes={notes}
        explanation={question.explanation}
        misconceptions={misconceptions}
      />
    </form>
  );
}
