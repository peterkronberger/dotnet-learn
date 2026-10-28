import MDXComponents from '@theme-original/MDXComponents';
import Quiz from '@site/src/components/Quiz';
import Misconception from '@site/src/components/Misconception';
import CSharpRunner from '@site/src/components/CSharpRunner';
import Tabs from '@theme/Tabs';
import TabItem from '@theme/TabItem';
import Since from '@site/src/components/Since';
import Term from '@site/src/components/Term';
import DevNav from '@site/src/components/DevNav';
import Diagram from '@site/src/components/Diagram';
import Forward from '@site/src/components/Forward';

// Components are available in all .mdx files without an import.
export default {
  ...MDXComponents,
  Quiz,
  Misconception,
  CSharpRunner,
  Tabs,
  TabItem,
  Since,
  Term,
  DevNav,
  Diagram,
  Forward,
};
