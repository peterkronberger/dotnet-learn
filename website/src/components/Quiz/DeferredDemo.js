import React, {useState} from 'react';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import {getQuestion} from '@site/src/data/quiz';
import {COMPONENTS} from './types';

// Dev demo of the deferred mode. Renders the type components directly,
// so nothing is written to the progress store.
export default function DeferredDemo({id}) {
  const {i18n} = useDocusaurusContext();
  const question = getQuestion(i18n.currentLocale, id);
  if (!question) throw new Error(`Unknown quiz id "${id}".`);
  const [last, setLast] = useState('(noch keine)');
  const Component = COMPONENTS[question.type];
  return (
    <div>
      <Component
        key={id}
        question={question}
        deferred
        onResult={(result) => setLast(result === null ? 'null' : result)}
      />
      <p>
        Letzter <code>onResult</code>-Wert: <code>{last}</code>
      </p>
    </div>
  );
}
