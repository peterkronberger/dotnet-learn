import React, {useState} from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import BrowserOnly from '@docusaurus/BrowserOnly';
import Translate, {translate} from '@docusaurus/Translate';
import useProgress from '@site/src/lib/progress/useProgress';
import {toLocalDate} from '@site/src/lib/progress/dates';
import {completeLesson, reopenLesson} from '@site/src/lib/progress/leitner';
import useLernstandData from '@site/src/components/Lernstand/useLernstandData';
import {formatDate} from '@site/src/components/Lernstand/format';
import base from '@site/src/components/Lernstand/styles.module.css';
import styles from './styles.module.css';

function Content({lessonId}) {
  const {progress, update} = useProgress();
  const [today] = useState(() => toLocalDate());
  const data = useLernstandData(progress, today);
  if (!progress || !data) return null;

  const entry = progress.lessons[lessonId];
  const next = data.next && data.next.id !== lessonId ? data.next : null;

  return (
    <div className={clsx(styles.box, entry && styles.done)}>
      {entry ? (
        <>
          <div className={styles.main}>
            <span aria-hidden="true" className={styles.check}>
              ✓
            </span>
            <div className={styles.text}>
              <p className={styles.titleDone}>
                {translate({id: 'lesson.complete.doneOn', message: 'Abgeschlossen am {date}'}, {date: formatDate(entry.completedAt)})}
              </p>
              {next && (
                <p>
                  <Translate id="lesson.complete.next">Als Nächstes im Lernstand:</Translate>{' '}
                  {next.path ? <Link to={next.path}>{next.title}</Link> : next.title}
                </p>
              )}
            </div>
          </div>
          <button type="button" className={base.linkButton} onClick={() => update((p) => reopenLesson(p, lessonId))}>
            <Translate id="lesson.complete.undo">Rückgängig</Translate>
          </button>
        </>
      ) : (
        <>
          <div className={styles.text}>
            <p className={styles.title}>
              <Translate id="lesson.complete.question">Lektion durchgearbeitet?</Translate>
            </p>
            <p>
              <Translate id="lesson.complete.hint">
                Markiere sie als abgeschlossen. Dein Lernstand schlägt dann die nächste Lektion vor.
              </Translate>
            </p>
          </div>
          <button
            type="button"
            className={base.buttonPrimary}
            onClick={() => update((p) => completeLesson(p, lessonId, toLocalDate()))}>
            <Translate id="lesson.complete.button">Lektion abschließen</Translate>
          </button>
        </>
      )}
    </div>
  );
}

// Shown at the end of every page with `lesson_id` (see src/theme/DocItem/Footer).
export default function LessonComplete({lessonId}) {
  return (
    <div role="status" aria-live="polite" className={styles.wrap}>
      <BrowserOnly>{() => <Content lessonId={lessonId} />}</BrowserOnly>
    </div>
  );
}
