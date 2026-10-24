import React from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import {translate} from '@docusaurus/Translate';
import {formatDate} from './format';
import styles from './styles.module.css';

// Consecutive lessons with the same module label form one group.
function groupByModule(lessons) {
  const groups = [];
  for (const lesson of lessons) {
    const last = groups[groups.length - 1];
    if (last && last.label === lesson.module.label) last.lessons.push(lesson);
    else groups.push({label: lesson.module.label, lessons: [lesson]});
  }
  return groups;
}

function statusText(lesson) {
  let text;
  if (lesson.status === 'done') {
    text = translate({id: 'lernstand.path.done', message: 'Abgeschlossen am {date}'}, {date: formatDate(lesson.completedAt)});
  } else if (lesson.status === 'started') {
    text = translate({id: 'lernstand.path.started', message: 'Angefangen · zuletzt am {date}'}, {date: formatDate(lesson.lastActivity)});
  } else {
    text = translate({id: 'lernstand.path.open', message: 'Offen'});
  }
  if (lesson.durationMin) {
    text += ` · ${translate({id: 'lernstand.duration', message: '{min} min'}, {min: lesson.durationMin})}`;
  }
  return text;
}

function barText(lesson) {
  if (lesson.total === 0) return translate({id: 'lernstand.path.noQuestions', message: 'Keine Quizfragen'});
  if (lesson.answered === 0) {
    return lesson.total === 1
      ? translate({id: 'lernstand.path.unanswered.one', message: '1 Quizfrage, noch nicht beantwortet'})
      : translate({id: 'lernstand.path.unanswered.other', message: '{count} Quizfragen, noch nicht beantwortet'}, {count: lesson.total});
  }
  return translate({id: 'lernstand.path.solid', message: '{solid} von {total} gefestigt'}, {solid: lesson.stages.solid, total: lesson.total});
}

const STATUS_ICON = {done: styles.iconDone, started: styles.iconStarted, open: styles.iconOpen};

export function StageBar({stages, className}) {
  return (
    <div className={className} aria-hidden="true">
      {['shaky', 'building', 'solid', 'open'].map((key) =>
        stages[key] > 0 ? <span key={key} className={styles[`stage_${key}`]} style={{flexGrow: stages[key]}} /> : null,
      )}
    </div>
  );
}

export default function LearningPath({data}) {
  return (
    <div className={styles.moduleList}>
      {groupByModule(data.lessons).map((group) => {
        const done = group.lessons.filter((l) => l.status === 'done').length;
        return (
          <div key={group.label || 'ohne-modul'} className={clsx(styles.card, styles.moduleCard)}>
            <div className={styles.moduleHead}>
              <h3 className={styles.title}>{group.label}</h3>
              <span className={styles.mutedSmall}>
                {translate(
                  {id: 'lernstand.path.moduleDone', message: '{done} von {total} abgeschlossen'},
                  {done, total: group.lessons.length},
                )}
              </span>
            </div>
            <ul className={styles.lessonList}>
              {group.lessons.map((lesson) => (
                <li key={lesson.id} className={styles.lessonRow}>
                  <div className={styles.lessonMain}>
                    <span aria-hidden="true" className={STATUS_ICON[lesson.status]}>
                      {lesson.status === 'done' ? '✓' : ''}
                    </span>
                    <div className={styles.stack2}>
                      {lesson.path ? (
                        <Link to={lesson.path} className={styles.lessonTitle}>
                          {lesson.title}
                        </Link>
                      ) : (
                        <span className={styles.lessonTitle}>{lesson.title}</span>
                      )}
                      <p className={styles.mutedSmall}>{statusText(lesson)}</p>
                    </div>
                  </div>
                  <div className={styles.lessonBar}>
                    <StageBar stages={lesson.stages} className={styles.miniBar} />
                    <p className={styles.mutedXs}>{barText(lesson)}</p>
                  </div>
                </li>
              ))}
            </ul>
          </div>
        );
      })}
    </div>
  );
}
