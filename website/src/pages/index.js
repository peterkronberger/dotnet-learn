import React from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import Layout from '@theme/Layout';
import ProgressTeaser from '@site/src/components/ProgressTeaser';
import styles from './index.module.css';

const cycle = [
  {
    title: '1 · Vorhersagen',
    text: 'Bevor du Code ausführst, überlegst du, was passiert. Auch ein falscher Tipp bereitet das Lernen vor.',
  },
  {
    title: '2 · Ausführen',
    text: 'Der eingebaute Playground kompiliert und startet dein C# direkt im Browser – ohne Installation.',
  },
  {
    title: '3 · Erklären',
    text: 'Quizfragen geben nach jeder Antwort eine Erklärung, nicht nur „richtig“ oder „falsch“.',
  },
  {
    title: '4 · Wiederholen',
    text: 'Rückblick-Fragen holen Stoff früherer Kapitel zurück. Abrufen festigt mehr als erneutes Lesen.',
  },
];

function Hero() {
  const {siteConfig} = useDocusaurusContext();
  return (
    <header className={styles.hero}>
      <div className="container">
        <p className={styles.eyebrow}>C# · .NET · Grundlagen</p>
        <h1 className={styles.title}>{siteConfig.title}</h1>
        <p className={styles.tagline}>{siteConfig.tagline}</p>
        <div className={styles.buttons}>
          <Link className={clsx('button button--lg', styles.primary)} to="/docs/m00-orientation/l01-first-program">
            Loslegen
          </Link>
          <Link className={clsx('button button--lg', styles.secondary)} to="/playground">
            Playground öffnen
          </Link>
        </div>
      </div>
    </header>
  );
}

export default function Home() {
  const {siteConfig} = useDocusaurusContext();
  return (
    <Layout title={siteConfig.title} description="C# und .NET lernen mit Vorhersagen, Playground und Quizzes">
      <Hero />
      <main>
        <ProgressTeaser />
        <div className="container margin-vert--xl">
          <h2 className={styles.heading}>So ist der Lernpfad aufgebaut</h2>
          <p className={styles.lead}>
            Kurze Kapitel, ein Konzept nach dem anderen und aktives Üben statt passivem Lesen. Die
            Hintergründe stehen auf der Seite <Link to="/docs/learning-principles">Lernprinzipien</Link>.
          </p>
          <div className={styles.grid}>
            {cycle.map((step) => (
              <section key={step.title} className={styles.card}>
                <h3>{step.title}</h3>
                <p>{step.text}</p>
              </section>
            ))}
          </div>
        </div>
      </main>
    </Layout>
  );
}
