import MDXComponents from '@theme-original/MDXComponents';
import Quiz from '@site/src/components/Quiz';
import Misconception from '@site/src/components/Misconception';
import CSharpRunner from '@site/src/components/CSharpRunner';

// Components are available in all .mdx files without an import.
export default {
  ...MDXComponents,
  Quiz,
  Misconception,
  CSharpRunner,
};
