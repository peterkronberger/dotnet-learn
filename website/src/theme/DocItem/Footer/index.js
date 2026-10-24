import React from 'react';
import Footer from '@theme-original/DocItem/Footer';
import {useDoc} from '@docusaurus/plugin-content-docs/client';
import LessonComplete from '@site/src/components/LessonComplete';

// Wraps the doc footer: lessons (front matter `lesson_id`) get the "Lektion abschließen" box.
export default function FooterWrapper(props) {
  const {frontMatter} = useDoc();
  const lessonId = typeof frontMatter.lesson_id === 'string' ? frontMatter.lesson_id : null;
  return (
    <>
      {lessonId && <LessonComplete lessonId={lessonId} />}
      <Footer {...props} />
    </>
  );
}
