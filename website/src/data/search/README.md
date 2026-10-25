# Search data

The plugin `website/plugins/search-index.js` builds the search index at build time and writes it with `actions.createData('search-index.json', …)`. One index per locale.

## What is indexed

- **Docs:** every `.md`/`.mdx` under `website/docs/` (not `draft` or `unlisted`), split at `##` and `###` headings. Text before the first `##` (plus the front matter `description`) is the section with anchor `""`. Imports, JSX tags, `{…}` expressions and fenced code are removed.
- **Glossary:** `website/src/data/glossary/<locale>.json`, one section per term (anchor = `id`, text = definition + aliases).
- **Static pages:** `website/src/data/search/<locale>.json` (this folder), for pages that are not MDX (Lernstand, Playground).

## What is not indexed

- **Quiz questions:** their texts and answers would reveal solutions in the search results.
- **Code examples:** they come from `.cs` files and are not page text.
- **Dev pages** (`/dev/…`).

## Format of `<locale>.json`

```json
{ "pages": [ { "id": "lernstand", "url": "/dashboard", "title": "Lernstand",
  "sections": [ { "anchor": "heute", "heading": "Heute", "text": "…" } ] } ] }
```

| Field | Rule |
| --- | --- |
| `id` | Unique; the index uses `page:<id>` |
| `url` | Relative to the site root, without `baseUrl` |
| `anchor` | Real `id` of an element on the page; `""` = top of the page |
| `heading`, `text` | Shown and searched text; only what the page really shows |

The file for a locale falls back to `de.json` if it is missing.

## Format of the generated index

```json
{ "version": 1, "locale": "de", "pages": [ { "id": "doc:<docId>", "kind": "doc|glossary|page",
  "docId": "<docId>|null", "url": "<path>|null", "title": "…", "context": "…",
  "sections": [ { "anchor": "", "heading": "…", "text": "…" } ] } ] }
```

- `url` is `null` for docs; the client resolves it from `docId` (`useAllDocsData`).
- `context` is the module label from `_category_.json` (docs only, else empty).
- Order: docs in sidebar order, then the glossary, then static pages in file order.
- The client loads it with `import('@generated/search-index/default/search-index.json')`.
