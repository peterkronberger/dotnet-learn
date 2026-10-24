import deM00 from './de/m00.json';
import deM01 from './de/m01.json';
import deM02 from './de/m02.json';
import deDev from './de/dev.json';
import {getExample} from './examples';
import {splitCloze, clozeCode} from './cloze';

export const QUESTION_TYPES = ['single', 'multi', 'output', 'explain', 'parsons', 'cloze']; // planned: find-bug, match

const DEFAULT_LOCALE = 'de';

const FILES = {
  de: [deM00, deM01, deM02, deDev],
};

const LESSON_PATTERN = /^[a-z0-9]+-l\d+$/;

// Returns a list of error messages; empty = valid.
export function validateQuestion(q) {
  const errors = [];
  const where = `Question "${q && q.id}"`;
  if (!q || typeof q !== 'object') return ['Question is not an object'];
  if (typeof q.id !== 'string' || !q.id) errors.push(`${where}: missing id`);
  if (typeof q.lesson !== 'string' || !LESSON_PATTERN.test(q.lesson)) {
    errors.push(`${where}: invalid lesson`);
  } else if (typeof q.id === 'string' && !q.id.startsWith(`${q.lesson}-q`)) {
    errors.push(`${where}: id must start with "${q.lesson}-q"`);
  }
  if (!QUESTION_TYPES.includes(q.type)) errors.push(`${where}: unknown type "${q.type}"`);
  if (typeof q.question !== 'string' || !q.question) errors.push(`${where}: missing question`);

  if (q.type === 'single' || q.type === 'multi') {
    if (!Array.isArray(q.options) || q.options.length < 2) {
      errors.push(`${where}: needs at least 2 options`);
    } else {
      q.options.forEach((o, i) => {
        if (!o || typeof o.text !== 'string' || !o.text) errors.push(`${where}: option ${i} has no text`);
      });
      const correct = q.options.filter((o) => o && o.correct === true).length;
      if (q.type === 'single' && correct !== 1) errors.push(`${where}: single needs exactly one correct option`);
      if (q.type === 'multi' && correct < 1) errors.push(`${where}: multi needs at least one correct option`);
    }
  }
  if (q.type === 'output' && (typeof q.example !== 'string' || !q.example)) {
    errors.push(`${where}: output needs example`);
  }
  if (q.type === 'parsons') {
    if (typeof q.example !== 'string' || !q.example) errors.push(`${where}: parsons needs example`);
    if (q.distractors !== undefined) {
      if (!Array.isArray(q.distractors) || q.distractors.some((d) => typeof d !== 'string' || !d.trim())) {
        errors.push(`${where}: distractors must be a list of non-empty strings`);
      }
    }
    const dm = q.distractorMisconceptions;
    if (dm !== undefined && (typeof dm !== 'object' || dm === null || Array.isArray(dm))) {
      errors.push(`${where}: distractorMisconceptions must be an object`);
    }
  }
  if (q.type === 'cloze') {
    if (typeof q.example !== 'string' || !q.example) {
      errors.push(`${where}: cloze needs example`);
    } else if (!Array.isArray(q.blanks) || q.blanks.length < 1) {
      errors.push(`${where}: cloze needs at least one blank`);
    } else if (q.blanks.some((b) => !b || typeof b.text !== 'string' || !b.text)) {
      errors.push(`${where}: every blank needs a text`);
    } else {
      const example = getExample(q.example);
      if (!example) {
        errors.push(`${where}: example ${q.example} or its .out.txt not found`);
      } else {
        try {
          splitCloze(clozeCode(example.code), q.blanks);
        } catch (e) {
          errors.push(`${where}: ${e.message}`);
        }
      }
    }
  }
  if (q.type === 'explain' && (typeof q.modelAnswer !== 'string' || !q.modelAnswer)) {
    errors.push(`${where}: explain needs modelAnswer`);
  }
  if (['single', 'multi', 'output', 'parsons', 'cloze'].includes(q.type) && (typeof q.explanation !== 'string' || !q.explanation)) {
    errors.push(`${where}: explanation is required`);
  }
  return errors;
}

function buildIndex(files) {
  const byLocale = {};
  for (const [locale, modules] of Object.entries(files)) {
    const map = new Map();
    for (const mod of modules) {
      for (const q of mod.questions) {
        if (map.has(q.id)) throw new Error(`Duplicate quiz id "${q.id}" in locale "${locale}"`);
        map.set(q.id, q);
      }
    }
    byLocale[locale] = map;
  }
  return byLocale;
}

const INDEX = buildIndex(FILES);

export function getQuestion(locale, id) {
  return (INDEX[locale] ?? INDEX[DEFAULT_LOCALE]).get(id) ?? INDEX[DEFAULT_LOCALE].get(id);
}

export function getAllQuestions(locale) {
  return [...(INDEX[locale] ?? INDEX[DEFAULT_LOCALE]).values()];
}
