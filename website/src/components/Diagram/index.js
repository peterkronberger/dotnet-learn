import React from 'react';
import Translate from '@docusaurus/Translate';
import styles from './styles.module.css';

function isSet(value) {
  return typeof value === 'string' && value.trim() !== '';
}

// Wraps a Mermaid code block with a caption and a text alternative (WCAG 2.2 AA).
export default function Diagram({caption, description, children}) {
  if (!isSet(caption) || !isSet(description)) {
    throw new Error('<Diagram>: "caption" and "description" are required (text alternative, WCAG).');
  }

  return (
    <figure className={styles.figure}>
      <div className={styles.diagram}>{children}</div>
      <figcaption className={styles.caption}>{caption}</figcaption>
      <details>
        <summary>
          <Translate id="diagram.textAlternative">Diagramm als Text</Translate>
        </summary>
        <p>{description}</p>
      </details>
    </figure>
  );
}
