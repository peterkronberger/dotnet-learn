import React, {useCallback, useEffect, useId, useMemo, useRef, useState} from 'react';
import {useHistory} from '@docusaurus/router';
import useBaseUrl from '@docusaurus/useBaseUrl';
import Translate, {translate} from '@docusaurus/Translate';
import {MIN_QUERY_LENGTH, search, topHits} from '@site/src/lib/search/engine';
import {hitUrl, searchPageUrl} from '@site/src/lib/search/url';
import {useSearchIndex} from '@site/src/lib/search/useSearchIndex';
import Highlight from '@site/src/components/Search/Highlight';
import styles from './styles.module.css';

const MAX_HITS = 3;

export default function SearchBar() {
  const history = useHistory();
  const baseUrl = useBaseUrl('/');
  const uid = useId().replace(/:/g, '');
  const listId = `search-list-${uid}`;
  const inputId = `search-input-${uid}`;
  const formRef = useRef(null);
  const inputRef = useRef(null);
  const {status, pages, load} = useSearchIndex();
  const [query, setQuery] = useState('');
  const [focused, setFocused] = useState(false);
  const [dismissed, setDismissed] = useState(false);
  const [active, setActive] = useState(-1);

  const longEnough = query.trim().length >= MIN_QUERY_LENGTH;
  const open = focused && !dismissed && longEnough;

  const {groups, total} = useMemo(() => {
    if (status !== 'ready' || !longEnough) return {groups: [], total: 0};
    const results = search(pages, query);
    return {
      groups: topHits(results, MAX_HITS),
      total: results.reduce((sum, r) => sum + r.hits.length, 0),
    };
  }, [status, pages, query, longEnough]);

  // Flat list of options: the hits in display order, then "all results".
  const options = useMemo(() => {
    const list = [];
    for (const group of groups) {
      for (const hit of group.hits) list.push({url: hitUrl(group.page, hit.anchor), id: `${listId}-o${list.length}`});
    }
    if (groups.length > 0) list.push({url: searchPageUrl(query), id: `${listId}-o${list.length}`});
    return list;
  }, [groups, query, listId]);

  useEffect(() => setActive(-1), [query]);

  useEffect(() => {
    if (!open) return undefined;
    const onMouseDown = (event) => {
      if (formRef.current && !formRef.current.contains(event.target)) setDismissed(true);
    };
    document.addEventListener('mousedown', onMouseDown);
    return () => document.removeEventListener('mousedown', onMouseDown);
  }, [open]);

  // `url` is relative to the site root; the router expects the baseUrl in front.
  const go = useCallback(
    (url) => {
      setDismissed(true);
      setActive(-1);
      inputRef.current?.blur();
      history.push(baseUrl.replace(/\/$/, '') + url);
    },
    [history, baseUrl],
  );

  const onSubmit = (event) => {
    event.preventDefault();
    if (!longEnough) return;
    go(active >= 0 && options[active] ? options[active].url : searchPageUrl(query));
  };

  const onKeyDown = (event) => {
    if (event.key === 'ArrowDown' || event.key === 'ArrowUp') {
      if (!open || options.length === 0) return;
      event.preventDefault();
      const step = event.key === 'ArrowDown' ? 1 : -1;
      setActive((a) => (a === -1 ? (step === 1 ? 0 : options.length - 1) : (a + step + options.length) % options.length));
    } else if (event.key === 'Escape') {
      if (open) {
        setDismissed(true);
        setActive(-1);
      } else {
        setQuery('');
      }
    }
  };

  let liveText = '';
  if (open && status === 'ready') {
    liveText =
      total === 0
        ? translate({id: 'search.live.none', message: 'Keine Treffer'})
        : translate({id: 'search.live.count', message: '{count} Treffer'}, {count: total});
  }

  const showList = open && status === 'ready' && groups.length > 0;
  const lastIndex = options.length - 1;

  const renderBody = () => {
    if (status === 'error') {
      return (
        <p className={styles.state}>
          <Translate id="search.error">Die Suche ist gerade nicht verfügbar.</Translate>
        </p>
      );
    }
    if (status !== 'ready') {
      return (
        <p className={styles.state}>
          <Translate id="search.loading">Suchindex wird geladen …</Translate>
        </p>
      );
    }
    if (groups.length === 0) {
      return (
        <p className={styles.state}>
          {translate({id: 'search.noHits', message: 'Keine Treffer für „{query}“.'}, {query: query.trim()})}
        </p>
      );
    }
    let optionIndex = -1;
    return (
      <ul
        role="listbox"
        id={listId}
        className={styles.list}
        aria-label={translate({id: 'search.results', message: 'Suchergebnisse'})}>
        {groups.map((group) => {
          const titleId = `${listId}-g-${group.page.id.replace(/[^\w-]/g, '_')}`;
          return (
            <li key={group.page.id} role="none">
              <ul role="group" aria-labelledby={titleId} className={styles.group}>
                <li role="presentation" id={titleId} className={styles.pageTitle}>
                  {group.page.title}
                  {group.page.context && <span className={styles.context}> · {group.page.context}</span>}
                </li>
                {group.hits.map((hit) => {
                  optionIndex += 1;
                  const index = optionIndex;
                  const option = options[index];
                  return (
                    <li
                      key={`${hit.anchor}-${index}`}
                      role="option"
                      id={option.id}
                      aria-selected={active === index}
                      className={`${styles.option} ${active === index ? styles.active : ''}`}
                      onMouseDown={(e) => e.preventDefault()}
                      onClick={() => go(option.url)}>
                      <span className={styles.heading}>
                        <Highlight text={hit.heading} ranges={hit.headingRanges} />
                      </span>
                      <span className={styles.snippet}>
                        <Highlight text={hit.snippet.text} ranges={hit.snippet.ranges} />
                      </span>
                    </li>
                  );
                })}
              </ul>
            </li>
          );
        })}
        <li
          role="option"
          id={options[lastIndex].id}
          aria-selected={active === lastIndex}
          className={`${styles.option} ${styles.all} ${active === lastIndex ? styles.active : ''}`}
          onMouseDown={(e) => e.preventDefault()}
          onClick={() => go(options[lastIndex].url)}>
          {translate({id: 'search.all', message: 'Alle Ergebnisse anzeigen ({count})'}, {count: total})}
        </li>
      </ul>
    );
  };

  return (
    <form
      ref={formRef}
      role="search"
      className={styles.search}
      aria-label={translate({id: 'search.form', message: 'Suche'})}
      onSubmit={onSubmit}
      onFocus={() => {
        setFocused(true);
        setDismissed(false);
        load();
      }}
      onBlur={(event) => {
        if (!formRef.current.contains(event.relatedTarget)) setFocused(false);
      }}>
      <label htmlFor={inputId} className="sr-only">
        <Translate id="search.label">Inhalte durchsuchen</Translate>
      </label>
      <input
        ref={inputRef}
        id={inputId}
        type="search"
        className={styles.input}
        placeholder={translate({id: 'search.placeholder', message: 'Suchen …'})}
        autoComplete="off"
        role="combobox"
        aria-expanded={showList}
        aria-controls={showList ? listId : undefined}
        aria-autocomplete="list"
        aria-activedescendant={showList && active >= 0 ? options[active]?.id : undefined}
        value={query}
        onChange={(e) => {
          setQuery(e.target.value);
          setDismissed(false);
          load();
        }}
        onKeyDown={onKeyDown}
      />
      <button type="submit" className={styles.button} aria-label={translate({id: 'search.submit', message: 'Suche starten'})}>
        <svg
          aria-hidden="true"
          focusable="false"
          width="18"
          height="18"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          strokeWidth="2"
          strokeLinecap="round">
          <circle cx="11" cy="11" r="7" />
          <path d="M20 20l-3.5-3.5" />
        </svg>
      </button>
      {open && <div className={styles.dropdown}>{renderBody()}</div>}
      <div className="sr-only" role="status" aria-live="polite">
        {liveText}
      </div>
    </form>
  );
}
