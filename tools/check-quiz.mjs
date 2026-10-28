// Checks quiz data files and their references in MDX. Usage: node tools/check-quiz.mjs
import {readdirSync, readFileSync, existsSync, statSync} from 'node:fs';
import {join, dirname, relative} from 'node:path';
import {fileURLToPath} from 'node:url';

const root = join(dirname(fileURLToPath(import.meta.url)), '..');
const website = join(root, 'website');
const quizDir = join(website, 'src/data/quiz');

const TYPES = ['single', 'multi', 'output', 'explain', 'parsons', 'cloze', 'find-bug', 'match'];
const failures = [];
const warnings = [];

// Forbidden attribution phrases (concept/sources.md, "Wording rules"). Keep in sync with the table there.
const FORBIDDEN_PHRASES = [
  /\blaut\s+(Microsoft|Quelle|derselben\s+Seite|Spezifikation|Presseberichten|Support-Policy|Dokumentation)/gi,
  /\bdie\s+Quelle\s+sagt/gi,
  /\bMicrosoft(\s+Learn)?\s+(schreibt|beschreibt|nennt|rät|sagt)\b/gi,
  /\bgemäß\s+(der\s+)?Dokumentation/gi,
];
// true: forbidden phrases are errors (exit code 1). Set by #126 after the module 0 cleanup.
const WORDING_IS_ERROR = true;

// Markup of the removed language-comparison box (#127, concept/style-and-language.md). Must not come back.
const REMOVED_LANGUAGE_BOX = [
  /<FromLanguage\b/g,
  /^\s*#{1,6}\s*Kommst du aus\b/gi,
];

// Line-based on the raw text, so the line number is exact (also for JSON).
function findForbiddenPhrases(file) {
  const found = [];
  readFileSync(file, 'utf8')
    .split(/\r?\n/)
    .forEach((line, i) => {
      for (const pattern of FORBIDDEN_PHRASES) {
        for (const m of line.matchAll(pattern)) {
          found.push(`${relative(root, file)}:${i + 1}: forbidden phrase "${m[0]}" (concept/sources.md)`);
        }
      }
    });
  return found;
}

function findRemovedLanguageBox(file) {
  const found = [];
  readFileSync(file, 'utf8')
    .split(/\r?\n/)
    .forEach((line, i) => {
      for (const pattern of REMOVED_LANGUAGE_BOX) {
        for (const m of line.matchAll(pattern)) {
          found.push(`${relative(root, file)}:${i + 1}: removed language-comparison box "${m[0].trim()}" (concept/style-and-language.md)`);
        }
      }
    });
  return found;
}

function walk(dir, ext, out = []) {
  for (const name of readdirSync(dir)) {
    const p = join(dir, name);
    if (statSync(p).isDirectory()) walk(p, ext, out);
    else if (p.endsWith(ext)) out.push(p);
  }
  return out;
}

// Same rules as validateQuestion in website/src/data/quiz/index.js (duplicated to stay dependency-free).
function validate(q) {
  const errors = [];
  const w = `Question "${q.id}"`;
  if (!q.id) errors.push('missing id');
  if (!/^[a-z0-9]+-l\d+$/.test(q.lesson ?? '')) errors.push(`${w}: invalid lesson`);
  else if (!String(q.id).startsWith(`${q.lesson}-q`)) errors.push(`${w}: id must start with "${q.lesson}-q"`);
  if (!TYPES.includes(q.type)) errors.push(`${w}: unknown type "${q.type}"`);
  if (!q.question) errors.push(`${w}: missing question`);
  if (q.type === 'single' || q.type === 'multi') {
    if (!Array.isArray(q.options) || q.options.length < 2) errors.push(`${w}: needs at least 2 options`);
    else {
      q.options.forEach((o, i) => {
        if (!o.text) errors.push(`${w}: option ${i} has no text`);
        if (o.pin !== undefined && o.pin !== 'last') errors.push(`${w}: option ${i}: pin must be "last"`);
      });
      if (q.options.every((o) => o.pin === 'last')) errors.push(`${w}: at least one option must not be pinned`);
      const correct = q.options.filter((o) => o.correct === true).length;
      if (q.type === 'single' && correct !== 1) errors.push(`${w}: single needs exactly one correct option`);
      if (q.type === 'multi' && correct < 1) errors.push(`${w}: multi needs at least one correct option`);
    }
  }
  if (q.type === 'output') {
    if (!q.example) errors.push(`${w}: output needs example`);
    else {
      const cs = join(website, 'examples', q.example);
      if (!existsSync(cs)) errors.push(`${w}: example ${q.example} not found`);
      if (!existsSync(cs.replace(/\.cs$/, '.out.txt'))) errors.push(`${w}: snapshot .out.txt missing`);
    }
  }
  if (q.type === 'parsons') {
    if (!q.example) errors.push(`${w}: parsons needs example`);
    else {
      const cs = join(website, 'examples', q.example);
      if (!existsSync(cs)) errors.push(`${w}: example ${q.example} not found`);
      else {
        if (!existsSync(cs.replace(/\.cs$/, '.out.txt'))) errors.push(`${w}: snapshot .out.txt missing`);
        const lines = readFileSync(cs, 'utf8').split(/\r?\n/).map((l) => l.trim()).filter(Boolean);
        for (const d of q.distractors ?? []) {
          if (lines.includes(String(d).trim())) errors.push(`${w}: distractor "${d}" is identical to a solution line`);
        }
        for (const key of Object.keys(q.distractorMisconceptions ?? {})) {
          if (!(q.distractors ?? []).includes(key)) errors.push(`${w}: distractorMisconceptions key "${key}" is not a distractor`);
        }
      }
    }
  }
  if (q.type === 'cloze') {
    if (!q.example) errors.push(`${w}: cloze needs example`);
    else if (!Array.isArray(q.blanks) || q.blanks.length < 1) errors.push(`${w}: cloze needs at least one blank`);
    else if (q.blanks.some((b) => !b || typeof b.text !== 'string' || !b.text)) errors.push(`${w}: every blank needs a text`);
    else {
      const cs = join(website, 'examples', q.example);
      if (!existsSync(cs)) errors.push(`${w}: example ${q.example} not found`);
      else {
        if (!existsSync(cs.replace(/\.cs$/, '.out.txt'))) errors.push(`${w}: snapshot .out.txt missing`);
        // Same search rule as splitCloze in website/src/data/quiz/cloze.js.
        const code = readFileSync(cs, 'utf8').replace(/\r\n?/g, '\n').replace(/\n+$/, '');
        let position = 0;
        q.blanks.forEach((b, i) => {
          const found = code.indexOf(b.text, position);
          if (found < 0) errors.push(`${w}: blank ${i + 1} "${b.text}" not found in ${q.example}`);
          else position = found + b.text.length;
        });
      }
    }
  }
  if (q.type === 'find-bug') {
    if (!q.example) errors.push(`${w}: find-bug needs example`);
    else if (!Array.isArray(q.bugLines) || q.bugLines.length < 1 || q.bugLines.some((n) => !Number.isInteger(n))) {
      errors.push(`${w}: find-bug needs bugLines (at least one line number)`);
    } else {
      const cs = join(website, 'examples', q.example);
      if (!existsSync(cs)) errors.push(`${w}: example ${q.example} not found`);
      else {
        if (!q.example.endsWith('.snippet.cs') && !existsSync(cs.replace(/\.cs$/, '.out.txt'))) errors.push(`${w}: snapshot .out.txt missing`);
        // Same line rule as findBugLines in website/src/data/quiz/findBug.js.
        const count = readFileSync(cs, 'utf8').replace(/\r\n?/g, '\n').replace(/\n+$/, '').split('\n').length;
        if (q.bugLines.some((n) => n < 1 || n > count)) errors.push(`${w}: bugLines must be within 1..${count}`);
        if (new Set(q.bugLines).size !== q.bugLines.length) errors.push(`${w}: bugLines has duplicates`);
        for (const k of Object.keys(q.lineFeedback ?? {})) {
          if (!/^\d+$/.test(k) || Number(k) < 1 || Number(k) > count) errors.push(`${w}: lineFeedback key "${k}" is not a line within 1..${count}`);
        }
      }
    }
  }
  if (q.type === 'match') {
    const pairs = q.pairs;
    if (!Array.isArray(pairs) || pairs.length < 2 || pairs.length > 8) errors.push(`${w}: match needs 2 to 8 pairs`);
    else if (pairs.some((p) => !p || typeof p.left !== 'string' || !p.left || typeof p.right !== 'string' || !p.right)) {
      errors.push(`${w}: every pair needs left and right`);
    } else {
      if (new Set(pairs.map((p) => p.left)).size !== pairs.length) errors.push(`${w}: left values must be unique`);
      if (new Set(pairs.map((p) => p.right)).size !== pairs.length) errors.push(`${w}: right values must be unique`);
      if (q.wrongMatches !== undefined && !Array.isArray(q.wrongMatches)) errors.push(`${w}: wrongMatches must be a list`);
      (Array.isArray(q.wrongMatches) ? q.wrongMatches : []).forEach((m, i) => {
        if (!m || !pairs.some((p) => p.left === m.left)) errors.push(`${w}: wrongMatches ${i} has an unknown left`);
        else if (!pairs.some((p) => p.right === m.right)) errors.push(`${w}: wrongMatches ${i} has an unknown right`);
        else if (pairs.some((p) => p.left === m.left && p.right === m.right)) errors.push(`${w}: wrongMatches ${i} is a correct pair`);
      });
    }
  }
  if (q.type === 'explain' && !q.modelAnswer) errors.push(`${w}: explain needs modelAnswer`);
  if (['single', 'multi', 'output', 'parsons', 'cloze', 'find-bug', 'match'].includes(q.type) && !q.explanation) errors.push(`${w}: explanation is required`);
  return errors;
}

// Same rules as validateGlossary in website/src/data/glossary/index.js (duplicated to stay dependency-free).
// Phrases that refer to the shown position of an option (German quiz texts). Case-insensitive.
const POSITION_PHRASES = [
  /\b(Antwort|Option|Möglichkeit|Auswahl|Aussage)\s+[A-F1-9]\b/i,
  /\b(erste|zweite|dritte|vierte|fünfte|letzte|obere|untere|vorige|vorherige|nächste|obige|untenstehende|obenstehende)[nrsm]?\s+(Antwort|Option|Möglichkeit|Auswahl|Aussage)/i,
  /\b(beide|alle|die)\s+(oben|unten)\b/i,
  /\b(oben|unten)\s+(genannt|stehend|aufgeführt)/i,
];
// Summary options: allowed only with "pin": "last".
const SUMMARY_OPTION = /\b(keine|alle)\s+(der\s+)?(genannten|obigen|anderen)\b|\bkeine\s+davon\b/i;

// Returns a list of messages "<id>: <field>: <text excerpt>" for one question.
function positionProblems(q) {
  const problems = [];
  const texts = [];
  (q.options ?? []).forEach((o, i) => {
    texts.push([`options[${i}].text`, o?.text]);
    texts.push([`options[${i}].feedback`, o?.feedback]);
  });
  texts.push(['explanation', q.explanation]);
  (q.wrongMatches ?? []).forEach((m, i) => texts.push([`wrongMatches[${i}].feedback`, m?.feedback]));
  for (const [field, text] of texts) {
    if (typeof text !== 'string') continue;
    for (const re of POSITION_PHRASES) {
      const m = text.match(re);
      if (m) problems.push(`${q.id}: ${field}: position reference "${m[0]}"`);
    }
  }
  if (q.type === 'single' || q.type === 'multi') {
    (q.options ?? []).forEach((o, i) => {
      if (typeof o?.text === 'string' && SUMMARY_OPTION.test(o.text) && o.pin !== 'last') {
        problems.push(`${q.id}: options[${i}].text: summary option needs "pin": "last"`);
      }
    });
  }
  return problems;
}

function validateGlossary(terms) {
  if (!Array.isArray(terms)) return ['Glossary "terms" is not a list'];
  const errors = [];
  const isText = (v) => typeof v === 'string' && v.trim() !== '';
  const ids = new Set(terms.map((t) => t?.id).filter((i) => typeof i === 'string'));
  const seen = new Set();
  for (const t of terms) {
    if (!t || typeof t !== 'object') {
      errors.push('Glossary entry is not an object');
      continue;
    }
    const w = `Term "${t.id}"`;
    if (typeof t.id !== 'string' || !/^[a-z0-9]+(-[a-z0-9]+)*$/.test(t.id)) errors.push(`${w}: invalid id`);
    else if (seen.has(t.id)) errors.push(`${w}: duplicate id`);
    else seen.add(t.id);
    if (!isText(t.term)) errors.push(`${w}: missing term`);
    if (t.german !== undefined && !isText(t.german)) errors.push(`${w}: german must be a non-empty string`);
    if (!['language', 'platform', 'course'].includes(t.category)) errors.push(`${w}: unknown category "${t.category}"`);
    if (!isText(t.definition)) errors.push(`${w}: missing definition`);
    else if (t.definition.length > 400) errors.push(`${w}: definition is longer than 400 characters`);
    if (t.aliases !== undefined && (!Array.isArray(t.aliases) || t.aliases.some((a) => !isText(a)))) {
      errors.push(`${w}: aliases must be a list of non-empty strings`);
    }
    if (t.related !== undefined) {
      if (!Array.isArray(t.related)) errors.push(`${w}: related must be a list`);
      else {
        for (const r of t.related) {
          if (r === t.id) errors.push(`${w}: related contains the entry itself`);
          else if (!ids.has(r)) errors.push(`${w}: related id "${r}" does not exist`);
        }
      }
    }
    if (t.lesson !== undefined && (typeof t.lesson !== 'string' || !/^[a-z0-9]+-l\d+$/.test(t.lesson))) errors.push(`${w}: invalid lesson`);
    const s = t.source;
    if (s === undefined) {
      if (t.category === 'language' || t.category === 'platform') errors.push(`${w}: source is required for category "${t.category}"`);
    } else if (!s || !isText(s.title) || !isText(s.url)) errors.push(`${w}: source needs title and url`);
    else if (!s.url.startsWith('https://learn.microsoft.com/')) errors.push(`${w}: source url must start with https://learn.microsoft.com/`);
  }
  return errors;
}

function check(name, errors) {
  if (errors.length === 0) console.log(`OK   ${name}`);
  else {
    console.log(`FAIL ${name}`);
    errors.forEach((e) => console.log(`       ${e}`));
    failures.push(name);
  }
}

// 1. Data files
const dataErrors = [];
const idsByLocale = {};
const questions = [];
for (const locale of readdirSync(quizDir).filter((n) => statSync(join(quizDir, n)).isDirectory())) {
  const ids = (idsByLocale[locale] = new Set());
  for (const file of walk(join(quizDir, locale), '.json')) {
    let data;
    try {
      data = JSON.parse(readFileSync(file, 'utf8'));
    } catch (e) {
      dataErrors.push(`${relative(root, file)}: ${e.message}`);
      continue;
    }
    for (const q of data.questions ?? []) {
      if (ids.has(q.id)) dataErrors.push(`${locale}: duplicate id ${q.id}`);
      ids.add(q.id);
      dataErrors.push(...validate(q));
      questions.push({...q, locale});
    }
  }
}
check('quiz data', dataErrors);

// 1b. Glossary data
const glossaryDir = join(website, 'src/data/glossary');
const glossaryErrors = [];
const glossaryLessons = [];
const glossaryIds = new Set();
for (const file of readdirSync(glossaryDir).filter((n) => n.endsWith('.json'))) {
  let data;
  try {
    data = JSON.parse(readFileSync(join(glossaryDir, file), 'utf8'));
  } catch (e) {
    glossaryErrors.push(`${file}: ${e.message}`);
    continue;
  }
  glossaryErrors.push(...validateGlossary(data.terms).map((e) => `${file}: ${e}`));
  for (const t of Array.isArray(data.terms) ? data.terms : []) {
    if (typeof t?.id === 'string' && file === 'de.json') glossaryIds.add(t.id);
    if (t?.lesson) glossaryLessons.push({id: t.id, lesson: t.lesson});
  }
}
check('glossary data', glossaryErrors);

// 2. MDX references
const mdxFiles = [...walk(join(website, 'docs'), '.mdx'), ...walk(join(website, 'src/pages'), '.mdx')];
const mdxErrors = [];
const glossaryRefErrors = [];
const anchors = new Set();
const lessonIds = new Set();
for (const file of mdxFiles) {
  const text = readFileSync(file, 'utf8');
  const rel = relative(root, file);
  for (const m of text.matchAll(/<Quiz\b[^>]*?(\/?)>/gs)) {
    const exact = /^<Quiz id="([^"]+)" \/>$/.exec(m[0]);
    if (!exact) mdxErrors.push(`${rel}: <Quiz> must have the form <Quiz id="…" />: ${m[0].slice(0, 60)}`);
    else if (!idsByLocale.de?.has(exact[1])) mdxErrors.push(`${rel}: unknown quiz id ${exact[1]}`);
  }
  for (const m of text.matchAll(/<Term\s+id="([^"]+)"/g)) {
    if (!glossaryIds.has(m[1])) glossaryRefErrors.push(`${rel}: unknown glossary id ${m[1]}`);
  }
  for (const m of text.matchAll(/<Misconception id="([^"]+)"\s*\/>/g)) anchors.add(m[1]);
  const lesson = /^lesson_id:\s*(\S+)/m.exec(text);
  if (lesson) lessonIds.add(lesson[1]);
}
check('MDX quiz references', mdxErrors);
check('glossary references', glossaryRefErrors);

// 3. Misconception links (DoD 11)
const referenced = new Set();
questions
  .filter((q) => q.locale === 'de')
  .forEach((q) => {
    (q.options ?? []).forEach((o) => o.misconception && referenced.add(o.misconception));
    Object.values(q.distractorMisconceptions ?? {}).forEach((m) => referenced.add(m));
    (q.wrongMatches ?? []).forEach((m) => m.misconception && referenced.add(m.misconception));
    Object.values(q.lineFeedback ?? {}).forEach((l) => l.misconception && referenced.add(l.misconception));
    (q.blanks ?? []).forEach((b) => (b.wrongAnswers ?? []).forEach((w) => w.misconception && referenced.add(w.misconception)));
  });
const linkErrors = [];
for (const a of anchors) if (!referenced.has(a)) linkErrors.push(`<Misconception id="${a}"> is not referenced by any option`);
for (const r of referenced) if (!anchors.has(r)) linkErrors.push(`misconception "${r}" has no anchor in MDX`);
check('misconception links', linkErrors);

// 4. Lessons (warning only)
for (const q of questions.filter((x) => x.locale === 'de')) {
  if (!q.lesson?.startsWith('dev-') && !lessonIds.has(q.lesson)) warnings.push(`${q.id}: lesson ${q.lesson} has no lesson_id in docs`);
}
for (const g of glossaryLessons) {
  if (!lessonIds.has(g.lesson)) warnings.push(`glossary ${g.id}: lesson ${g.lesson} has no lesson_id in docs`);
}

// 4b. Position phrases (options are shown in random order)
const positionErrors = questions.filter((x) => x.locale === 'de').flatMap(positionProblems);
const fixtureFile = join(root, 'tools/fixtures/quiz-position-phrases.json');
const fixtureQuestions = JSON.parse(readFileSync(fixtureFile, 'utf8')).questions ?? [];
if (fixtureQuestions.flatMap(positionProblems).length === 0) {
  positionErrors.push('position phrase self-test: fixture not detected');
}
check('quiz position phrases', positionErrors);

// 5. Wording (concept/sources.md)
const wordingFiles = [
  ...walk(join(website, 'docs'), '.mdx'),
  ...walk(join(quizDir, 'de'), '.json'),
  join(website, 'src/data/glossary/de.json'),
];
const wordingFindings = wordingFiles.flatMap(findForbiddenPhrases);
if (WORDING_IS_ERROR) check('wording', wordingFindings);
else {
  warnings.push(...wordingFindings);
  if (wordingFindings.length === 0) console.log('OK   wording');
}

// 6. No language comparison box (concept/style-and-language.md)
const boxFiles = ['docs', 'src/pages'].flatMap((d) =>
  ['.md', '.mdx', '.js'].flatMap((ext) => walk(join(website, d), ext)),
);
check('no language comparison box', boxFiles.flatMap(findRemovedLanguageBox));

[...new Set(warnings)].forEach((w) => console.log(`WARN ${w}`));

process.exit(failures.length > 0 ? 1 : 0);
