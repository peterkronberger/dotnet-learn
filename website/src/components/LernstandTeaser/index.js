import React, {useState} from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import BrowserOnly from '@docusaurus/BrowserOnly';
import Translate, {translate} from '@docusaurus/Translate';
import useProgress from '@site/src/lib/progress/useProgress';
import {toLocalDate} from '@site/src/lib/progress/dates';
import useLernstandData from '@site/src/components/Lernstand/useLernstandData';
import base from '@site/src/components/Lernstand/styles.module.css';
import styles from './styles.module.css';

// "Modul 1 – Erste Programme, …" -> "Modul 1"
function shortModule(label) {
  return label.split(' – ')[0];
}

function TeaserContent() {
  const {progress} = useProgress();
  const [today] = useState(() => toLocalDate());
  const data = useLernstandData(progress, today);
  if (!data || data.lessons.length === 0) return <Placeholder />;

  const {next, lessons, due} = data;
  const done = lessons.filter((l) => l.status === 'done').length;
  const firstVisit = !data.answeredAny && done === 0;

  const chips = [];
  if (next && next.module.label) chips.push({text: firstVisit ? next.module.label : shortModule(next.module.label)});
  if (next && next.durationMin) {
    chips.push({text: translate({id: 'lernstand.duration', message: '{min} min'}, {min: next.durationMin})});
  }
  if (firstVisit && next && next.total > 0) {
    chips.push({
      text:
        next.total === 1
          ? translate({id: 'lernstand.count.question.one', message: '1 Quizfrage'})
          : translate({id: 'lernstand.count.question.other', message: '{count} Quizfragen'}, {count: next.total}),
    });
  }
  if (due.length > 0) {
    chips.push({
      accent: true,
      text:
        due.length === 1
          ? translate({id: 'lernstand.teaser.due.one', message: '1 Wiederholung fällig'})
          : translate({id: 'lernstand.teaser.due.other', message: '{count} Wiederholungen fällig'}, {count: due.length}),
    });
  }

  let eyebrow;
  let title;
  if (firstVisit) {
    eyebrow = translate({id: 'lernstand.teaser.new', message: 'Neu hier?'});
    title = translate({id: 'lernstand.teaser.first', message: 'Starte mit Lektion 1: {title}'}, {title: next.title});
  } else {
    eyebrow = translate({id: 'lernstand.teaser.eyebrow', message: 'Dein Lernstand'});
    title = next
      ? translate({id: 'lernstand.teaser.continue', message: 'Weiter mit: {title}'}, {title: next.title})
      : translate({id: 'lernstand.next.allDone', message: 'Alle verfügbaren Lektionen abgeschlossen'});
  }

  return (
    <section className={clsx(base.card, base.cardAccent, styles.teaser)} aria-labelledby="teaser-title">
      <div className={styles.text}>
        <p className={base.eyebrowAccent}>{eyebrow}</p>
        <h2 id="teaser-title" className={styles.title}>
          {title}
        </h2>
        {chips.length > 0 && (
          <div className={styles.chips}>
            {chips.map((c) => (
              <span key={c.text} className={c.accent ? styles.chipAccent : styles.chip}>
                {c.text}
              </span>
            ))}
          </div>
        )}
        {firstVisit ? (
          <p className={base.mutedSmall}>
            <Translate id="lernstand.teaser.local">Dein Fortschritt bleibt in diesem Browser, ohne Konto.</Translate>
          </p>
        ) : (
          <div className={styles.progress}>
            <div className={clsx(base.track, styles.progressTrack)} aria-hidden="true">
              <div className={base.trackFill} style={{width: `${(done / lessons.length) * 100}%`}} />
            </div>
            <span className={base.mutedSmall}>
              {translate(
                {id: 'lernstand.path.summary', message: '{done} von {total} Lektionen abgeschlossen'},
                {done, total: lessons.length},
              )}
            </span>
          </div>
        )}
      </div>
      <div className={styles.buttons}>
        {next && next.path && (
          <Link to={next.path} className={base.buttonPrimary}>
            {firstVisit || next.status === 'open' ? (
              <Translate id="lernstand.next.start">Lektion starten</Translate>
            ) : (
              <Translate id="lernstand.next.continue">Weiterlernen</Translate>
            )}{' '}
            <span aria-hidden="true">→</span>
          </Link>
        )}
        <Link to="/dashboard" className={next ? base.buttonSecondary : base.buttonPrimary}>
          <Translate id="lernstand.teaser.toDashboard">Zum Lernstand</Translate>
        </Link>
      </div>
    </section>
  );
}

// Reserves the space while the stored progress is read in the browser.
function Placeholder() {
  return <div className={clsx(base.card, base.cardAccent, styles.teaser, styles.placeholder)} aria-hidden="true" />;
}

export default function LernstandTeaser() {
  return (
    <div className={clsx('container', styles.wrap)}>
      <BrowserOnly fallback={<Placeholder />}>{() => <TeaserContent />}</BrowserOnly>
    </div>
  );
}
