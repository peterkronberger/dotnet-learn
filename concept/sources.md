# Sources – verification and visible citation

As of 2026-10-27.

## Principle

Every statement is verified. Not every statement is cited in the text. The course text states facts directly, in its own words. A source in the text is a service for the learner (to check, to read more, to see the date), never a way to hand off responsibility.

## Level 0 – Verification (always, not visible in the lesson)

- Every technical statement is checked against a primary source before it is written. Use the layer from `concept/module-plan.md`, "Reference: which source counts as the specification": C# standard draft → feature specification; library behavior → Microsoft Learn conceptual docs or API reference; dates and support → the .NET support policy.
- The PR that adds or changes lesson text contains a section **"Verification"**: a table with one row per checked statement: short statement · URL (with clause or heading) · date checked. This is the audit trail. It does not go into the lesson.
- If no primary source can be found, leave the statement out or mark it as an open question (`CLAUDE.md` rule 7).

Format of the table in the PR:

```markdown
## Verification

| Statement | Source (URL, clause or heading) | Checked |
| --- | --- | --- |
| A statement list runs in order | https://learn.microsoft.com/dotnet/csharp/language-reference/language-specification/statements – §13.3.2 Statement lists | 2026-10-27 |
| .NET 10 is an LTS release | https://dotnet.microsoft.com/platform/support/policy/dotnet-core – table of supported versions | 2026-10-27 |
```

## Level 1 – No visible source (default)

Stable basic facts that the lesson itself explains and that the learner can observe in the runner (for example "a statement ends with `;`", "`WriteLine` ends the line"). The runner output is the evidence. No link, no attribution.

## Level 2 – Inline link in the body text (only in these cases)

1. **Time-dependent facts** (versions, release dates, support periods, plans): link to the primary source plus "Stand: YYYY-MM-DD" (as of date).
2. **Numbers or limits the learner cannot check in the runner** (for example "ReadyToRun files are usually two to three times larger").
3. **Normative language rules** the learner may want to read in the original: preferably in a "Vertiefung" section, with the clause as link text (for example "§13.3.2 Statement lists").
4. **Statements that contradict a common belief** (inside an "Achtung" box): optional link.
5. **Information that has only a secondary source** (for example a planned release date): the only case where the provenance is part of the content. Word it as a status, not as an attribution: "geplant ist der 10. November 2026 (Stand: 2026-10-27, noch nicht offiziell bestätigt)", with the link.

**Form:** the link sits on a descriptive noun phrase ("[Support-Richtlinie für .NET](…)"), never on "hier" (here) or on an attribution verb. At most one inline link per paragraph.

## Level 3 – "Weiterlesen"

1–3 links per lesson for further reading, to Microsoft Learn or the C# specification. The links must exist.

## Level 4 – Teaching literature

Only on the page "Wie dieser Kurs funktioniert" (how this course works). The references are kept in `concept/didactics.md`.

## Wording rules

- **Forbidden** in lesson text, quiz texts and glossary definitions: attribution phrases that shift responsibility, for example "laut Microsoft", "laut Microsoft Learn", "laut Quelle", "laut derselben Seite", "laut Spezifikation", "laut Presseberichten", "die Quelle sagt", "Microsoft (Learn) schreibt / beschreibt / nennt / rät", "gemäß der Dokumentation". Exception: level 2 case 5 uses the status wording above.
- **No English quotes** in German text. Paraphrase in German. A direct quote is allowed only when the exact wording is the subject (for example an official product name or definition), at most once per lesson, with a German translation. This is a review point, not an automatic check.
- The specification may be the *subject* of a sentence when the lesson teaches the difference between language and implementation (M0 L2), for example "Die Spezifikation legt fest, …". That is content, not attribution.

## Automatic check

`node tools/check-quiz.mjs` checks the attribution phrases above line by line, case-insensitive, in:

- `website/docs/**/*.mdx`
- `website/src/data/quiz/de/*.json`
- `website/src/data/glossary/de.json`

Source of truth is the constant `FORBIDDEN_PHRASES` in `tools/check-quiz.mjs`. This file documents the same list. If the two differ, the script wins; change both together.

| # | Regular expression (JavaScript, flag `i`) | Example match | Why |
| --- | --- | --- | --- |
| 1 | `\blaut\s+(Microsoft\|Quelle\|derselben\s+Seite\|Spezifikation\|Presseberichten\|Support-Policy\|Dokumentation)` | "laut Microsoft Learn", "laut Quelle" | Hands the responsibility for the fact to the source |
| 2 | `\bdie\s+Quelle\s+sagt` | "Die Quelle sagt" | Same, and it does not say which source |
| 3 | `\bMicrosoft(\s+Learn)?\s+(schreibt\|beschreibt\|nennt\|rät\|sagt)\b` | "Microsoft Learn beschreibt", "Microsoft nennt" | The company is made the speaker instead of stating the fact |
| 4 | `\bgemäß\s+(der\s+)?Dokumentation` | "gemäß der Dokumentation" | Hands the responsibility to "the documentation" |

(In this table `|` is escaped as `\|`; in the script it is a plain `|`.)

"Die Spezifikation legt fest / schreibt vor" is not matched on purpose. English quotes are not checked automatically (too many false positives with code and product names).

**Current state:** the check reports a **warning** (`WARN`, exit code stays 0), because module 0 still contains known findings. The module 0 wording cleanup (#126) removes them and switches the check to an error in the same PR.

## Evidence

- Links inside a text are not free: hypertext reading adds decision-making and visual-processing demands, and restricting links helped readers [DeStefano & LeFevre 2007].
- Extraneous material binds limited working memory [Sweller, van Merriënboer & Paas 2019].
- The wording rules (no attribution phrases) are an **editorial decision**, not a study finding. The studies support only the general point that extra links and extra material cost attention.

References: see `concept/didactics.md`.
