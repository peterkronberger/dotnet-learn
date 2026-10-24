import React, {useEffect, useRef, useState} from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import Translate, {translate} from '@docusaurus/Translate';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import Quiz from '@site/src/components/Quiz';
import useProgress from '@site/src/lib/progress/useProgress';
import {nextReviewAfter} from '@site/src/lib/progress/leitner';
import {toLocalDate} from '@site/src/lib/progress/dates';
import {getQuestion} from '@site/src/data/quiz';
import {formatDate, formatWeekday} from './format';
import styles from './styles.module.css';

// Shows the given questions one after another. The id list is fixed by the caller.
export default function ReviewSession({questionIds, lessonPaths, onExit}) {
  const {i18n} = useDocusaurusContext();
  const {progress} = useProgress();
  const [index, setIndex] = useState(0);
  const [answered, setAnswered] = useState(false);
  const headingRef = useRef(null);

  // Move focus to the heading of the current step (also on start, the cards are gone).
  useEffect(() => {
    if (headingRef.current) headingRef.current.focus();
  }, [index]);

  const total = questionIds.length;

  if (index >= total) {
    const next = progress ? nextReviewAfter(progress, toLocalDate()) : null;
    return (
      <article className={clsx(styles.card, styles.cardAccent, styles.session)} aria-labelledby="review-done">
        <p className={styles.eyebrowAccent}>
          <Translate id="lernstand.session.ended">Wiederholung beendet</Translate>
        </p>
        <h3 id="review-done" tabIndex={-1} ref={headingRef} className={styles.titleLarge}>
          <Translate id="lernstand.session.done">Für heute geschafft.</Translate>
        </h3>
        <p className={styles.body}>
          {total === 1
            ? translate({id: 'lernstand.session.summary.one', message: '1 Frage wiederholt.'})
            : translate({id: 'lernstand.session.summary.other', message: '{count} Fragen wiederholt.'}, {count: total})}{' '}
          {next &&
            translate(
              {id: 'lernstand.session.next', message: 'Nächste Wiederholung: {weekday} {date}.'},
              {weekday: formatWeekday(next.date, i18n.currentLocale), date: formatDate(next.date)},
            )}
        </p>
        <div className={styles.actions}>
          <button type="button" className={styles.buttonSecondary} onClick={onExit}>
            <Translate id="lernstand.session.back">Zurück zur Übersicht</Translate>
          </button>
        </div>
      </article>
    );
  }

  const id = questionIds[index];
  const question = getQuestion(i18n.currentLocale, id);
  const lessonPath = question ? lessonPaths[question.lesson] : null;

  return (
    <article className={clsx(styles.card, styles.cardAccent, styles.session)} aria-labelledby="review-step">
      <div className={styles.sessionHead}>
        <h3 id="review-step" tabIndex={-1} ref={headingRef} className={styles.eyebrowAccent}>
          <Translate id="lernstand.session.step" values={{index: index + 1, total}}>
            {'Wiederholung · Frage {index} von {total}'}
          </Translate>
        </h3>
        <button type="button" className={styles.linkButton} onClick={onExit}>
          <Translate id="lernstand.session.cancel">Abbrechen</Translate>
        </button>
      </div>
      <div className={styles.track} aria-hidden="true">
        <div className={styles.trackFill} style={{width: `${((index + 1) / total) * 100}%`}} />
      </div>
      <Quiz key={id} id={id} onAnswered={() => setAnswered(true)} />
      <div className={styles.actionsBetween}>
        {lessonPath ? (
          <Link to={lessonPath} className={styles.textLink}>
            <Translate id="lernstand.session.toLesson">Zur Lektion</Translate>
          </Link>
        ) : (
          <span />
        )}
        <button
          type="button"
          className={styles.buttonPrimary}
          disabled={!answered}
          onClick={() => {
            setAnswered(false);
            setIndex(index + 1);
          }}>
          {index + 1 < total ? (
            <Translate id="lernstand.session.nextQuestion">Nächste Frage</Translate>
          ) : (
            <Translate id="lernstand.session.finish">Abschließen</Translate>
          )}
        </button>
      </div>
    </article>
  );
}
