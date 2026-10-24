import React, {useId} from 'react';
import Tabs from '@theme/Tabs';
import {translate} from '@docusaurus/Translate';
import styles from './styles.module.css';

// Fixed order and labels; keys must match the <TabItem value> of the children.
export const FROM_LANGUAGES = {
  java: 'Java',
  python: 'Python',
  js: 'JavaScript/TypeScript',
  go: 'Go',
  cpp: 'C++',
};

const ORDER = Object.keys(FROM_LANGUAGES);

export default function FromLanguage({children}) {
  const titleId = useId();
  const items = React.Children.toArray(children).filter(React.isValidElement);

  if (items.length === 0) {
    throw new Error('<FromLanguage>: at least one <TabItem> is required.');
  }
  items.forEach((item) => {
    if (!ORDER.includes(item.props.value)) {
      throw new Error(
        `<FromLanguage>: unknown value "${item.props.value}". Allowed: ${ORDER.join(', ')}.`,
      );
    }
  });

  const sorted = items
    .map((item) =>
      React.cloneElement(item, {
        label: FROM_LANGUAGES[item.props.value],
        key: item.props.value,
      }),
    )
    .sort((a, b) => ORDER.indexOf(a.props.value) - ORDER.indexOf(b.props.value));

  return (
    <aside className={styles.box} aria-labelledby={titleId}>
      <p id={titleId} className={styles.title}>
        {translate({
          id: 'fromLanguage.title',
          message: 'Kommst du aus einer anderen Sprache?',
        })}
      </p>
      <Tabs groupId="from-language">{sorted}</Tabs>
    </aside>
  );
}
