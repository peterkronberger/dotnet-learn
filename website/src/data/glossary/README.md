# Glossary data

One file per language: `website/src/data/glossary/<locale>.json` (today only `de.json`). The page `/glossary` is generated from it. Each entry has the anchor `/glossary#<id>`.

Validation runs in two places: `validateGlossary()` in `index.js` (throws during the build) and a duplicated check in `tools/check-quiz.mjs` (CI, before the build).

## Format

```json
{ "terms": [ { "id": "value-type", "term": "Value Type", "german": "Werttyp", "category": "language", "definition": "…" } ] }
```

| Field | Required | Rule |
| --- | --- | --- |
| `id` | yes | English kebab-case, `^[a-z0-9]+(-[a-z0-9]+)*$`, unique |
| `term` | yes | Shown name. Technical terms in English (`Value Type`), course terms in German (`Leitner-System`) |
| `german` | no | German explanation, shown as "Value Type (Werttyp)". Leave out if `term` is already German |
| `category` | yes | `language` (C# language), `platform` (.NET, tools, runtime, libraries) or `course` (terms of this site) |
| `definition` | yes | German, 1–3 short sentences, at most 400 characters, backticks for code |
| `aliases` | no | List of non-empty strings (other spellings, plural forms); used by the search field |
| `related` | no | List of ids of other entries; each must exist and must not be the entry itself |
| `lesson` | no | `lesson_id` of the lesson that introduces the term (`^[a-z0-9]+-l\d+$`) |
| `source` | yes for `language`/`platform` | `{title, url}`, both non-empty, `url` starts with `https://learn.microsoft.com/` |

## Rules for writing entries

- The definition is German, 1–3 short sentences.
- Write nothing that the lessons do not teach.
- Technical statements need a source (`CLAUDE.md`, rule 7). Entries of category `course` need none.
- One entry per concept. Put plural forms and other spellings into `aliases`.
- Sorting and grouping use `sortKey()`: leading non-letters are ignored (`.NET` is sorted under N).
