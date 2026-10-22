import React, {useEffect, useId, useState} from 'react';
import clsx from 'clsx';
import styles from './styles.module.css';

const STORAGE_PREFIX = 'dotnet-learn:quiz:';

function readProgress(id) {
  try {
    const raw = window.localStorage.getItem(STORAGE_PREFIX + id);
    return raw ? JSON.parse(raw) : null;
  } catch (error) {
    return null; // z. B. privater Modus oder blockierter Speicher
  }
}

function writeProgress(id, value) {
  try {
    window.localStorage.setItem(STORAGE_PREFIX + id, JSON.stringify(value));
  } catch (error) {
    // Fortschritt ist nur eine Komfortfunktion – Fehler bewusst ignoriert.
  }
}

// Wandelt `code` in Backticks in <code>-Elemente um.
function renderInline(text) {
  return String(text)
    .split('`')
    .map((part, index) =>
      index % 2 === 1 ? <code key={index}>{part}</code> : <span key={index}>{part}</span>,
    );
}

/**
 * Multiple-Choice-Frage mit erklärendem Feedback (Abrufübung).
 *
 * Props:
 *  - id:       eindeutiger Schlüssel (für den lokalen Lernfortschritt)
 *  - question: Fragetext (Backticks = Inline-Code)
 *  - options:  Array von Antworttexten
 *  - answer:   Index der richtigen Antwort (0-basiert)
 *  - children: Erklärung, wird nach dem Prüfen angezeigt
 */
export default function Quiz({id, question, options, answer, children}) {
  const groupName = useId();
  const [selected, setSelected] = useState(null);
  const [checked, setChecked] = useState(false);
  const [previous, setPrevious] = useState(null);

  useEffect(() => {
    setPrevious(readProgress(id));
  }, [id]);

  const isCorrect = checked && selected === answer;

  function check(event) {
    event.preventDefault();
    if (selected === null) {
      return;
    }
    setChecked(true);
    const before = readProgress(id) ?? {attempts: 0};
    const next = {
      attempts: before.attempts + 1,
      correct: selected === answer,
      lastAttempt: new Date().toISOString(),
    };
    writeProgress(id, next);
    setPrevious(next);
  }

  function retry() {
    setSelected(null);
    setChecked(false);
  }

  return (
    <form className={styles.quiz} onSubmit={check}>
      <fieldset className={styles.fieldset} disabled={checked}>
        <legend className={styles.question}>{renderInline(question)}</legend>
        {options.map((option, index) => (
          <label
            key={index}
            className={clsx(
              styles.option,
              checked && index === answer && styles.optionCorrect,
              checked && index === selected && index !== answer && styles.optionWrong,
            )}>
            <input
              type="radio"
              name={groupName}
              value={index}
              checked={selected === index}
              onChange={() => setSelected(index)}
            />
            <span>{renderInline(option)}</span>
          </label>
        ))}
      </fieldset>

      <div className={styles.actions}>
        {!checked && (
          <button type="submit" className="button button--primary" disabled={selected === null}>
            Antwort prüfen
          </button>
        )}
        {checked && !isCorrect && (
          <button type="button" className="button button--secondary" onClick={retry}>
            Nochmal versuchen
          </button>
        )}
        {previous?.correct && !checked && (
          <span className={styles.badge}>✓ schon einmal richtig beantwortet</span>
        )}
      </div>

      <div aria-live="polite">
        {checked && (
          <div className={clsx(styles.feedback, isCorrect ? styles.feedbackOk : styles.feedbackBad)}>
            <strong>{isCorrect ? 'Richtig.' : 'Noch nicht ganz.'}</strong>
            <div className={styles.explanation}>{children}</div>
          </div>
        )}
      </div>
    </form>
  );
}
