import { MIN_QUERY_LENGTH, makeSnippet, normalize, normalizeWithMap, parseQuery, search, topHits } from './engine';

function check(condition, detail) {
  if (!condition) throw new Error(detail);
}

function expectEqual(actual, expected, label) {
  const a = JSON.stringify(actual);
  const e = JSON.stringify(expected);
  check(a === e, `${label}: expected ${e}, got ${a}`);
}

function marked(snippet) {
  return snippet.ranges.map(([a, b]) => snippet.text.slice(a, b));
}

const INDEX = [
  {
    id: 'doc-a',
    kind: 'doc',
    docId: 'a',
    url: '/docs/a',
    title: 'Variablen',
    context: 'Modul 1',
    sections: [
      { anchor: 'intro', heading: 'Einführung', text: 'Eine Variable speichert einen Wert.' },
      { anchor: 'overview', heading: 'Überblick', text: 'Alle Typen im Überblick: int, double und string.' },
    ],
  },
  {
    id: 'doc-b',
    kind: 'doc',
    docId: 'b',
    url: '/docs/b',
    title: 'Schleifen',
    context: 'Modul 2',
    sections: [
      { anchor: 'while', heading: 'while-Schleife', text: 'Die Schleife läuft, solange die Bedingung gilt.' },
      { anchor: 'for', heading: 'for-Schleife', text: 'Zähler und Bedingung stehen im Kopf.' },
      { anchor: 'misc', heading: 'Sonstiges', text: 'Eine Schleife kann auch mit break enden. Bedingung prüfen.' },
    ],
  },
  {
    id: 'glossary-x',
    kind: 'glossary',
    docId: 'x',
    url: '/glossary#x',
    title: 'Straße',
    context: 'Glossar',
    sections: [{ anchor: 'x', heading: 'Straße', text: 'Ein Begriff mit ß.' }],
  },
];

const TESTS = [
  ['normalize: diacritics and ß, map to original chars', () => {
    expectEqual(normalize('Überblick'), 'uberblick', 'Überblick');
    expectEqual(normalize('Straße'), 'strasse', 'Straße');
    expectEqual(normalizeWithMap('Straße').map, [0, 1, 2, 3, 4, 4, 5], 'map of Straße');
    expectEqual(normalizeWithMap('Üb').map, [0, 1], 'map of Üb');
    expectEqual(normalizeWithMap('a😀b').map, [0, 1, 1, 3], 'surrogate pair');
  }],
  ['parseQuery: short query, words, duplicates', () => {
    expectEqual(MIN_QUERY_LENGTH, 2, 'MIN_QUERY_LENGTH');
    expectEqual(parseQuery('a'), [], 'one char');
    expectEqual(parseQuery('  Über  überblick ÜBER '), ['uber', 'uberblick'], 'words');
    expectEqual(search(INDEX, 'x'), [], 'search with one char');
  }],
  ['search: AND logic over two words', () => {
    const results = search(INDEX, 'schleife bedingung');
    expectEqual(results.length, 1, 'pages');
    expectEqual(results[0].hits.map((h) => h.anchor).sort(), ['for', 'misc', 'while'], 'sections');
    expectEqual(search(INDEX, 'schleife variable'), [], 'words on different pages');
    expectEqual(search(INDEX, 'break zahler'), [], 'words in different sections');
  }],
  ['search: umlaut-insensitive', () => {
    expectEqual(search(INDEX, 'uberblick')[0].hits[0].anchor, 'overview', 'uberblick');
    expectEqual(search(INDEX, 'Überblick')[0].hits[0].anchor, 'overview', 'Überblick');
    expectEqual(search(INDEX, 'strasse')[0].page.id, 'glossary-x', 'ß');
  }],
  ['search: heading match ranks before text-only match', () => {
    const page = {
      ...INDEX[0],
      sections: [
        { anchor: 't', heading: 'Anderes', text: 'Wert Wert Wert' },
        { anchor: 'h', heading: 'Wert', text: '' },
      ],
    };
    expectEqual(search([page], 'wert')[0].hits.map((h) => h.anchor), ['h', 't'], 'heading before text');
    expectEqual(search(INDEX, 'schleife')[0].hits.map((h) => h.anchor), ['while', 'for', 'misc'], 'order of hits');
  }],
  ['search: two sections on one page → one PageResult', () => {
    const results = search(INDEX, 'bedingung');
    expectEqual(results.length, 1, 'pages');
    expectEqual(results[0].hits.length, 3, 'hits');
    expectEqual(results[0].page.id, 'doc-b', 'page');
    check(results[0].score > results[0].hits[0].score, 'page score includes bonus for more hits');
  }],
  ['topHits: at most n hits, grouped by page', () => {
    const top = topHits(search(INDEX, 'ung'), 3);
    const hitCount = top.reduce((sum, r) => sum + r.hits.length, 0);
    check(hitCount <= 3, `hit count ${hitCount}`);
    expectEqual(new Set(top.map((r) => r.page.id)).size, top.length, 'one group per page');
    const loops = topHits(search(INDEX, 'schleife'), 2);
    expectEqual([loops.length, loops[0].hits.map((h) => h.anchor)], [1, ['while', 'for']], 'two best hits of one page');
  }],
  ['snippet: ranges point to the original text', () => {
    expectEqual(marked(makeSnippet('Größe ist wichtig. Straße und Überblick sind Wörter.', ['uberblick'])), ['Überblick'], 'umlaut');
    expectEqual(marked(makeSnippet('Straße strasse x', ['strasse'])), ['Straße', 'strasse'], 'ß and plain');
    expectEqual(marked(makeSnippet('Maß für Größe: Straße', ['strasse'])), ['Straße'], 'ß before the match');
  }],
  ['snippet: long text is cut with ellipsis', () => {
    const text = `${'wort '.repeat(60)}ziel ${'wort '.repeat(60)}`;
    const snippet = makeSnippet(text, ['ziel'], 160);
    check(snippet.text.length <= 162, `length ${snippet.text.length}`);
    check(snippet.text.startsWith('…') && snippet.text.endsWith('…'), 'ellipsis at both ends');
    expectEqual(marked(snippet), ['ziel'], 'marked');
    const head = makeSnippet(`ziel ${'wort '.repeat(60)}`, ['ziel']);
    check(!head.text.startsWith('…') && head.text.endsWith('…'), 'only the end is cut');
  }],
  ['snippet: all occurrences marked, overlaps merged', () => {
    expectEqual(makeSnippet('aaa bbb aaa', ['aaa', 'aa']).ranges, [[0, 3], [8, 11]], 'ranges');
    const hit = search(INDEX, 'schleife')[0].hits[0];
    expectEqual(hit.headingRanges.map(([a, b]) => hit.heading.slice(a, b)), ['Schleife'], 'heading ranges');
  }],
];

export function runSearchSelfTests() {
  return TESTS.map(([name, fn]) => {
    try {
      fn();
      return { name, passed: true, detail: '' };
    } catch (e) {
      return { name, passed: false, detail: e instanceof Error ? e.message : String(e) };
    }
  });
}
