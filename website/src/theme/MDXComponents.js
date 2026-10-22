import MDXComponents from '@theme-original/MDXComponents';
import Quiz from '@site/src/components/Quiz';
import CSharpRunner from '@site/src/components/CSharpRunner';

// Komponenten stehen in allen .mdx-Dateien ohne eigenen Import zur Verfügung.
export default {
  ...MDXComponents,
  Quiz,
  CSharpRunner,
};
