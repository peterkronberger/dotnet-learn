import React, {useEffect, useState} from 'react';
import Link from '@docusaurus/Link';
import BrowserOnly from '@docusaurus/BrowserOnly';
import Translate from '@docusaurus/Translate';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import {getQuestion} from '@site/src/data/quiz';
import useProgress from '@site/src/lib/progress/useProgress';
import {boxCounts, dueQuestionIds, nextDueDate} from '@site/src/lib/progress/leitner';
import {toLocalDate} from '@site/src/lib/progress/dates';
import {LEITNER_INTERVALS_DAYS} from '@site/src/lib/progress/config';
import ReviewSession from './ReviewSession';
import ProgressTransfer from './ProgressTransfer';
import {formatDate} from './format';
import styles from './styles.module.css';

function BoxOverview({progress}) {
  const counts = boxCounts(progress);
  return (
    <>
      <h3>
        <Translate id="dashboard.boxes.title">Deine Boxen</Translate>
      </h3>
      <ul className={styles.boxes}>
        {counts.map((n, i) => {
          const days = LEITNER_INTERVALS_DAYS[i];
          return (
            <li key={i}>
              {days === 1 ? (
                <Translate id="dashboard.boxes.daily" values={{box: i + 1, count: n}}>
                  {'Box {box} (täglich): {count}'}
                </Translate>
              ) : (
                <Translate id="dashboard.boxes.item" values={{box: i + 1, days, count: n}}>
                  {'Box {box} (alle {days} Tage): {count}'}
                </Translate>
              )}{' '}
              {n === 1 ? (
                <Translate id="dashboard.question.one">Frage</Translate>
              ) : (
                <Translate id="dashboard.question.other">Fragen</Translate>
              )}
            </li>
          );
        })}
      </ul>
    </>
  );
}

function DashboardContent() {
  const {i18n} = useDocusaurusContext();
  const {progress, replace, available} = useProgress();
  // The due list is computed once per visit so answered questions do not jump.
  const [dueIds, setDueIds] = useState(null);

  useEffect(() => {
    if (progress && dueIds === null) {
      setDueIds(dueQuestionIds(progress, toLocalDate()).filter((id) => getQuestion(i18n.currentLocale, id)));
    }
  }, [progress, dueIds, i18n.currentLocale]);

  if (progress === null) {
    return (
      <p>
        <Translate id="dashboard.loading">Lernstand wird geladen …</Translate>
      </p>
    );
  }

  // Questions without a data record (e.g. deleted ones) are skipped and not counted.
  const known = Object.fromEntries(
    Object.entries(progress.questions).filter(([id]) => getQuestion(i18n.currentLocale, id)),
  );
  const knownProgress = {...progress, questions: known};
  const knownCount = Object.keys(known).length;
  const next = nextDueDate(knownProgress);
  const due = dueIds ?? [];

  return (
    <>
      {!available && (
        <p className={styles.notice}>
          <Translate id="dashboard.noStorage">
            Dein Browser erlaubt keinen lokalen Speicher. Der Lernstand gilt nur für diesen Besuch.
          </Translate>
        </p>
      )}
      {knownCount === 0 ? (
        <>
          <p className={styles.status}>
            <Translate id="dashboard.empty">
              Noch keine Fragen beantwortet. Jede Quizfrage, die du in einer Lektion beantwortest, kommt hier zur Wiederholung.
            </Translate>
          </p>
          <p>
            <Link to="/docs/intro">
              <Translate id="dashboard.start">Mit der ersten Lektion beginnen</Translate>
            </Link>
          </p>
        </>
      ) : due.length === 0 ? (
        <p className={styles.status}>
          <Translate id="dashboard.nothingDue" values={{date: next ? formatDate(next) : '–'}}>
            {'Heute ist nichts fällig. Nächste Wiederholung: {date}.'}
          </Translate>
        </p>
      ) : (
        <>
          <p className={styles.status}>
            {due.length === 1 ? (
              <Translate id="dashboard.dueCount.one">Heute fällig: 1 Frage</Translate>
            ) : (
              <Translate id="dashboard.dueCount.other" values={{count: due.length}}>
                {'Heute fällig: {count} Fragen'}
              </Translate>
            )}
          </p>
          <ReviewSession questionIds={due} />
        </>
      )}
      {knownCount > 0 && <BoxOverview progress={knownProgress} />}
      <ProgressTransfer progress={progress} replace={replace} />
    </>
  );
}

export default function DueDashboard() {
  return (
    <section className={styles.dashboard} aria-labelledby="due-dashboard-title">
      <h2 id="due-dashboard-title">
        <Translate id="dashboard.title">Heute fällig</Translate>
      </h2>
      <BrowserOnly
        fallback={
          <p>
            <Translate id="dashboard.loading">Lernstand wird geladen …</Translate>
          </p>
        }>
        {() => <DashboardContent />}
      </BrowserOnly>
    </section>
  );
}
