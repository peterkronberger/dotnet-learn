import React, {useId, useState} from 'react';
import clsx from 'clsx';
import Translate, {translate} from '@docusaurus/Translate';
import Feedback from './Feedback';
import DeferredActions, {useEditFocus} from './DeferredActions';
import {renderInline} from './QuizText';
import {useShuffledOrder} from './shuffle';
import styles from './styles.module.css';

const t = (id, message, values) => translate({id, message}, values);

export default function MatchQuestion({question, onResult, deferred = false}) {
  const baseId = useId();
  const {order, reshuffle} = useShuffledOrder(question.pairs.length);
  const rights = order.map((i) => question.pairs[i].right);
  const [chosen, setChosen] = useState(() => question.pairs.map(() => ''));
  const [result, setResult] = useState(null);
  const checked = result !== null;
  const reveal = checked && !deferred;
  const {formRef, requestFocus} = useEditFocus();
  const complete = chosen.every((c) => c !== '');

  function check(event) {
    event.preventDefault();
    if (!complete) return;
    const next = question.pairs.every((p, i) => chosen[i] === p.right) ? 'correct' : 'wrong';
    setResult(next);
    onResult(next);
  }

  function retry() {
    setChosen(question.pairs.map(() => ''));
    setResult(null);
    reshuffle();
  }

  function edit() {
    requestFocus();
    setResult(null);
    onResult(null);
  }

  const wrongMatches = question.wrongMatches ?? [];
  const notes = [];
  const misconceptions = [];
  if (reveal) {
    question.pairs.forEach((p, i) => {
      if (chosen[i] === p.right) return;
      const w = wrongMatches.find((m) => m.left === p.left && m.right === chosen[i]);
      if (w && w.feedback) notes.push(`${p.left}: ${w.feedback}`);
      if (w && w.misconception) misconceptions.push(w.misconception);
    });
  }

  return (
    <form className={styles.quiz} onSubmit={check} ref={formRef}>
      <fieldset className={styles.fieldset} disabled={checked}>
        <legend className={styles.question}>{renderInline(question.question)}</legend>
        <div className={styles.matchRows}>
          {question.pairs.map((pair, i) => {
            const selectId = `${baseId}-${i}`;
            const ok = chosen[i] === pair.right;
            return (
              <div key={pair.left} className={styles.matchRow}>
                <label htmlFor={selectId} className={styles.matchLabel}>
                  {renderInline(pair.left)}
                </label>
                <select
                  id={selectId}
                  className={clsx(styles.matchSelect, reveal && (ok ? styles.optionCorrect : styles.optionWrong))}
                  value={chosen[i]}
                  onChange={(e) => setChosen((prev) => prev.map((c, j) => (j === i ? e.target.value : c)))}>
                  <option value="">{t('quiz.match.choose', 'Bitte wählen')}</option>
                  {rights.map((right) => (
                    <option key={right} value={right}>
                      {right}
                    </option>
                  ))}
                </select>
                {reveal && ok && (
                  <span className={clsx(styles.mark, styles.markOk)}>
                    ✓ <Translate id="quiz.match.right">Stimmt</Translate>
                  </span>
                )}
                {reveal && !ok && (
                  <span className={clsx(styles.mark, styles.markBad)}>
                    ✗ {t('quiz.match.wrong', 'Stimmt nicht – richtig ist: {answer}', {answer: pair.right})}
                  </span>
                )}
              </div>
            );
          })}
        </div>
      </fieldset>
      {deferred ? (
        <DeferredActions saved={checked} canSave={complete} onEdit={edit} />
      ) : (
      <div className={styles.actions}>
        {!checked && (
          <button type="submit" className="button button--primary" disabled={!complete}>
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
      <Feedback result={reveal ? result : null} notes={notes} explanation={question.explanation} misconceptions={misconceptions} />
    </form>
  );
}
