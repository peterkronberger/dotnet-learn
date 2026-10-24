import React, {useState} from 'react';
import BrowserOnly from '@docusaurus/BrowserOnly';
import Translate, {translate} from '@docusaurus/Translate';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import useProgress from '@site/src/lib/progress/useProgress';
import {toLocalDate} from '@site/src/lib/progress/dates';
import useLernstandData from './useLernstandData';
import Today from './Today';
import Overview from './Overview';
import LearningPath from './LearningPath';
import Knowledge from './Knowledge';
import Activity, {RangeSwitch} from './Activity';
import ProgressTransfer from './ProgressTransfer';
import {formatDate} from './format';
import styles from './styles.module.css';

function Section({id, title, aside, children}) {
  return (
    <section id={id} aria-labelledby={`${id}-title`} className={styles.section}>
      <div className={styles.sectionHead}>
        <h2 id={`${id}-title`} className={styles.sectionTitle}>
          {title}
        </h2>
        {aside}
      </div>
      {children}
    </section>
  );
}

function Loading() {
  return (
    <p className={styles.body}>
      <Translate id="dashboard.loading">Lernstand wird geladen …</Translate>
    </p>
  );
}

function Content() {
  const {i18n} = useDocusaurusContext();
  const {progress, replace, available} = useProgress();
  const [today] = useState(() => toLocalDate());
  const [range, setRange] = useState('4w');
  const data = useLernstandData(progress, today);

  if (!data) return <Loading />;

  const nothingYet = !data.answeredAny && data.lessons.every((l) => l.status === 'open');
  const weekday = new Intl.DateTimeFormat(i18n.currentLocale, {weekday: 'long'}).format(new Date());
  const doneLessons = data.lessons.filter((l) => l.status === 'done').length;

  return (
    <>
      <div className={styles.head}>
        <p className={styles.label}>
          {translate(
            {id: 'lernstand.head.date', message: '{weekday}, {date} · gespeichert nur in diesem Browser'},
            {weekday, date: formatDate(today)},
          )}
        </p>
        <h1 className={styles.pageTitle}>
          <Translate id="lernstand.title">Lernstand</Translate>
        </h1>
        <p className={styles.lead}>
          {nothingYet
            ? translate({
                id: 'lernstand.lead.empty',
                message: 'Hier siehst du, was heute ansteht und wie weit du bist. Noch ist alles leer – los geht es mit der ersten Lektion.',
              })
            : translate({
                id: 'lernstand.lead',
                message: 'Was heute ansteht, wie weit du im Lernpfad bist und wie sicher der Stoff sitzt.',
              })}
        </p>
        {!available && (
          <p className={styles.notice}>
            <Translate id="dashboard.noStorage">
              Dein Browser erlaubt keinen lokalen Speicher. Der Lernstand gilt nur für diesen Besuch.
            </Translate>
          </p>
        )}
      </div>

      <Section id="heute" title={translate({id: 'lernstand.section.today', message: 'Heute'})}>
        <Today data={data} />
      </Section>

      <Section id="ueberblick" title={translate({id: 'lernstand.section.overview', message: 'Überblick'})}>
        <Overview data={data} />
      </Section>

      <Section
        id="lernpfad"
        title={translate({id: 'lernstand.section.path', message: 'Lernpfad'})}
        aside={
          <span className={styles.chip}>
            {translate(
              {id: 'lernstand.path.summary', message: '{done} von {total} Lektionen abgeschlossen'},
              {done: doneLessons, total: data.lessons.length},
            )}
          </span>
        }>
        <LearningPath data={data} />
      </Section>

      <Section id="wissensstand" title={translate({id: 'lernstand.section.knowledge', message: 'Wissensstand'})}>
        <Knowledge data={data} />
      </Section>

      <Section
        id="aktivitaet"
        title={translate({id: 'lernstand.section.activity', message: 'Aktivität'})}
        aside={<RangeSwitch range={range} onChange={setRange} />}>
        <Activity progress={progress} today={today} range={range} />
      </Section>

      <Section id="daten" title={translate({id: 'lernstand.section.data', message: 'Deine Daten'})}>
        <ProgressTransfer progress={progress} replace={replace} />
      </Section>
    </>
  );
}

export default function Lernstand() {
  return (
    <div className={styles.page}>
      <div className={styles.container}>
        <BrowserOnly fallback={<Loading />}>{() => <Content />}</BrowserOnly>
      </div>
    </div>
  );
}
