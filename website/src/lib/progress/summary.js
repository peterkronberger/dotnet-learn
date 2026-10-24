// Pure helpers for the "Lernstand" page: derive lesson status, stages and activity
// from the stored progress. No React, no storage access – easy to test.
import { STAGE_BY_BOX } from './config';
import { addDays } from './dates';

const EMPTY_DAY = { answered: 0, correct: 0, partial: 0 };

function toDate(iso) {
  const [y, m, d] = iso.split('-').map(Number);
  return new Date(y, m - 1, d);
}

// 0 = Monday … 6 = Sunday
export function weekdayIndex(iso) {
  return (toDate(iso).getDay() + 6) % 7;
}

export function mondayOf(iso) {
  return addDays(iso, -weekdayIndex(iso));
}

// ISO 8601 week number of the week that contains `iso`.
export function isoWeek(iso) {
  const thursday = toDate(addDays(mondayOf(iso), 3));
  const jan4 = new Date(thursday.getFullYear(), 0, 4);
  const firstThursday = toDate(addDays(mondayOf(`${jan4.getFullYear()}-01-04`), 3));
  const ms = thursday.getTime() - firstThursday.getTime();
  return 1 + Math.round(ms / (7 * 86400000));
}

// Status of every lesson plus its question counts per stage, in lesson order.
// lessons: [{id, …}] from the lesson index; questionIds: {lessonId: [questionId]}.
export function lessonOverview(lessons, questionIds, progress) {
  const done = progress.lessons ?? {};
  return lessons.map((lesson) => {
    const ids = questionIds[lesson.id] ?? [];
    const stages = { shaky: 0, building: 0, solid: 0, open: 0 };
    let lastActivity = null;
    for (const id of ids) {
      const entry = progress.questions[id];
      if (!entry) {
        stages.open += 1;
        continue;
      }
      stages[STAGE_BY_BOX[entry.box - 1]] += 1;
      if (lastActivity === null || entry.lastReviewed > lastActivity) lastActivity = entry.lastReviewed;
    }
    const answered = ids.length - stages.open;
    const completedAt = done[lesson.id] ? done[lesson.id].completedAt : null;
    let status = 'open';
    if (completedAt) status = 'done';
    else if (answered > 0) status = 'started';
    return { ...lesson, status, completedAt, lastActivity, stages, answered, total: ids.length };
  });
}

// The lesson to continue with: the most recently worked started lesson,
// otherwise the first open lesson in order; null when all are done.
export function nextLesson(overview) {
  const started = overview.filter((l) => l.status === 'started');
  if (started.length > 0) {
    return started.reduce((best, l) => (l.lastActivity > best.lastActivity ? l : best));
  }
  return overview.find((l) => l.status === 'open') ?? null;
}

export function stageTotals(overview) {
  const totals = { shaky: 0, building: 0, solid: 0, open: 0 };
  for (const l of overview) {
    for (const key of Object.keys(totals)) totals[key] += l.stages[key];
  }
  return totals;
}

// Sum of the activity log over the `days` days ending with `today` (inclusive).
export function activitySum(progress, today, days) {
  const log = progress.activity ?? {};
  const sum = { answered: 0, correct: 0, partial: 0, activeDays: 0 };
  for (let i = 0; i < days; i++) {
    const day = log[addDays(today, -i)];
    if (!day) continue;
    sum.answered += day.answered;
    sum.correct += day.correct;
    sum.partial += day.partial;
    if (day.answered > 0) sum.activeDays += 1;
  }
  return sum;
}

function addInto(target, day) {
  target.answered += day.answered;
  target.correct += day.correct;
  target.partial += day.partial;
}

// Buckets for the activity chart.
// range '4w': 28 days · '3m': 13 weeks (Monday to Sunday) · 'all': months since the first entry.
export function activityBuckets(progress, today, range) {
  const log = progress.activity ?? {};
  const buckets = [];
  if (range === '4w') {
    for (let i = 27; i >= 0; i--) {
      const day = addDays(today, -i);
      buckets.push({ kind: 'day', start: day, ...EMPTY_DAY, ...(log[day] ?? {}) });
    }
  } else if (range === '3m') {
    const monday = mondayOf(today);
    for (let w = 12; w >= 0; w--) {
      const start = addDays(monday, -7 * w);
      const bucket = { kind: 'week', start, week: isoWeek(start), ...EMPTY_DAY };
      for (let d = 0; d < 7; d++) {
        const day = log[addDays(start, d)];
        if (day) addInto(bucket, day);
      }
      buckets.push(bucket);
    }
  } else {
    const days = Object.keys(log).sort();
    const first = days.length > 0 && days[0] < today ? days[0] : today;
    let [y, m] = first.split('-').map(Number);
    const [ty, tm] = today.split('-').map(Number);
    while (y < ty || (y === ty && m <= tm)) {
      const prefix = `${y}-${String(m).padStart(2, '0')}`;
      const bucket = { kind: 'month', start: `${prefix}-01`, year: y, month: m, ...EMPTY_DAY };
      for (const day of days) if (day.startsWith(prefix)) addInto(bucket, log[day]);
      buckets.push(bucket);
      m += 1;
      if (m > 12) {
        m = 1;
        y += 1;
      }
    }
  }
  return buckets.map((b) => ({ ...b, wrong: b.answered - b.correct - b.partial }));
}

export function hasActivity(progress) {
  return Object.values(progress.activity ?? {}).some((d) => d.answered > 0);
}

// Percentage rounded to an integer, or null without a base.
export function percent(part, whole) {
  return whole > 0 ? Math.round((part / whole) * 100) : null;
}
