# Style and language

## Languages

- **Repository content is English.** This covers code, identifiers, comments, CSS, docs, commit messages, PR titles and descriptions, and replies by Claude to issues in PRs.
- **Only learner-facing content is German:**
  - the learning content in `website/docs/**/*.mdx`
  - learner-facing UI texts (React components, playground UI, Docusaurus config texts)
- **More languages later:** through the built-in Docusaurus i18n (`i18n.locales`, for now only `de`). Translations go later under `website/i18n/<locale>/…`. MDX pages are translated as a whole.
- **Code shown to learners** (examples, outputs, comments, identifiers) is always English. This includes examples, quizzes and the playground start code. Explanations belong in the lesson text or in the solution, not in code comments.
- **Technical terms:** English. At first use, add a German explanation: „Value Type (Werttyp)“. Every term goes into the glossary.
- **UI texts** (React components): use `<Translate>`/`code.json`, do not hard-code them.
- **Quiz texts, learning goals, hints:** in language-specific data files (see `komponenten.md`).
- **Slugs:** English, so URLs stay the same in all languages.

## Tone

This section describes the tone of the German learner text.

- Informal "du" form, matter-of-fact, friendly. Short sentences. One idea per paragraph.
- No judging superlatives („genial“ (brilliant), „erschreckend“ (shocking)). No empty phrases.
- Mistakes are a normal step in learning. Feedback explains instead of judging.
- Checks are neutral: ✓/✗, „Stimmt“ (Correct) / „Stimmt nicht – erwartet war …“ (Not correct – expected was …).

## Code style

- C# 14 / .NET 10, top-level statements (type declarations at the end of the file), nullable enabled, `var` where the type is obvious.
- Prefer modern idioms. Show older forms only where you often meet them in other people's code. Then add the `<Since>` badge and „So schrieb man es vorher“ (How it was written before).
- Naming conventions follow Microsoft Learn (PascalCase for types and members, camelCase for local variables).
- Every example runs on its own or is clearly marked as a fragment (file extension `.snippet.cs`, skipped by CI).
- Write outputs so they do not depend on culture (the playground uses `InvariantGlobalization`): `3.5`, not `3,5`.

## Code location

- Path: `website/examples/mXX/lYY/<english-name>.cs`, one example per file.
- Included with `raw-loader` (official Docusaurus docs, "Importing code snippets").
- Shown outputs, SQL statements, HTTP responses and test runner outputs come from real CI runs (S2).

## Sources and currency

- Technical statements → Microsoft Learn or the C# specification. At the end of a lesson, add „Weiterlesen“ (further reading) with 1–3 links.
- Teaching literature appears only on the page „Wie dieser Kurs funktioniert“ (How this course works), not in every lesson.
- Content about versions, history and AI carries „Stand: YYYY-MM-DD“ (as of: date) and is reviewed twice a year.
- Do not make things up. If a source is missing, leave the statement out or mark it as an open question.

## Accessibility (WCAG 2.2 AA)

- All interactions work with the keyboard. Focus is visible.
- Body text contrast ≥ 4.5:1 in light and dark mode.
- Diagrams and visualizations have a text description. Color is never the only signal.
- Live regions (`aria-live`) for runner and quiz results.
