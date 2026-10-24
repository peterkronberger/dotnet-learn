import { BOX_COUNT, LEITNER_INTERVALS_DAYS } from './config';
import { addDays } from './dates';

// First answer: correct -> box 2, otherwise box 1.
// Later: correct -> box + 1 (max 5), partial -> unchanged, wrong -> box 1.
export function nextBox(currentBox, result) {
  if (currentBox === undefined || currentBox === null) {
    return result === 'correct' ? 2 : 1;
  }
  if (result === 'correct') return Math.min(currentBox + 1, BOX_COUNT);
  if (result === 'partial') return currentBox;
  return 1;
}

export function dueDate(entry, intervals = LEITNER_INTERVALS_DAYS) {
  return addDays(entry.lastReviewed, intervals[entry.box - 1]);
}

export function isDue(entry, today, intervals = LEITNER_INTERVALS_DAYS) {
  // ISO dates compare correctly as strings
  return dueDate(entry, intervals) <= today;
}

// Sorted: oldest due date first, then id.
export function dueQuestionIds(progress, today, intervals = LEITNER_INTERVALS_DAYS) {
  return Object.entries(progress.questions)
    .map(([id, entry]) => ({ id, due: dueDate(entry, intervals) }))
    .filter((x) => x.due <= today)
    .sort((a, b) => (a.due < b.due ? -1 : a.due > b.due ? 1 : a.id < b.id ? -1 : a.id > b.id ? 1 : 0))
    .map((x) => x.id);
}

// Earliest due date of all questions, or null without questions.
export function nextDueDate(progress, intervals = LEITNER_INTERVALS_DAYS) {
  let best = null;
  for (const entry of Object.values(progress.questions)) {
    const due = dueDate(entry, intervals);
    if (best === null || due < best) best = due;
  }
  return best;
}

// Pure: returns a new progress object, the input stays unchanged.
// Every recorded answer also counts in the daily activity log (no time limit).
export function recordAnswer(progress, questionId, result, today) {
  const old = progress.questions[questionId];
  const entry = {
    box: nextBox(old ? old.box : undefined, result),
    lastReviewed: today,
    attempts: (old ? old.attempts : 0) + 1,
    correctCount: (old ? old.correctCount : 0) + (result === 'correct' ? 1 : 0),
    lastResult: result,
  };
  const activity = progress.activity ?? {};
  const day = activity[today] ?? { answered: 0, correct: 0, partial: 0 };
  return {
    ...progress,
    questions: { ...progress.questions, [questionId]: entry },
    activity: {
      ...activity,
      [today]: {
        answered: day.answered + 1,
        correct: day.correct + (result === 'correct' ? 1 : 0),
        partial: day.partial + (result === 'partial' ? 1 : 0),
      },
    },
  };
}

// Pure: marks a lesson as completed (keeps the first date if it is already marked).
export function completeLesson(progress, lessonId, today) {
  const lessons = progress.lessons ?? {};
  if (lessons[lessonId]) return progress;
  return { ...progress, lessons: { ...lessons, [lessonId]: { completedAt: today } } };
}

// Pure: removes the completion mark of a lesson.
export function reopenLesson(progress, lessonId) {
  const lessons = { ...(progress.lessons ?? {}) };
  delete lessons[lessonId];
  return { ...progress, lessons };
}

// Questions due after `today`: the earliest such date and how many are due then, or null.
export function nextReviewAfter(progress, today, intervals = LEITNER_INTERVALS_DAYS) {
  let best = null;
  let count = 0;
  for (const entry of Object.values(progress.questions)) {
    const due = dueDate(entry, intervals);
    if (due <= today) continue;
    if (best === null || due < best) {
      best = due;
      count = 1;
    } else if (due === best) {
      count += 1;
    }
  }
  return best === null ? null : { date: best, count };
}

// Length 5, index 0 = box 1.
export function boxCounts(progress) {
  const counts = new Array(BOX_COUNT).fill(0);
  for (const entry of Object.values(progress.questions)) counts[entry.box - 1] += 1;
  return counts;
}
