# Components and conventions

As of 2026-10-24. Status: **available** · **planned**. All components are registered globally in `website/src/theme/MDXComponents.js` (no import needed in MDX).

## Admonitions (Docusaurus, available)

| Purpose | Syntax |
| --- | --- |
| Learning goals | `:::note[Lernziele]` |
| Prediction | `:::info[Vorhersage]` |
| Misconception | `:::warning[Achtung]` |
| Project goal of a module (first lesson of every module) | `:::tip[Projekt: Was SecLog nach diesem Modul kann]` |
| Typical AI answer (module 19 only) | `:::note[Typische KI-Antwort (konstruiertes Beispiel)]` |

Collapsible: `<details><summary>Lösung und Erklärung anzeigen</summary> … </details>` or `Vertiefung: … (optional)` (blank lines before and after the content).

## `<CSharpRunner>` – available, being extended

Today: `code` (string). The height is automatic: the iframe is as high as the runner page (code, output, diagnostics, test results, step view), so nothing scrolls inside it. Before the runner has loaded, the iframe reserves a height estimated from the line count of `code`. Optional `minHeight` (px, default 0) only for the full-page playground; lessons never set a height. Embeds `/runner/` as an iframe, code as Base64url in the URL fragment, `loading="lazy"`, theme via `postMessage` (`{type: 'theme', value}`).

Available (S7):
- `id`: stable key of the instance (default: automatic). Matches `run-result` to the right instance.
- `predict` (bool) + `expected` (string, output snapshot): one input field per expected output line. After "Ausführen" (run), ✓/✗ per line in neutral wording, result in an `aria-live` region. The check is against the real output.
- `tests` (string): C# check code with `public static class TaskTests` and `Run()` (tuple `Name, Visible, Passed, Feedback`). The runner then runs only the tests. Result per test, hidden tests with a neutral name.
- `onResult`: callback with the `run-result` message.
- `code` comes from a `.cs` file (`import x from '!!raw-loader!@site/examples/…'`, S1). The former README limit "no backticks, backslashes, `${`" no longer applies.
- `trace` (bool, S5): turns on the step view (variables, heap model).
- `stdin` (string): pre-filled console input, read line by line by `Console.ReadLine()`; after the last line it returns `null`. Shown in the runner as a labeled input field ("Eingabe (Konsole)") below the editor; the learner can edit it and "Zurücksetzen" restores it. The field exists only if `stdin` is set (an empty string gives an empty field). Without `stdin`, `Console.In` is an empty reader, so `Console.ReadLine()` returns `null` at once. Works in normal, test and trace runs. Fragment key: `&stdin=<Base64url>`. Not on the full playground page.
  - CI: an example `<name>.cs` may have `<name>.in.txt`; `check-examples.sh` feeds it as stdin (otherwise `/dev/null`). The lesson imports the same file with `raw-loader` and passes it as `stdin`.
- Demo: `/dev/runner-demo`.
- All dev showcase pages are listed on `/dev/` (`<DevNav />`, list in `website/src/components/DevNav/index.js`). Add new dev pages there.

Planned:
- Loading: unload instances that are scrolled far away, keep the typed code; runtime from the browser cache.
- Language of the runner UI (today fixed German in `App.razor`) through a parameter (e.g. `#lang=de`) – for i18n.

Message protocol (S7, available; origin check `window.location.origin` in both directions, the page also checks `event.source` and `id`):
- Page → runner: `{type: 'theme', value}`; configuration in the fragment: `#code=<b64>&id=<id>&tests=<b64>&trace=1&stdin=<b64>`
- Runner → page: `{type: 'run-result', id, success, output, diagnostics: [{isError, text, line}], tests?: [{name, visible, passed, feedback}]}`
- Runner → page: `{type: 'resize', id, height}` – sent whenever the content height of the runner page changes; the page sets the iframe height from it (same origin, `event.source` and `id` checks as for `run-result`).

## Playground (`playground/`) – available

- `<textarea>` editor; Tab, Esc, Ctrl/⌘+Enter available. No autocompletion (decided).
- Syntax colors, line highlight, error marking – available (S4): own tokenizer in `playground/wwwroot/js/editor.js` (no Prism). Overlay lines use the classes `pg-hl-line*`. They must not change text metrics (font, font weight, italic, ligatures, padding, border), or the cursor sits in the wrong place.
- The editor grows with its code (minimum about 3 lines) and never scrolls vertically; long lines wrap. The highlight overlay keeps identical text metrics.
- Loop guard (stop after about 3 s with a German message and line number) – available (S3). No guard in operators, accessors and expression bodies; `catch (Exception)` in user code catches it.
- Compiler errors as a list with line/column (available) + German explanation of common errors in addition to the original message (planned).
- Step-by-step visualization (variables, heap model), switchable per runner with `trace` – available (S5).
- Fixed preloaded packages (`Microsoft.Extensions.DependencyInjection`, `…Logging`) – available (S6); Configuration and Hosting are missing. No free NuGet install.
- `InvariantGlobalization=true`: numbers always appear with a dot (`3.5`). CI must run with `InvariantCulture` so that shown outputs match.

## `<Quiz>` – available, being grown into a family

Available:
- **Questions in data files per language**, e.g. `website/src/data/quiz/de/m01.json` (format: `website/src/data/quiz/README.md`). In MDX only `<Quiz id="m01-l03-q1" />`. Existing inline quizzes are migrated.
- Types: `single`, `multi`, `output` (predict the output), `explain` (explain in your own words, reveal a model answer, self-rating), `parsons` (put the lines of a CI-checked example file in order, lines in random order; buttons for add/remove/move, announced in an `aria-live` region; optional distractor lines), `cloze` (fill the blanks in a CI-checked example file; one input per blank, ✓/✗ as text per blank, optional `accept` list and `wrongAnswers` with misconception links), `find-bug` (select the faulty line(s) of an example file; native radio/checkbox per line with label "Zeile N: code", ✓/✗ as text, per-line feedback with misconception links; `*.snippet.cs` allowed), `match` (match terms to each other; one native `<select>` per left term, labelled with the term, right entries in random order; ✓/✗ as text per row, `wrongMatches` with feedback and misconception links).
- Answer order: `single`/`multi` options, the `parsons` line pool and the `match` right entries are shuffled on every mount and every retry (shared helper `website/src/components/Quiz/shuffle.js`, shuffle after mount in a layout effect, so the server render matches the first client render). Options with `"pin": "last"` stay at the end. Evaluation maps back to the original option.
- Every answer option can carry a `misconception` reference. `<Misconception id="…" />` is the anchor in the "Achtung" box (DoD point 11). `node tools/check-quiz.mjs` checks IDs and links.
- Feedback always explains the *why*.
- `deferred` prop of the question components: the answer is reported to the parent (used by `<PlacementTest>`) instead of being checked and stored at once; no feedback and no Leitner entry until the parent evaluates.

## Learning progress – available

Available:
- One versioned store `dotnet-learn:v1:progress` in localStorage, all access in `try/catch`. Fields: `questions` (Leitner state per question), `lessons` (`{completedAt}` per `lesson_id`, set by the button "Lektion abschließen" at the end of every lesson; `via: "placement"` marks a lesson completed by the placement test), `placement` (`{date, correct, total, passed}` per module key, stored only on a pass) and `activity` (`{answered, correct, partial}` per day, no time limit). The old keys `dotnet-learn:quiz:*` are no longer migrated (there was no stored data yet).
- **Leitner system** with 5 boxes; start values of the intervals 1 · 2 · 4 · 8 · 16 days (assumption, adjustable). For display, the boxes form three stages: box 1 "wackelig" (shaky), box 2–3 "im Aufbau" (building), box 4–5 "gefestigt" (solid) – a decision, not an empirical value.
- **Page "Lernstand"** (`/dashboard`, navbar): Heute (next lesson with learning goals, due reviews with an inline session), Überblick (key figures), Lernpfad (lessons per module with status), Wissensstand (stages and boxes), Aktivität (chart and table: 4 weeks / 3 months / all), Deine Daten (export/import). No streaks, points or badges. The start page shows a teaser card.
- **Lesson index:** the local plugin `website/plugins/lesson-index.js` reads every doc with `lesson_id` at build time (title, `description`, `duration_min`, the bullet points of `:::note[Lernziele]`, module label from `_category_.json`). The learning goals stay in the lesson; new lessons appear automatically.
- **Export/import** as a JSON file (with lessons and activity).
- Test page `/dev/progress-selftest`.

- **Placement test** – available: `<PlacementTest module="N" />` (registered in `MDXComponents.js`). Place: the first lesson of the module, after "Lernziele". Flow: the questions are deferred question components (no feedback while answering), the evaluation comes at the end (pass = at most 1 wrong answer), then retake and reset. Data: `"placement": true` on the questions and `placementLessons` in the quiz data, checked by `check-quiz.mjs`. Option A: the answers are not stored in the Leitner system; on a pass every lesson of the module gets `via: "placement"`. A lesson completed this way shows "· per Einstufungstest" in "Lernpfad" and in `<LessonComplete>` (`completedVia` in `lessonOverview`).

## More components

| Component | Status | Purpose |
| --- | --- | --- |
| `<Term id="…">` | available | Link to the glossary entry at the first use of a term in a lesson (`<Term id="value-type" />`, or own link text as children) |
| `<Since csharp="13" dotnet="9" />` | available | Version badge for features from C# 12 / .NET 8, with a collapsible "So schrieb man es vorher" (how it used to be written) |
| `<FeedbackLink />` (`website/src/components/FeedbackLink`) | available (B8) | Link "Fehler melden / Feedback" at the end of every docs page → GitHub issue form `.github/ISSUE_TEMPLATE/feedback.yml` with page URL and `lesson_id` pre-filled; the form sets the label `needs-human` |
| `<Forward concepts={[…]} lesson="…" />` or `module="N"` | available | Fixed German note for a forward reference: "Das Konzept von **X** wurde bisher nicht erläutert und folgt in der Lektion „…“ / Modul N. Für dieses Beispiel ist das weitere Verständnis dafür nicht notwendig." (plural form for several concepts; backtick parts render as code). Exactly one of `lesson` or `module`. An unknown `lesson`, an early-implementation lesson (`m02-l01`) or other invalid props fail the build with a `<Forward>:` message. Rules: `concept/lesson-template.md`, "Forward references". Demo: `/dev/components-demo` |
| `<Diagram caption="…" description="…">` | available | Mermaid code block inside, `accTitle` set (same text as `caption`). Renders `<figure>` with `<figcaption>` and a `<details>` "Diagramm als Text". Text description is required (a missing one fails the build). Color is never the only signal. |
| `<CodeBlock>` | Official Docusaurus component for code that cannot run, taken from `.cs` files |

## Glossary – available

- Page `/glossary` (navbar "Glossar") with a search field.
- Data: `website/src/data/glossary/<locale>.json`, format in its `README.md`.
- Anchors: `/glossary#<id>`.
- `<Term id="…">` is a normal link (no tooltip, because of WCAG 2.2 SC 1.4.13). An unknown id fails the build.
- Checks: `node tools/check-quiz.mjs` (glossary data and `<Term>` ids).

## Site extensions

- `@docusaurus/theme-mermaid` for diagrams – **available** (light theme `neutral`, dark theme `dark`). Use it through `<Diagram>` (with a text description). Demo: `/dev/diagram-demo`.
- Local full-text search – **available**. Own build-time index plugin `website/plugins/search-index.js` (format and static pages: `website/src/data/search/README.md`), navbar field `@theme/SearchBar` (top 3 hits, Enter or icon opens the search page) and page `/search?q=…` (all hits grouped by page, `noindex`). Indexed: lessons (by heading section), glossary entries, and the static pages Lernstand and Playground. Quiz questions are **not** indexed, because their texts and answers would reveal solutions in the results. Hits on the Lernstand page open `/dashboard#<section>`; the dashboard scrolls to the section after it has rendered. Self-test: `/dev/search-selftest`.
- Playwright E2E tests with an automatic accessibility check (e.g. axe-core) in CI.
- Later: `@docusaurus/plugin-pwa`.
