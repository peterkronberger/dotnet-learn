// Note for a forward reference: a concept that a lesson uses before the course explains it.
import React from 'react';
import Link from '@docusaurus/Link';
import {translate} from '@docusaurus/Translate';
import {useLessons} from '@site/src/lib/lessons/useLessons';
import {validateForwardProps, conceptParts} from './forward';
import styles from './styles.module.css';

function Concept({name}) {
  return (
    <strong>
      {conceptParts(name).map((part, i) =>
        part.code ? <code key={i}>{part.text}</code> : <React.Fragment key={i}>{part.text}</React.Fragment>,
      )}
    </strong>
  );
}

// Replaces {placeholder} markers in a translated message with React nodes.
function fill(message, values) {
  return message.split(/(\{\w+\})/).map((piece, i) => {
    const match = /^\{(\w+)\}$/.exec(piece);
    return match && match[1] in values ? (
      <React.Fragment key={i}>{values[match[1]]}</React.Fragment>
    ) : (
      piece
    );
  });
}

export default function Forward({concepts, lesson, module}) {
  const lessons = useLessons();
  validateForwardProps({concepts, lesson, module}, new Set(lessons.map((l) => l.id)));

  let target;
  if (lesson) {
    const found = lessons.find((l) => l.id === lesson);
    target = fill(translate({id: 'forward.lessonTarget', message: 'der Lektion „{lesson}“'}), {
      lesson: found.path ? <Link to={found.path}>{found.title}</Link> : found.title,
    });
  } else {
    target = translate(
      {id: 'forward.moduleTarget', message: 'Modul {module}'},
      {module: String(Number(module))},
    );
  }

  const and = translate({id: 'forward.and', message: 'und'});
  const nodes = concepts.map((c, i) => <Concept key={i} name={c} />);
  const list =
    nodes.length === 1 ? (
      nodes[0]
    ) : (
      <>
        {nodes.slice(0, -1).map((n, i) => (
          <React.Fragment key={i}>
            {i > 0 ? ', ' : ''}
            {n}
          </React.Fragment>
        ))}
        {` ${and} `}
        {nodes[nodes.length - 1]}
      </>
    );

  const message =
    concepts.length === 1
      ? translate({
          id: 'forward.single',
          message:
            'Das Konzept von {concepts} wurde bisher nicht erläutert und folgt in {target}. Für dieses Beispiel ist das weitere Verständnis dafür nicht notwendig.',
        })
      : translate({
          id: 'forward.plural',
          message:
            'Die Konzepte von {concepts} wurden bisher nicht erläutert und folgen in {target}. Für dieses Beispiel ist das weitere Verständnis dafür nicht notwendig.',
        });

  return (
    <aside
      className={styles.forward}
      aria-label={translate({id: 'forward.label', message: 'Hinweis: Vorgriff'})}>
      <p>{fill(message, {concepts: list, target})}</p>
    </aside>
  );
}
