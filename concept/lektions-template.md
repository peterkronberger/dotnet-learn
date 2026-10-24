# Lektions-Template

Note: The course text for learners (MDX lessons) is German; this document is English.

Every lesson follows this order. Optional parts are in brackets. Length 15–25 min, one core concept. Check against the Definition of Done in `CLAUDE.md`.

## Ablauf (Flow)

| # | Schritt | Form | Hinweis |
| --- | --- | --- | --- |
| 1 | Learning goals | `:::note[Lernziele]`, 2–4 points | As observable behavior: „Du kannst …“ (you can …), „Du erkennst …“ (you recognize …) |
| 2 | Warm-up | 2–3 `<Quiz>` on earlier lessons | Heading „Rückblick“ (review); retrieval practice, not new material |
| 3 | Concept | Short text + small, isolated example | Only the current concept; no project context |
| 4 | Practice example | **Practice example pattern** (below) | Required, at least once |
| 5 | Gap example | Example with gaps, subgoal labels stay | Fading: the gaps are in the code |
| 6 | Modify and make your own | Automatically checked task | Automatically checked task with `<CSharpRunner tests={…}>`; a collapsed model solution only as a fallback, for example for topics that cannot run |
| 7 | Misconceptions | `:::warning[Achtung]`, 1–3 short boxes | Each box is linked to a quiz distractor |
| 8 | (Switcher box) | Tabs Java / Python / JS/TS / Go | Only differences to C#, do not explain C# again |
| 9 | (Deep dive) | `<details>` „Vertiefung: … (optional)“ | Replaces „Unter der Haube“ (under the hood); technical claims with a Microsoft Learn source |
| 10 | Summary | 3–5 sentences | No new content |
| 11 | Closing | Quiz (→ Leitner pool), „In eigenen Worten erklären“ (explain in your own words), transfer question, (SecLog extra exercise), „Weiterlesen“ (further reading) | „Weiterlesen“: 1–3 links to Microsoft Learn / C# specification |

## Praxisbeispiel-Muster (practice example pattern; required in step 4)

This matches PRIMM. It is already built this way in the MVP (`website/docs/grundlagen/variablen-und-typen.mdx`, section „Zwei Stolperfallen“):

1. **Prediction** (`:::info[Vorhersage]`): a task to think ahead about what the result will be. Optionally with an input field per output line. After the run, a plain check ✓/✗ (no "surprise" wording). The input field comes from `<CSharpRunner predict expected={…}>`, with `expected` from the `.out.txt` snapshot via `raw-loader`.
2. **`<CSharpRunner>`** (Run)
3. **`<details>` „Lösung und Erklärung anzeigen“** (show solution and explanation) (Investigate): the output, then the *why*, split up with subgoal labels. Where it fits, a question „Was passiert, wenn …?“ (what happens if …?).
4. **`<details>` „Vertiefung: … (optional)“**

„Ändern“ (modify) and „Selbermachen“ (make your own) follow as separate tasks (step 5/6).

## Teilziel-Beschriftungen (Subgoal labels)

Worked examples split the solution into named steps. Example for LINQ: „1. Datenquelle wählen → 2. Filtern → 3. Projizieren → 4. Ergebnis materialisieren“ (1. choose data source → 2. filter → 3. project → 4. materialize the result). The labels go in the running text or in the solution, **not** as a German code comment (code comments are English).

## Skeleton

```mdx
---
title: Ganzzahldivision und Gleitkommazahlen
sidebar_position: 2
description: Warum 7 / 2 in C# 3 ergibt – und wann double und decimal helfen.
lesson_id: m01-l02
duration_min: 20
---

import CodeBlock from '@theme/CodeBlock';
import pitfalls from '!!raw-loader!@site/examples/m01/l02/numeric-pitfalls.cs';

# Ganzzahldivision und Gleitkommazahlen

:::note[Lernziele]
- Du sagst das Ergebnis einer Division zweier `int` korrekt voraus.
:::

## Rückblick
<Quiz id="m01-l01-q1" … />

## Zwei Stolperfallen

:::info[Vorhersage]
Was geben die vier Zeilen aus? Notiere deinen Tipp für jede Zeile und führe den Code dann aus.
:::

<CSharpRunner height={420} code={pitfalls} />

<details>
<summary>Lösung und Erklärung anzeigen</summary>
…
</details>

<details>
<summary>Vertiefung: Wert- und Referenztypen (optional)</summary>
…
</details>

## Weiterlesen
- Microsoft Learn: …
```

Code that cannot run looks like a normal code block with a title. Because the code comes from a file, use `<CodeBlock language="csharp" title="…">{code}</CodeBlock>` (the official Docusaurus component). It looks the same as ` ```csharp title="…" `.

## SecLog extra exercises

New concepts are **always** shown first with small, standalone examples. The practice project SecLog comes after that as an optional extra exercise (step 11). Details: `concept/modulplan.md`.
