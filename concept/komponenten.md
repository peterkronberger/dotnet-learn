# Components and conventions

Note: The course text for learners (MDX lessons) is German; this document is English.

Stand: 2026-10-23 (as of). Status: **vorhanden** (exists) · **geplant** (planned). All components are registered globally in `website/src/theme/MDXComponents.js` (no import needed in MDX).

## Admonitions (Docusaurus, vorhanden)

| Zweck | Syntax |
| --- | --- |
| Learning goals | `:::note[Lernziele]` |
| Prediction | `:::info[Vorhersage]` |
| Misconception | `:::warning[Achtung]` |
| Typical AI answer (module 19 only) | `:::note[Typische KI-Antwort (konstruiertes Beispiel)]` |

Collapsible parts: `<details><summary>Lösung und Erklärung anzeigen</summary> … </details>` (show solution and explanation) or `Vertiefung: … (optional)` (deep dive). Put blank lines before and after the content.

## `<CSharpRunner>` – exists, will be extended

Today: `code` (string), `height` (px, default 460). It embeds `/runner/` as an iframe. The code goes in the URL fragment as Base64url, `loading="lazy"`, theme via `postMessage` (`{type: 'theme', value}`).

Exists (S7):
- `id`: stable key of the instance (default: automatic). It maps `run-result` to the right instance.
- `predict` (bool) + `expected` (string, output snapshot): one input field per expected output line. After „Ausführen“ (run), ✓/✗ per line, worded plainly, result in an `aria-live` region. The check is against the real output.
- `tests` (string): C# test code with `public static class TaskTests` and `Run()` (tuple `Name, Visible, Passed, Feedback`). The Runner then runs only the tests. Result per test, hidden tests with a neutral name.
- `onResult`: callback with the `run-result` message.
- `code` comes from a `.cs` file (`import x from '!!raw-loader!@site/examples/…'`, S1). The old README limit "no backticks, backslashes, `${`" no longer applies.
- `trace` (bool, S5): turns on the step view (variables, heap model).
- Demo: `/runner-demo`.

Planned:
- `stdin` (string): prefilled console input, read line by line by `Console.ReadLine()`. In the Runner it is an input field under the editor.
- Loading: unload instances that are scrolled far away; typed code is kept; runtime from the browser cache.
- Language of the Runner UI (today fixed German in `App.razor`) through a parameter (for example `#lang=de`) – for i18n.

Message protocol (S7, exists; origin check `window.location.origin` in both directions, the page also checks `event.source` and `id`):
- Page → Runner: `{type: 'theme', value}`; configuration in the fragment: `#code=<b64>&id=<id>&tests=<b64>`
- Runner → page: `{type: 'run-result', id, success, output, diagnostics: [{isError, text, line}], tests?: [{name, visible, passed, feedback}]}`

## Playground (`playground/`) – exists

- `<textarea>` editor; Tab, Esc, Ctrl/⌘+Enter exist (Strg/⌘+Enter in the German UI). No autocomplete (decided).
- Syntax colors, line highlight, error marks – exist (S4): own tokenizer in `playground/wwwroot/js/editor.js` (no Prism). Overlay lines use the classes `pg-hl-line*`. They must not change text metrics (font, font weight, italic, ligatures, padding, border). Otherwise the cursor sits in the wrong place.
- Loop guard (stops after about 3 s with a German message and line number) – exists (S3). No guard in operators, accessors, and expression bodies. `catch (Exception)` in user code catches it.
- Compiler errors as a list with line/column (exists) + German explanation of common errors in addition to the original message (planned).
- Step-by-step visualization (variables, heap model), can be turned on per Runner with `trace` – exists (S5).
- Fixed preloaded packages (`Microsoft.Extensions.DependencyInjection`, `…Logging`) – exist (S6). Configuration and Hosting are missing. No free NuGet install.
- `InvariantGlobalization=true`: numbers always appear with a dot (`3.5`). CI must run with `InvariantCulture` so that shown outputs match.

## `<Quiz>` – exists (single choice), will grow into a family

Today: `id`, `question`, `options`, `answer`, `children` (explanation). Progress is stored under `dotnet-learn:quiz:<id>` in localStorage.

Planned:
- **Questions in data files per language** instead of inline in MDX, for example `website/src/data/quiz/de/m01.json`. In MDX only `<Quiz id="m01-l02-q3" />`. Reason: the Leitner dashboard and placement tests need all questions in one place; translation per language. Migrate existing inline quizzes.
- Types: `single`, `multi`, `output` (predict the output), `parsons` (with keyboard alternative), `cloze` (code fill-in), `find-bug` (click a line), `match` (matching), `explain` (explain in your own words, reveal a model answer, self-rating).
- Each answer option can carry a `misconception` reference (link to the „Achtung“ box, DoD point 11).
- Feedback always explains the *why*.

## Lernfortschritt (learning progress) – planned

- One versioned store (for example `dotnet-learn:v1:progress`) in localStorage/IndexedDB, all access in `try/catch`. Old keys `dotnet-learn:quiz:*` are migrated.
- **Leitner system** with 5 boxes. Start values of the intervals 1 · 2 · 4 · 8 · 16 days (an assumption, can be changed).
- **„Heute fällig“ dashboard** (due today) on the start page.
- **Placement test** per module (8–12 questions, ≥ 80 % → "known", questions go to a higher box).
- **Export/Import** as a JSON file.

## Weitere Komponenten (more components) – planned

| Komponente | Zweck |
| --- | --- |
| `<Since csharp="13" dotnet="9" />` | Version badge for features from C# 12 / .NET 8, with a collapsible „So schrieb man es vorher“ (how it was written before) |
| Switcher box (Umsteiger-Box) | Docusaurus `<Tabs groupId="from-language">` with Java / Python / JS/TS / Go. The choice carries across pages (check how `groupId` behaves in the Docusaurus docs before building) |
| Feedback link | Per page „Fehler melden / Feedback“ (report error / feedback) → prefilled GitHub issue template with the page URL (create the template under `.github/ISSUE_TEMPLATE/`) |
| `<CodeBlock>` | Official Docusaurus component for code from `.cs` files that cannot run |

## Site extensions

- `@docusaurus/theme-mermaid` for diagrams (with a text description).
- Local full-text search (when choosing a package, check Docusaurus 3 and i18n compatibility).
- Playwright E2E tests with an automatic accessibility check (for example axe-core) in CI.
- Later: `@docusaurus/plugin-pwa`.
