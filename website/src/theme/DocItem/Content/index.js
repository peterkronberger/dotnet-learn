import React from 'react';
import Content from '@theme-original/DocItem/Content';
import {useDoc} from '@docusaurus/plugin-content-docs/client';
import ReviewReminder from '@site/src/components/ReviewReminder';

// Wraps the doc content: lessons (front matter `lesson_id`) start with the review reminder box.
export default function ContentWrapper(props) {
  const {frontMatter} = useDoc();
  const lessonId = typeof frontMatter.lesson_id === 'string' ? frontMatter.lesson_id : null;
  return (
    <>
      {lessonId && <ReviewReminder />}
      <Content {...props} />
    </>
  );
}
