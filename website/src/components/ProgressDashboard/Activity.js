import React, {useState} from 'react';
import clsx from 'clsx';
import Translate, {translate} from '@docusaurus/Translate';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import {activityBuckets, hasActivity, percent, weekdayIndex} from '@site/src/lib/progress/summary';
import {formatMonth, formatShortDate, formatWeekday} from './format';
import styles from './styles.module.css';

const RANGES = [
  {id: '4w', label: () => translate({id: 'lernstand.activity.range.4w', message: '4 Wochen'})},
  {id: '3m', label: () => translate({id: 'lernstand.activity.range.3m', message: '3 Monate'})},
  {id: 'all', label: () => translate({id: 'lernstand.activity.range.all', message: 'Gesamt'})},
];

function labels(bucket, index, locale) {
  if (bucket.kind === 'day') {
    // Axis label on Mondays only; the table has every day.
    const monday = weekdayIndex(bucket.start) === 0;
    return {
      axis: monday ? formatShortDate(bucket.start) : '',
      row: `${formatWeekday(bucket.start, locale)} ${formatShortDate(bucket.start)}`,
    };
  }
  if (bucket.kind === 'week') {
    const week = translate({id: 'lernstand.activity.week', message: 'KW {week}'}, {week: bucket.week});
    return {
      axis: index % 2 === 0 ? week : '',
      row: translate(
        {id: 'lernstand.activity.weekRow', message: '{week} (ab {date})'},
        {week, date: formatShortDate(bucket.start)},
      ),
    };
  }
  const month = formatMonth(bucket.start, locale);
  return {axis: month, row: month};
}

function niceMax(n) {
  return Math.max(4, Math.ceil(n / 2) * 2);
}

export function RangeSwitch({range, onChange}) {
  return (
    <div role="group" aria-label={translate({id: 'lernstand.activity.range', message: 'Zeitraum'})} className={styles.segmented}>
      {RANGES.map((r) => (
        <button
          key={r.id}
          type="button"
          aria-pressed={range === r.id}
          className={clsx(styles.segment, range === r.id && styles.segmentOn)}
          onClick={() => onChange(r.id)}>
          {r.label()}
        </button>
      ))}
    </div>
  );
}

export default function Activity({progress, today, range}) {
  const {i18n} = useDocusaurusContext();
  if (!hasActivity(progress)) {
    return (
      <div className={styles.card}>
        <div className={styles.empty}>
          <p className={styles.emptyTitle}>
            <Translate id="lernstand.activity.empty.title">Noch keine Aktivität</Translate>
          </p>
          <p>
            <Translate id="lernstand.activity.empty.text">
              Sobald du Quizfragen beantwortest, siehst du hier, wie viel und wie sicher du lernst.
            </Translate>
          </p>
        </div>
      </div>
    );
  }

  const buckets = activityBuckets(progress, today, range).map((b, i) => ({...b, ...labels(b, i, i18n.currentLocale)}));
  const max = niceMax(Math.max(...buckets.map((b) => b.answered)));
  const total = buckets.reduce(
    (s, b) => ({answered: s.answered + b.answered, correct: s.correct + b.correct, partial: s.partial + b.partial, wrong: s.wrong + b.wrong}),
    {answered: 0, correct: 0, partial: 0, wrong: 0},
  );
  const rate = percent(total.correct, total.answered);
  const n = buckets.length;
  const points = buckets
    .map((b, i) => (b.answered > 0 ? `${(((i + 0.5) / n) * 100).toFixed(2)},${(100 - (b.correct / b.answered) * 100).toFixed(2)}` : null))
    .filter(Boolean)
    .join(' ');
  const height = (v) => `${((v / max) * 100).toFixed(2)}%`;
  const density = n > 20 ? styles.colsDense : n > 6 ? styles.colsMedium : styles.colsWide;
  const unit = {
    '4w': translate({id: 'lernstand.activity.unit.day', message: 'Tag'}),
    '3m': translate({id: 'lernstand.activity.unit.week', message: 'Woche'}),
    all: translate({id: 'lernstand.activity.unit.month', message: 'Monat'}),
  }[range];
  const stats = [
    [translate({id: 'lernstand.activity.answers', message: 'Antworten'}), total.answered],
    [translate({id: 'lernstand.activity.correct', message: 'richtig'}), total.correct],
    [translate({id: 'lernstand.activity.partial', message: 'teilweise'}), total.partial],
    [translate({id: 'lernstand.activity.wrong', message: 'falsch'}), total.wrong],
    [translate({id: 'lernstand.activity.rate', message: 'Trefferquote'}), rate === null ? '–' : `${rate} %`],
  ];

  return (
    <div className={styles.card}>
      <dl className={styles.stats}>
        {stats.map(([label, value]) => (
          <div key={label} className={styles.stack2}>
            <dt className={styles.mutedXs}>{label}</dt>
            <dd className={styles.numberMedium}>{value}</dd>
          </div>
        ))}
      </dl>

      {/* The chart repeats the table below; screen readers use the table. */}
      <div className={styles.chart} aria-hidden="true">
        <div className={styles.axisLeft}>
          <span>{max}</span>
          <span>{max / 2}</span>
          <span>0</span>
        </div>
        <div className={styles.plot}>
          <div className={styles.gridTop} />
          <div className={styles.gridMid} />
          <div className={clsx(styles.cols, density)}>
            {buckets.map((b) => (
              <div key={b.start} className={styles.col}>
                {b.wrong > 0 && <div className={styles.stage_shaky} style={{height: height(b.wrong)}} />}
                {b.partial > 0 && <div className={styles.stage_building} style={{height: height(b.partial)}} />}
                {b.correct > 0 && <div className={styles.stage_solid} style={{height: height(b.correct)}} />}
              </div>
            ))}
          </div>
          <svg className={styles.line} viewBox="0 0 100 100" preserveAspectRatio="none">
            <polyline points={points} vectorEffect="non-scaling-stroke" />
          </svg>
        </div>
        <div className={styles.axisRight}>
          <span>100 %</span>
          <span>50 %</span>
          <span>0 %</span>
        </div>
        <div />
        <div className={clsx(styles.xLabels, density)}>
          {buckets.map((b) => (
            <span key={b.start} className={styles.xLabel}>
              {b.axis}
            </span>
          ))}
        </div>
        <div />
      </div>

      <div className={styles.chartLegend} aria-hidden="true">
        <span>
          <span className={clsx(styles.swatch, styles.stage_solid)} />
          <Translate id="lernstand.activity.correct">richtig</Translate>
        </span>
        <span>
          <span className={clsx(styles.swatch, styles.stage_building)} />
          <Translate id="lernstand.activity.partial">teilweise</Translate>
        </span>
        <span>
          <span className={clsx(styles.swatch, styles.stage_shaky)} />
          <Translate id="lernstand.activity.wrong">falsch</Translate>
        </span>
        <span>
          <span className={styles.swatchLine} />
          <Translate id="lernstand.activity.rateAxis">Trefferquote (rechte Achse)</Translate>
        </span>
      </div>

      <details className={styles.details}>
        <summary className={styles.summary}>
          <Translate id="lernstand.activity.table">Werte als Tabelle</Translate>
        </summary>
        <div className={styles.tableWrap}>
          <table className={styles.table}>
            <caption className={styles.srOnly}>
              <Translate id="lernstand.activity.caption">Aktivität im gewählten Zeitraum</Translate>
            </caption>
            <thead>
              <tr>
                <th scope="col">{unit}</th>
                {stats.map(([label]) => (
                  <th key={label} scope="col" className={styles.num}>
                    {label}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {buckets.map((b) => {
                const r = percent(b.correct, b.answered);
                return (
                  <tr key={b.start}>
                    <th scope="row">{b.row}</th>
                    <td className={styles.num}>{b.answered}</td>
                    <td className={styles.num}>{b.correct}</td>
                    <td className={styles.num}>{b.partial}</td>
                    <td className={styles.num}>{b.wrong}</td>
                    <td className={styles.num}>{r === null ? '–' : `${r} %`}</td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </details>
    </div>
  );
}
