import React from 'react';
import {translate} from '@docusaurus/Translate';
import {dueDate} from '@site/src/lib/progress/leitner';
import styles from './styles.module.css';

// 'YYYY-MM-DD' -> 'TT.MM.JJJJ'
function formatDate(iso) {
  const [y, m, d] = iso.split('-');
  return `${d}.${m}.${y}`;
}

// Shows the Leitner box and next review date; renders nothing without a stored answer.
export default function ProgressHint({entry}) {
  if (!entry) return null;
  const text = translate(
    {
      id: 'quiz.progressHint',
      message: 'Box {box} von 5 · nächste Wiederholung: {date}',
    },
    {box: entry.box, date: formatDate(dueDate(entry))},
  );
  return <p className={styles.progressHint}>{text}</p>;
}
