import { LEGACY_PREFIX, PROGRESS_KEY } from './config';
import { addDays } from './dates';
import { parseImport, serializeProgress } from './exportImport';
import { boxCounts, dueQuestionIds, isDue, nextBox, nextDueDate, recordAnswer } from './leitner';
import { createMemoryStorage, emptyProgress, loadProgress, migrateLegacy, saveProgress } from './storage';

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
  ['Migration: old keys → v1, old keys deleted, only once', () => {
    const storage = createMemoryStorage({
      [`${LEGACY_PREFIX}old-a`]: JSON.stringify({ attempts: 2, correct: true, lastAttempt: '2026-10-20T12:00:00' }),
      [`${LEGACY_PREFIX}old-b`]: JSON.stringify({ attempts: 1, correct: false, lastAttempt: 'broken' }),
      [`${LEGACY_PREFIX}broken`]: '{not json',
      unrelated: 'x',
    });
    const p = loadProgress(storage, { legacyIdMap: { 'old-a': 'm01-l01-q1' } });
    expectEqual(Object.keys(p.questions).sort(), ['m01-l01-q1', 'old-b'], 'IDs');
    expectEqual(p.questions['m01-l01-q1'], { box: 2, lastReviewed: '2026-10-20', attempts: 2, correctCount: 1, lastResult: 'correct' }, 'entry a');
    expectEqual([p.questions['old-b'].box, p.questions['old-b'].lastResult, p.questions['old-b'].correctCount], [1, 'wrong', 0], 'entry b');
    check(storage.getItem(PROGRESS_KEY) !== null, 'v1 written');
    expectEqual(storage.length, 2, 'left over: v1 + unrelated (the broken key is deleted too)');
    storage.setItem(`${LEGACY_PREFIX}late`, JSON.stringify({ attempts: 1, correct: true, lastAttempt: '2026-10-21T00:00:00' }));
    const again = loadProgress(storage);
    expectEqual(Object.keys(again.questions).sort(), ['m01-l01-q1', 'old-b'], 'no second migration');
    check(storage.getItem(`${LEGACY_PREFIX}late`) !== null, 'old key is ignored');
  }],
  ['Migration: without old keys returns null', () => {
    check(migrateLegacy(createMemoryStorage()) === null, 'expected null');
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
    const cases = {
      'not JSON': 'this is not json',
      'wrong format': JSON.stringify({ ...valid, format: 'x' }),
      'box 6': withEntry({ box: 6 }),
      'date 2026-13-01': withEntry({ lastReviewed: '2026-13-01' }),
      'correctCount > attempts': withEntry({ attempts: 1, correctCount: 2 }),
      'lastResult unknown': withEntry({ lastResult: 'maybe' }),
      'too large': ' '.repeat(1_000_001),
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
    check(migrateLegacy(s) === null, 'migrateLegacy');
  }],
  ['Broken v1 entry is only replaced on save', () => {
    const storage = createMemoryStorage({ [PROGRESS_KEY]: '{broken' });
    expectEqual(loadProgress(storage), emptyProgress(), 'load');
    expectEqual(storage.getItem(PROGRESS_KEY), '{broken', 'unchanged after load');
    saveProgress(sample(), storage);
    expectEqual(loadProgress(storage), sample(), 'after save');
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
