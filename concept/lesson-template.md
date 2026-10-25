# Lesson template

Every lesson follows this order. Optional elements are in parentheses. Length 15–25 min, one core concept. Check against the Definition of Done in `CLAUDE.md`.

Lesson text is German. The German names of sections and boxes below are the real learner-facing text and stay as they are.

## Flow

There is no reference lesson yet. The existing lessons are early implementations and are no pattern (`module-plan.md`, "Existing content"). The first rebuilt lesson that passes its module review becomes the reference lesson; add it here then.

| # | Step | Form | Note |
| --- | --- | --- | --- |
| 1 | Learning goals ("Lernziele") | `:::note[Lernziele]`, 2–4 points | As observable behavior: "Du kannst …" (you can …), "Du erkennst …" (you recognize …) |
| 2 | Warm-up | 2–3 `<Quiz>` on earlier lessons | Heading "Rückblick" (review); retrieval practice, not new material |
| 3 | Concept | Short text + small, isolated example | Only the current concept; no project context. First use of a technical term: `<Term id="…">`, entry in the glossary |
| 4 | Practice example | **Practice-example pattern** (below) | Required, at least once |
| 5 | Gap example | Example with gaps, subgoal labels stay | Fading: the gaps are in the code |
| 6 | Change and make it yourself | Automatically checked task | Automatically checked task with `<CSharpRunner tests={…}>`; a collapsed model solution only as a fallback, e.g. for topics that cannot run |
| 7 | Misconceptions | `:::warning[Achtung]`, 1–3 short boxes; `<Misconception id="…" />` as the first line of each box | Every box is linked to a quiz distractor |
| 8 | (Switcher box) | `<FromLanguage>` with tabs Java / Python / JS/TS / Go (C++ only where it fits) | Only differences to C#, do not explain C# again |
| 9 | (Deep dive) | `<details>` "Vertiefung: … (optional)" | Replaces "Unter der Haube" (under the hood); technical statements with a Microsoft Learn source |
| 10 | Summary | 3–5 sentences | No new content |
| 11 | Wrap-up | Quiz (→ Leitner pool), "In eigenen Worten erklären" (explain in your own words), transfer question, (SecLog extra exercise), "Weiterlesen" (further reading) | "Weiterlesen": 1–3 links to Microsoft Learn / C# specification |

## Practice-example pattern (required in step 4)

This matches PRIMM:

1. **Prediction** (`:::info[Vorhersage]`): a task to think ahead about what the output will be. Optionally with an input field per output line. After the run, a sober check ✓/✗ (no "surprise" wording). Input field via `<CSharpRunner predict expected={…}>`, with `expected` taken from the `.out.txt` snapshot via `raw-loader`.
2. **`<CSharpRunner>`** (Run)
3. **`<details>` "Lösung und Erklärung anzeigen"** (Investigate): the output, then the *why*, structured with subgoal labels. Where it fits, add a question "Was passiert, wenn …?" (what happens if …?).
4. **`<details>` "Vertiefung: … (optional)"**

"Modify" and "Make" follow as their own tasks (step 5/6).

## Subgoal labels

Worked examples split the solution into named steps. For LINQ, for example: "1. Choose data source → 2. Filter → 3. Project → 4. Materialize result". The labels go in the body text or in the solution, **not** as a German code comment (code comments are English).

## Skeleton

The values below (title, ids, file names) only show the format.

```mdx
---
title: Ganzzahldivision und Gleitkommazahlen
sidebar_position: 2
description: Warum 7 / 2 in C# 3 ergibt – und wann double und decimal helfen.
lesson_id: m01-l03
duration_min: 20
---

import CodeBlock from '@theme/CodeBlock';
import pitfalls from '!!raw-loader!@site/examples/m01/l03/numeric-pitfalls.cs';

# Ganzzahldivision und Gleitkommazahlen

:::note[Lernziele]
- Du sagst das Ergebnis einer Division zweier `int` korrekt voraus.
:::

## Rückblick
<Quiz id="m01-l03-q1" />

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

Code that cannot run looks like a normal code block with a title. Because the code comes from a file, use `<CodeBlock language="csharp" title="…">{code}</CodeBlock>` (official Docusaurus component). The look is identical to ` ```csharp title="…" `.

## SecLog extra exercises

New concepts are **always** shown first in small, self-contained examples. The practice project SecLog comes afterwards as an optional extra exercise (step 11). Details: `concept/module-plan.md`.
