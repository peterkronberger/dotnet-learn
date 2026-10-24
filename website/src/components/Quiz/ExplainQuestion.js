import React, {useId, useState} from 'react';
import Translate from '@docusaurus/Translate';
import QuizText, {renderInline} from './QuizText';
import styles from './styles.module.css';

// The learner's own explanation is not stored; only the self-assessment is.
export default function ExplainQuestion({question, onResult}) {
  const inputId = useId();
  const [text, setText] = useState('');
  const [revealed, setRevealed] = useState(false);
  const [rating, setRating] = useState(null);

  function rate(value) {
    setRating(value);
    onResult(value);
  }

  return (
    <div className={styles.quiz}>
      <p className={styles.question}>{renderInline(question.question)}</p>
      <label htmlFor={inputId}>
        <Translate id="quiz.explainLabel">Deine Erklärung (wird nicht gespeichert)</Translate>
      </label>
      <textarea
        id={inputId}
        className={styles.textarea}
        rows={4}
        value={text}
        onChange={(e) => setText(e.target.value)}
      />
      <div className={styles.actions}>
        {!revealed && (
          <button type="button" className="button button--primary" onClick={() => setRevealed(true)}>
            <Translate id="quiz.showModelAnswer">Musterantwort zeigen</Translate>
          </button>
        )}
      </div>
      <div aria-live="polite">
        {revealed && (
          <div className={styles.feedback}>
            <strong><Translate id="quiz.modelAnswer">Musterantwort</Translate></strong>
            <QuizText text={question.modelAnswer} />
            {question.explanation && <QuizText text={question.explanation} />}
            <fieldset className={styles.fieldset}>
              <legend>
                <Translate id="quiz.selfAssessment">Wie gut hast du es erklärt?</Translate>
              </legend>
              <div className={styles.actions}>
                {[
                  ['correct', <Translate key="c" id="quiz.rating.correct">Vollständig erklärt</Translate>],
                  ['partial', <Translate key="p" id="quiz.rating.partial">Teilweise</Translate>],
                  ['wrong', <Translate key="w" id="quiz.rating.wrong">Nicht erklärt</Translate>],
                ].map(([value, label]) => (
                  <button
                    key={value}
                    type="button"
                    className={`button button--secondary${rating === value ? ' button--active' : ''}`}
                    aria-pressed={rating === value}
                    disabled={rating !== null && rating !== value}
                    onClick={() => rate(value)}>
                    {label}
                  </button>
                ))}
              </div>
            </fieldset>
          </div>
        )}
      </div>
    </div>
  );
}
