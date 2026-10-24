# CLAUDE.md – dotnet-learn

Learning site for C# and .NET: Docusaurus 3 (learning path, quizzes) + a Blazor WebAssembly playground with Roslyn (.NET 10), hosted on GitHub Pages. Audience: programming beginners **and** people switching from other languages. Teaching style: example-driven, scientifically grounded, fundamentals are explained.

This document is binding for all work in the repo – for humans and for Claude (also in the workflows `claude-architect.yml` / `claude-implement.yml`).

## Repo structure

| Path | Content |
| --- | --- |
| `website/` | Docusaurus site. Lessons in `website/docs/`, components in `website/src/components/` |
| `website/examples/` | **All** C# examples as `.cs` files (one source for all languages, checked by CI) – see S1/S2 |
| `playground/` | Blazor WebAssembly + Roslyn; published under `/runner/` and embedded by iframe |
| `concept/` | Learning concept (imported below) |
| `.github/workflows/` | `pages.yml` (build + deploy), `claude-*.yml` (issue automation) |

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
```

Before every commit: run both build checks. If one fails, do not commit.

## Non-negotiable rules

1. **Language.** The text of the lessons (`website/docs/**/*.mdx`, quiz texts) and the learner-facing UI texts are German (more languages later via Docusaurus i18n). **Everything else is English**: code, identifiers, code comments (also in CSS, Razor, JS, config files), program output of the examples, documentation (`CLAUDE.md`, `concept/`, `README.md`, `.claude/`), commit messages, and **pull requests (title and description)**. Use simple English. Technical terms are English; at first use in a lesson they get a German explanation: "Value Type (Werttyp)".
2. **Base: C# 14 / .NET 10 (LTS).** Top-level statements at the start, `Main` is explained later. Nullable reference types on. Features from C# 12 / .NET 8 get a version badge (`<Since csharp="…" dotnet="…" />`).
3. **Code lives in `.cs` files** under `website/examples/mXX/lYY/` and is included into code blocks and `<CSharpRunner>` by `raw-loader`. No new inline code in MDX.
4. **Every shown output is real**: it must match the actual run (CI comparison, S2). No outputs invented by hand.
5. **Not runnable ≠ marked**: code examples that the playground cannot run (ASP.NET Core, EF Core, unit tests) are normal ` ```csharp title="…" ` blocks without a note.
6. **Runnable** is only what runs in `<CSharpRunner>`. Practice examples follow the pattern *predict → runner → solution and explanation → deepening (optional)*.
7. **Facts need a source**: technical statements link to Microsoft Learn or the C# specification; teaching statements link to the literature in `concept/didactics.md`. Do not invent anything – if in doubt, mark it as an open question.
8. **Lesson = 15–25 min, one core concept.** Longer → split.
9. **Accessibility WCAG 2.2 AA**: keyboard use for all interactions, contrast in light and dark mode, text alternative for diagrams.
10. **No tracking**, no accounts. Learning progress only local (localStorage/IndexedDB) with JSON export/import.
11. **Dependent content only after the spike is decided** (table below). Do not build around an open spike.
12. **No AI content outside module 19** – only exception: one paragraph on the page "Wie dieser Kurs funktioniert" (How this course works).

## Definition of Done – lesson

A lesson is done when all points are met:

1. Every learning goal is covered by at least one quiz or exercise task.
2. All code examples pass the CI check (compile, outputs match).
3. The required methods are included: worked example with fading, PRIMM practice example, retrieval practice, questions for spaced repetition.
4. At least one misconception is handled (box "Achtung").
5. Features from C# 12 / .NET 8 carry the version badge.
6. Technical statements in "Vertiefung" (deepening) sections link to Microsoft Learn or the C# specification.
7. Duration 15–25 min (estimated by word count and number of tasks).
8. Every worked example has subgoal labels.
9. At least one task has a real application context (mostly SecLog or the phishing classifier).
10. At least one transfer question ("Where would you use this?") or an extra project exercise.
11. Every misconception box is linked to at least one wrong answer in the quiz.
12. At least one task "In eigenen Worten erklären" (explain in your own words).

In addition: `npm run build` passes, no dead links, a "Weiterlesen" (further reading) section with 1–3 links to Microsoft Learn or the specification.

## Status of the spikes (prototype tasks)

Details, acceptance criteria and process: `concept/spikes.md`. Status: open · running · decided. The decisions are listed below in short form.

| # | Spike | Blocks | Status |
| --- | --- | --- | --- |
| S0 | Raise Roslyn to 5.x (C# 14) | all content with C# 14 features | decided; manual deploy check pending |
| S1 | Include code from `.cs` files in code block and `<CSharpRunner>` | all new content | decided |
| S2 | CI: compile all `.cs` files and compare outputs | all new content | decided |
| S3 | Roslyn loop guard against endless loops | module 2 (loops) | decided |
| S4 | Syntax color overlay in the editor | module 2 | decided |
| S5 | Step-by-step visualization | module 4 | decided; manual browser check pending |
| S6 | `Microsoft.Extensions.*` in the playground | module 15 | decided; manual browser check pending |
| S7 | Runner ↔ page: output/test result via `postMessage` | prediction field, checked tasks | decided; manual browser check pending |
All spikes are decided; the next work packages are the backlog B1–B11 in `concept/spikes.md`.

### Decisions in short form

- **S0:** Roslyn 5.9.0 (C# 14), +0.7 MiB. Fallback: C# 14 features only as non-runnable examples.
- **S1:** `.cs` files via `raw-loader` into `<CSharpRunner>` and `<CodeBlock>`.
- **S2:** `tools/check-examples.sh` runs every `*.cs` (invariant globalization) and compares with `<name>.out.txt`; `*.snippet.cs` are skipped; EF Core, Minimal API and xUnit run as projects under `website/examples/_ci/<name>/`.
- **S3:** A Roslyn rewriter inserts a time check (about 3 s from start, wall-clock time). No guard in operators, accessors, expression bodies; `catch (Exception)` in user code catches it.
- **S4:** Own tokenizer (`playground/wwwroot/js/editor.js`, no Prism) colors a `<pre>` behind the transparent `<textarea>`. Overlay lines use the classes `pg-hl-line*`; they must not change text metrics (font, weight, italic, ligatures, padding, border), or the cursor will sit wrong. Fallback: plain `<textarea>`.
- **S5:** Roslyn instrumentation records line and variables per step (`trace`). Stack/heap stays a model.
- **S6:** Fixed package set `Microsoft.Extensions.DependencyInjection` and `…Logging` (+0.6 MiB); Configuration/Hosting are missing.
- **S7:** `postMessage` protocol see `concept/components.md`; check code via `TaskTests.Run()`; fallback: task with a collapsed sample solution.

## Way of working

- Larger tasks as an issue with label `claude:auto-architect`, small ones with `claude:auto-implement` (see `.github/workflows/claude-*.yml`). Write dependencies in the issue as their own line `Depends on #<number>`.
- Spikes always end with an entry under "Decisions in short form" and an update of the status table above.
- Changes to `.github/workflows/` need a human (set in `claude-implement.yml`).
- Pull requests (title and description) and commit messages are written in English.
- Half-yearly review (next: 2027-04): version and history lesson (module 0), AI module (state of studies, links), dead links.

## Concept (binding)

@concept/didactics.md
@concept/lesson-template.md
@concept/module-plan.md
@concept/components.md
@concept/style-and-language.md
@concept/ai-module.md
@concept/spikes.md
