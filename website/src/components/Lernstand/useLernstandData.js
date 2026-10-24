import {useMemo} from 'react';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import {getQuestion} from '@site/src/data/quiz';
import {useLessons, useQuestionIdsByLesson} from '@site/src/lib/lessons/useLessons';
import {boxCounts, dueQuestionIds, nextReviewAfter} from '@site/src/lib/progress/leitner';
import {activitySum, lessonOverview, nextLesson, stageTotals} from '@site/src/lib/progress/summary';

// Everything the Lernstand page and the start page teaser need, derived from the progress.
export default function useLernstandData(progress, today) {
  const {i18n} = useDocusaurusContext();
  const lessons = useLessons();
  const questionIds = useQuestionIdsByLesson();

  return useMemo(() => {
    if (!progress) return null;
    // Only questions of indexed lessons count for stages and boxes.
    const lessonQuestionIds = new Set(lessons.flatMap((l) => questionIds[l.id] ?? []));
    const lessonQuestions = Object.fromEntries(
      Object.entries(progress.questions).filter(([id]) => lessonQuestionIds.has(id)),
    );
    // Questions without a data record (e.g. deleted ones) are skipped for reviews.
    const knownQuestions = Object.fromEntries(
      Object.entries(progress.questions).filter(([id]) => getQuestion(i18n.currentLocale, id)),
    );
    const known = {...progress, questions: knownQuestions};

    const overview = lessonOverview(lessons, questionIds, progress);
    const due = dueQuestionIds(known, today);
    const dueLessonIds = [...new Set(due.map((id) => getQuestion(i18n.currentLocale, id).lesson))];
    const lessonPaths = Object.fromEntries(lessons.map((l) => [l.id, l.path]));

    return {
      lessons: overview,
      next: nextLesson(overview),
      totals: stageTotals(overview),
      totalQuestions: lessonQuestionIds.size,
      boxes: boxCounts({...progress, questions: lessonQuestions}),
      due,
      dueLessons: dueLessonIds.map((id) => overview.find((l) => l.id === id)).filter(Boolean),
      nextReview: nextReviewAfter(known, today),
      answeredAny: Object.keys(knownQuestions).length > 0,
      last28: activitySum(progress, today, 28),
      last14: activitySum(progress, today, 14),
      lessonPaths,
    };
  }, [progress, today, lessons, questionIds, i18n.currentLocale]);
}
