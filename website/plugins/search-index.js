// Local Docusaurus plugin: builds the search index at build time.
// One JSON file (`search-index.json`) with all searchable pages split into sections with
// anchors: docs (from the MDX source), glossary and static pages (src/data/search/<locale>.json).
// Quiz questions and code are not indexed. Format: src/data/search/README.md.
const fs = require('fs');
const path = require('path');
const {createSlugger} = require('@docusaurus/utils');
const {listDocs, parseFrontMatter, readCategory} = require('./lesson-index');

const FENCE = /^\s*(```|~~~)/;

// Attribute part of a JSX tag: quoted strings, `{…}` expressions (one nesting level) or other characters.
const ATTRS = String.raw`(?:\s+(?:"[^"]*"|'[^']*'|\{(?:[^{}]|\{[^{}]*\})*\}|[^>"'{}])*)?`;
const SELF_CLOSING_TAG = new RegExp(String.raw`<[A-Za-z][\w.]*${ATTRS}/>`, 'g');
const TAG = new RegExp(String.raw`</?[A-Za-z][\w.]*${ATTRS}>`, 'g');

// Removes fenced code blocks. Returns the remaining lines.
function stripFences(text) {
  const out = [];
  let fence = null;
  for (const line of text.split(/\r?\n/)) {
    const m = FENCE.exec(line);
    if (fence) {
      if (m && m[1] === fence) fence = null;
      continue;
    }
    if (m) {
      fence = m[1];
      continue;
    }
    out.push(line);
  }
  return out;
}

// Removes `{…}` expressions, including nested braces.
function stripBraces(text) {
  let out = '';
  let depth = 0;
  for (const ch of text) {
    if (ch === '{') depth++;
    else if (ch === '}') {
      if (depth > 0) depth--;
      else out += ch;
    } else if (depth === 0) out += ch;
  }
  return out;
}

// Markdown/MDX text -> plain text.
function toPlainText(mdx) {
  let text = stripFences(mdx)
    .filter((l) => !/^\s*(import|export)\s/.test(l))
    .join('\n');
  // Self-closing tags first, then the remaining open/close tags (their inner text stays).
  text = text.replace(SELF_CLOSING_TAG, ' ').replace(TAG, ' ');
  text = stripBraces(text);
  text = text
    .split('\n')
    .map((line) => {
      let l = line.trim();
      const adm = /^:::\w*(?:\[([^\]]*)\])?.*$/.exec(l);
      if (adm) return adm[1] || '';
      if (/^\|?[\s:|-]+\|?$/.test(l) && l.includes('-')) return '';
      l = l.replace(/^>+\s?/, '').replace(/^([-*+]|\d+[.)])\s+/, '');
      return l.replace(/\|/g, ' ');
    })
    .join('\n');
  return text
    .replace(/!\[[^\]]*\]\([^)]*\)/g, ' ')
    .replace(/\[([^\]]*)\]\([^)]*\)/g, '$1')
    .replace(/`+/g, '')
    .replace(/(\*\*|__|\*)/g, '')
    .replace(/(^|[\s(])_+([^_]+?)_+(?=[\s).,;:!?]|$)/g, '$1$2')
    .replace(/\s+/g, ' ')
    .trim();
}

// MDX body (front matter removed) -> sections split at `##` and `###`.
function extractSections(body, pageTitle) {
  const slugger = createSlugger();
  const sections = [];
  let current = {anchor: '', heading: pageTitle, lines: []};
  let fence = null;
  for (const line of body.split(/\r?\n/)) {
    const f = FENCE.exec(line);
    if (f) {
      if (!fence) fence = f[1];
      else if (f[1] === fence) fence = null;
      current.lines.push(line);
      continue;
    }
    const h = !fence && /^(#{1,3})\s+(.*?)\s*#*\s*$/.exec(line);
    if (h) {
      if (h[1].length === 1) continue; // the title
      sections.push(current);
      let raw = h[2];
      let id = null;
      const custom = /\s*\{#([^}\s]+)\}\s*$/.exec(raw);
      if (custom) {
        id = custom[1];
        raw = raw.slice(0, custom.index);
      }
      const heading = toPlainText(raw);
      current = {anchor: id || slugger.slug(heading), heading, lines: []};
      continue;
    }
    current.lines.push(line);
  }
  sections.push(current);
  return sections
    .map((s) => ({anchor: s.anchor, heading: s.heading, text: toPlainText(s.lines.join('\n'))}))
    .filter((s) => s.heading || s.text);
}

function glossarySections(terms) {
  return terms.map((t) => {
    const heading = t.german ? `${t.term} (${t.german})` : t.term;
    const text = [toPlainText(t.definition || ''), ...(t.aliases || [])].filter(Boolean).join(' ');
    return {anchor: t.id, heading, text};
  });
}

// Reads `<dir>/<locale>.json`, falls back to `de.json`.
function readLocaleJson(dir, locale) {
  let file = path.join(dir, `${locale}.json`);
  if (!fs.existsSync(file)) file = path.join(dir, 'de.json');
  return JSON.parse(fs.readFileSync(file, 'utf8'));
}

function docPages(docsDir) {
  const categories = new Map();
  const docs = [];
  for (const file of listDocs(docsDir)) {
    const {frontMatter, body} = parseFrontMatter(fs.readFileSync(file, 'utf8'));
    if (frontMatter.draft === 'true' || frontMatter.unlisted === 'true') continue;
    const rel = path.relative(docsDir, file).split(path.sep).join('/');
    const dirName = path.dirname(rel);
    const docId = (dirName === '.' ? '' : `${dirName}/`) + (frontMatter.id || path.basename(rel, path.extname(rel)));
    const h1 = /^#\s+(.+?)\s*$/m.exec(stripFences(body).join('\n'));
    const title =
      typeof frontMatter.title === 'string' && frontMatter.title ? frontMatter.title : h1 ? toPlainText(h1[1]) : docId;
    const category = readCategory(path.dirname(file), categories);
    const sections = extractSections(body, title);
    if (sections.length === 0 || sections[0].anchor !== '') sections.unshift({anchor: '', heading: title, text: ''});
    if (typeof frontMatter.description === 'string' && frontMatter.description) {
      sections[0].text = `${frontMatter.description} ${sections[0].text}`.trim();
    }
    docs.push({
      page: {
        id: `doc:${docId}`,
        kind: 'doc',
        docId,
        url: null,
        title,
        context: category && typeof category.label === 'string' ? category.label : '',
        sections,
      },
      module: category && typeof category.position === 'number' ? category.position : Number.MAX_SAFE_INTEGER,
      position: typeof frontMatter.sidebar_position === 'number' ? frontMatter.sidebar_position : Number.MAX_SAFE_INTEGER,
    });
  }
  // Same order as the autogenerated sidebar (like lesson-index).
  docs.sort((a, b) => a.module - b.module || a.position - b.position || a.page.docId.localeCompare(b.page.docId));
  return docs.map((d) => d.page);
}

function buildSearchIndex({siteDir, locale}) {
  const pages = docPages(path.join(siteDir, 'docs'));
  const glossary = readLocaleJson(path.join(siteDir, 'src/data/glossary'), locale);
  pages.push({
    id: 'glossary',
    kind: 'glossary',
    docId: null,
    url: '/glossary',
    title: 'Glossar',
    context: '',
    sections: glossarySections(glossary.terms || []),
  });
  const statics = readLocaleJson(path.join(siteDir, 'src/data/search'), locale);
  for (const p of statics.pages || []) {
    pages.push({
      id: `page:${p.id}`,
      kind: 'page',
      docId: null,
      url: p.url,
      title: p.title,
      context: '',
      sections: p.sections || [],
    });
  }
  return {version: 1, locale, pages};
}

module.exports = function searchIndexPlugin(context) {
  const {siteDir} = context;
  return {
    name: 'search-index',
    async loadContent() {
      return buildSearchIndex({siteDir, locale: context.i18n.currentLocale});
    },
    async contentLoaded({content, actions}) {
      await actions.createData('search-index.json', JSON.stringify(content));
    },
    getPathsToWatch() {
      return [
        path.join(siteDir, 'docs/**/*.{md,mdx}'),
        path.join(siteDir, 'docs/**/_category_.json'),
        path.join(siteDir, 'src/data/glossary/*.json'),
        path.join(siteDir, 'src/data/search/*.json'),
      ];
    },
  };
};

module.exports.toPlainText = toPlainText;
module.exports.extractSections = extractSections;
module.exports.glossarySections = glossarySections;
module.exports.buildSearchIndex = buildSearchIndex;
