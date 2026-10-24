import React, {useEffect, useState} from 'react';
import clsx from 'clsx';
import Translate from '@docusaurus/Translate';
import QuizText, {renderInline} from './QuizText';
import styles from './styles.module.css';

// Shared feedback: headline, option feedback, explanation, optional link to the "Achtung" box.
export default function Feedback({result, notes = [], explanation, misconceptions = []}) {
  const [anchors, setAnchors] = useState([]);

  // Only link to boxes that exist on the current page.
  useEffect(() => {
    setAnchors(misconceptions.filter((m) => document.getElementById(`misconception-${m}`)));
  }, [misconceptions.join('|')]);

  const ok = result === 'correct';
  return (
    <div aria-live="polite">
      {result && (
        <div className={clsx(styles.feedback, ok ? styles.feedbackOk : styles.feedbackBad)}>
          <strong>
            {ok ? (
              <Translate id="quiz.correct">Richtig.</Translate>
            ) : (
              <Translate id="quiz.incorrect">Noch nicht ganz.</Translate>
            )}
          </strong>
          {notes.map((note, i) => (
            <p key={i}>{renderInline(note)}</p>
          ))}
          {explanation && (
            <div className={styles.explanation}>
              <QuizText text={explanation} />
            </div>
          )}
          {anchors.map((m) => (
            <p key={m}>
              <a href={`#misconception-${m}`}>
                <Translate id="quiz.misconceptionLink">Mehr dazu in der Box „Achtung“</Translate>
              </a>
            </p>
          ))}
        </div>
      )}
    </div>
  );
}
