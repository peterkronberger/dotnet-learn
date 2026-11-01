import React, {useEffect, useId, useMemo, useRef, useState} from 'react';
import Link from '@docusaurus/Link';
import Translate from '@docusaurus/Translate';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import {getPlacementQuestions} from '@site/src/data/quiz';
import {useLessons} from '@site/src/lib/lessons/useLessons';
import useProgress from '@site/src/lib/progress/useProgress';
import {toLocalDate} from '@site/src/lib/progress/dates';
import {PLACEMENT_QUESTION_COUNT} from '@site/src/lib/progress/config';
import {
  applyPlacementPass,
  evaluatePlacement,
  moduleKeyOf,
  placementResult,
  resetPlacement,
} from '@site/src/lib/progress/placement';
import {formatDate} from '@site/src/components/ProgressDashboard/format';
import {COMPONENTS} from '../Quiz/types';
import QuizText, {renderInline} from '../Quiz/QuizText';
import styles from './styles.module.css';

/**
 * Placement test of one module. Props: module (number or numeric string).
 * Renders the type components in deferred mode and never calls recordAnswer,
 * so nothing is written to progress.questions or progress.activity.
 */
export default function PlacementTest({module: moduleNumber}) {
  const moduleKey = 'm' + String(moduleNumber).padStart(2, '0');
  const {i18n} = useDocusaurusContext();
  const questions = getPlacementQuestions(i18n.currentLocale, moduleKey);
  if (questions.length !== PLACEMENT_QUESTION_COUNT) {
    throw new Error(
      `<PlacementTest>: module "${moduleNumber}" needs exactly ${PLACEMENT_QUESTION_COUNT} placement questions, found ${questions.length}.`,
    );
  }
  const count = questions.length;

  const {progress, update} = useProgress();
  const lessons = useLessons();
  const [open, setOpen] = useState(false);
  const [attempt, setAttempt] = useState(0);
  const [answers, setAnswers] = useState({});
  const [evaluation, setEvaluation] = useState(null);
  const baseId = useId();
  const titleId = `${baseId}-title`;
  const panelId = `${baseId}-panel`;
  const resultHeading = useRef(null);

  // The evaluation replaces the questions; the focus moves to its heading.
  useEffect(() => {
    if (evaluation && resultHeading.current) resultHeading.current.focus();
  }, [evaluation]);

  const moduleLessons = useMemo(
    () => lessons.filter((l) => moduleKeyOf(l.id) === moduleKey),
    [lessons, moduleKey],
  );
  const nextLesson = useMemo(() => {
    let last = -1;
    lessons.forEach((l, i) => {
      if (moduleKeyOf(l.id) === moduleKey) last = i;
    });
    if (last < 0) return null;
    return lessons.slice(last + 1).find((l) => moduleKeyOf(l.id) !== moduleKey && l.path) ?? null;
  }, [lessons, moduleKey]);
  const lessonById = useMemo(() => new Map(lessons.map((l) => [l.id, l])), [lessons]);

  const stored = progress ? placementResult(progress, moduleKey) : null;
  const saved = questions.filter((q) => answers[q.id] != null).length;

  function evaluate() {
    const result = evaluatePlacement(questions.map((q) => answers[q.id]));
    const wrongIds = questions.filter((q) => answers[q.id] !== 'correct').map((q) => q.id);
    setEvaluation({...result, wrongIds});
    setOpen(true);
    if (result.passed) {
      const lessonIds = moduleLessons.map((l) => l.id);
      update((p) => applyPlacementPass(p, moduleKey, lessonIds, result, toLocalDate()));
    }
  }

  function retake() {
    setAttempt((n) => n + 1);
    setAnswers({});
    setEvaluation(null);
  }

  function reset() {
    update((p) => resetPlacement(p, moduleKey));
    setEvaluation(null);
    setAnswers({});
    setAttempt((n) => n + 1);
    setOpen(false);
  }

  const nextLink = nextLesson ? (
    <p>
      <Link to={nextLesson.path}>
        <Translate
          id="placement.next"
          values={{module: Number(moduleKeyOf(nextLesson.id).slice(1)), title: nextLesson.title}}>
          {'Weiter mit Modul {module}: {title}'}
        </Translate>
      </Link>
    </p>
  ) : null;

  function renderEvaluation() {
    const wrong = evaluation.wrongIds.map((id) => questions.find((q) => q.id === id));
    return (
      <div className={styles.evaluation}>
        <h3 tabIndex={-1} ref={resultHeading} className={styles.resultHeading}>
          <Translate id="placement.result" values={{correct: evaluation.correct, total: evaluation.total}}>
            {'Ergebnis: {correct} von {total} richtig'}
          </Translate>
        </h3>
        <p>
          <strong>
            {evaluation.passed ? (
              <Translate id="placement.passed" values={{module: moduleNumber}}>
                {'✓ Bestanden. Alle Lektionen von Modul {module} sind jetzt als abgeschlossen markiert.'}
              </Translate>
            ) : (
              <Translate id="placement.failed" values={{min: count - 1}}>
                {'✗ Nicht bestanden. Zum Bestehen brauchst du mindestens {min} richtige Antworten. Diese Lektionen helfen dir weiter:'}
              </Translate>
            )}
          </strong>
        </p>
        {evaluation.passed && nextLink}
        {wrong.length > 0 && (
          <ul className={styles.wrongList}>
            {wrong.map((q) => {
              const ids = [q.lesson, ...(q.placementLessons ?? [])];
              return (
                <li key={q.id} className={styles.wrongItem}>
                  <p className={styles.wrongQuestion}>{renderInline(q.question)}</p>
                  <p>
                    <strong>
                      <Translate id="placement.wrong">✗ Stimmt nicht</Translate>
                    </strong>
                  </p>
                  {q.explanation && <QuizText text={q.explanation} />}
                  <p>
                    <Translate id="placement.recommended">Empfohlen: </Translate>
                    {ids.map((id, i) => {
                      const l = lessonById.get(id);
                      return (
                        <React.Fragment key={id}>
                          {i > 0 && ', '}
                          {l && l.path ? <Link to={l.path}>{l.title}</Link> : id}
                        </React.Fragment>
                      );
                    })}
                  </p>
                </li>
              );
            })}
          </ul>
        )}
        {!evaluation.passed && (
          <button type="button" className="button button--secondary" onClick={retake}>
            <Translate id="placement.retake">Test wiederholen</Translate>
          </button>
        )}
      </div>
    );
  }

  const intro = (
    <>
      <p>
        {nextLesson ? (
          <Translate id="placement.intro" values={{module: moduleNumber, count}}>
            {'Du kennst den Stoff von Modul {module} schon? Beantworte {count} Fragen. Bei höchstens einem Fehler gilt das ganze Modul als abgeschlossen und du machst mit dem nächsten Modul weiter.'}
          </Translate>
        ) : (
          <Translate id="placement.introLast" values={{module: moduleNumber, count}}>
            {'Du kennst den Stoff von Modul {module} schon? Beantworte {count} Fragen. Bei höchstens einem Fehler gilt das ganze Modul als abgeschlossen.'}
          </Translate>
        )}
      </p>
      <p>
        <Translate id="placement.noFeedback">
          Du siehst die Auswertung erst am Ende. Die Antworten zählen nicht für die Wiederholungen.
        </Translate>
      </p>
    </>
  );

  return (
    <section className={styles.box} aria-labelledby={titleId}>
      <h2 id={titleId} className={styles.title}>
        <Translate id="placement.title">Einstufungstest: Modul überspringen</Translate>
      </h2>
      {stored && stored.passed ? (
        <>
          {evaluation && renderEvaluation()}
          <p>
            <strong>
              <Translate
                id="placement.storedPassed"
                values={{date: formatDate(stored.date), correct: stored.correct, total: stored.total}}>
                {'✓ Einstufungstest bestanden am {date} ({correct} von {total} richtig). Alle Lektionen dieses Moduls gelten als abgeschlossen.'}
              </Translate>
            </strong>
          </p>
          {!evaluation && nextLink}
          <button type="button" className="button button--secondary" onClick={reset}>
            <Translate id="placement.reset">Einstufung zurücksetzen</Translate>
          </button>
          <p className={styles.hint}>
            <Translate id="placement.resetHint">
              Zurücksetzen entfernt nur die Markierungen aus dem Einstufungstest. Lektionen, die du selbst abgeschlossen hast, bleiben abgeschlossen.
            </Translate>
          </p>
        </>
      ) : (
        <>
          {intro}
          <button
            type="button"
            className="button button--primary"
            aria-expanded={open}
            aria-controls={panelId}
            onClick={() => setOpen((o) => !o)}>
            {open ? (
              <Translate id="placement.close">Einstufungstest schließen</Translate>
            ) : (
              <Translate id="placement.start">Einstufungstest starten</Translate>
            )}
          </button>
          <div id={panelId} hidden={!open}>
            {open && evaluation && renderEvaluation()}
            {open && !evaluation && (
              <>
                <ol className={styles.questions} key={attempt}>
                  {questions.map((q, i) => {
                    const Component = COMPONENTS[q.type];
                    return (
                      <li key={q.id} className={styles.question}>
                        <h3 className={styles.questionHeading}>
                          <Translate id="placement.questionOf" values={{i: i + 1, count}}>
                            {'Frage {i} von {count}'}
                          </Translate>
                        </h3>
                        <Component
                          question={q}
                          deferred
                          onResult={(r) => setAnswers((a) => ({...a, [q.id]: r}))}
                        />
                      </li>
                    );
                  })}
                </ol>
                <p aria-live="polite" className={styles.status}>
                  <Translate id="placement.saved" values={{saved, count}}>
                    {'{saved} von {count} Antworten gespeichert'}
                  </Translate>
                </p>
                <button
                  type="button"
                  className="button button--primary"
                  disabled={saved < count}
                  onClick={evaluate}>
                  <Translate id="placement.evaluate">Auswerten</Translate>
                </button>
              </>
            )}
          </div>
        </>
      )}
    </section>
  );
}
