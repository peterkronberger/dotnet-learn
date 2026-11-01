import { LESSON_ID_PATTERN, PLACEMENT_MAX_WRONG, PLACEMENT_VIA } from './config';

// 'm01-l03' -> 'm01'; null if the id does not match LESSON_ID_PATTERN.
export function moduleKeyOf(lessonId) {
  if (typeof lessonId !== 'string' || !LESSON_ID_PATTERN.test(lessonId)) return null;
  return lessonId.slice(0, lessonId.lastIndexOf('-l'));
}

// results: array of 'correct' | 'partial' | 'wrong'. `partial` counts as wrong.
export function evaluatePlacement(results) {
  const total = results.length;
  const correct = results.filter((r) => r === 'correct').length;
  return { correct, total, passed: total > 0 && total - correct <= PLACEMENT_MAX_WRONG };
}

// Pure. Never touches `questions` or `activity`.
export function applyPlacementPass(progress, moduleKey, lessonIds, result, today) {
  if (!result || result.passed !== true) throw new Error('applyPlacementPass: the result is not passed');
  const lessons = { ...progress.lessons };
  for (const id of lessonIds) {
    if (!(id in lessons)) lessons[id] = { completedAt: today, via: PLACEMENT_VIA };
  }
  return {
    ...progress,
    lessons,
    placement: {
      ...(progress.placement ?? {}),
      [moduleKey]: { date: today, correct: result.correct, total: result.total, passed: true },
    },
  };
}

// Pure. Removes the result and the lessons marked by this placement; hand-completed lessons stay.
export function resetPlacement(progress, moduleKey) {
  const lessons = {};
  for (const [id, e] of Object.entries(progress.lessons ?? {})) {
    if (e.via === PLACEMENT_VIA && moduleKeyOf(id) === moduleKey) continue;
    lessons[id] = e;
  }
  const placement = { ...(progress.placement ?? {}) };
  delete placement[moduleKey];
  return { ...progress, lessons, placement };
}

// The stored result or null.
export function placementResult(progress, moduleKey) {
  return (progress.placement ?? {})[moduleKey] ?? null;
}
