import React from 'react';
import {translate} from '@docusaurus/Translate';
import {percent} from '@site/src/lib/progress/summary';
import styles from './styles.module.css';

function Tile({label, value, unit, bar, barClass, sub}) {
  return (
    <div className={styles.kpi}>
      <p className={styles.label}>{label}</p>
      <p className={styles.kpiValue}>
        {value}
        {unit && <span className={styles.kpiUnit}>{unit}</span>}
      </p>
      {bar !== undefined && (
        <div className={styles.track} aria-hidden="true">
          <div className={barClass} style={{width: `${bar}%`}} />
        </div>
      )}
      <p className={styles.mutedXs}>{sub}</p>
    </div>
  );
}

export default function Overview({data}) {
  const {lessons, totals, totalQuestions, last28, last14} = data;
  const done = lessons.filter((l) => l.status === 'done').length;
  const started = lessons.filter((l) => l.status === 'started').length;
  const answered = totals.shaky + totals.building + totals.solid;
  const rate = percent(last14.correct, last14.answered);

  let lessonSub;
  if (done === 0 && started === 0 && lessons.length > 0) {
    lessonSub = translate({id: 'lernstand.kpi.lessons.start', message: 'Starte mit „{title}“'}, {title: lessons[0].title});
  } else if (started > 0) {
    lessonSub = translate({id: 'lernstand.kpi.lessons.started', message: '{count} angefangen'}, {count: started});
  } else {
    lessonSub = translate({id: 'lernstand.kpi.lessons.allDone', message: 'Alle verfügbaren abgeschlossen'});
  }

  const of = (n) => translate({id: 'lernstand.kpi.of', message: 'von {total}'}, {total: n});

  return (
    <div className={styles.kpiGrid}>
      <Tile
        label={translate({id: 'lernstand.kpi.lessons', message: 'Lektionen abgeschlossen'})}
        value={done}
        unit={of(lessons.length)}
        bar={percent(done, lessons.length) ?? 0}
        barClass={styles.trackFill}
        sub={lessonSub}
      />
      <Tile
        label={translate({id: 'lernstand.kpi.solid', message: 'Fragen gefestigt'})}
        value={totals.solid}
        unit={of(totalQuestions)}
        bar={percent(totals.solid, totalQuestions) ?? 0}
        barClass={styles.trackFillSolid}
        sub={
          answered === 0
            ? translate({id: 'lernstand.kpi.solid.hint', message: 'Box 4–5 im Leitner-System'})
            : translate(
                {id: 'lernstand.kpi.solid.rest', message: '{building} im Aufbau · {shaky} wackelig'},
                {building: totals.building, shaky: totals.shaky},
              )
        }
      />
      <Tile
        label={translate({id: 'lernstand.kpi.days', message: 'Lerntage'})}
        value={last28.activeDays}
        unit={translate({id: 'lernstand.kpi.days.unit', message: 'von 28 Tagen'})}
        sub={translate({id: 'lernstand.kpi.days.sub', message: 'Tage mit mindestens einer Antwort'})}
      />
      <Tile
        label={translate({id: 'lernstand.kpi.rate', message: 'Trefferquote'})}
        value={rate === null ? '–' : rate}
        unit={rate === null ? '' : '%'}
        sub={
          rate === null
            ? translate({id: 'lernstand.kpi.rate.none', message: 'Noch keine Antworten in den letzten 14 Tagen'})
            : translate(
                {id: 'lernstand.kpi.rate.sub', message: '{correct} von {answered} Antworten richtig, letzte 14 Tage'},
                {correct: last14.correct, answered: last14.answered},
              )
        }
      />
    </div>
  );
}
