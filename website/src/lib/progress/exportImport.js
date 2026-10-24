import {
  BOX_COUNT,
  EXPORT_FORMAT,
  MAX_IMPORT_LENGTH,
  MAX_QUESTION_ID_LENGTH,
} from './config';
import { isValidIsoDate } from './dates';

const RESULTS = ['correct', 'partial', 'wrong'];

function isCount(v) {
  return Number.isInteger(v) && v >= 0;
}

function fail(error) {
  return { ok: false, error };
}

function invalidField(field, id) {
  return fail(`Die Datei ist kein gültiger Lernfortschritt (Feld "${field}" in ${id}).`);
}

// Validates a progress object; only known fields are copied into the result.
export function validateProgress(value) {
  if (!value || typeof value !== 'object' || Array.isArray(value) || value.version !== 1) {
    return fail('Die Datei ist kein gültiger Lernfortschritt (Feld "version").');
  }
  const questions = value.questions;
  if (!questions || typeof questions !== 'object' || Array.isArray(questions)) {
    return fail('Die Datei ist kein gültiger Lernfortschritt (Feld "questions").');
  }
  const clean = {};
  for (const [id, e] of Object.entries(questions)) {
    if (id.length === 0 || id.length > MAX_QUESTION_ID_LENGTH) {
      return fail('Die Datei ist kein gültiger Lernfortschritt (ungültige Frage-ID).');
    }
    if (!e || typeof e !== 'object' || Array.isArray(e)) return invalidField('entry', id);
    if (!Number.isInteger(e.box) || e.box < 1 || e.box > BOX_COUNT) return invalidField('box', id);
    if (!isValidIsoDate(e.lastReviewed)) return invalidField('lastReviewed', id);
    if (!isCount(e.attempts)) return invalidField('attempts', id);
    if (!isCount(e.correctCount) || e.correctCount > e.attempts) return invalidField('correctCount', id);
    if (!RESULTS.includes(e.lastResult)) return invalidField('lastResult', id);
    clean[id] = {
      box: e.box,
      lastReviewed: e.lastReviewed,
      attempts: e.attempts,
      correctCount: e.correctCount,
      lastResult: e.lastResult,
    };
  }
  return { ok: true, progress: { version: 1, questions: clean } };
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
