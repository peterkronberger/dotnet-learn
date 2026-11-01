import SingleChoice from './SingleChoice';
import MultiChoice from './MultiChoice';
import OutputQuestion from './OutputQuestion';
import ExplainQuestion from './ExplainQuestion';
import ParsonsQuestion from './ParsonsQuestion';
import ClozeQuestion from './ClozeQuestion';
import FindBugQuestion from './FindBugQuestion';
import MatchQuestion from './MatchQuestion';

// Question type -> component. Shared by <Quiz> and components that render the types directly.
export const COMPONENTS = {
  single: SingleChoice,
  multi: MultiChoice,
  output: OutputQuestion,
  explain: ExplainQuestion,
  parsons: ParsonsQuestion,
  cloze: ClozeQuestion,
  'find-bug': FindBugQuestion,
  match: MatchQuestion,
};

// Types that can run in deferred mode (all except 'explain').
export const DEFERRED_TYPES = ['single', 'multi', 'output', 'parsons', 'cloze', 'find-bug', 'match'];
