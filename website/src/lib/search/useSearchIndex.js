import {useCallback, useEffect, useMemo, useState} from 'react';
import {useAllDocsData} from '@docusaurus/plugin-content-docs/client';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';

// One promise per page visit, so the index file is loaded at most once.
let indexPromise = null;

function loadIndex() {
  if (!indexPromise) {
    indexPromise = import('@generated/search-index/default/search-index.json')
      .then((module) => module.default ?? module)
      .catch((error) => {
        indexPromise = null; // allow a retry
        throw error;
      });
  }
  return indexPromise;
}

function stripBaseUrl(path, baseUrl) {
  const base = baseUrl.replace(/\/$/, '');
  return base && path.startsWith(base) ? path.slice(base.length) || '/' : path;
}

// status: 'idle' | 'loading' | 'ready' | 'error'. pages: index pages with a resolved `url`.
export function useSearchIndex({eager = false} = {}) {
  const [state, setState] = useState({status: 'idle', index: null});
  const allDocs = useAllDocsData();
  const {siteConfig} = useDocusaurusContext();

  const load = useCallback(() => {
    setState((s) => (s.status === 'idle' || s.status === 'error' ? {status: 'loading', index: null} : s));
    loadIndex().then(
      (index) => setState({status: 'ready', index}),
      () => setState({status: 'error', index: null}),
    );
  }, []);

  useEffect(() => {
    if (eager) load();
  }, [eager, load]);

  const pages = useMemo(() => {
    if (!state.index) return [];
    const docs = allDocs && allDocs.default;
    const version = docs ? docs.versions.find((v) => v.isLast) ?? docs.versions[0] : null;
    const paths = new Map(version ? version.docs.map((d) => [d.id, d.path]) : []);
    const resolved = [];
    for (const page of state.index.pages) {
      if (page.kind === 'doc') {
        const path = paths.get(page.docId);
        if (path) resolved.push({...page, url: stripBaseUrl(path, siteConfig.baseUrl)});
      } else if (page.url) {
        resolved.push(page);
      }
    }
    return resolved;
  }, [state.index, allDocs, siteConfig.baseUrl]);

  return {status: state.status, pages, load};
}
