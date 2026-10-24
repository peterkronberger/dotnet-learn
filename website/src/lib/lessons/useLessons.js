import {useMemo} from 'react';
import {usePluginData} from '@docusaurus/useGlobalData';
import {useAllDocsData} from '@docusaurus/plugin-content-docs/client';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import {getAllQuestions} from '@site/src/data/quiz';

// Lessons from the lesson-index plugin, in sidebar order, each with its page path.
export function useLessons() {
  const data = usePluginData('lesson-index');
  const allDocs = useAllDocsData();
  return useMemo(() => {
    const docs = allDocs && allDocs.default;
    const version = docs ? docs.versions.find((v) => v.isLast) ?? docs.versions[0] : null;
    const paths = new Map(version ? version.docs.map((d) => [d.id, d.path]) : []);
    const lessons = data && Array.isArray(data.lessons) ? data.lessons : [];
    return lessons.map((l) => ({...l, path: paths.get(l.docId) ?? null}));
  }, [data, allDocs]);
}

// Question ids per lesson id for the current locale.
export function useQuestionIdsByLesson() {
  const {i18n} = useDocusaurusContext();
  return useMemo(() => {
    const byLesson = {};
    for (const q of getAllQuestions(i18n.currentLocale)) {
      if (!byLesson[q.lesson]) byLesson[q.lesson] = [];
      byLesson[q.lesson].push(q.id);
    }
    return byLesson;
  }, [i18n.currentLocale]);
}
