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
  check(eq(actual, expected), `${label}: erwartet ${JSON.stringify(expected)}, erhalten ${JSON.stringify(actual)}`);
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
  ['Leitner: neue Frage richtig → Box 2, fällig nach 2 Tagen', () => {
    const p = recordAnswer(emptyProgress(), 'q', 'correct', '2026-10-23');
    const e = p.questions.q;
    expectEqual(e.box, 2, 'Box');
    check(!isDue(e, '2026-10-24'), 'am 2026-10-24 nicht fällig');
    check(isDue(e, '2026-10-25'), 'am 2026-10-25 fällig');
  }],
  ['Leitner: neue Frage falsch → Box 1', () => {
    expectEqual(recordAnswer(emptyProgress(), 'q', 'wrong', '2026-10-23').questions.q.box, 1, 'Box');
  }],
  ['Leitner: Box 5 + richtig bleibt 5; partial hält die Box; falsch → Box 1', () => {
    expectEqual(nextBox(5, 'correct'), 5, 'Box 5 richtig');
    expectEqual(nextBox(3, 'partial'), 3, 'partial');
    expectEqual(nextBox(4, 'wrong'), 1, 'falsch');
    expectEqual(nextBox(1, 'correct'), 2, 'Box 1 richtig');
  }],
  ['Leitner: recordAnswer verändert die Eingabe nicht und zählt Versuche', () => {
    const p = recordAnswer(emptyProgress(), 'q', 'correct', '2026-10-23');
    const before = JSON.stringify(p);
    const p2 = recordAnswer(p, 'q', 'wrong', '2026-10-24');
    expectEqual(JSON.stringify(p), before, 'Eingabe');
    expectEqual([p2.questions.q.attempts, p2.questions.q.correctCount, p2.questions.q.lastResult], [2, 1, 'wrong'], 'Zähler');
  }],
  ['Leitner: Reihenfolge fälliger Fragen, nextDueDate, boxCounts', () => {
    const p = sample();
    // q1: box 3, reviewed 10-25, due 10-29; l02-q1: box 1, due 10-25; l02-q2: box 1, due 10-25
    expectEqual(dueQuestionIds(p, '2026-10-25'), ['m01-l02-q1', 'm01-l02-q2'], 'fällig am 25.');
    expectEqual(dueQuestionIds(p, '2026-10-29'), ['m01-l02-q1', 'm01-l02-q2', 'm01-l01-q1'], 'fällig am 29.');
    expectEqual(nextDueDate(p), '2026-10-25', 'nextDueDate');
    expectEqual(nextDueDate(emptyProgress()), null, 'nextDueDate leer');
    expectEqual(boxCounts(p), [2, 0, 1, 0, 0], 'boxCounts');
  }],
  ['Leitner: eigene Intervalle [1,1,1,1,1]', () => {
    const p = recordAnswer(emptyProgress(), 'q', 'correct', '2026-10-23');
    check(isDue(p.questions.q, '2026-10-24', [1, 1, 1, 1, 1]), 'nach 1 Tag fällig');
    expectEqual(dueQuestionIds(p, '2026-10-24', [1, 1, 1, 1, 1]), ['q'], 'dueQuestionIds');
  }],
  ['Datum: Monats-, Jahresgrenze und Zeitumstellung', () => {
    expectEqual(addDays('2026-03-28', 2), '2026-03-30', 'Zeitumstellung');
    expectEqual(addDays('2026-12-31', 1), '2027-01-01', 'Jahresgrenze');
    expectEqual(addDays('2026-01-31', 1), '2026-02-01', 'Monatsgrenze');
    expectEqual(addDays('2028-02-28', 1), '2028-02-29', 'Schaltjahr');
  }],
  ['Migration: alte Schlüssel → v1, alte Schlüssel gelöscht, nur einmal', () => {
    const storage = createMemoryStorage({
      [`${LEGACY_PREFIX}old-a`]: JSON.stringify({ attempts: 2, correct: true, lastAttempt: '2026-10-20T12:00:00' }),
      [`${LEGACY_PREFIX}old-b`]: JSON.stringify({ attempts: 1, correct: false, lastAttempt: 'kaputt' }),
      [`${LEGACY_PREFIX}broken`]: '{nicht json',
      unrelated: 'x',
    });
    const p = loadProgress(storage, { legacyIdMap: { 'old-a': 'm01-l01-q1' } });
    expectEqual(Object.keys(p.questions).sort(), ['m01-l01-q1', 'old-b'], 'IDs');
    expectEqual(p.questions['m01-l01-q1'], { box: 2, lastReviewed: '2026-10-20', attempts: 2, correctCount: 1, lastResult: 'correct' }, 'Eintrag a');
    expectEqual([p.questions['old-b'].box, p.questions['old-b'].lastResult, p.questions['old-b'].correctCount], [1, 'wrong', 0], 'Eintrag b');
    check(storage.getItem(PROGRESS_KEY) !== null, 'v1 geschrieben');
    expectEqual(storage.length, 2, 'übrig: v1 + unrelated (auch der kaputte Schlüssel wird gelöscht)');
    storage.setItem(`${LEGACY_PREFIX}late`, JSON.stringify({ attempts: 1, correct: true, lastAttempt: '2026-10-21T00:00:00' }));
    const again = loadProgress(storage);
    expectEqual(Object.keys(again.questions).sort(), ['m01-l01-q1', 'old-b'], 'keine erneute Migration');
    check(storage.getItem(`${LEGACY_PREFIX}late`) !== null, 'alter Schlüssel bleibt unberücksichtigt');
  }],
  ['Migration: ohne alte Schlüssel null', () => {
    check(migrateLegacy(createMemoryStorage()) === null, 'null erwartet');
  }],
  ['Export → Import ergibt denselben Zustand', () => {
    const p = sample();
    const r = parseImport(serializeProgress(p, new Date('2026-10-23T10:00:00Z')));
    check(r.ok, 'Import ok');
    expectEqual(r.progress, p, 'Zustand');
    check(serializeProgress(p).includes('\n  "format"'), 'Einrückung mit 2 Leerzeichen');
  }],
  ['Ungültiger Import ändert nichts', () => {
    const p = sample();
    const valid = JSON.parse(serializeProgress(p));
    const withEntry = (patch) => JSON.stringify({ ...valid, progress: { version: 1, questions: { 'm01-l01-q1': { ...p.questions['m01-l01-q1'], ...patch } } } });
    const cases = {
      'kein JSON': 'das ist kein json',
      'falsches format': JSON.stringify({ ...valid, format: 'x' }),
      'box 6': withEntry({ box: 6 }),
      'Datum 2026-13-01': withEntry({ lastReviewed: '2026-13-01' }),
      'correctCount > attempts': withEntry({ attempts: 1, correctCount: 2 }),
      'lastResult unbekannt': withEntry({ lastResult: 'maybe' }),
      'zu groß': ' '.repeat(1_000_001),
    };
    const storage = createMemoryStorage();
    saveProgress(p, storage);
    const before = storage.getItem(PROGRESS_KEY);
    for (const [name, text] of Object.entries(cases)) {
      const r = parseImport(text);
      check(!r.ok, `${name}: sollte ungültig sein`);
      check(typeof r.error === 'string' && r.error.length > 0, `${name}: Fehlertext fehlt`);
      if (r.ok) saveProgress(r.progress, storage);
    }
    expectEqual(storage.getItem(PROGRESS_KEY), before, 'Speicher');
  }],
  ['Import verwirft unbekannte Zusatzfelder', () => {
    const p = sample();
    const data = JSON.parse(serializeProgress(p));
    data.progress.questions['m01-l01-q1'].extra = 'x';
    const r = parseImport(JSON.stringify(data));
    check(r.ok, 'Import ok');
    expectEqual(r.progress, p, 'Zustand ohne Zusatzfeld');
  }],
  ['Ohne Speicher: nichts wirft, leerer Fortschritt', () => {
    expectEqual(loadProgress(null), emptyProgress(), 'loadProgress(null)');
    expectEqual(saveProgress(sample(), null), false, 'saveProgress(null)');
  }],
  ['Speicher wirft bei Zugriff: nichts wirft', () => {
    const s = throwingStorage();
    expectEqual(loadProgress(s), emptyProgress(), 'loadProgress');
    expectEqual(saveProgress(sample(), s), false, 'saveProgress');
    check(migrateLegacy(s) === null, 'migrateLegacy');
  }],
  ['Kaputter v1-Eintrag wird erst beim Speichern ersetzt', () => {
    const storage = createMemoryStorage({ [PROGRESS_KEY]: '{kaputt' });
    expectEqual(loadProgress(storage), emptyProgress(), 'Laden');
    expectEqual(storage.getItem(PROGRESS_KEY), '{kaputt', 'unverändert nach Laden');
    saveProgress(sample(), storage);
    expectEqual(loadProgress(storage), sample(), 'nach Speichern');
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
