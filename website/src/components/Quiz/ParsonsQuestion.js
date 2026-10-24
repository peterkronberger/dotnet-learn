import React, {useEffect, useRef, useState} from 'react';
import clsx from 'clsx';
import Translate, {translate} from '@docusaurus/Translate';
import Feedback from './Feedback';
import {renderInline} from './QuizText';
import {getExample} from '../../data/quiz/examples';
import styles from './styles.module.css';

// Simple string hash (FNV-1a) used as the seed.
function hashSeed(seed) {
  let h = 2166136261;
  for (let i = 0; i < seed.length; i++) {
    h ^= seed.charCodeAt(i);
    h = Math.imul(h, 16777619);
  }
  return h >>> 0;
}

// Deterministic PRNG (mulberry32).
function random(seedNumber) {
  let a = seedNumber;
  return () => {
    a = (a + 0x6d2b79f5) >>> 0;
    let t = a;
    t = Math.imul(t ^ (t >>> 15), t | 1);
    t ^= t + Math.imul(t ^ (t >>> 7), t | 61);
    return ((t ^ (t >>> 14)) >>> 0) / 4294967296;
  };
}

// Fisher-Yates shuffle with a fixed seed, so SSR and client render the same order.
export function seededShuffle(items, seed) {
  const result = [...items];
  const next = random(hashSeed(seed));
  for (let i = result.length - 1; i > 0; i--) {
    const j = Math.floor(next() * (i + 1));
    [result[i], result[j]] = [result[j], result[i]];
  }
  return result;
}

// Non-empty lines of the example file; indentation stays part of the line.
export function solutionLines(code) {
  return String(code)
    .replace(/\r\n?/g, '\n')
    .split('\n')
    .filter((line) => line.trim() !== '');
}

const t = (id, message, values) => translate({id, message}, values);

export default function ParsonsQuestion({question, onResult}) {
  const example = getExample(question.example);
  if (!example) {
    throw new Error(`Quiz "${question.id}": example "${question.example}" or its .out.txt not found`);
  }
  const correct = solutionLines(example.code);
  const distractors = question.distractors ?? [];
  const distractorMisconceptions = question.distractorMisconceptions ?? {};
  // Items get a stable id: solution lines first, then distractors.
  const items = [...correct, ...distractors].map((text, id) => ({id, text}));
  const [order] = useState(() => seededShuffle(items, question.id).map((item) => item.id));

  const [chosen, setChosen] = useState([]); // item ids in the learner's order
  const [result, setResult] = useState(null);
  const [announcement, setAnnouncement] = useState('');
  const buttons = useRef({});
  const pendingFocus = useRef(null);
  const checked = result !== null;
  const available = order.filter((id) => !chosen.includes(id));

  // Keeps the focus on the moved line after the list was re-rendered.
  useEffect(() => {
    if (pendingFocus.current) {
      const button = buttons.current[pendingFocus.current];
      pendingFocus.current = null;
      if (button) button.focus();
    }
  });

  const line = (id) => items[id].text.trim();
  const isRight = (id, index) => index < correct.length && line(id) === correct[index].trim();

  function announcePosition(id, position, total) {
    setAnnouncement(
      t('quiz.parsons.position', 'Zeile {position} von {total}: {line}', {position, total, line: line(id)}),
    );
  }

  function add(id) {
    pendingFocus.current = `${id}:remove`;
    setChosen([...chosen, id]);
    announcePosition(id, chosen.length + 1, chosen.length + 1);
  }

  function remove(id) {
    pendingFocus.current = `${id}:add`;
    setChosen(chosen.filter((c) => c !== id));
    setAnnouncement(
      t('quiz.parsons.removed', 'Zeile entfernt: {line}. {count} Zeilen in deiner Lösung.', {
        line: line(id),
        count: chosen.length - 1,
      }),
    );
  }

  function move(id, delta) {
    const from = chosen.indexOf(id);
    const to = from + delta;
    pendingFocus.current = `${id}:${delta < 0 ? 'up' : 'down'}`;
    if (to < 0 || to >= chosen.length) {
      setAnnouncement(
        delta < 0
          ? t('quiz.parsons.atTop', 'Die Zeile steht schon ganz oben.')
          : t('quiz.parsons.atBottom', 'Die Zeile steht schon ganz unten.'),
      );
      return;
    }
    const next = [...chosen];
    next.splice(from, 1);
    next.splice(to, 0, id);
    setChosen(next);
    announcePosition(id, to + 1, chosen.length);
  }

  function check(event) {
    event.preventDefault();
    if (chosen.length === 0) return;
    const ok = chosen.length === correct.length && chosen.every((id, i) => isRight(id, i));
    const next = ok ? 'correct' : 'wrong';
    setResult(next);
    onResult(next);
  }

  function retry() {
    setResult(null);
    setAnnouncement('');
  }

  const rows = Math.max(chosen.length, correct.length);
  const misconceptions = checked
    ? chosen
        .filter((id) => id >= correct.length)
        .map((id) => distractorMisconceptions[items[id].text])
        .filter(Boolean)
    : [];

  function register(key) {
    return (node) => {
      if (node) buttons.current[key] = node;
      else delete buttons.current[key];
    };
  }

  function lineButton(id, action, label, labelId, onClick) {
    return (
      <button
        type="button"
        className="button button--secondary button--sm"
        ref={register(`${id}:${action}`)}
        disabled={checked}
        aria-label={t(`quiz.parsons.${action}Label`, `${label}: {line}`, {line: line(id)})}
        onClick={onClick}>
        {t(labelId, label)}
      </button>
    );
  }

  return (
    <form className={styles.quiz} onSubmit={check}>
      <p className={styles.question}>{renderInline(question.question)}</p>
      <p className={styles.hint}>
        <Translate id="quiz.parsons.hint">
          Füge Zeilen mit „Hinzufügen“ zu deiner Lösung hinzu und ordne sie mit „Nach oben“ und „Nach unten“.
          Nicht jede Zeile gehört zwingend in die Lösung.
        </Translate>
      </p>

      <h4 className={styles.parsonsHeading} id={`${question.id}-available`}>
        <Translate id="quiz.parsons.available">Verfügbare Zeilen</Translate>
      </h4>
      <ol className={styles.parsonsList} aria-labelledby={`${question.id}-available`}>
        {available.map((id) => (
          <li key={id} className={styles.parsonsItem}>
            <code className={styles.parsonsCode}>{items[id].text}</code>
            {lineButton(id, 'add', 'Hinzufügen', 'quiz.parsons.add', () => add(id))}
          </li>
        ))}
      </ol>
      {available.length === 0 && (
        <p className={styles.hint}>
          <Translate id="quiz.parsons.noneLeft">Keine Zeilen mehr verfügbar.</Translate>
        </p>
      )}

      <h4 className={styles.parsonsHeading} id={`${question.id}-solution`}>
        <Translate id="quiz.parsons.solution">Deine Lösung</Translate>
      </h4>
      <ol className={styles.parsonsList} aria-labelledby={`${question.id}-solution`}>
        {chosen.map((id, index) => (
          <li key={id} className={styles.parsonsItem}>
            <code className={styles.parsonsCode}>{items[id].text}</code>
            <span className={styles.parsonsButtons}>
              {lineButton(id, 'up', 'Nach oben', 'quiz.parsons.up', () => move(id, -1))}
              {lineButton(id, 'down', 'Nach unten', 'quiz.parsons.down', () => move(id, 1))}
              {lineButton(id, 'remove', 'Entfernen', 'quiz.parsons.remove', () => remove(id))}
            </span>
            {checked && (
              <span className={clsx(styles.mark, isRight(id, index) ? styles.markOk : styles.markBad)}>
                {isRight(id, index) ? `✓ Stimmt (Position ${index + 1})` : `✗ Stimmt nicht (Position ${index + 1})`}
              </span>
            )}
          </li>
        ))}
      </ol>
      {chosen.length === 0 && (
        <p className={styles.hint}>
          <Translate id="quiz.parsons.empty">Noch keine Zeilen in deiner Lösung.</Translate>
        </p>
      )}

      <p className="sr-only" role="status" aria-live="polite">
        {announcement}
      </p>

      <div className={styles.actions}>
        {!checked && (
          <button type="submit" className="button button--primary" disabled={chosen.length === 0}>
            <Translate id="quiz.check">Antwort prüfen</Translate>
          </button>
        )}
        {checked && result !== 'correct' && (
          <button type="button" className="button button--secondary" onClick={retry}>
            <Translate id="quiz.retry">Nochmal versuchen</Translate>
          </button>
        )}
      </div>

      {checked && result === 'wrong' && (
        <table className={styles.compare}>
          <thead>
            <tr>
              <th><Translate id="quiz.parsons.positionHeader">Position</Translate></th>
              <th><Translate id="quiz.yourAnswer">Deine Antwort</Translate></th>
              <th><Translate id="quiz.parsons.expected">Erwartet</Translate></th>
              <th><span className="sr-only"><Translate id="quiz.lineResult">Ergebnis</Translate></span></th>
            </tr>
          </thead>
          <tbody>
            {Array.from({length: rows}, (_, i) => {
              const given = i < chosen.length ? line(chosen[i]) : undefined;
              const expected = i < correct.length ? correct[i].trim() : undefined;
              const same = given === expected;
              return (
                <tr key={i} className={same ? styles.optionCorrect : styles.optionWrong}>
                  <td>{i + 1}</td>
                  <td><code>{given ?? ''}</code></td>
                  <td><code>{expected ?? ''}</code></td>
                  <td>{same ? '✓ Stimmt' : `✗ Stimmt nicht – erwartet war ${expected ?? '(keine Zeile)'}`}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      )}
      <Feedback result={result} explanation={question.explanation} misconceptions={misconceptions} />
    </form>
  );
}
