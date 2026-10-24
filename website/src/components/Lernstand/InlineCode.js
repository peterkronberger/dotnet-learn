import React from 'react';
import styles from './styles.module.css';

// Renders plain text with `backtick` spans as <code>, as used in learning goals.
export default function InlineCode({text}) {
  return String(text)
    .split('`')
    .map((part, i) =>
      i % 2 === 1 ? (
        <code key={i} className={styles.code}>
          {part}
        </code>
      ) : (
        <React.Fragment key={i}>{part}</React.Fragment>
      ),
    );
}
