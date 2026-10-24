// Checks quiz data files and their references in MDX. Usage: node tools/check-quiz.mjs
import {readdirSync, readFileSync, existsSync, statSync} from 'node:fs';
import {join, dirname, relative} from 'node:path';
import {fileURLToPath} from 'node:url';

const root = join(dirname(fileURLToPath(import.meta.url)), '..');
const website = join(root, 'website');
const quizDir = join(website, 'src/data/quiz');

const TYPES = ['single', 'multi', 'output', 'explain'];
const failures = [];
const warnings = [];

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
      q.options.forEach((o, i) => !o.text && errors.push(`${w}: option ${i} has no text`));
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
  if (q.type === 'explain' && !q.modelAnswer) errors.push(`${w}: explain needs modelAnswer`);
  if (['single', 'multi', 'output'].includes(q.type) && !q.explanation) errors.push(`${w}: explanation is required`);
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

// 2. MDX references
const mdxFiles = [...walk(join(website, 'docs'), '.mdx'), ...walk(join(website, 'src/pages'), '.mdx')];
const mdxErrors = [];
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
  for (const m of text.matchAll(/<Misconception id="([^"]+)"\s*\/>/g)) anchors.add(m[1]);
  const lesson = /^lesson_id:\s*(\S+)/m.exec(text);
  if (lesson) lessonIds.add(lesson[1]);
}
check('MDX quiz references', mdxErrors);

// 3. Misconception links (DoD 11)
const referenced = new Set();
questions
  .filter((q) => q.locale === 'de')
  .forEach((q) => (q.options ?? []).forEach((o) => o.misconception && referenced.add(o.misconception)));
const linkErrors = [];
for (const a of anchors) if (!referenced.has(a)) linkErrors.push(`<Misconception id="${a}"> is not referenced by any option`);
for (const r of referenced) if (!anchors.has(r)) linkErrors.push(`misconception "${r}" has no anchor in MDX`);
check('misconception links', linkErrors);

// 4. Lessons (warning only)
for (const q of questions.filter((x) => x.locale === 'de')) {
  if (!q.lesson?.startsWith('dev-') && !lessonIds.has(q.lesson)) warnings.push(`${q.id}: lesson ${q.lesson} has no lesson_id in docs`);
}
[...new Set(warnings)].forEach((w) => console.log(`WARN ${w}`));

process.exit(failures.length > 0 ? 1 : 0);
