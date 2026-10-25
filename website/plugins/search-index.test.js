const test = require('node:test');
const assert = require('node:assert');
const path = require('path');
const {toPlainText, extractSections, glossarySections, buildSearchIndex} = require('./search-index');

test('toPlainText keeps the inner text of tags', () => {
  assert.strictEqual(toPlainText('Ein <Term id="x">Teilziel</Term> hier'), 'Ein Teilziel hier');
});

test('toPlainText removes self-closing tags', () => {
  assert.strictEqual(toPlainText('a\n<Quiz id="x" />\nb'), 'a b');
  assert.strictEqual(toPlainText('a <CSharpRunner code={a} height={3} /> b'), 'a b');
});

test('toPlainText handles links, code, admonitions, tables', () => {
  assert.strictEqual(toPlainText('Siehe [Doku](https://x.y) und `int`'), 'Siehe Doku und int');
  assert.strictEqual(toPlainText(':::note[Lernziele]\n- Du kannst **etwas**\n:::'), 'Lernziele Du kannst etwas');
  assert.strictEqual(toPlainText('| A | B |\n| --- | --- |\n| 1 | 2 |'), 'A B 1 2');
});

test('toPlainText removes imports and fenced code', () => {
  assert.strictEqual(toPlainText("import x from 'y';\ntext\n```csharp\nvar a = 1;\n```\nend"), 'text end');
});

test('extractSections splits at headings', () => {
  const s = extractSections('# Titel\nIntro\n## Zwei Stolperfallen\nText\n### `if` und `else`\nMehr', 'Titel');
  assert.deepStrictEqual(
    s.map((x) => x.anchor),
    ['', 'zwei-stolperfallen', 'if-und-else'],
  );
  assert.strictEqual(s[0].heading, 'Titel');
  assert.strictEqual(s[0].text, 'Intro');
  assert.strictEqual(s[2].heading, 'if und else');
});

test('extractSections handles duplicates, custom ids and fences', () => {
  const s = extractSections('## X\na\n## X\nb\n## Y {#custom}\nc\n```\n## Not a heading\n```', 'T');
  assert.deepStrictEqual(
    s.map((x) => x.anchor),
    ['', 'x', 'x-1', 'custom'],
  );
  assert.strictEqual(s[3].heading, 'Y');
  assert.strictEqual(s[3].text, 'c');
});

test('glossarySections', () => {
  const s = glossarySections([
    {id: 'value-type', term: 'Value Type', german: 'Werttyp', definition: 'Ein `int`.', aliases: ['Werttypen']},
    {id: 'leitner', term: 'Leitner-System', definition: 'Karten.'},
  ]);
  assert.deepStrictEqual(s[0], {anchor: 'value-type', heading: 'Value Type (Werttyp)', text: 'Ein int. Werttypen'});
  assert.strictEqual(s[1].heading, 'Leitner-System');
});

test('buildSearchIndex on the real site', () => {
  const index = buildSearchIndex({siteDir: path.join(__dirname, '..'), locale: 'de'});
  const ids = index.pages.map((p) => p.id);
  assert.ok(ids.includes('doc:m01-variables-and-types/l03-numbers'));
  assert.ok(ids.includes('glossary'));
  assert.ok(ids.includes('page:lernstand'));
  assert.strictEqual(new Set(ids).size, ids.length);
});
