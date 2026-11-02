// Checks the fold markers in the examples and the SecLog continuity chain.
// Usage: node tools/check-seclog.mjs
//
// 1. Markers: every "// @fold-start" has a matching "// @fold-end" (no nesting, no empty fold,
//    no markers in m00).
// 2. Chain: the start code of every SecLog project step (seclog-*-task.cs) equals the previous
//    solution (seclog-*-solution.cs) once the marker lines are removed.
import {readdirSync, readFileSync, existsSync} from 'node:fs';
import {join, dirname, relative, sep} from 'node:path';
import {fileURLToPath} from 'node:url';

const root = join(dirname(fileURLToPath(import.meta.url)), '..');
const examples = join(root, 'website/examples');

// Task files that may differ from the previous solution. Key: 'mXX/lYY', value: reason.
// An entry that matches no task file is an error.
const CHAIN_EXCEPTIONS = {
  'm00/l01': 'first step of the course',
};

const FOLD_START = '// @fold-start';
const FOLD_END = '// @fold-end';
const isMarker = (line) => {
  const t = line.trim();
  return t === FOLD_START || t === FOLD_END;
};

let files = 0;
let failed = 0;
const fail = (msg) => {
  failed++;
  console.log(`FAIL  ${msg}`);
};
const rel = (p) => relative(root, p).split(sep).join('/');

function walk(dir, out = []) {
  for (const e of readdirSync(dir, {withFileTypes: true})) {
    if (e.name === 'bin' || e.name === 'obj') continue;
    const p = join(dir, e.name);
    if (e.isDirectory()) walk(p, out);
    else out.push(p);
  }
  return out;
}

const csFiles = walk(examples).filter((f) => f.endsWith('.cs'));

// 1. Markers
for (const file of csFiles) {
  files++;
  const lines = readFileSync(file, 'utf8').split(/\r?\n/);
  const inM00 = rel(file).startsWith('website/examples/m00/');
  let open = 0; // line number of the open fold, 0 = none
  let content = 0;
  lines.forEach((line, i) => {
    const n = i + 1;
    const t = line.trim();
    if (t !== FOLD_START && t !== FOLD_END) {
      if (open && t !== '') content++;
      return;
    }
    if (inM00) return fail(`${rel(file)}:${n}: fold markers are not allowed in module 0`);
    if (t === FOLD_START) {
      if (open) fail(`${rel(file)}:${n}: nested ${FOLD_START} (fold from line ${open} is still open)`);
      open = n;
      content = 0;
    } else if (!open) {
      fail(`${rel(file)}:${n}: ${FOLD_END} without ${FOLD_START}`);
    } else {
      if (content === 0) fail(`${rel(file)}:${open}: empty fold`);
      open = 0;
    }
  });
  if (open) fail(`${rel(file)}:${open}: ${FOLD_START} without ${FOLD_END}`);
}

// 2. Chain
const stepRe = /^website\/examples\/m(\d+)\/l(\d+)\/(seclog-.+)-(task|solution)\.cs$/;
const steps = [];
for (const file of csFiles) {
  const m = rel(file).match(stepRe);
  if (m) steps.push({file, mod: +m[1], les: +m[2], key: `m${m[1]}/l${m[2]}`, kind: m[4]});
}
steps.sort((a, b) => a.mod - b.mod || a.les - b.les || a.file.localeCompare(b.file));

const norm = (file, isCs) => {
  const lines = readFileSync(file, 'utf8').replace(/\r\n/g, '\n').split('\n');
  return isCs ? lines.filter((l) => !isMarker(l)) : lines;
};
function firstDiff(a, b) {
  const len = Math.max(a.length, b.length);
  for (let i = 0; i < len; i++) if (a[i] !== b[i]) return i + 1;
  return 0;
}

const usedExceptions = new Set();
for (const task of steps.filter((s) => s.kind === 'task')) {
  const prev = steps
    .filter((s) => s.kind === 'solution' && (s.mod < task.mod || (s.mod === task.mod && s.les < task.les)))
    .at(-1);
  if (CHAIN_EXCEPTIONS[task.key]) usedExceptions.add(task.key);
  if (!prev || CHAIN_EXCEPTIONS[task.key]) continue;
  const pairs = [[task.file, prev.file, true]];
  const taskIn = task.file.replace(/\.cs$/, '.in.txt');
  const prevIn = prev.file.replace(/\.cs$/, '.in.txt');
  if (existsSync(taskIn) && existsSync(prevIn)) pairs.push([taskIn, prevIn, false]);
  for (const [t, p, isCs] of pairs) {
    files++;
    const line = firstDiff(norm(t, isCs), norm(p, isCs));
    if (line) fail(`${rel(t)}: differs from ${rel(p)} (line ${line})`);
  }
}
for (const key of Object.keys(CHAIN_EXCEPTIONS)) {
  if (!usedExceptions.has(key)) fail(`CHAIN_EXCEPTIONS: '${key}' matches no task file`);
}

console.log(`SecLog check: ${files} files, ${failed} failed`);
process.exit(failed ? 1 : 0);
