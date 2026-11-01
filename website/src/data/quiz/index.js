import deM00 from './de/m00.json';
import deM01 from './de/m01.json';
import deM02 from './de/m02.json';
import deDev from './de/dev.json';
import {getExample} from './examples';
import {splitCloze, clozeCode} from './cloze';
import {findBugLines} from './findBug';

export const QUESTION_TYPES = ['single', 'multi', 'output', 'explain', 'parsons', 'cloze', 'find-bug', 'match'];

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
        if (o && o.pin !== undefined && o.pin !== 'last') errors.push(`${where}: option ${i}: pin must be "last"`);
      });
      if (q.options.every((o) => o && o.pin === 'last')) errors.push(`${where}: at least one option must not be pinned`);
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
  if (q.type === 'find-bug') {
    if (typeof q.example !== 'string' || !q.example) {
      errors.push(`${where}: find-bug needs example`);
    } else if (!Array.isArray(q.bugLines) || q.bugLines.length < 1 || q.bugLines.some((n) => !Number.isInteger(n))) {
      errors.push(`${where}: find-bug needs bugLines (at least one line number)`);
    } else {
      const example = getExample(q.example);
      if (!example) {
        errors.push(`${where}: example ${q.example} not found`);
      } else {
        const count = findBugLines(example.code).length;
        if (q.bugLines.some((n) => n < 1 || n > count)) errors.push(`${where}: bugLines must be within 1..${count}`);
        if (new Set(q.bugLines).size !== q.bugLines.length) errors.push(`${where}: bugLines has duplicates`);
        const lf = q.lineFeedback;
        if (lf !== undefined) {
          if (typeof lf !== 'object' || lf === null || Array.isArray(lf)) {
            errors.push(`${where}: lineFeedback must be an object`);
          } else if (Object.keys(lf).some((k) => !/^\d+$/.test(k) || Number(k) < 1 || Number(k) > count)) {
            errors.push(`${where}: lineFeedback keys must be line numbers within 1..${count}`);
          }
        }
      }
    }
  }
  if (q.type === 'match') {
    const pairs = q.pairs;
    if (!Array.isArray(pairs) || pairs.length < 2 || pairs.length > 8) {
      errors.push(`${where}: match needs 2 to 8 pairs`);
    } else if (pairs.some((p) => !p || typeof p.left !== 'string' || !p.left || typeof p.right !== 'string' || !p.right)) {
      errors.push(`${where}: every pair needs left and right`);
    } else {
      if (new Set(pairs.map((p) => p.left)).size !== pairs.length) errors.push(`${where}: left values must be unique`);
      if (new Set(pairs.map((p) => p.right)).size !== pairs.length) errors.push(`${where}: right values must be unique`);
      const wm = q.wrongMatches;
      if (wm !== undefined && !Array.isArray(wm)) errors.push(`${where}: wrongMatches must be a list`);
      (Array.isArray(wm) ? wm : []).forEach((w, i) => {
        if (!w || !pairs.some((p) => p.left === w.left)) errors.push(`${where}: wrongMatches ${i} has an unknown left`);
        else if (!pairs.some((p) => p.right === w.right)) errors.push(`${where}: wrongMatches ${i} has an unknown right`);
        else if (pairs.some((p) => p.left === w.left && p.right === w.right)) errors.push(`${where}: wrongMatches ${i} is a correct pair`);
      });
    }
  }
  if (q.placement !== undefined && q.placement !== true) errors.push(`${where}: placement must be true`);
  if (q.placement === true && q.type === 'explain') errors.push(`${where}: explain questions cannot be placement questions`);
  if (q.placementLessons !== undefined) {
    const pl = q.placementLessons;
    if (q.placement !== true) errors.push(`${where}: placementLessons needs placement: true`);
    if (!Array.isArray(pl) || pl.length < 1 || pl.some((l) => typeof l !== 'string' || !LESSON_PATTERN.test(l))) {
      errors.push(`${where}: placementLessons must be a non-empty list of lesson ids`);
    } else {
      if (new Set(pl).size !== pl.length) errors.push(`${where}: placementLessons has duplicates`);
      if (pl.includes(q.lesson)) errors.push(`${where}: placementLessons must not contain the question's own lesson`);
      const moduleKey = typeof q.lesson === 'string' ? q.lesson.split('-l')[0] : '';
      if (pl.some((l) => l.split('-l')[0] !== moduleKey)) errors.push(`${where}: placementLessons must belong to the same module`);
    }
  }
  if (q.type === 'explain' && (typeof q.modelAnswer !== 'string' || !q.modelAnswer)) {
    errors.push(`${where}: explain needs modelAnswer`);
  }
  if (['single', 'multi', 'output', 'parsons', 'cloze', 'find-bug', 'match'].includes(q.type) && (typeof q.explanation !== 'string' || !q.explanation)) {
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

// Placement questions of one module ('m01'), sorted by lesson order (lesson number, then data order).
export function getPlacementQuestions(locale, moduleKey) {
  const lessonNumber = (q) => Number(q.lesson.split('-l')[1]);
  return getAllQuestions(locale)
    .map((q, index) => ({q, index}))
    .filter(({q}) => q.placement === true && q.lesson.split('-l')[0] === moduleKey)
    .sort((a, b) => lessonNumber(a.q) - lessonNumber(b.q) || a.index - b.index)
    .map(({q}) => q);
}
