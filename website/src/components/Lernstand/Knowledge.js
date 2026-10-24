import React from 'react';
import clsx from 'clsx';
import Translate, {translate} from '@docusaurus/Translate';
import {LEITNER_INTERVALS_DAYS} from '@site/src/lib/progress/config';
import {StageBar} from './LearningPath';
import styles from './styles.module.css';

export default function Knowledge({data}) {
  const {totals, totalQuestions, boxes} = data;
  const legend = [
    {key: 'shaky', label: translate({id: 'lernstand.stage.shaky', message: 'wackelig'}), desc: translate({id: 'lernstand.stage.shaky.desc', message: 'Box 1'})},
    {key: 'building', label: translate({id: 'lernstand.stage.building', message: 'im Aufbau'}), desc: translate({id: 'lernstand.stage.building.desc', message: 'Box 2–3'})},
    {key: 'solid', label: translate({id: 'lernstand.stage.solid', message: 'gefestigt'}), desc: translate({id: 'lernstand.stage.solid.desc', message: 'Box 4–5'})},
    {key: 'open', label: translate({id: 'lernstand.stage.open', message: 'noch offen'}), desc: translate({id: 'lernstand.stage.open.desc', message: 'nicht beantwortet'})},
  ];
  const aria = legend.map((g) => `${totals[g.key]} ${g.label}`).join(', ');

  return (
    <div className={styles.card}>
      <h3 className={styles.title}>
        <Translate id="lernstand.knowledge.title" values={{count: totalQuestions}}>
          {'Alle {count} Quizfragen'}
        </Translate>
      </h3>
      <div role="img" aria-label={aria}>
        <StageBar stages={totals} className={styles.bigBar} />
      </div>
      <div className={styles.knowledgeGrid}>
        <ul className={styles.legend}>
          {legend.map((g) => (
            <li key={g.key} className={styles.legendRow}>
              <span aria-hidden="true" className={clsx(styles.legendDot, styles[`stage_${g.key}`])} />
              <span className={styles.legendText}>
                <strong>{g.label}</strong> <span className={styles.mutedInline}>{g.desc}</span>
              </span>
              <span className={styles.number}>{totals[g.key]}</span>
            </li>
          ))}
        </ul>
        <div className={styles.stack10}>
          <p className={styles.label}>
            <Translate id="lernstand.knowledge.boxes">Die fünf Boxen</Translate>
          </p>
          <div className={styles.boxGrid}>
            {boxes.map((n, i) => {
              const days = LEITNER_INTERVALS_DAYS[i];
              return (
                <div key={i} className={styles.boxTile}>
                  <p className={styles.mutedXs}>
                    <Translate id="lernstand.knowledge.box" values={{box: i + 1}}>
                      {'Box {box}'}
                    </Translate>
                  </p>
                  <p className={styles.numberMedium}>{n}</p>
                  <p className={styles.mutedXs}>
                    {days === 1
                      ? translate({id: 'lernstand.knowledge.daily', message: 'täglich'})
                      : translate({id: 'lernstand.knowledge.every', message: 'alle {days} Tage'}, {days})}
                  </p>
                </div>
              );
            })}
          </div>
        </div>
      </div>
    </div>
  );
}
