// Inline link from a lesson to its glossary entry. Use it only at the first use of a term in a lesson.
import React from 'react';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import {getTerm} from '@site/src/data/glossary';
import styles from './styles.module.css';

export default function Term({id, children}) {
  const {i18n} = useDocusaurusContext();
  const entry = getTerm(id, i18n.currentLocale);
  if (!entry) {
    throw new Error(
      '<Term id="' + id + '">: unknown glossary id. Add it to website/src/data/glossary/<locale>.json.',
    );
  }
  const label = children ?? (entry.german ? `${entry.term} (${entry.german})` : entry.term);
  return (
    <Link to={`/glossary#${id}`} className={styles.term}>
      {label}
    </Link>
  );
}
