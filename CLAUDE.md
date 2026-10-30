# CLAUDE.md – dotnet-learn

Learning site for C# and .NET: Docusaurus 3 (learning path, quizzes) + a Blazor WebAssembly playground with Roslyn (.NET 10), hosted on GitHub Pages. Audience: programming beginners **and** people with prior programming experience. Teaching style: example-driven, scientifically grounded, fundamentals are explained.

This document is binding for all work in the repo – for humans and for Claude (also in the workflows `claude-architect.yml` / `claude-implement.yml`).

## Repo structure

| Path | Content |
| --- | --- |
| `website/` | Docusaurus site. Lessons in `website/docs/`, components in `website/src/components/` |
| `website/examples/` | **All** C# examples as `.cs` files (one source for all languages, checked by CI) – see S1/S2 and "Example folders" below |
| `website/src/data/glossary/` | Glossary entries per language (`de.json`), format in its `README.md` |
| `website/src/data/quiz/` | Quiz questions per language and module (`de/mXX.json`), format in its `README.md` |
| `website/plugins/` | Local Docusaurus plugins (`lesson-index.js`) |
| `playground/` | Blazor WebAssembly + Roslyn; published under `/runner/` and embedded by iframe |
| `tools/` | `check-examples.sh` (S2), `check-quiz.mjs` (quiz data, links and wording) |
| `concept/` | Learning concept and module digest (read on demand, see below) |
| `.claude/rules/` | Path-scoped rules for lessons, examples and the playground |
| `.github/workflows/` | `pages.yml` (build + deploy), `examples.yml` (example check), `claude-*.yml` (issue automation) |

### Example folders

Under `website/examples/`:

- `mXX/lYY/`: examples of the lessons.
- `dev/`: examples of the dev showcase pages (`/dev/`).
- `playground/`: start code of the playground page.
- `_ci/<name>/`: projects that need NuGet packages (EF Core, Minimal API, xUnit, DI and logging). Each has a `run.sh` and an `expected.out.txt`.

## Commands

```bash
# Learning site
cd website && npm install && npm run build   # build check (must pass without errors)
cd website && npm start                       # dev server: http://localhost:3000/dotnet-learn/

# Playground
dotnet build playground/Playground.csproj     # build check
dotnet run --project playground               # run locally

# Check example code (compile + compare output with <name>.out.txt), see S2
bash tools/check-examples.sh
bash tools/check-examples.sh --update   # rewrite snapshots on purpose, review the diff

# Check quiz data, links and wording (runs in CI before the build)
node tools/check-quiz.mjs
```

Before every commit: run both build checks. If one fails, do not commit.

## Non-negotiable rules

1. **Language.** The text of the lessons (`website/docs/**/*.mdx`, quiz texts) and the learner-facing UI texts are German (more languages later via Docusaurus i18n). **Everything else is English**: code, identifiers, code comments (also in CSS, Razor, JS, config files), program output of the examples, documentation (`CLAUDE.md`, `concept/`, `README.md`, `.claude/`), commit messages, and **pull requests (title and description)**. Use simple English. Technical terms are English; at first use in a lesson they get a German explanation: "Value Type (Werttyp)".
2. **Base: C# 14 / .NET 10 (LTS).** Top-level statements at the start, `Main` is explained later. Nullable reference types on. New features beginning from C# 12 / .NET 8 get a version badge (`<Since csharp="…" dotnet="…" />`).
3. **Code lives in `.cs` files** under `website/examples/mXX/lYY/` and is included into code blocks and `<CSharpRunner>` by `raw-loader`. No new inline code in MDX.
4. **Every shown output is real**: it must match the actual run (CI comparison, S2). No outputs invented by hand.
5. **Not runnable ≠ marked**: code examples that the playground cannot run (ASP.NET Core, EF Core, unit tests) are normal ` ```csharp title="…" ` blocks without a note.
6. **Runnable** is only what runs in `<CSharpRunner>`. Practice examples follow the pattern *predict → runner → solution and explanation → deepening (optional)*.
7. **Facts need a source**: every statement is verified against a primary source (Microsoft Learn or the C# specification). The lesson shows a source only where it helps the learner and words it without attribution phrases. Teaching statements link to the literature in `concept/didactics.md`. Do not invent anything – if in doubt, mark it as an open question. Details: `concept/sources.md`.
8. **Lesson = 15–25 min, one core concept.** Longer → split.
9. **Accessibility WCAG 2.2 AA**: keyboard use for all interactions, contrast in light and dark mode, text alternative for diagrams.
10. **No tracking**, no accounts. Learning progress only local (localStorage/IndexedDB) with JSON export/import.
11. **No AI content outside module 19** – only exception: one paragraph on the page "Wie dieser Kurs funktioniert" (How this course works).

## Definition of Done – lesson

A lesson is done when all points are met:

1. Every learning goal is covered by at least one quiz or exercise task.
2. All code examples pass the CI check (compile, outputs match).
3. The required methods are included: worked example with fading, PRIMM practice example, retrieval practice, questions for spaced repetition.
4. At least one misconception is handled (box "Achtung").
5. Features from C# 12 / .NET 8 or newer carry the version badge.
6. Technical statements in "Vertiefung" (deepening) sections are verified and cited as in `concept/sources.md` level 2 (clause as link text).
7. Duration 15–25 min (estimated by word count and number of tasks).
8. Every worked example has subgoal labels.
9. Every lesson has a project step ("Projektschritt") that builds on the previous state of SecLog, or one sentence why not (`concept/project-seclog.md`).
10. At least one transfer question ("Where would you use this?") or an extra project exercise.
11. Every misconception box is linked to at least one wrong answer in the quiz.
12. At least one task "In eigenen Worten erklären" (explain in your own words).
13. Each information regarding C# is validated against the C# language specification on Microsoft Learn (verification: `concept/sources.md` level 0)
14. Every information is described in detail before it is part of any quiz
15. When finalizing a module, ensure that the lessons follow a cohesive thread throughout. Across all lessons, do not assume any programming-related information unless it has been explained beforehand—either in this module or in a preceding one. The SecLog thread follows `concept/project-seclog.md` (milestones and continuity rule).
16. The PR contains the section "Verification" (table: statement · URL with clause or heading · date checked), see `concept/sources.md` level 0.
17. Every programming term or construct that a lesson or its examples use is explained in this lesson or an earlier one, or it is marked with a `<Forward>` note at its first place in the lesson ("order first": prefer examples that need no forward reference). Quiz questions never need a concept that is only marked. See `concept/lesson-template.md`, "Forward references".

In addition: `npm run build` passes, no dead links, new technical terms are in the glossary and linked with `<Term>` at first use, a "Weiterlesen" (further reading) section with 1–3 links to Microsoft Learn or the specification.

## Way of working

- Larger tasks as an issue with label `claude:auto-architect`, small ones with `claude:auto-implement` (see `.github/workflows/claude-*.yml`). Write dependencies in the issue as their own line `Depends on #<number>`.
- A new module starts with one issue `Module <N>` (label `claude:auto-architect`); the architect derives all lessons and issues from `concept/module-plan.md` as described in `concept/issue-blueprint.md`.
- Changes to `.github/workflows/` need a human (set in `claude-implement.yml`).
- Pull requests (title and description) and commit messages are written in English.
- Half-yearly review (next: 2027-04) about new C#/.NET features, AI module (state of studies, links), dead links.

## Context budget (module work)

The repo grows with every module. Work on module N (architect, lesson issue, module review) loads only what module N needs. Earlier modules are known through `concept/module-digest.md`, not through their files.

Allowed for every role:

- `CLAUDE.md`, the concept files that the "Concept" table names for the task, and `.claude/rules/*`.
- `concept/module-plan.md`: only the section `### M<N>` (search it) and the rows of "Reference", "Deliberately not covered" and "Open questions" that name module N. Not the sections of other modules.
- `concept/project-seclog.md`: the general rules and the milestone rows of module N-1 and N only.
- `concept/module-digest.md` (whole file).
- All files of module N itself (lessons, examples, its quiz file `de/<prefix>NN.json`).
- Targeted lookups in shared or earlier files: `grep` for one id, term or path (for example a glossary id, a quiz id, a `"lesson": "mXX-` field). Read only the lines found.
- One file of an earlier module only when the digest or the issue names it, for example the previous SecLog solution file for the continuity rule.

Not allowed:

- Reading lessons, examples or quiz files of earlier modules in full, or "for orientation".
- Reading `website/src/data/glossary/de.json` or `concept/module-plan.md` in full.
- Copying text of earlier modules into issues. Issues name ids and paths instead.

If the digest is missing something that module N needs, look it up with a targeted search and add it to the digest in the module review of module N (or say so in the PR).

Not limited: tasks that are not module work (tools, components, playground, workflows, the half-yearly review). A fix in an earlier module treats that module as module N. Running the checks (`bash tools/check-examples.sh`, `node tools/check-quiz.mjs`, `npm run build`) is always allowed.

## Concept (binding, read on demand)

The files below are binding but are **not** imported, to save tokens. Read a file with the Read tool before you work in its area. For lessons, `.claude/rules/lessons.md` already lists the key rules.

| File | Read it when you ... |
| --- | --- |
| `concept/lesson-template.md` | write or review a lesson (flow, practice-example pattern, `<Forward>`, project step frame) |
| `concept/module-plan.md` | write, split or review a module (lesson proposals, spec clauses, badges). It is large: search it for your module (`### M<N>`) instead of reading it all |
| `concept/module-digest.md` | start work on a module (what earlier modules taught, SecLog end state); a module review adds its section |
| `concept/project-seclog.md` | add a project step or quiz/example data (continuity rule, milestones, sample data) |
| `concept/sources.md` | verify facts, cite sources, write the PR "Verification" table, or touch the wording check |
| `concept/style-and-language.md` | write German text, code style or accessibility-relevant UI |
| `concept/components.md` | use or change a component (`<CSharpRunner>`, `<Quiz>`, `<Forward>`, `<Diagram>`, progress) |
| `concept/didactics.md` | choose learning methods or edit "Wie dieser Kurs funktioniert" |
| `concept/ai-module.md` | work on module 19 or any AI statement |
| `concept/issue-blueprint.md` | split a module into issues (architect) |
