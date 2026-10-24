# Spikes (prototype tasks) and technical backlog

As of 2026-10-23. The status is in the table in `CLAUDE.md`.

All spikes are decided. The results are in `CLAUDE.md` under "Entscheidungen in Kurzform" (decisions in short form).

## Process

1. **One GitHub issue per spike**, label `spike` (create once) plus the start label of the existing automation:
   - small spikes (S0, S1, S6): `claude:auto-implement`
   - larger spikes (S2, S3, S5, S7): `claude:auto-architect`
   - dependencies as their own line `Depends on #<number>` (the workflows set such issues to `claude:queued`).
2. **Issue structure:** question · hypothesis · acceptance criteria · time limit · fallback · dependent content (copy the sections below).
3. **Implementation** in its own branch (`claude/issue-<nr>`) through a pull request.
4. **Record the result** under "Entscheidungen in Kurzform" in `CLAUDE.md` – even if the hypothesis was wrong.
5. **Update the status table in `CLAUDE.md`** in the same PR (status).
6. Content that depends on a spike is written only after it.

Order: S0 → S1 → S2 → S3 → S4 → S7 → S5 → S6. S3, S4, S5 and S7 all change the playground (`App.razor`, `index.html`). So they run one after another (`Depends on`), not in parallel.

Time limits are estimates. If one is exceeded: stop, record the interim state in `CLAUDE.md`, a human decides.

## S0 – Raise Roslyn to 5.x

- **Question:** Does the playground compile C# 14?
- **Finding from the MVP comparison:** `playground/Playground.csproj` uses `Microsoft.CodeAnalysis.CSharp` 4.14.0. According to the Roslyn wiki, 4.14 matches **C# 13** (VS 2022 17.14 / .NET 9). Only 5.0 brings C# 14 (VS 2026 18.0 / .NET 10). So `LanguageVersion.Latest` gives at most C# 13 with 4.14.
- **Hypothesis:** Version 5.x runs unchanged in Blazor WebAssembly.
- **Acceptance criteria:** An example with the `field` keyword and extension members compiles and runs in the published playground. Existing examples still run. Download size before/after is recorded in "Entscheidungen in Kurzform".
- **Time limit:** 2 h · **Fallback:** C# 14 features only as examples that cannot run.
- **Blocks:** any content with C# 14 features.

## S1 – Include code from `.cs` files

- **Question:** Can code be included with `raw-loader` (officially documented: Docusaurus "Importing code snippets") in `<CSharpRunner code={…}>` and `<CodeBlock>`?
- **Hypothesis:** Yes. The runner gets a normal string.
- **Acceptance criteria:** Pilot: `variablen-und-typen.mdx` uses only files under `website/examples/`. Code with backslash, backtick and `${` works. `npm run build` has no errors. The look of the non-runnable block is identical to ` ```csharp title="…" `.
- **Time limit:** 2 h · **Fallback:** own Webpack loader or remark plugin.
- **Blocks:** any new content.

## S2 – CI: compile examples and compare outputs

- **Question:** How does CI check all `.cs` files and make sure that shown outputs are real?
- **Hypothesis:** Runnable examples are run in CI (e.g. as file-based apps of the .NET 10 SDK – check before building). The output is stored as a snapshot (`<name>.out.txt`) next to the file, and the lesson includes the snapshot. If a run differs from the snapshot, CI fails. For EF Core (SQL), ASP.NET Core (HTTP) and xUnit (test runner output) there are separate CI projects that produce the snapshots.
- **Open sub-questions:** How to mark fragments so learners do not see it (proposal: file ending `.snippet.cs` = compile only or skip). Force `InvariantCulture` (the playground uses `InvariantGlobalization`).
- **Acceptance criteria:** The CI job fails on a compiler error or an output difference. One example of each kind (console, EF Core, Minimal API, xUnit) produces a snapshot.
- **Time limit:** 1 day · **Fallback:** compile only, check outputs by hand (rule 4 in `CLAUDE.md` still applies).
- **Note:** changes `.github/workflows/` → according to `claude-implement.yml`, this needs a human.
- **Blocks:** any new content.

## S3 – Loop guard

- **Question:** Can Roslyn insert a time check into every loop and method before running?
- **Hypothesis:** A `CSharpSyntaxRewriter` inserts a call that throws its own exception after about 3 s. The runner shows "Dein Programm läuft vermutlich endlos – prüfe die Abbruchbedingung in Zeile X" (your program probably runs forever – check the stop condition in line X).
- **Acceptance criteria:** `while (true) { }` and endless recursion stop after about 3 s. Line numbers in compiler messages stay unchanged. Normal examples run without a noticeable delay (measurement recorded in "Entscheidungen in Kurzform").
- **Time limit:** 1 day · **Fallback:** hint as before. Design loop exercises so that endless loops are unlikely. (Web Worker is not pursued further.)
- **Blocks:** module 2.

## S4 – Syntax color overlay in the editor (built without Prism, see decision)

- **Question:** Can a colored code block that lines up exactly be shown behind the `<textarea>`?
- **Acceptance criteria:** Lines up exactly with long lines, wrapping, scrolling, mobile devices and IME input. Current line and error lines can be highlighted. Smooth up to about 200 lines. Tab/Esc/Ctrl+Enter still work.
- **Time limit:** 1 day · **Fallback:** plain `<textarea>` (status quo) plus error list.
- **Blocks:** module 2.

## S7 – Runner ↔ page: result through `postMessage`

- **Question:** Can the runner report output and test results to the learning page?
- **Acceptance criteria:** `{type: 'run-result', id, success, output, diagnostics, tests?}` arrives. The prediction field shows ✓/✗ per line. A task with visible and hidden tests reports a result per test. Origin check (`origin`) in both directions.
- **Time limit:** 2 days · **Fallback:** tasks with a collapsed model solution.
- **Blocks:** prediction field, automatically checked tasks.

## S5 – Step-by-step visualization

- **Question:** Can the playground show variable values per step and references (stack/heap)?
- **Hypothesis:** Roslyn instrumentation (like S3) logs line and local variables per statement. A view shows steps forward/back (model: Online Python Tutor).
- **Acceptance criteria:** v1: a 15-line example with a loop shows line, variables and output per step. v2: reference arrows for a value/reference type example from module 4. Keyboard accessible, with a text alternative.
- **Time limit:** 2 days for v1 · **Fallback:** static diagrams (Mermaid/SVG) with a text description.
- **Blocks:** module 4.

## S6 – `Microsoft.Extensions.*` in the playground

- **Question:** Can user code use `ServiceCollection` and logging?
- **Note:** The Blazor app already loads `Microsoft.Extensions.DependencyInjection` at run time (`Program.cs`). The references for the compiler are missing. The packages are not part of `Microsoft.NETCore.App.Ref`, so they cannot be added through `ReferenceName`.
- **Acceptance criteria:** An example with `ServiceCollection`, lifetimes and console logging runs. Download size is recorded in "Entscheidungen in Kurzform".
- **Time limit:** 0.5 day · **Fallback:** module 15 as examples that cannot run.
- **Blocks:** module 15.

## Technical backlog from the MVP comparison (not spikes)

| # | Task | Depends on | Status |
| --- | --- | --- | --- |
| B1 | Convert MVP lessons: identifiers and comments in English, new folder structure, fix the solution text "`3,5` or `3.5`" (the output is always `3.5`) | S1 | erledigt (done) |
| B2 | Start code in English: `playground/Samples.cs`, `website/src/pages/playground.js` | – | erledigt (done) |
| B3 | Quiz questions in data files per language, quiz types, versioned progress store, Leitner, dashboard, export/import, migration of the old keys | – | erledigt (done; rest: CI check #42) |
| B4 | Adapt the README section "Inhalte schreiben" (writing content) to the new rules | S1 | erledigt (done) |
| B5 | Translatable runner UI (language parameter) | – | offen (open) |
| B6 | Console input: `stdin` prop, input field, `Console.SetIn` | – | offen (open) |
| B7 | Commit `package-lock.json`, switch `pages.yml` to `npm ci` | – (workflow → human) | erledigt (done) |
| B8 | Issue template "Fehler melden / Feedback" + link per page | – | offen (open) |
| B9 | Mermaid, local search, Playwright + accessibility check | – | offen (open) |
| B10 | `lernprinzipien.mdx` → "Wie dieser Kurs funktioniert": literature from `didaktik.md`, AI paragraph, how to use | B3 | offen (open) |
| B11 | `<Since>` badge, switcher tabs | – | erledigt (done) |
