# Style and language

## Languages

- **Lesson text and learner-facing UI:** German. Several languages will come through the built-in Docusaurus i18n (`i18n.locales`, only `de` for now). Translations come later under `website/i18n/<locale>/…`. MDX pages are translated as a whole.
- **Everything else** (docs, concept files, PRs, commits, issues): English.
- **Code, identifiers, code comments:** always English – also in examples, quizzes and the playground start code. Explanations belong in the body text or in the solution, not in German comments.
- **Technical terms:** English. At first use, add a German explanation: "Value Type (Werttyp)". Every term goes into the glossary (`website/src/data/glossary/de.json`). At the first use in a lesson, link it with `<Term id="…">`; later uses stay plain text.
- **UI texts** (React components): use `<Translate>`/`code.json`, do not hard-code them.
- **Quiz texts, learning goals, hints:** in language-specific data files (see `components.md`).
- **Slugs:** English, so URLs stay the same in all languages.

## Tone

- Informal "Du" form, matter-of-fact, friendly. Short sentences. One idea per paragraph.
- No judging superlatives ("genial" = brilliant, "erschreckend" = alarming). No filler phrases.
- Mistakes are a normal step in learning. Feedback explains instead of judging.
- Checks are sober: ✓/✗, "Stimmt" (correct) / "Stimmt nicht – erwartet war …" (not correct – expected was …).

## Code style

- C# 14 / .NET 10, top-level statements (type declarations at the end of the file), nullable enabled, `var` where the type is obvious.
- Prefer modern idioms. Show older styles only where people often meet them in other people's code. Then add the `<Since>` badge and "So schrieb man es vorher" (how it used to be written).
- Naming conventions follow Microsoft Learn (PascalCase for types/members, camelCase for local variables).
- Every example runs on its own or is clearly marked as a fragment (file ending `.snippet.cs`, skipped by CI).
- Write outputs so that they do not depend on culture (the playground uses `InvariantGlobalization`): `3.5`, not `3,5`.

## Code location

- Path: `website/examples/mXX/lYY/<english-name>.cs`, one example per file.
- Include with `raw-loader` (official Docusaurus docs, "Importing code snippets").
- Shown outputs, SQL statements, HTTP responses and test runner outputs come from real CI runs (S2).

## Sources and currency

- Verification, visible citation, "Weiterlesen" and wording rules: `concept/sources.md`.
- Content about versions, history and AI carries "Stand: YYYY-MM-DD" (as of date) and is reviewed twice a year.

## Accessibility (WCAG 2.2 AA)

- All interactions work with the keyboard. Focus is visible.
- Contrast for body text ≥ 4.5:1 in light and dark mode.
- Diagrams and visualizations have a text description. Color is never the only signal.
- Live regions (`aria-live`) for results of the runner and quizzes.
