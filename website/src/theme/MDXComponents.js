import MDXComponents from '@theme-original/MDXComponents';
import Quiz from '@site/src/components/Quiz';
import CSharpRunner from '@site/src/components/CSharpRunner';

// These components are available in all .mdx files without their own import.
export default {
  ...MDXComponents,
  Quiz,
  CSharpRunner,
};
