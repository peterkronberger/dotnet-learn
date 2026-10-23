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
export function recordAnswer(progress, questionId, result, today) {
  const old = progress.questions[questionId];
  const entry = {
    box: nextBox(old ? old.box : undefined, result),
    lastReviewed: today,
    attempts: (old ? old.attempts : 0) + 1,
    correctCount: (old ? old.correctCount : 0) + (result === 'correct' ? 1 : 0),
    lastResult: result,
  };
  return { ...progress, questions: { ...progress.questions, [questionId]: entry } };
}

// Length 5, index 0 = box 1.
export function boxCounts(progress) {
  const counts = new Array(BOX_COUNT).fill(0);
  for (const entry of Object.values(progress.questions)) counts[entry.box - 1] += 1;
  return counts;
}
