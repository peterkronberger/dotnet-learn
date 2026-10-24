import React, {useRef} from 'react';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import {getQuestion, validateQuestion} from '@site/src/data/quiz';
import useProgress from '@site/src/lib/progress/useProgress';
import {recordAnswer} from '@site/src/lib/progress/leitner';
import {toLocalDate} from '@site/src/lib/progress/dates';
import SingleChoice from './SingleChoice';
import MultiChoice from './MultiChoice';
import OutputQuestion from './OutputQuestion';
import ExplainQuestion from './ExplainQuestion';
import ParsonsQuestion from './ParsonsQuestion';
import ClozeQuestion from './ClozeQuestion';
import FindBugQuestion from './FindBugQuestion';
import ProgressHint from './ProgressHint';

const COMPONENTS = {
  single: SingleChoice,
  multi: MultiChoice,
  output: OutputQuestion,
  explain: ExplainQuestion,
  parsons: ParsonsQuestion,
  cloze: ClozeQuestion,
  'find-bug': FindBugQuestion,
};

/**
 * Renders a question from the data files in src/data/quiz.
 * Props: id (question id), onAnswered(result) (optional).
 */
export default function Quiz(props) {
  const {id, onAnswered} = props;
  const unsupported = Object.keys(props).filter((key) => !['id', 'onAnswered'].includes(key));
  if (unsupported.length > 0) {
    throw new Error(
      `Inline quiz props are no longer supported (${unsupported.join(', ')}). Use <Quiz id="…" /> and add the question to src/data/quiz.`,
    );
  }

  const {i18n} = useDocusaurusContext();
  const question = getQuestion(i18n.currentLocale, id);
  if (!question) {
    throw new Error(`Unknown quiz id "${id}". Add the question to a file in src/data/quiz/<locale>/.`);
  }
  const errors = validateQuestion(question);
  if (errors.length > 0) {
    throw new Error(`Invalid quiz question "${id}": ${errors.join('; ')}`);
  }

  const {progress, update} = useProgress();
  const recorded = useRef(false);

  // Only the first attempt per page view counts for the Leitner system.
  function handleResult(result) {
    if (!recorded.current) {
      recorded.current = true;
      update((p) => recordAnswer(p, id, result, toLocalDate()));
    }
    if (onAnswered) onAnswered(result);
  }

  const Component = COMPONENTS[question.type];
  return (
    <div>
      <Component key={id} question={question} onResult={handleResult} />
      <ProgressHint entry={progress ? progress.questions[id] : undefined} />
    </div>
  );
}
