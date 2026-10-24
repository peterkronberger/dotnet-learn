# Components and conventions

As of 2026-10-23. Status: **available** · **planned**. All components are registered globally in `website/src/theme/MDXComponents.js` (no import needed in MDX).

## Admonitions (Docusaurus, available)

| Purpose | Syntax |
| --- | --- |
| Learning goals | `:::note[Lernziele]` |
| Prediction | `:::info[Vorhersage]` |
| Misconception | `:::warning[Achtung]` |
| Typical AI answer (module 19 only) | `:::note[Typische KI-Antwort (konstruiertes Beispiel)]` |

Collapsible: `<details><summary>Lösung und Erklärung anzeigen</summary> … </details>` or `Vertiefung: … (optional)` (blank lines before and after the content).

## `<CSharpRunner>` – available, being extended

Today: `code` (string), `height` (px, default 460). Embeds `/runner/` as an iframe, code as Base64url in the URL fragment, `loading="lazy"`, theme via `postMessage` (`{type: 'theme', value}`).

Available (S7):
- `id`: stable key of the instance (default: automatic). Matches `run-result` to the right instance.
- `predict` (bool) + `expected` (string, output snapshot): one input field per expected output line. After "Ausführen" (run), ✓/✗ per line in neutral wording, result in an `aria-live` region. The check is against the real output.
- `tests` (string): C# check code with `public static class TaskTests` and `Run()` (tuple `Name, Visible, Passed, Feedback`). The runner then runs only the tests. Result per test, hidden tests with a neutral name.
- `onResult`: callback with the `run-result` message.
- `code` comes from a `.cs` file (`import x from '!!raw-loader!@site/examples/…'`, S1). The former README limit "no backticks, backslashes, `${`" no longer applies.
- `trace` (bool, S5): turns on the step view (variables, heap model).
- Demo: `/runner-demo`.

Planned:
- `stdin` (string): pre-filled console input, read line by line by `Console.ReadLine()`. Shown in the runner as an input field below the editor.
- Loading: unload instances that are scrolled far away, keep the typed code; runtime from the browser cache.
- Language of the runner UI (today fixed German in `App.razor`) through a parameter (e.g. `#lang=de`) – for i18n.

Message protocol (S7, available; origin check `window.location.origin` in both directions, the page also checks `event.source` and `id`):
- Page → runner: `{type: 'theme', value}`; configuration in the fragment: `#code=<b64>&id=<id>&tests=<b64>`
- Runner → page: `{type: 'run-result', id, success, output, diagnostics: [{isError, text, line}], tests?: [{name, visible, passed, feedback}]}`

## Playground (`playground/`) – available

- `<textarea>` editor; Tab, Esc, Ctrl/⌘+Enter available. No autocompletion (decided).
- Syntax colors, line highlight, error marking – available (S4): own tokenizer in `playground/wwwroot/js/editor.js` (no Prism). Overlay lines use the classes `pg-hl-line*`. They must not change text metrics (font, font weight, italic, ligatures, padding, border), or the cursor sits in the wrong place.
- Loop guard (stop after about 3 s with a German message and line number) – available (S3). No guard in operators, accessors and expression bodies; `catch (Exception)` in user code catches it.
- Compiler errors as a list with line/column (available) + German explanation of common errors in addition to the original message (planned).
- Step-by-step visualization (variables, heap model), switchable per runner with `trace` – available (S5).
- Fixed preloaded packages (`Microsoft.Extensions.DependencyInjection`, `…Logging`) – available (S6); Configuration and Hosting are missing. No free NuGet install.
- `InvariantGlobalization=true`: numbers always appear with a dot (`3.5`). CI must run with `InvariantCulture` so that shown outputs match.

## `<Quiz>` – available (single choice), being grown into a family

Today: `id`, `question`, `options`, `answer`, `children` (explanation). Progress is stored under `dotnet-learn:quiz:<id>` in localStorage.

Planned:
- **Questions in data files per language** instead of inline in MDX, e.g. `website/src/data/quiz/de/m01.json`. In MDX only `<Quiz id="m01-l02-q3" />`. Reason: the Leitner dashboard and placement tests need all questions in one place; translation per language. Migrate existing inline quizzes.
- Types: `single`, `multi`, `output` (predict the output), `parsons` (with keyboard alternative), `cloze` (code with gaps), `find-bug` (click the line), `match` (matching), `explain` (explain in your own words, reveal a model answer, self-rating).
- Every answer option can carry a `misconception` reference (link to the "Achtung" box, DoD point 11).
- Feedback always explains the *why*.

## Learning progress – planned

- One versioned store (e.g. `dotnet-learn:v1:progress`) in localStorage/IndexedDB, all access in `try/catch`. Old keys `dotnet-learn:quiz:*` are migrated.
- **Leitner system** with 5 boxes; start values of the intervals 1 · 2 · 4 · 8 · 16 days (assumption, adjustable).
- **"Heute fällig" dashboard** (due today) on the start page.
- **Placement test** per module (8–12 questions, ≥ 80 % → "known", questions go to a higher box).
- **Export/import** as a JSON file.

## More components – planned

| Component | Purpose |
| --- | --- |
| `<Since csharp="13" dotnet="9" />` | Version badge for features from C# 12 / .NET 8, with a collapsible "So schrieb man es vorher" (how it used to be written) |
| Switcher box | Docusaurus `<Tabs groupId="from-language">` with Java / Python / JS/TS / Go; choice is kept across pages (check the behavior of `groupId` in the Docusaurus docs before building) |
| Feedback link | Per page "Fehler melden / Feedback" (report error / feedback) → pre-filled GitHub issue template with the page URL (create the template under `.github/ISSUE_TEMPLATE/`) |
| `<CodeBlock>` | Official Docusaurus component for code that cannot run, taken from `.cs` files |

## Site extensions

- `@docusaurus/theme-mermaid` for diagrams (with a text description).
- Local full-text search (when choosing a package, check Docusaurus 3 and i18n compatibility).
- Playwright E2E tests with an automatic accessibility check (e.g. axe-core) in CI.
- Later: `@docusaurus/plugin-pwa`.
