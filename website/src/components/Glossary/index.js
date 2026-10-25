import React, {useMemo, useState} from 'react';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import Translate, {translate} from '@docusaurus/Translate';
import {getGlossary, sortKey} from '@site/src/data/glossary';
import {renderInline} from '@site/src/components/Quiz/QuizText';
import {useLessons} from '@site/src/lib/lessons/useLessons';
import styles from './styles.module.css';

function letterOf(term) {
  const first = sortKey(term).charAt(0).toUpperCase();
  return /^[A-Z]$/.test(first) ? first : '#';
}

const letterId = (letter) => (letter === '#' ? 'letter-other' : `letter-${letter.toLowerCase()}`);

export default function Glossary() {
  const {i18n} = useDocusaurusContext();
  const lessons = useLessons();
  const [query, setQuery] = useState('');
  const all = getGlossary(i18n.currentLocale);
  const byId = useMemo(() => new Map(all.map((t) => [t.id, t])), [all]);

  const visible = useMemo(() => {
    const q = query.trim().toLowerCase();
    if (!q) return all;
    return all.filter((t) => [t.term, t.german, ...(t.aliases ?? [])].some((s) => s && s.toLowerCase().includes(q)));
  }, [all, query]);

  const groups = useMemo(() => {
    const map = new Map();
    for (const t of visible) {
      const letter = letterOf(t);
      if (!map.has(letter)) map.set(letter, []);
      map.get(letter).push(t);
    }
    return [...map.entries()];
  }, [visible]);

  const countText =
    visible.length === 0
      ? translate({id: 'glossary.count.none', message: 'Kein Begriff gefunden.'})
      : translate({id: 'glossary.count', message: '{count} Begriffe'}, {count: visible.length});

  return (
    <>
      <p>
        <Translate id="glossary.intro">
          Das Glossar erklärt die Fachbegriffe des Kurses kurz. Fachbegriffe sind englisch und bekommen eine deutsche Erklärung.
        </Translate>
      </p>
      <div className={styles.search}>
        <label htmlFor="glossary-search">
          <Translate id="glossary.search.label">Begriff suchen</Translate>
        </label>
        <input id="glossary-search" type="search" value={query} onChange={(e) => setQuery(e.target.value)} />
      </div>
      <p className={styles.count} aria-live="polite">
        {countText}
      </p>
      {groups.length > 0 && (
        <nav aria-label={translate({id: 'glossary.letters.label', message: 'Buchstaben'})} className={styles.letters}>
          {groups.map(([letter]) => (
            <a key={letter} href={`#${letterId(letter)}`}>
              {letter}
            </a>
          ))}
        </nav>
      )}
      <div className={styles.list}>
        {groups.map(([letter, terms]) => (
          <section key={letter}>
            <h2 id={letterId(letter)}>{letter}</h2>
            <dl>
              {terms.map((t) => {
                const lesson = t.lesson ? lessons.find((l) => l.id === t.lesson) : null;
                const related = (t.related ?? []).map((id) => byId.get(id)).filter(Boolean);
                return (
                  <React.Fragment key={t.id}>
                    <dt id={t.id}>{t.german ? `${t.term} (${t.german})` : t.term}</dt>
                    <dd>
                      <p>{renderInline(t.definition)}</p>
                      {related.length > 0 && (
                        <p className={styles.meta}>
                          <Translate id="glossary.related">Siehe auch:</Translate>{' '}
                          {related.map((r, i) => (
                            <React.Fragment key={r.id}>
                              {i > 0 && ', '}
                              <a href={`#${r.id}`}>{r.term}</a>
                            </React.Fragment>
                          ))}
                        </p>
                      )}
                      {lesson && lesson.path && (
                        <p className={styles.meta}>
                          <Translate id="glossary.lesson">Eingeführt in:</Translate>{' '}
                          <Link to={lesson.path}>{lesson.title}</Link>
                        </p>
                      )}
                      {t.source && (
                        <p className={styles.meta}>
                          <Translate id="glossary.source">Quelle:</Translate>{' '}
                          <a href={t.source.url}>{t.source.title}</a>
                        </p>
                      )}
                    </dd>
                  </React.Fragment>
                );
              })}
            </dl>
          </section>
        ))}
      </div>
    </>
  );
}
