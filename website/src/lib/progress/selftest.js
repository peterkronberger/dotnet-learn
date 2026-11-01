import { PROGRESS_KEY } from './config';
import { addDays } from './dates';
import { parseImport, serializeProgress, validateProgress } from './exportImport';
import {
  boxCounts,
  completeLesson,
  dueQuestionIds,
  isDue,
  nextBox,
  nextDueDate,
  nextReviewAfter,
  recordAnswer,
  reopenLesson,
} from './leitner';
import { applyPlacementPass, evaluatePlacement, placementResult, resetPlacement } from './placement';
import { createMemoryStorage, emptyProgress, loadProgress, saveProgress } from './storage';
import { activityBuckets, activitySum, isoWeek, lessonOverview, nextLesson, stageTotals } from './summary';

function eq(a, b) {
  return JSON.stringify(a) === JSON.stringify(b);
}

function check(condition, detail) {
  if (!condition) throw new Error(detail);
}

function expectEqual(actual, expected, label) {
  check(eq(actual, expected), `${label}: expected ${JSON.stringify(expected)}, got ${JSON.stringify(actual)}`);
}

function throwingStorage() {
  const fail = () => {
    throw new Error('storage blocked');
  };
  return { getItem: fail, setItem: fail, removeItem: fail, key: fail, clear: fail, length: 0 };
}

const sample = () => {
  let p = emptyProgress();
  p = recordAnswer(p, 'm01-l01-q1', 'correct', '2026-10-23');
  p = recordAnswer(p, 'm01-l01-q1', 'correct', '2026-10-25');
  p = recordAnswer(p, 'm01-l02-q1', 'wrong', '2026-10-24');
  p = recordAnswer(p, 'm01-l02-q2', 'partial', '2026-10-24');
  return p;
};

const TESTS = [
  ['Leitner: new question correct → box 2, due after 2 days', () => {
    const p = recordAnswer(emptyProgress(), 'q', 'correct', '2026-10-23');
    const e = p.questions.q;
    expectEqual(e.box, 2, 'box');
    check(!isDue(e, '2026-10-24'), 'not due on 2026-10-24');
    check(isDue(e, '2026-10-25'), 'due on 2026-10-25');
  }],
  ['Leitner: new question wrong → box 1', () => {
    expectEqual(recordAnswer(emptyProgress(), 'q', 'wrong', '2026-10-23').questions.q.box, 1, 'box');
  }],
  ['Leitner: box 5 + correct stays 5; partial keeps the box; wrong → box 1', () => {
    expectEqual(nextBox(5, 'correct'), 5, 'box 5 correct');
    expectEqual(nextBox(3, 'partial'), 3, 'partial');
    expectEqual(nextBox(4, 'wrong'), 1, 'wrong');
    expectEqual(nextBox(1, 'correct'), 2, 'box 1 correct');
  }],
  ['Leitner: recordAnswer does not change its input and counts attempts', () => {
    const p = recordAnswer(emptyProgress(), 'q', 'correct', '2026-10-23');
    const before = JSON.stringify(p);
    const p2 = recordAnswer(p, 'q', 'wrong', '2026-10-24');
    expectEqual(JSON.stringify(p), before, 'input');
    expectEqual([p2.questions.q.attempts, p2.questions.q.correctCount, p2.questions.q.lastResult], [2, 1, 'wrong'], 'counters');
  }],
  ['Leitner: order of due questions, nextDueDate, boxCounts', () => {
    const p = sample();
    // q1: box 3, reviewed 10-25, due 10-29; l02-q1: box 1, due 10-25; l02-q2: box 1, due 10-25
    expectEqual(dueQuestionIds(p, '2026-10-25'), ['m01-l02-q1', 'm01-l02-q2'], 'due on the 25th');
    expectEqual(dueQuestionIds(p, '2026-10-29'), ['m01-l02-q1', 'm01-l02-q2', 'm01-l01-q1'], 'due on the 29th');
    expectEqual(nextDueDate(p), '2026-10-25', 'nextDueDate');
    expectEqual(nextDueDate(emptyProgress()), null, 'nextDueDate empty');
    expectEqual(boxCounts(p), [2, 0, 1, 0, 0], 'boxCounts');
  }],
  ['Leitner: custom intervals [1,1,1,1,1]', () => {
    const p = recordAnswer(emptyProgress(), 'q', 'correct', '2026-10-23');
    check(isDue(p.questions.q, '2026-10-24', [1, 1, 1, 1, 1]), 'due after 1 day');
    expectEqual(dueQuestionIds(p, '2026-10-24', [1, 1, 1, 1, 1]), ['q'], 'dueQuestionIds');
  }],
  ['Date: month boundary, year boundary and daylight saving time', () => {
    expectEqual(addDays('2026-03-28', 2), '2026-03-30', 'daylight saving time');
    expectEqual(addDays('2026-12-31', 1), '2027-01-01', 'year boundary');
    expectEqual(addDays('2026-01-31', 1), '2026-02-01', 'month boundary');
    expectEqual(addDays('2028-02-28', 1), '2028-02-29', 'leap year');
  }],
  ['Export → import gives the same state', () => {
    const p = sample();
    const r = parseImport(serializeProgress(p, new Date('2026-10-23T10:00:00Z')));
    check(r.ok, 'import ok');
    expectEqual(r.progress, p, 'state');
    check(serializeProgress(p).includes('\n  "format"'), 'indentation with 2 spaces');
  }],
  ['Invalid import changes nothing', () => {
    const p = sample();
    const valid = JSON.parse(serializeProgress(p));
    const withEntry = (patch) => JSON.stringify({ ...valid, progress: { version: 1, questions: { 'm01-l01-q1': { ...p.questions['m01-l01-q1'], ...patch } } } });
    const withProgress = (patch) => JSON.stringify({ ...valid, progress: { ...valid.progress, ...patch } });
    const cases = {
      'not JSON': 'this is not json',
      'wrong format': JSON.stringify({ ...valid, format: 'x' }),
      'box 6': withEntry({ box: 6 }),
      'date 2026-13-01': withEntry({ lastReviewed: '2026-13-01' }),
      'correctCount > attempts': withEntry({ attempts: 1, correctCount: 2 }),
      'lastResult unknown': withEntry({ lastResult: 'maybe' }),
      'too large': ' '.repeat(1_000_001),
      'lesson id invalid': withProgress({ lessons: { 'Lektion 1': { completedAt: '2026-10-24' } } }),
      'lesson date invalid': withProgress({ lessons: { 'm01-l02': { completedAt: '24.10.2026' } } }),
      'activity date invalid': withProgress({ activity: { '2026-02-30': { answered: 1, correct: 1, partial: 0 } } }),
      'activity correct > answered': withProgress({ activity: { '2026-10-24': { answered: 1, correct: 1, partial: 1 } } }),
    };
    const storage = createMemoryStorage();
    saveProgress(p, storage);
    const before = storage.getItem(PROGRESS_KEY);
    for (const [name, text] of Object.entries(cases)) {
      const r = parseImport(text);
      check(!r.ok, `${name}: should be invalid`);
      check(typeof r.error === 'string' && r.error.length > 0, `${name}: error text missing`);
      if (r.ok) saveProgress(r.progress, storage);
    }
    expectEqual(storage.getItem(PROGRESS_KEY), before, 'storage');
  }],
  ['Import drops unknown extra fields', () => {
    const p = sample();
    const data = JSON.parse(serializeProgress(p));
    data.progress.questions['m01-l01-q1'].extra = 'x';
    const r = parseImport(JSON.stringify(data));
    check(r.ok, 'import ok');
    expectEqual(r.progress, p, 'state without extra field');
  }],
  ['Without storage: nothing throws, progress is empty', () => {
    expectEqual(loadProgress(null), emptyProgress(), 'loadProgress(null)');
    expectEqual(saveProgress(sample(), null), false, 'saveProgress(null)');
  }],
  ['Storage throws on access: nothing throws', () => {
    const s = throwingStorage();
    expectEqual(loadProgress(s), emptyProgress(), 'loadProgress');
    expectEqual(saveProgress(sample(), s), false, 'saveProgress');
  }],
  ['Broken v1 entry is only replaced on save', () => {
    const storage = createMemoryStorage({ [PROGRESS_KEY]: '{broken' });
    expectEqual(loadProgress(storage), emptyProgress(), 'load');
    expectEqual(storage.getItem(PROGRESS_KEY), '{broken', 'unchanged after load');
    saveProgress(sample(), storage);
    expectEqual(loadProgress(storage), sample(), 'after save');
  }],
  ['Activity log: every answer counts per day, correct and partial separately', () => {
    const p = sample();
    // Keys appear in the order the answers were recorded (see sample()).
    expectEqual(p.activity, {
      '2026-10-23': { answered: 1, correct: 1, partial: 0 },
      '2026-10-25': { answered: 1, correct: 1, partial: 0 },
      '2026-10-24': { answered: 2, correct: 0, partial: 1 },
    }, 'activity');
    expectEqual(activitySum(p, '2026-10-25', 2), { answered: 3, correct: 1, partial: 1, activeDays: 2 }, 'sum of 2 days');
  }],
  ['Lessons: complete keeps the first date, reopen removes the mark', () => {
    let p = completeLesson(emptyProgress(), 'm01-l02', '2026-10-24');
    p = completeLesson(p, 'm01-l02', '2026-10-30');
    expectEqual(p.lessons, { 'm01-l02': { completedAt: '2026-10-24' } }, 'complete');
    expectEqual(reopenLesson(p, 'm01-l02').lessons, {}, 'reopen');
  }],
  ['Validation: file without lessons and activity is valid and filled with empty objects', () => {
    const r = validateProgress({ version: 1, questions: {} });
    check(r.ok, 'ok');
    expectEqual(r.progress, emptyProgress(), 'progress');
  }],
  ['nextReviewAfter: earliest date after today and its count', () => {
    const p = sample();
    // l02-q1 and l02-q2 are due on 10-25, l01-q1 on 10-29
    expectEqual(nextReviewAfter(p, '2026-10-24'), { date: '2026-10-25', count: 2 }, 'after 10-24');
    expectEqual(nextReviewAfter(p, '2026-10-25'), { date: '2026-10-29', count: 1 }, 'after 10-25');
    expectEqual(nextReviewAfter(p, '2026-10-29'), null, 'nothing later');
  }],
  ['Overview: status per lesson, stages and next lesson', () => {
    const lessons = [{ id: 'm00-l01' }, { id: 'm01-l01' }, { id: 'm01-l02' }, { id: 'm02-l01' }];
    const ids = { 'm00-l01': ['m00-l01-q1'], 'm01-l01': ['m01-l01-q1'], 'm01-l02': ['m01-l02-q1', 'm01-l02-q2', 'm01-l02-q3'], 'm02-l01': [] };
    let p = completeLesson(sample(), 'm00-l01', '2026-10-20');
    const o = lessonOverview(lessons, ids, p);
    expectEqual(o.map((l) => l.status), ['done', 'started', 'started', 'open'], 'status');
    expectEqual(o[2].stages, { shaky: 2, building: 0, solid: 0, open: 1 }, 'stages m01-l02');
    expectEqual(o[1].stages, { shaky: 0, building: 1, solid: 0, open: 0 }, 'stages m01-l01 (box 3)');
    expectEqual(nextLesson(o).id, 'm01-l01', 'most recently worked started lesson (10-25)');
    expectEqual(stageTotals(o), { shaky: 2, building: 1, solid: 0, open: 2 }, 'totals');
    p = completeLesson(completeLesson(p, 'm01-l01', '2026-10-26'), 'm01-l02', '2026-10-26');
    expectEqual(nextLesson(lessonOverview(lessons, ids, p)).id, 'm02-l01', 'first open lesson');
    p = completeLesson(p, 'm02-l01', '2026-10-26');
    check(nextLesson(lessonOverview(lessons, ids, p)) === null, 'all done → null');
  }],
  ['Activity buckets: 28 days, 13 weeks, months', () => {
    const p = sample();
    const days = activityBuckets(p, '2026-10-25', '4w');
    expectEqual([days.length, days[0].start, days[27].start, days[26].answered, days[26].wrong], [28, '2026-09-28', '2026-10-25', 2, 1], '4w');
    const weeks = activityBuckets(p, '2026-10-25', '3m');
    expectEqual([weeks.length, weeks[12].start, weeks[12].week, weeks[12].answered], [13, '2026-10-19', 43, 4], '3m');
    const months = activityBuckets({ ...p, activity: { ...p.activity, '2026-08-31': { answered: 2, correct: 1, partial: 0 } } }, '2026-10-25', 'all');
    expectEqual(months.map((m) => [m.start, m.answered]), [['2026-08-01', 2], ['2026-09-01', 0], ['2026-10-01', 4]], 'all');
    expectEqual(activityBuckets(emptyProgress(), '2026-10-25', 'all').length, 1, 'all without data: current month');
  }],
  ['ISO week numbers', () => {
    expectEqual(isoWeek('2026-01-01'), 1, '2026-01-01 (Thursday)');
    expectEqual(isoWeek('2026-12-31'), 53, '2026-12-31');
    expectEqual(isoWeek('2027-01-03'), 53, '2027-01-03 (Sunday)');
    expectEqual(isoWeek('2027-01-04'), 1, '2027-01-04 (Monday)');
  }],
  ['Placement: evaluatePlacement pass rule (at most 1 wrong, partial counts as wrong)', () => {
    const rep = (n, r) => Array(n).fill(r);
    const passed = (a) => evaluatePlacement(a).passed;
    check(passed(rep(8, 'correct')), '8 correct');
    check(passed([...rep(7, 'correct'), 'wrong']), '7 correct + 1 wrong');
    check(passed([...rep(7, 'correct'), 'partial']), '7 correct + 1 partial');
    check(!passed([...rep(6, 'correct'), 'partial', 'wrong']), '6 correct + partial + wrong');
    check(!passed([...rep(6, 'correct'), 'partial', 'partial']), '6 correct + 2 partial');
    check(!passed([]), 'empty');
    expectEqual(evaluatePlacement([...rep(7, 'correct'), 'wrong']), { correct: 7, total: 8, passed: true }, 'counts');
  }],
  ['Placement: applyPlacementPass marks open lessons, keeps done ones, leaves questions and activity', () => {
    const base = completeLesson(sample(), 'm01-l01', '2026-10-20');
    const before = JSON.stringify(base);
    const result = { correct: 7, total: 8, passed: true };
    const p = applyPlacementPass(base, 'm01', ['m01-l01', 'm01-l02'], result, '2026-11-01');
    expectEqual(p.lessons['m01-l01'], { completedAt: '2026-10-20' }, 'hand-completed lesson');
    expectEqual(p.lessons['m01-l02'], { completedAt: '2026-11-01', via: 'placement' }, 'placement lesson');
    expectEqual(p.placement.m01, { date: '2026-11-01', correct: 7, total: 8, passed: true }, 'placement');
    expectEqual(placementResult(p, 'm01'), p.placement.m01, 'placementResult');
    expectEqual(placementResult(p, 'm02'), null, 'placementResult none');
    check(p.questions === base.questions && p.activity === base.activity, 'questions and activity are the same objects');
    expectEqual(JSON.stringify(base), before, 'input unchanged');
  }],
  ['Placement: applyPlacementPass throws for a result that is not passed', () => {
    let thrown = false;
    try {
      applyPlacementPass(emptyProgress(), 'm01', ['m01-l01'], { correct: 6, total: 8, passed: false }, '2026-11-01');
    } catch {
      thrown = true;
    }
    check(thrown, 'should throw');
  }],
  ['Placement: resetPlacement removes only placement lessons of that module', () => {
    let p = completeLesson(emptyProgress(), 'm01-l01', '2026-10-20');
    const pass = { correct: 8, total: 8, passed: true };
    p = applyPlacementPass(p, 'm00', ['m00-l01'], pass, '2026-10-21');
    p = applyPlacementPass(p, 'm01', ['m01-l01', 'm01-l02'], pass, '2026-11-01');
    const r = resetPlacement(p, 'm01');
    expectEqual(r.lessons, { 'm01-l01': { completedAt: '2026-10-20' }, 'm00-l01': { completedAt: '2026-10-21', via: 'placement' } }, 'lessons');
    expectEqual(Object.keys(r.placement), ['m00'], 'placement');
    check(r.questions === p.questions && r.activity === p.activity, 'questions and activity unchanged');
    check('m01' in p.placement, 'input unchanged');
  }],
  ['Placement: export → import keeps placement and via', () => {
    const p = applyPlacementPass(sample(), 'm01', ['m01-l03'], { correct: 8, total: 8, passed: true }, '2026-11-01');
    const r = parseImport(serializeProgress(p));
    check(r.ok, 'import ok');
    expectEqual(r.progress, p, 'state');
  }],
  ['Placement: invalid via and placement values are rejected', () => {
    const entry = { date: '2026-11-01', correct: 7, total: 8, passed: true };
    const base = { version: 1, questions: {} };
    const cases = {
      'via hand': { lessons: { 'm01-l01': { completedAt: '2026-10-24', via: 'hand' } } },
      'placement as array': { placement: [] },
      'correct > total': { placement: { m01: { ...entry, correct: 9 } } },
      'total 0': { placement: { m01: { ...entry, correct: 0, total: 0 } } },
      'passed "yes"': { placement: { m01: { ...entry, passed: 'yes' } } },
      'module key "M 1"': { placement: { 'M 1': entry } },
      'date invalid': { placement: { m01: { ...entry, date: '2026-02-30' } } },
    };
    for (const [name, patch] of Object.entries(cases)) {
      const r = validateProgress({ ...base, ...patch });
      check(!r.ok, `${name}: should be invalid`);
      check(typeof r.error === 'string' && r.error.length > 0, `${name}: error text missing`);
    }
  }],
  ['Placement: old file without placement is valid and gets an empty placement', () => {
    const r = validateProgress({ version: 1, questions: {}, lessons: { 'm01-l01': { completedAt: '2026-10-24' } } });
    check(r.ok, 'ok');
    expectEqual(r.progress.placement, {}, 'placement');
    expectEqual(r.progress.lessons, { 'm01-l01': { completedAt: '2026-10-24' } }, 'lessons without via');
    expectEqual(emptyProgress(), { version: 1, questions: {}, lessons: {}, activity: {}, placement: {} }, 'emptyProgress');
  }],
];

export function runProgressSelfTests() {
  return TESTS.map(([name, fn]) => {
    try {
      fn();
      return { name, passed: true, detail: '' };
    } catch (e) {
      return { name, passed: false, detail: e instanceof Error ? e.message : String(e) };
    }
  });
}
