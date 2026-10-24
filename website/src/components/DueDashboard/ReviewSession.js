import React, {useEffect, useRef, useState} from 'react';
import Link from '@docusaurus/Link';
import Translate from '@docusaurus/Translate';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import Quiz from '@site/src/components/Quiz';
import useProgress from '@site/src/lib/progress/useProgress';
import {nextDueDate} from '@site/src/lib/progress/leitner';
import {getQuestion} from '@site/src/data/quiz';
import {useLessonPath} from './lessonLinks';
import {formatDate} from './format';
import styles from './styles.module.css';

function LessonLink({lessonId}) {
  const path = useLessonPath(lessonId);
  if (!path) return null;
  return (
    <Link to={path}>
      <Translate id="dashboard.toLesson">Zur Lektion</Translate>
    </Link>
  );
}

// Shows the given questions one after another. The id list is fixed by the caller.
export default function ReviewSession({questionIds}) {
  const {i18n} = useDocusaurusContext();
  const {progress} = useProgress();
  const [index, setIndex] = useState(0);
  const [answered, setAnswered] = useState(false);
  const headingRef = useRef(null);
  const moved = useRef(false);

  // Move focus to the heading of the next question (not on first render).
  useEffect(() => {
    if (moved.current && headingRef.current) headingRef.current.focus();
  }, [index]);

  const total = questionIds.length;

  if (index >= total) {
    const next = progress ? nextDueDate(progress) : null;
    return (
      <div>
        <h3 tabIndex={-1} ref={headingRef} className={styles.heading}>
          <Translate id="dashboard.done">Für heute geschafft.</Translate>
        </h3>
        {next && (
          <p>
            <Translate id="dashboard.nextReview" values={{date: formatDate(next)}}>
              {'Nächste Wiederholung: {date}.'}
            </Translate>
          </p>
        )}
      </div>
    );
  }

  const id = questionIds[index];
  const question = getQuestion(i18n.currentLocale, id);

  return (
    <div>
      <h3 tabIndex={-1} ref={headingRef} className={styles.heading}>
        <Translate id="dashboard.questionOf" values={{index: index + 1, total}}>
          {'Frage {index} von {total}'}
        </Translate>
      </h3>
      <Quiz key={id} id={id} onAnswered={() => setAnswered(true)} />
      <div className={styles.actions}>
        {question && <LessonLink lessonId={question.lesson} />}
        <button
          type="button"
          className="button button--primary"
          disabled={!answered}
          onClick={() => {
            moved.current = true;
            setAnswered(false);
            setIndex(index + 1);
          }}>
          {index + 1 < total ? (
            <Translate id="dashboard.next">Nächste Frage</Translate>
          ) : (
            <Translate id="dashboard.finish">Abschließen</Translate>
          )}
        </button>
      </div>
    </div>
  );
}
