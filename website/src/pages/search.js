import React, {useEffect, useMemo, useState} from 'react';
import Layout from '@theme/Layout';
import Head from '@docusaurus/Head';
import Link from '@docusaurus/Link';
import {useHistory, useLocation} from '@docusaurus/router';
import useBaseUrl from '@docusaurus/useBaseUrl';
import Translate, {translate} from '@docusaurus/Translate';
import {MIN_QUERY_LENGTH, search} from '@site/src/lib/search/engine';
import {hitUrl, searchPageUrl} from '@site/src/lib/search/url';
import {useSearchIndex} from '@site/src/lib/search/useSearchIndex';
import Highlight from '@site/src/components/Search/Highlight';
import styles from './search.module.css';

export default function SearchPage() {
  const history = useHistory();
  const location = useLocation();
  const baseUrl = useBaseUrl('/');
  const urlQuery = new URLSearchParams(location.search).get('q') ?? '';
  const [query, setQuery] = useState(urlQuery);
  const {status, pages} = useSearchIndex({eager: true});

  // Keep the field in sync when the URL changes (e.g. a new search from the navbar).
  useEffect(() => setQuery(urlQuery), [urlQuery]);

  const trimmed = query.trim();
  const longEnough = trimmed.length >= MIN_QUERY_LENGTH;

  const results = useMemo(
    () => (status === 'ready' && longEnough ? search(pages, query) : []),
    [status, pages, query, longEnough],
  );
  const total = results.reduce((sum, r) => sum + r.hits.length, 0);

  const onSubmit = (event) => {
    event.preventDefault();
    history.replace(baseUrl.replace(/\/$/, '') + searchPageUrl(query));
  };

  let statusText;
  if (status === 'error') {
    statusText = translate({id: 'search.error', message: 'Die Suche ist gerade nicht verfügbar.'});
  } else if (!longEnough) {
    statusText = translate({id: 'search.page.minLength', message: 'Gib mindestens 2 Zeichen ein.'});
  } else if (status !== 'ready') {
    statusText = translate({id: 'search.loading', message: 'Suchindex wird geladen …'});
  } else if (total === 0) {
    statusText = translate({id: 'search.noHits', message: 'Keine Treffer für „{query}“.'}, {query: trimmed});
  } else {
    statusText = translate(
      {id: 'search.page.count', message: '{hits} Treffer auf {pages} Seiten'},
      {hits: total, pages: results.length},
    );
  }

  return (
    <Layout
      title={translate({id: 'search.page.title', message: 'Suche'})}
      description={translate({
        id: 'search.page.description',
        message: 'Alle Suchergebnisse im Kurs, nach Seiten gruppiert.',
      })}>
      <Head>
        <meta name="robots" content="noindex" />
      </Head>
      <main className="container margin-vert--lg">
        <h1>
          <Translate id="search.page.heading">Suche</Translate>
          {longEnough &&
            ' – ' +
              translate({id: 'search.page.headingQuery', message: 'Ergebnisse für „{query}“'}, {query: trimmed})}
        </h1>
        <form role="search" className={styles.form} onSubmit={onSubmit}>
          <label htmlFor="search-page-input" className={styles.label}>
            <Translate id="search.page.label">Suchbegriff</Translate>
          </label>
          <input
            id="search-page-input"
            type="search"
            className={styles.input}
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
          <button type="submit" className="button button--primary">
            <Translate id="search.page.submit">Suchen</Translate>
          </button>
        </form>
        <p className={styles.status} role="status" aria-live="polite">
          {statusText}
        </p>
        {results.map(({page, hits}, n) => {
          const headingId = `result-${n}`;
          return (
            <section key={page.id} aria-labelledby={headingId} className={styles.group}>
              <h2 id={headingId} className={styles.pageTitle}>
                <Link to={hitUrl(page, '')}>{page.title}</Link>
              </h2>
              {page.context && <p className={styles.context}>{page.context}</p>}
              <ul className={styles.hits}>
                {hits.map((hit) => (
                  <li key={hit.anchor || '_top'} className={styles.hit}>
                    <Link to={hitUrl(page, hit.anchor)} className={styles.hitLink}>
                      <span>
                        <Highlight text={hit.heading || page.title} ranges={hit.heading ? hit.headingRanges : []} />
                      </span>
                    </Link>
                    <p className={styles.snippet}>
                      <Highlight text={hit.snippet.text} ranges={hit.snippet.ranges} />
                    </p>
                  </li>
                ))}
              </ul>
            </section>
          );
        })}
      </main>
    </Layout>
  );
}
