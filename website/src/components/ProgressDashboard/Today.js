import React, {useState} from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import Translate, {translate} from '@docusaurus/Translate';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import InlineCode from './InlineCode';
import ReviewSession from './ReviewSession';
import {formatDate, formatWeekday} from './format';
import styles from './styles.module.css';

function questionCount(n) {
  return n === 1
    ? translate({id: 'lernstand.count.question.one', message: '1 Quizfrage'})
    : translate({id: 'lernstand.count.question.other', message: '{count} Quizfragen'}, {count: n});
}

function NextLessonCard({data, className}) {
  const {next, lessons} = data;
  if (!next) {
    if (lessons.length === 0) return null;
    return (
      <article className={className} aria-labelledby="next-title">
        <p className={styles.eyebrowAccent}>
          <Translate id="lernstand.next.eyebrow">Als Nächstes</Translate>
        </p>
        <h3 id="next-title" className={styles.titleLarge}>
          <Translate id="lernstand.next.allDone">Alle verfügbaren Lektionen abgeschlossen</Translate>
        </h3>
        <p className={styles.body}>
          <Translate id="lernstand.next.allDone.text">
            Neue Module folgen nach und nach. Sie erscheinen hier automatisch, sobald sie online sind.
          </Translate>
        </p>
        <div className={styles.actions}>
          <a href="#lernpfad" className={styles.buttonSecondary}>
            <Translate id="lernstand.next.toPath">Lernpfad ansehen</Translate>
          </a>
        </div>
      </article>
    );
  }

  const nothingYet = lessons.every((l) => l.status === 'open');
  let eyebrow = translate({id: 'lernstand.next.eyebrow', message: 'Als Nächstes'});
  if (next.status === 'started') {
    eyebrow = translate({id: 'lernstand.next.eyebrow.started', message: 'Als Nächstes · angefangen'});
  } else if (nothingYet) {
    eyebrow = translate({id: 'lernstand.next.eyebrow.first', message: 'Als Nächstes · dein Einstieg'});
  }
  const meta = [];
  if (next.durationMin) {
    meta.push(translate({id: 'lernstand.duration', message: '{min} min'}, {min: next.durationMin}));
  }
  if (next.status === 'started') {
    meta.push(
      translate(
        {id: 'lernstand.next.answered', message: '{answered} von {total} Quizfragen beantwortet'},
        {answered: next.answered, total: next.total},
      ),
    );
  } else if (next.total > 0) {
    meta.push(questionCount(next.total));
  }

  return (
    <article className={className} aria-labelledby="next-title">
      <div className={styles.stack6}>
        <p className={styles.eyebrowAccent}>{eyebrow}</p>
        {next.module.label && <p className={styles.mutedSmall}>{next.module.label}</p>}
        <h3 id="next-title" className={styles.titleLarge}>
          {next.title}
        </h3>
        {meta.length > 0 && <p className={styles.meta}>{meta.join(' · ')}</p>}
      </div>
      {next.goals.length > 0 && (
        <div className={styles.stack10}>
          <p className={styles.label}>
            <Translate id="lernstand.next.goals">Das lernst du</Translate>
          </p>
          <ul className={styles.goals}>
            {next.goals.map((goal) => (
              <li key={goal} className={styles.goal}>
                <span aria-hidden="true" className={styles.goalMark}>
                  ✓
                </span>
                <span>
                  <InlineCode text={goal} />
                </span>
              </li>
            ))}
          </ul>
        </div>
      )}
      {next.path && (
        <div className={styles.actions}>
          <Link to={next.path} className={styles.buttonPrimary}>
            {next.status === 'started' ? (
              <Translate id="lernstand.next.continue">Weiterlernen</Translate>
            ) : (
              <Translate id="lernstand.next.start">Lektion starten</Translate>
            )}{' '}
            <span aria-hidden="true">→</span>
          </Link>
        </div>
      )}
    </article>
  );
}

function quotedList(titles) {
  const quoted = titles.map((t) => `„${t}“`);
  if (quoted.length <= 1) return quoted.join('');
  return translate(
    {id: 'lernstand.list.and', message: '{first} und {last}'},
    {first: quoted.slice(0, -1).join(', '), last: quoted[quoted.length - 1]},
  );
}

function ReviewCard({data, onStart, className}) {
  const {i18n} = useDocusaurusContext();
  const {due, dueLessons, nextReview, answeredAny, next, lessons} = data;
  const count = due.length;
  const allDone = lessons.length > 0 && !next;

  let text;
  if (count > 0) {
    text = translate({id: 'lernstand.review.from', message: 'Aus {lessons}.'}, {lessons: quotedList(dueLessons.map((l) => l.title))});
    if (allDone) {
      text += ` ${translate({
        id: 'lernstand.review.main',
        message: 'Alle Lektionen sind abgeschlossen, die Wiederholung ist jetzt deine Hauptaufgabe.',
      })}`;
    }
  } else if (answeredAny) {
    text = translate({id: 'lernstand.review.nothingDue', message: 'Heute ist nichts fällig.'});
  } else {
    text = translate({
      id: 'lernstand.review.empty',
      message: 'Noch nichts zu wiederholen. Jede Quizfrage, die du in einer Lektion beantwortest, kommt hier in die Wiederholung.',
    });
  }

  let after = null;
  if (nextReview) {
    const values = {
      weekday: formatWeekday(nextReview.date, i18n.currentLocale),
      date: formatDate(nextReview.date),
      questions:
        nextReview.count === 1
          ? translate({id: 'lernstand.count.q.one', message: '1 Frage'})
          : translate({id: 'lernstand.count.q.other', message: '{count} Fragen'}, {count: nextReview.count}),
    };
    after =
      count > 0
        ? translate({id: 'lernstand.review.after', message: 'Danach: {weekday} {date} · {questions}'}, values)
        : translate({id: 'lernstand.review.nextOn', message: 'Nächste Wiederholung: {weekday} {date} · {questions}'}, values);
  }

  return (
    <article className={className} aria-labelledby="review-title">
      <p className={styles.eyebrowAccent}>
        <Translate id="lernstand.review.eyebrow">Wiederholen</Translate>
      </p>
      <div className={styles.bigNumberRow}>
        <span className={styles.bigNumber}>{count}</span>
        <h3 id="review-title" className={styles.bigNumberLabel}>
          {count === 1 ? (
            <Translate id="lernstand.review.due.one">Frage fällig</Translate>
          ) : (
            <Translate id="lernstand.review.due.other">Fragen fällig</Translate>
          )}
        </h3>
      </div>
      <p className={styles.body}>{text}</p>
      {count > 0 && (
        <div className={styles.actions}>
          <button type="button" className={styles.buttonPrimary} onClick={onStart}>
            <Translate id="lernstand.review.start">Wiederholung starten</Translate>
          </button>
        </div>
      )}
      {after && <p className={styles.mutedSmall}>{after}</p>}
    </article>
  );
}

export default function Today({data}) {
  // The due list is fixed when a session starts, so answered questions do not jump.
  const [session, setSession] = useState(null);

  if (session) {
    return <ReviewSession questionIds={session} lessonPaths={data.lessonPaths} onExit={() => setSession(null)} />;
  }

  const reviewFirst = !data.next && data.lessons.length > 0;
  const accent = clsx(styles.card, styles.cardAccent);
  return (
    <div className={clsx(styles.todayGrid, reviewFirst && styles.todayGridReviewFirst)}>
      <NextLessonCard data={data} className={reviewFirst ? styles.card : accent} />
      <ReviewCard data={data} onStart={() => setSession(data.due)} className={reviewFirst ? accent : styles.card} />
    </div>
  );
}
