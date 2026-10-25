import deGlossary from './de.json';

export const GLOSSARY_CATEGORIES = ['language', 'platform', 'course'];

const DEFAULT_LOCALE = 'de';

const FILES = {de: deGlossary};

const ID_PATTERN = /^[a-z0-9]+(-[a-z0-9]+)*$/;
const LESSON_PATTERN = /^[a-z0-9]+-l\d+$/;
const SOURCE_PREFIX = 'https://learn.microsoft.com/';
const MAX_DEFINITION = 400;

const isText = (value) => typeof value === 'string' && value.trim() !== '';

// Returns a list of error messages; empty = valid.
export function validateGlossary(terms) {
  if (!Array.isArray(terms)) return ['Glossary "terms" is not a list'];
  const errors = [];
  const ids = new Set(terms.map((t) => t && t.id).filter((id) => typeof id === 'string'));
  const seen = new Set();
  for (const t of terms) {
    if (!t || typeof t !== 'object') {
      errors.push('Glossary entry is not an object');
      continue;
    }
    const where = `Term "${t.id}"`;
    if (typeof t.id !== 'string' || !ID_PATTERN.test(t.id)) errors.push(`${where}: invalid id`);
    else if (seen.has(t.id)) errors.push(`${where}: duplicate id`);
    else seen.add(t.id);
    if (!isText(t.term)) errors.push(`${where}: missing term`);
    if (t.german !== undefined && !isText(t.german)) errors.push(`${where}: german must be a non-empty string`);
    if (!GLOSSARY_CATEGORIES.includes(t.category)) errors.push(`${where}: unknown category "${t.category}"`);
    if (!isText(t.definition)) errors.push(`${where}: missing definition`);
    else if (t.definition.length > MAX_DEFINITION) errors.push(`${where}: definition is longer than ${MAX_DEFINITION} characters`);
    if (t.aliases !== undefined && (!Array.isArray(t.aliases) || t.aliases.some((a) => !isText(a)))) {
      errors.push(`${where}: aliases must be a list of non-empty strings`);
    }
    if (t.related !== undefined) {
      if (!Array.isArray(t.related)) errors.push(`${where}: related must be a list`);
      else {
        for (const r of t.related) {
          if (r === t.id) errors.push(`${where}: related contains the entry itself`);
          else if (!ids.has(r)) errors.push(`${where}: related id "${r}" does not exist`);
        }
      }
    }
    if (t.lesson !== undefined && (typeof t.lesson !== 'string' || !LESSON_PATTERN.test(t.lesson))) {
      errors.push(`${where}: invalid lesson`);
    }
    const s = t.source;
    if (s === undefined) {
      if (t.category === 'language' || t.category === 'platform') errors.push(`${where}: source is required for category "${t.category}"`);
    } else if (!s || !isText(s.title) || !isText(s.url)) {
      errors.push(`${where}: source needs title and url`);
    } else if (!s.url.startsWith(SOURCE_PREFIX)) {
      errors.push(`${where}: source url must start with ${SOURCE_PREFIX}`);
    }
  }
  return errors;
}

// Term without leading non-letters (".NET" -> "NET"), diacritics removed.
export function sortKey(term) {
  return String(term.term)
    .normalize('NFD')
    .replace(/[̀-ͯ]/g, '')
    .replace(/^[^\p{L}\p{N}]+/u, '');
}

const compare = (a, b) => sortKey(a).localeCompare(sortKey(b), 'de', {sensitivity: 'base'});

const BY_LOCALE = {};
const loadErrors = [];
for (const [locale, data] of Object.entries(FILES)) {
  loadErrors.push(...validateGlossary(data.terms).map((e) => `${locale}: ${e}`));
  BY_LOCALE[locale] = Array.isArray(data.terms) ? [...data.terms].sort(compare) : [];
}
if (loadErrors.length > 0) {
  throw new Error(`Invalid glossary data:\n${loadErrors.join('\n')}`);
}

export function getGlossary(locale) {
  return BY_LOCALE[locale] ?? BY_LOCALE[DEFAULT_LOCALE];
}

export function getTerm(id, locale) {
  return getGlossary(locale).find((t) => t.id === id) ?? null;
}
