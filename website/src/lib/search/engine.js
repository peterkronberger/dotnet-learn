// Pure search logic: no React, no Docusaurus imports.

export const MIN_QUERY_LENGTH = 2;

const SNIPPET_LEAD = 40;
const ELLIPSIS = '…';

// Lowercase, remove combining marks after NFD, 'ß' -> 'ss'.
// map[i] is the index of the original char that produced normalized char i.
export function normalizeWithMap(text) {
  let normalized = '';
  const map = [];
  let index = 0;
  for (const ch of text) {
    const part = ch.normalize('NFD').replace(/\p{M}/gu, '').toLowerCase().replace(/ß/g, 'ss');
    normalized += part;
    for (let i = 0; i < part.length; i++) map.push(index);
    index += ch.length;
  }
  return { normalized, map };
}

export function normalize(text) {
  return normalizeWithMap(text).normalized;
}

export function parseQuery(query) {
  const normalized = normalize(query).trim();
  if (normalized.length < MIN_QUERY_LENGTH) return [];
  return [...new Set(normalized.split(/\s+/).filter(Boolean))];
}

function isWordChar(ch) {
  return /[\p{L}\p{N}]/u.test(ch);
}

function occurrences(haystack, word) {
  const starts = [];
  let from = 0;
  for (;;) {
    const i = haystack.indexOf(word, from);
    if (i === -1) return starts;
    starts.push(i);
    from = i + 1;
  }
}

function charLength(text, index) {
  return String.fromCodePoint(text.codePointAt(index)).length;
}

function mergeRanges(ranges) {
  const sorted = [...ranges].sort((a, b) => a[0] - b[0] || a[1] - b[1]);
  const merged = [];
  for (const [start, end] of sorted) {
    const last = merged[merged.length - 1];
    if (last && start <= last[1]) last[1] = Math.max(last[1], end);
    else merged.push([start, end]);
  }
  return merged;
}

// Ranges [start, end) into the original text for all occurrences of all words.
function findRanges(text, words) {
  const { normalized, map } = normalizeWithMap(text);
  const ranges = [];
  for (const word of words) {
    for (const start of occurrences(normalized, word)) {
      const lastIndex = map[start + word.length - 1];
      let end = lastIndex + charLength(text, lastIndex);
      // Include combining marks that directly follow the last matched char.
      while (end < text.length && /^\p{M}/u.test(String.fromCodePoint(text.codePointAt(end)))) {
        end += charLength(text, end);
      }
      ranges.push([map[start], end]);
    }
  }
  return mergeRanges(ranges);
}

export function makeSnippet(text, words, maxLength = 160) {
  const { normalized, map } = normalizeWithMap(text);
  let first = -1;
  for (const word of words) {
    const i = normalized.indexOf(word);
    if (i !== -1 && (first === -1 || i < first)) first = i;
  }
  let start = 0;
  if (first !== -1) {
    const firstOriginal = map[first];
    start = Math.max(0, firstOriginal - SNIPPET_LEAD);
    if (start > 0) {
      const space = text.indexOf(' ', start);
      if (space !== -1 && space < firstOriginal) start = space + 1;
    }
  }
  const end = Math.min(text.length, start + maxLength);
  const prefix = start > 0 ? ELLIPSIS : '';
  const suffix = end < text.length ? ELLIPSIS : '';
  const shift = prefix.length - start;
  const ranges = findRanges(text, words)
    .filter(([s, e]) => e > start && s < end)
    .map(([s, e]) => [Math.max(s, start) + shift, Math.min(e, end) + shift]);
  return { text: prefix + text.slice(start, end) + suffix, ranges };
}

// Returns -1 when a word is missing, else the summed score.
function scoreSection(words, heading, text, title) {
  let score = 0;
  for (const word of words) {
    const inHeading = heading.includes(word);
    const inTitle = title.includes(word);
    const textCount = occurrences(text, word).length;
    if (!inHeading && !inTitle && textCount === 0) return -1;
    if (inHeading) {
      score += 10;
      if (occurrences(heading, word).some((i) => i === 0 || !isWordChar(heading[i - 1]))) score += 5;
    }
    if (inTitle) score += 4;
    score += Math.min(textCount, 3);
  }
  return score;
}

function plainSnippet(text, maxLength = 160) {
  return { text: text.slice(0, maxLength) + (text.length > maxLength ? ELLIPSIS : ''), ranges: [] };
}

// All results, best page first.
export function search(pages, query) {
  const words = parseQuery(query);
  if (words.length === 0) return [];
  const results = [];
  for (const page of pages) {
    const title = normalize(page.title);
    const hits = [];
    for (const section of page.sections) {
      const text = normalize(section.text);
      const score = scoreSection(words, normalize(section.heading), text, title);
      if (score < 0) continue;
      hits.push({
        anchor: section.anchor,
        heading: section.heading,
        score,
        snippet: words.some((w) => text.includes(w)) ? makeSnippet(section.text, words) : plainSnippet(section.text),
        headingRanges: findRanges(section.heading, words),
      });
    }
    if (hits.length === 0) continue;
    hits.sort((a, b) => b.score - a.score); // stable: ties keep section order
    results.push({ page, score: hits[0].score + 0.5 * (hits.length - 1), hits });
  }
  results.sort((a, b) => b.score - a.score); // stable: ties keep index order
  return results;
}

// The n best hits overall, grouped again by page (page order = order of their best hit).
export function topHits(results, n = 3) {
  const flat = [];
  results.forEach((result, pageRank) => {
    result.hits.forEach((hit, hitOrder) => flat.push({ result, hit, pageRank, hitOrder }));
  });
  flat.sort((a, b) => b.hit.score - a.hit.score || a.pageRank - b.pageRank || a.hitOrder - b.hitOrder);
  const grouped = new Map();
  for (const { result, hit } of flat.slice(0, n)) {
    if (!grouped.has(result)) grouped.set(result, { page: result.page, score: hit.score, hits: [] });
    grouped.get(result).hits.push(hit);
  }
  return [...grouped.values()];
}
