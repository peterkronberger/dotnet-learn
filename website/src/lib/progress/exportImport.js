import {
  BOX_COUNT,
  EXPORT_FORMAT,
  LESSON_ID_PATTERN,
  MAX_IMPORT_LENGTH,
  MAX_LESSON_ID_LENGTH,
  MAX_MODULE_KEY_LENGTH,
  MAX_QUESTION_ID_LENGTH,
  MODULE_KEY_PATTERN,
  PLACEMENT_VIA,
} from './config';
import { isValidIsoDate } from './dates';

const RESULTS = ['correct', 'partial', 'wrong'];

function isCount(v) {
  return Number.isInteger(v) && v >= 0;
}

function isPlainObject(v) {
  return !!v && typeof v === 'object' && !Array.isArray(v);
}

function fail(error) {
  return { ok: false, error };
}

function invalidField(field, id) {
  return fail(`Die Datei ist kein gültiger Lernfortschritt (Feld "${field}" in ${id}).`);
}

function invalid(field) {
  return fail(`Die Datei ist kein gültiger Lernfortschritt (Feld "${field}").`);
}

// Validates a progress object; only known fields are copied into the result.
// `lessons`, `activity` and `placement` are optional, so a file with questions only stays valid.
export function validateProgress(value) {
  if (!isPlainObject(value) || value.version !== 1) return invalid('version');

  const questions = value.questions;
  if (!isPlainObject(questions)) return invalid('questions');
  const cleanQuestions = {};
  for (const [id, e] of Object.entries(questions)) {
    if (id.length === 0 || id.length > MAX_QUESTION_ID_LENGTH) {
      return fail('Die Datei ist kein gültiger Lernfortschritt (ungültige Frage-ID).');
    }
    if (!isPlainObject(e)) return invalidField('entry', id);
    if (!Number.isInteger(e.box) || e.box < 1 || e.box > BOX_COUNT) return invalidField('box', id);
    if (!isValidIsoDate(e.lastReviewed)) return invalidField('lastReviewed', id);
    if (!isCount(e.attempts)) return invalidField('attempts', id);
    if (!isCount(e.correctCount) || e.correctCount > e.attempts) return invalidField('correctCount', id);
    if (!RESULTS.includes(e.lastResult)) return invalidField('lastResult', id);
    cleanQuestions[id] = {
      box: e.box,
      lastReviewed: e.lastReviewed,
      attempts: e.attempts,
      correctCount: e.correctCount,
      lastResult: e.lastResult,
    };
  }

  const lessons = value.lessons ?? {};
  if (!isPlainObject(lessons)) return invalid('lessons');
  const cleanLessons = {};
  for (const [id, e] of Object.entries(lessons)) {
    if (id.length > MAX_LESSON_ID_LENGTH || !LESSON_ID_PATTERN.test(id)) {
      return fail('Die Datei ist kein gültiger Lernfortschritt (ungültige Lektions-ID).');
    }
    if (!isPlainObject(e) || !isValidIsoDate(e.completedAt)) return invalidField('completedAt', id);
    if (e.via !== undefined && e.via !== PLACEMENT_VIA) return invalidField('via', id);
    cleanLessons[id] = { completedAt: e.completedAt };
    if (e.via === PLACEMENT_VIA) cleanLessons[id].via = PLACEMENT_VIA;
  }

  const activity = value.activity ?? {};
  if (!isPlainObject(activity)) return invalid('activity');
  const cleanActivity = {};
  for (const [day, e] of Object.entries(activity)) {
    if (!isValidIsoDate(day)) return fail('Die Datei ist kein gültiger Lernfortschritt (ungültiges Datum in "activity").');
    if (!isPlainObject(e) || !isCount(e.answered)) return invalidField('answered', day);
    if (!isCount(e.correct) || !isCount(e.partial) || e.correct + e.partial > e.answered) {
      return invalidField('correct/partial', day);
    }
    cleanActivity[day] = { answered: e.answered, correct: e.correct, partial: e.partial };
  }

  const placement = value.placement ?? {};
  if (!isPlainObject(placement)) return invalid('placement');
  const cleanPlacement = {};
  for (const [key, e] of Object.entries(placement)) {
    if (key.length > MAX_MODULE_KEY_LENGTH || !MODULE_KEY_PATTERN.test(key)) {
      return fail('Die Datei ist kein gültiger Lernfortschritt (ungültige Modul-ID).');
    }
    if (!isPlainObject(e)) return invalidField('entry', key);
    if (!isValidIsoDate(e.date)) return invalidField('date', key);
    if (!isCount(e.total) || e.total < 1) return invalidField('total', key);
    if (!isCount(e.correct) || e.correct > e.total) return invalidField('correct', key);
    if (typeof e.passed !== 'boolean') return invalidField('passed', key);
    cleanPlacement[key] = { date: e.date, correct: e.correct, total: e.total, passed: e.passed };
  }

  return {
    ok: true,
    progress: { version: 1, questions: cleanQuestions, lessons: cleanLessons, activity: cleanActivity, placement: cleanPlacement },
  };
}

// Pretty JSON (2 spaces).
export function serializeProgress(progress, now = new Date()) {
  return JSON.stringify(
    { format: EXPORT_FORMAT, version: 1, exportedAt: now.toISOString(), progress },
    null,
    2,
  );
}

export function parseImport(text) {
  if (typeof text !== 'string') return fail('Die Datei konnte nicht gelesen werden.');
  if (text.length > MAX_IMPORT_LENGTH) return fail('Die Datei ist zu groß (maximal 1 MB).');
  let data;
  try {
    data = JSON.parse(text);
  } catch {
    return fail('Die Datei ist kein gültiges JSON.');
  }
  if (!data || typeof data !== 'object' || data.format !== EXPORT_FORMAT || data.version !== 1) {
    return fail('Die Datei ist keine Lernfortschritt-Datei dieses Kurses (Feld "format" oder "version").');
  }
  return validateProgress(data.progress);
}
