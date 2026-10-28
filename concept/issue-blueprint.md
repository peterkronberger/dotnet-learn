# Issue blueprint – one module from its number

As of 2026-10-25. Input: **only the module number** (`0`–`19`, or `W1`–`W4` for a review module). Everything else is derived from `concept/module-plan.md` and the rule files. This file only says *where* each value comes from and *which* issues to create; it does not repeat the rules.

## 1. Start (human)

Create one issue and set the label `claude:auto-architect`:

```markdown
Title: Module <N>

Create module <N>. Follow concept/issue-blueprint.md.
```

The architect (`claude-architect.yml`) does the rest: it derives the content (section 2), creates the issues (section 3) and starts the implementation.

## 2. What comes from where

`<N>` = the input. `NN` = the number with two digits (`3` → `03`). Prefix `m` for modules, `w` for review modules (`W1` → `w01`).

| Value | Source / rule |
| --- | --- |
| Module section | The heading `### M<N> …` (or `### W<N> …`) in `module-plan.md` up to the next `###`. If it is missing: stop, status `needs_human`. |
| Module title (German) | The heading text after `M<N>`, without the part in parentheses and without ▶/◻. |
| Module folder | `<prefix>NN-<slug>`, slug = short English kebab-case name of the topic (from the English part in parentheses, otherwise from the heading). An existing folder name is kept if it fits this rule. Review modules: `wNN-review-<slug>` (`module-plan.md`, "Location and naming"). |
| `_category_.json` | Label "Modul <N> – <German title>" (review: "Wiederholung <N> – Module A–B"), `position` = `<N>` (review: decimal from "Location and naming"). |
| Lessons | Only from the plan, never from existing files (section 2a). In plan order: the topics of the first line of the section (separated by `·`) and every line `*(new)* Lesson "…"`. Every other `*(new)*` line and every `Spec`/`Library` item goes into the lesson it fits. Split or merge so that each lesson has one core concept and 15–25 min (`CLAUDE.md` rule 8). |
| `lesson_id`, file | Numbered from `l01` in sidebar order: `<prefix>NN-lYY`, file `lYY-<slug>.mdx`, slug = English kebab-case from the lesson name. |
| Lesson title (German) | German translation of the lesson name in the plan (lines already in German stay as they are). |
| ▶ / ◻ | Marker in the heading; a marker on a single line wins. |
| Spec, library, badges, misconceptions | The `Spec`, `Library`, `Badge` and `misconceptions` lines of the section. Each item goes to exactly one lesson. Misconception id = `<lesson_id>-<English kebab-slug>`. |
| Sources | Spec items: clause of the draft or feature specification ("Reference" in `module-plan.md`); library items: Microsoft Learn. Only pages that were opened; visible citation per `concept/sources.md`. |
| Application context | The step of this module in "Practice project SecLog". |
| Out of scope | Rows of "Deliberately not covered" that name this module or its topics. |
| Blockers | Lines of "Open questions" that name this module; components the lessons need that are "planned" in `components.md`; for ◻ the CI project in `website/examples/_ci/` named in the section. |
| Example, quiz, glossary files | Paths from `CLAUDE.md` ("Repo structure", "Example folders") and `.claude/rules/*.md`: `website/examples/<prefix>NN/lYY/`, `website/src/data/quiz/de/<prefix>NN.json`, `website/src/data/glossary/de.json`. Example file names are chosen by the implementer (`.claude/rules/examples.md`). |
| Warm-up questions | Quiz ids of earlier lessons (`website/src/data/quiz/de/*.json`). |
| Lesson structure, DoD | `concept/lesson-template.md`, `CLAUDE.md` (Definition of Done, rules 1–11). Not copied into issues – referenced. |

## 2a. Existing content of the module

Content that already exists for the module (today M0–M2) is an **early implementation**: it does not follow the current rules. The module is rebuilt completely; nothing is kept because it exists.

- The lesson cut, `lesson_id`s, titles and files come only from section 2. Old lesson ids are not protected.
- Old text, examples and quiz questions are not copied. They may only be used as raw material when the new lesson issue says so and the result meets all rules.
- Removing the old content is part of the scaffold issue (section 3). Until the new lessons are merged, the module is empty on the site; this is accepted.
- Learning progress: the dashboard ignores stored answers to question ids that no longer exist (`useProgressDashboardData.js`), so removed questions do no harm. A reused question id keeps its old Leitner state; this is accepted for early implementations.

## 3. Issues to create

Order and chain (one PR per issue; every issue that changes a file another issue also changes gets `Depends on #<number>` – `de.json` and the quiz file are shared by all lessons, so lessons are one chain):

1. **Decision/research issue** per blocker from "Open questions" (label `needs-human:decision` if a human must decide). Everything else depends on it.
2. **CI project issue** – only for ◻ lessons whose CI project does not exist yet. Changes to `.github/workflows/` go to "What a human must do".
3. **Scaffold issue**: remove the early implementation of the module (lesson files, `website/examples/<prefix>NN/`, the questions in `de/<prefix>NN.json`) and fix every reference to it (find them with a search for the old ids and paths, for example `website/plugins/search-index.test.js`, `README.md`, the `lesson` field in `de.json`); then `_category_.json`, empty quiz file, import in `website/src/data/quiz/index.js`, example folder. Glossary entries stay until the lesson that introduces the term rewrites them.
4. **One lesson issue per lesson**, in sidebar order, each depending on the one before (the first on the scaffold).
5. **Module review issue**: every spec item and misconception is taught in its lesson; no file of the early implementation is left; glossary entries that no new lesson introduces or links are removed or fixed; 8–12 questions marked as placement test candidates; `module-plan.md` updated (answered open questions removed).

Do not start a second module while one is in progress (shared `de.json`).

## 4. Content of a lesson issue

Use the sections that `claude-architect.yml` requires. Fill them only with values from section 2:

- **Part of** `#<module issue>` · **Goal**: `lesson_id`, German title, core concept in one sentence.
- **Context**: ▶/◻; 2–4 learning goals ("Du kannst …"); the spec and library items of this lesson with their sources; badges; misconceptions with their ids; warm-up quiz ids; SecLog task; topics that belong to other lessons or are out of scope.
- **Affected files**: lesson file, example folder, quiz file, `de.json` (paths from section 2).
- **Interfaces**: front matter (`lesson_id`, `sidebar_position`, `duration_min`), quiz ids `<lesson_id>-q<n>`, misconception ids, new glossary ids.
- **Implementation steps**: "Follow `concept/lesson-template.md` steps 1–11."
- **Acceptance criteria**: "Definition of Done 1–16 and rules 1–11 of `CLAUDE.md`, ticked off in the PR; the PR contains the \"Verification\" table (`concept/sources.md`, level 0); every spec item of this issue is taught and its source linked."
- **Tests**: `bash tools/check-examples.sh`, `node tools/check-quiz.mjs`, `cd website && npm run build`.
- **Not part of this issue**: other lessons, components, playground.

## 5. Stop and ask (status `needs_human`)

- The module section is missing or the previous module is not done.
- An open question or a "planned" component blocks a lesson and no decision issue can solve it.
- Review modules: the open decision on which DoD points apply ("Location and naming" in `module-plan.md`) is not made yet.
