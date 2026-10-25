# Issue blueprint – from module plan to issues

As of 2026-10-25. This file describes how to turn one module of `concept/module-plan.md` into GitHub issues that the automation (`claude-architect.yml` → `claude-implement.yml`) can work on without questions. Use it once per module, in module order.

It does not add new rules. It collects the binding rules from `CLAUDE.md`, `concept/*.md` and `.claude/rules/*.md` and says where each one goes in an issue.

## 1. How the issues flow

```
Human: module epic  ──label claude:auto-architect──▶  Architect (Opus)
                                                         │ splits into implement issues
                                                         ▼
                                     scaffold issue ─▶ lesson issue L1 ─▶ L2 ─▶ … ─▶ module review issue
                                     (each one PR; chained with "Depends on #…")
                                                         │ claude-implement.yml (Sonnet) opens one PR per issue
                                                         ▼
                                     Human reviews and merges each PR
```

- The **epic** holds the functional requirement: goal, scope, acceptance criteria. The architect treats it as fixed and only decides the technical *how*. So everything content-related must already be in the epic: lesson cut, spec coverage, misconceptions, sources.
- The architect creates the **implement issues** with the sections it needs (see section 5). The templates in section 5 are what the architect should produce; a human can also create them by hand with the same sections.
- Every implement issue = exactly one pull request.

## 2. Inputs to read before writing the epic

| Input | What you take from it |
| --- | --- |
| `concept/module-plan.md`, section of the module | Lesson list (existing + *(new)*), **Spec**, **Library**, **Badge**, misconceptions, ▶/◻, notes and dependencies |
| `concept/module-plan.md`, "Reference" | Which source layer to link per spec item (draft clause or feature specification) |
| `concept/module-plan.md`, "Deliberately not covered" | The "Out of scope" list of the epic |
| `concept/module-plan.md`, "Practice project SecLog" | The SecLog step of this module (application context, DoD 9) |
| `concept/module-plan.md`, "Open questions" | Questions that block this module |
| `concept/lesson-template.md` | The 11 steps of every lesson |
| `CLAUDE.md` | Non-negotiable rules 1–11, Definition of Done 1–13, build checks |
| `concept/didactics.md` | Required methods; literature only for the page "Wie dieser Kurs funktioniert" |
| `concept/components.md` | Which components are available or only planned |
| `concept/style-and-language.md` | Language, tone, code style, sources and "Stand" dates |
| `.claude/rules/lessons.md`, `examples.md`, `playground.md` | Path rules for lessons, examples and the runner |
| `website/src/data/quiz/README.md`, `website/src/data/glossary/README.md` | Data formats for quiz questions and glossary entries |
| Lessons of earlier modules | Terms that already exist in the glossary, quiz ids for the warm-up ("Rückblick") |

## 3. Procedure per module

**Step 0 – Check that the module can start.**
- The previous module is done (module order may not change; warm-up questions use earlier lessons).
- No open question in "Open questions" blocks this module. If one does, create a research issue first (template 5.5) and make the epic depend on it.
- Every component the module needs is **available** in `components.md`. A planned component (for example `stdin` for M1, the placement test) needs its own prerequisite issue, or the lesson must work without it.
- For ◻ modules: the CI project under `website/examples/_ci/<name>/` exists (`xunit`, `ef-core`, `minimal-api`, `di-logging` today). If not, add a CI project issue (template 5.4).

**Step 1 – Extract the module content** from the module plan into the epic (section 4): lessons, spec items, library items, badges, misconceptions, SecLog step.

**Step 2 – Cut the lessons.**
- One core concept per lesson, 15–25 min (`CLAUDE.md` rule 8). If a lesson in the plan holds several concepts, split it; if two small parts fit one concept, merge them (the plan allows changing the order inside a module).
- Map **every** spec item and library item of the module to exactly one lesson (coverage matrix in the epic). An item that fits no lesson is either a new lesson or goes to "Out of scope" with a reason.
- Map every misconception to the lesson that handles it ("Achtung" box + quiz distractor, DoD 4 and 11).
- Mark each lesson ▶ or ◻.

**Step 3 – Give ids and names.**
- `lesson_id` = `mXX-lYY`. Existing ids never change (they key quiz data and learning progress). New lessons get the next free `YY`; the reading order is set by `sidebar_position`, not by the id.
- Slugs in English, titles in German (`module-plan.md`, "Location and naming").
- Misconception ids: `<lesson_id>-<kebab-slug>` (quiz README).
- Quiz ids: `<lesson_id>-q<n>`.
- Glossary ids: English kebab-case; check first that the term does not exist yet.

**Step 4 – Collect sources.** For each lesson: 1–3 "Weiterlesen" links (Microsoft Learn or the C# specification), the spec clause per spec item (layer 2 or 3), Microsoft Learn docs for library items. Only links you opened. Version, history and AI content gets "Stand: YYYY-MM-DD".

**Step 5 – Plan the split and the chain** (section 6), then write the epic and set the label `claude:auto-architect`.

## 4. Template: module epic

Title: `Module MXX – <German module title>`
Label: `claude:auto-architect`

```markdown
## Goal
Write module MXX "<German title>" as described in `concept/module-plan.md`
(section "MXX"), following `CLAUDE.md`, `concept/lesson-template.md` and
`concept/issue-blueprint.md`.

## Scope – lessons
| lesson_id | Slug | German title | Core concept | ▶/◻ | Est. min |
| --- | --- | --- | --- | --- | --- |
| mXX-l01 | … | … | … | ▶ | 20 |

Order in the sidebar: …

## Spec coverage (every item from the module plan → one lesson)
| Spec/library item (from module-plan.md) | Source (clause or Learn page) | Lesson |
| --- | --- | --- |
| §… … | https://learn.microsoft.com/… | mXX-l01 |

## Version badges
| Feature | C# / .NET | Lesson |
| --- | --- | --- |

## Misconceptions
| Misconception (plan wording) | Misconception id | Lesson |
| --- | --- | --- |

## Application context (SecLog / classifier)
Which SecLog step this module adds (from "Practice project SecLog"), per lesson.

## Glossary
New terms (id, term, German) and the lesson that introduces them.
Existing terms that are only linked.

## Prerequisites
- Previous module: …
- Components needed (all must be "available" in components.md): …
- CI project (◻ only): website/examples/_ci/<name>/
- Open questions that must be answered first: …

## Out of scope
- Items from "Deliberately not covered" that touch this module, with the reason.
- Items that are "only by name" (one sentence + link, no exercise).

## Acceptance criteria (module level)
- [ ] One PR per lesson; every lesson meets the Definition of Done in CLAUDE.md.
- [ ] Every row of "Spec coverage" is taught in the named lesson.
- [ ] Every misconception has an "Achtung" box and at least one linked quiz distractor.
- [ ] Warm-up questions of the first lesson use earlier modules.
- [ ] Module review issue done (section 5.6).

## Recommended split (for the architect)
1. Scaffold issue (template 5.2)
2. One lesson issue per row of "Scope" (template 5.3), chained in sidebar order
3. Module review issue (template 5.6)
```

## 5. Templates: implement issues

All implement issues use the sections the architect must write (`claude-architect.yml`): **Part of**, **Goal**, **Context**, **Affected files**, **Interfaces**, **Implementation steps**, **Acceptance criteria**, **Tests**, **Not part of this issue**, and one `Depends on #<number>` line per dependency. For content work, "Interfaces" means the stable keys other issues rely on: `lesson_id`, file paths, quiz ids, misconception ids, glossary ids, component props.

### 5.1 Common acceptance criteria (copy into every lesson issue)

```markdown
- [ ] DoD 1: every learning goal is checked by at least one quiz or exercise task
- [ ] DoD 2: all examples pass `bash tools/check-examples.sh` (compile, outputs match)
- [ ] DoD 3: worked example with fading, PRIMM practice example, retrieval practice, spaced-repetition questions
- [ ] DoD 4: at least one "Achtung" box
- [ ] DoD 5: features from C# 12 / .NET 8 on carry `<Since>`
- [ ] DoD 6: technical statements in "Vertiefung" link to Microsoft Learn or the specification
- [ ] DoD 7: duration 15–25 min (`duration_min` in the front matter)
- [ ] DoD 8: every worked example has subgoal labels (in text, not as German code comments)
- [ ] DoD 9: at least one task with a real application context (SecLog or classifier)
- [ ] DoD 10: at least one transfer question or SecLog extra exercise
- [ ] DoD 11: every "Achtung" box starts with `<Misconception id="…" />` and is linked to a wrong quiz answer
- [ ] DoD 12: at least one "In eigenen Worten erklären" task
- [ ] DoD 13: every C# statement checked against the specification (clause or feature spec linked in the PR)
- [ ] New terms in `de.json`, first use linked with `<Term>`
- [ ] "Weiterlesen" with 1–3 links that exist
- [ ] Rule 1: lesson text German, code/identifiers/comments/outputs English
- [ ] Rule 3: no inline code in MDX; all code from `website/examples/mXX/lYY/*.cs`
- [ ] Rule 4: every shown output comes from a CI snapshot (`.out.txt` or `_ci/*/expected.out.txt`)
- [ ] Rule 9: keyboard use, contrast, text description for every diagram
- [ ] Rule 11: no AI content (outside module 19)
- [ ] `node tools/check-quiz.mjs` and `cd website && npm run build` pass
- [ ] The PR description ticks off the DoD point by point (`.claude/rules/lessons.md`)
```

### 5.2 Scaffold issue (first issue of a module)

Title: `MXX: module scaffold`

```markdown
**Part of #<epic>**

## Goal
Create the empty structure of module MXX so that lesson issues only add content.

## Context
Lesson issues all write to the same quiz file; the import in `index.js` is done once here.

## Affected files
- new `website/docs/mXX-<slug>/_category_.json` (label "Modul XX – <German title>", position XX)
- new `website/src/data/quiz/de/mXX.json` (empty question list)
- change `website/src/data/quiz/index.js` (import the new file – the README says this is manual)
- new folder `website/examples/mXX/` (◻ modules: check or extend `website/examples/_ci/<name>/`)

## Interfaces
- Module folder `website/docs/mXX-<slug>/`, quiz file `de/mXX.json`

## Implementation steps
1. …

## Acceptance criteria
- [ ] Build and `check-quiz.mjs` pass with the empty module
- [ ] Earlier modules look unchanged in the sidebar and on the "Lernstand" page

## Tests
`node tools/check-quiz.mjs`, `cd website && npm run build`

## Not part of this issue
Lesson content, quiz questions, glossary entries.
```

### 5.3 Lesson issue (one per lesson)

Title: `MXX-LYY: <German lesson title>`

```markdown
**Part of #<epic>**

## Goal
Write lesson `mXX-lYY` "<German title>": <core concept in one sentence>.

## Context
- Module plan: `concept/module-plan.md`, MXX, item "<lesson line>"
- Runnable: ▶ / ◻ (◻ → normal code blocks via `<CodeBlock>`, no "not runnable" note, rule 5)
- Learning goals (2–4, observable: "Du kannst …", "Du erkennst …"):
  1. …
- Spec content to teach (from the epic coverage matrix), with source:
  - <item> – <clause URL or feature spec URL>
- Library content (not spec), with Microsoft Learn source:
  - …
- Version badges: <feature> → `<Since csharp="…" dotnet="…" />`
- Misconceptions handled here: <plan wording> → id `mXX-lYY-<slug>`
- Warm-up ("Rückblick"): 2–3 existing quiz ids from earlier lessons: …
- SecLog / classifier task: …
- Switcher box (`<FromLanguage>`): yes/no; which differences
- Deep dive ("Vertiefung"): topic, source
- Out of scope for this lesson: … (taught in mXX-lZZ / not covered)

## Affected files
- new `website/docs/mXX-<slug>/lYY-<slug>.mdx`
- new `website/examples/mXX/lYY/<name>.cs` + `<name>.out.txt` (CI snapshot) per example; naming as in the reference lesson `m01/l03`: practice example, `*.gaps.snippet.cs` (gap example), `*-task.cs` + `*-solution.cs` + `*.tests.snippet.cs` (automatically checked task), `quiz-*.cs` (output/parsons/cloze/find-bug questions)
- ◻: code in `website/examples/_ci/<name>/`, output from its `expected.out.txt`
- change `website/src/data/quiz/de/mXX.json` (questions `mXX-lYY-q1` …)
- change `website/src/data/glossary/de.json` (new terms)

## Interfaces
- Front matter: `title`, `sidebar_position`, `description`, `lesson_id: mXX-lYY`, `duration_min`
- Quiz ids, misconception ids, glossary ids (list them)

## Implementation steps
Follow `concept/lesson-template.md`:
1. Lernziele (`:::note[Lernziele]`)
2. Rückblick (2–3 `<Quiz>` on earlier lessons)
3. Concept: short text + small isolated example, `<Term>` at first use
4. Practice example (PRIMM): `:::info[Vorhersage]` → `<CSharpRunner predict expected={…}>` → "Lösung und Erklärung anzeigen" with subgoal labels → optional "Vertiefung"
5. Gap example (`*.gaps.snippet.cs`, subgoal labels stay)
6. Change and make it yourself: `<CSharpRunner tests={…}>` (◻: collapsed model solution)
7. Misconceptions: `:::warning[Achtung]`, first line `<Misconception id="…" />`
8. (Switcher box)
9. (Deep dive) with Microsoft Learn source
10. Summary (3–5 sentences, nothing new)
11. Wrap-up: quiz, "In eigenen Worten erklären", transfer question, (SecLog extra exercise), "Weiterlesen"

## Acceptance criteria
<copy section 5.1>
- [ ] Every spec item listed in "Context" is taught and its source is linked in the PR

## Tests
`bash tools/check-examples.sh` (new snapshots only with `--update`, diff reviewed), `node tools/check-quiz.mjs`, `cd website && npm run build`

## Not part of this issue
Other lessons; changes to components or the playground (separate issue); placement test.

Depends on #<scaffold issue or previous lesson issue>
```

### 5.4 CI project issue (◻ modules, only when needed)

Title: `MXX: CI project <name> for examples that cannot run`

Sections as above. Affected files: `website/examples/_ci/<name>/` with project file, `run.sh`, `expected.out.txt`. Acceptance: `bash tools/check-examples.sh` runs the project and compares the output; outputs are culture-independent (`InvariantCulture`). Changes to `.github/workflows/` are **not** part of it (they need a human, `CLAUDE.md`) – if CI wiring is needed, mark it "What a human must do" (label `needs-human:access`).

### 5.5 Research / decision issue (open questions)

Title: `MXX: decide <question>`
Label: `needs-human:decision` (decision) or `claude:auto-implement` (pure source check that ends in a `module-plan.md` change).

Content: the open question as written in `module-plan.md`, the sources already found, what must be decided, and the file that gets the answer (`concept/module-plan.md` "Open questions" → remove the line and write the result into the module section).

### 5.6 Module review issue (last issue of a module)

Title: `MXX: module review`

Acceptance criteria:
- [ ] Coverage matrix of the epic: every row is taught in the named lesson (link section per row in the PR)
- [ ] All misconception boxes are linked to quiz distractors (`check-quiz.mjs` passes)
- [ ] 8–12 questions marked as candidates for the placement test (the component is planned; record the ids in the PR)
- [ ] Cross-links between lessons and to earlier modules work; no dead links
- [ ] `module-plan.md`: module status updated, answered open questions removed, new findings added
- [ ] Glossary: no duplicate concepts, every new entry has a `source` (categories `language`/`platform`)

`Depends on #<last lesson issue>`

## 6. Dependency and split rules

The architect must chain issues that change the same file (`claude-architect.yml`). For this repo that means:

| Shared file | Changed by | Consequence |
| --- | --- | --- |
| `website/src/data/glossary/de.json` | almost every lesson of every module | Lesson issues run one after another, also across modules. Do not run two module epics at the same time. |
| `website/src/data/quiz/de/mXX.json` | every lesson of the module | Lesson issues of one module are a linear chain. |
| `website/src/data/quiz/index.js` | scaffold issue only | Only the scaffold adds the import. |
| `website/docs/mXX-<slug>/_category_.json` | scaffold issue only | – |
| `concept/module-plan.md` | research and review issues | Chain them, or do them before/after the lesson chain. |
| `website/examples/_ci/<name>/` | CI project issue, ◻ lesson issues | The CI project issue comes first. |

Order inside a module: research issues (if any) → CI project issue (if any) → scaffold → lessons in sidebar order → module review.

Use exactly one line `Depends on #<number>` per dependency; a wrong number blocks the issue forever (`claude-architect.yml`, unlock job).

## 7. Module-specific notes

Fill the epic of each module with these extra points (on top of its section in `module-plan.md`).

| Module | ▶/◻ | Extra points for the epic |
| --- | --- | --- |
| M0 | ▶ | L2–L5 are mostly text; content on versions carries "Stand: YYYY-MM-DD"; check support dates on the .NET support policy page on the day of writing (open question .NET 10 end of support). The page "Wie dieser Kurs funktioniert" is its own issue; it is the only place for literature from `didactics.md` and for the one AI paragraph. |
| M1 | ▶ | Existing lessons `m01-l02`, `m01-l03` keep their ids; L1 "Statements and output" is still missing. Console input depends on the planned `stdin` prop → prerequisite issue or task without input. Culture-independent outputs (`3.5`). |
| M2 | ▶ | Split of the existing `m02-l01` is part of the epic: keep `m02-l01` for the first part, new ids for the rest; move quiz questions to the matching new `lesson` values. |
| M3–M6 | ▶ | `trace` (step view) fits recursion (M3), argument passing (M4), arrays (M5) and objects (M6). M6 explains `Main` (`CLAUDE.md` rule 2 "explained later"). |
| W1–W4 | ▶ | Mixed practice only, no new content: questions must first make learners recognize the concept. Folder and `lesson_id` format for review modules is not defined yet → research/decision issue first. |
| M7 | ▶/◻ | File lesson: decide ▶ or ◻ first (open question on the playground file system). |
| M8 | ▶ | Sealing: check the current design guideline and analyzer rule before writing (open question). |
| M9–M12 | ▶ | M11: extension members (C# 14) need the badge; M12: check `with` for structs/anonymous types in the feature spec first. |
| M13 | ▶ (A) / ◻ (B) | Part B: no runnable code. Use `<Diagram>` for interleavings and quiz types `multi`, `find-bug`, `parsons`, `explain`. Racy examples never show a claimed output (rule 4) – decide the "compile only" mode for `check-examples.sh` first (research issue). Add the concurrency literature to `didactics.md` only after checking it. |
| M14 | ◻ | Needs `_ci/xunit`; publishing outputs (file lists, sizes in MB) only from CI runs with the date. Attributes lesson can be ▶. |
| M15 | ▶/◻ | DI and logging are runnable (preloaded packages S6); configuration, options and hosting are ◻ via `_ci/di-logging`. Keyed services: check on Microsoft Learn first. |
| M16 | ◻ | Needs `_ci/ef-core`; shown SQL only from the CI run. |
| M17 | ◻ | Needs `_ci/minimal-api`; HTTP requests and responses only from the CI run. JSON lesson: decide ▶ or ◻ first. |
| M18 | ▶/◻ | Final project: ▶ parts with automatically checked tasks; ◻ parts as code-review tasks (`find-bug`). |
| M19 | ◻/▶ | Rules from `concept/ai-module.md` apply in addition: neutrality checklist, "Stand" date, label "Typische KI-Antwort (konstruiertes Beispiel)", study findings vs. best practice. |

## 8. Example: epic for M3 (excerpt)

```markdown
## Scope – lessons
| lesson_id | Slug | German title | Core concept | ▶/◻ | Est. min |
| --- | --- | --- | --- | --- | --- |
| m03-l01 | define-and-call | Methoden definieren und aufrufen | definition, parameters, return | ▶ | 20 |
| m03-l02 | return-vs-output | Rückgabe statt Ausgabe | return vs. console output | ▶ | 15 |
| m03-l03 | scope | Gültigkeitsbereiche | scope, name hiding | ▶ | 15 |
| m03-l04 | overloading | Überladen | signatures, overload resolution | ▶ | 20 |
| m03-l05 | optional-named | Optionale und benannte Parameter | default values, named arguments | ▶ | 15 |
| m03-l06 | local-functions | Lokale Funktionen und Kurzform | local functions, expression bodies | ▶ | 15 |
| m03-l07 | params | Beliebig viele Argumente | parameter arrays, params collections | ▶ | 15 |
| m03-l08 | ref-out | ref und out | reference and output parameters | ▶ | 20 |
| m03-l09 | tuples | Mehrere Werte zurückgeben | tuples, deconstruction | ▶ | 20 |
| m03-l10 | recursion | Rekursion und Aufrufstapel | recursion, call stack | ▶ | 25 |

## Spec coverage (excerpt)
| Item | Source | Lesson |
| --- | --- | --- |
| §7 signatures and overloading (return type not part of the signature) | basic-concepts (draft) | m03-l04 |
| §15 parameter arrays | classes (draft) | m03-l07 |
| `params` collections (C# 13) | feature specification | m03-l07 |
| §9 definite assignment of output parameters | variables (draft) | m03-l08 |

## Version badges
| `params` collections | C# 13 / .NET 9 | m03-l07 |

## Misconceptions
| `return` prints something | m03-l02-return-prints | m03-l02 |
| a method changes the caller's variable | m03-l08-callee-changes-caller | m03-l08 |
| a recursive method needs no base case | m03-l10-no-base-case | m03-l10 |
| a tuple is a reference type | m03-l09-tuple-is-reference | m03-l09 |
```

The lesson titles, cut and minutes above are an example of the format, not a decision; the real cut is made when the M3 epic is written.

## 9. Known inconsistencies (fix in a separate issue)

- `.claude/rules/lessons.md` refers to `concept/lektions-template.md` and `concept/stil-und-sprache.md`; the files are `concept/lesson-template.md` and `concept/style-and-language.md`.
- `.claude/rules/lessons.md` asks to check a "spike table in `CLAUDE.md`"; `CLAUDE.md` has no such table (spikes S1–S7 are only referenced by name).
- The format of review modules (W1–W4: folder, `lesson_id`, sidebar) is not defined anywhere.
