# Lesson template

Every lesson follows this order. Optional elements are in parentheses. Length 15–25 min, one core concept. Check against the Definition of Done in `CLAUDE.md`.

Lesson text is German. The German names of sections and boxes below are the real learner-facing text and stay as they are.

## Flow

There is no reference lesson yet. The existing lessons are early implementations and are no pattern (`module-plan.md`, "Existing content"). The first rebuilt lesson that passes its module review becomes the reference lesson; add it here then.

| # | Step | Form | Note |
| --- | --- | --- | --- |
| 1 | Learning goals ("Lernziele") | `:::note[Lernziele]`, 2–4 points | As observable behavior: "Du kannst …" (you can …), "Du erkennst …" (you recognize …) |
| 2 | Warm-up | 2–3 `<Quiz>` on earlier lessons | Heading "Rückblick" (review); retrieval practice, not new material |
| 3 | Concept | Short text + small, isolated example | Only the current concept; no project context. First use of a technical term: `<Term id="…">`, entry in the glossary. Use only what is explained so far; unavoidable forward references get a `<Forward>` note (section "Forward references") |
| 4 | Practice example | **Practice-example pattern** (below) | Required, at least once |
| 5 | Gap example | Example with gaps, subgoal labels stay | Fading: the gaps are in the code |
| 6 | Projektschritt (project step) | Fixed frame (section "Project step (Projektschritt)" below), `<CSharpRunner tests={…}>` | The automatically checked make task of the lesson, always on SecLog. Start code = previous solution (`concept/project-seclog.md`). A collapsed model solution is part of the frame; for ◻ lessons the runner is replaced by a `<CodeBlock>` |
| 7 | Misconceptions | `:::warning[Achtung]`, 1–3 short boxes; `<Misconception id="…" />` as the first line of each box | Every box is linked to a quiz distractor |
| 8 | (Deep dive) | `<details>` "Vertiefung: … (optional)" | Replaces "Unter der Haube" (under the hood); technical statements with a source as in `concept/sources.md` level 2 (case 3: clause as link text) |
| 9 | Summary | 3–5 sentences | No new content |
| 10 | Wrap-up | Quiz (→ Leitner pool), "In eigenen Worten erklären" (explain in your own words), transfer question, (optional extra exercise for more practice), "Weiterlesen" (further reading) | "Weiterlesen": level 3 of `concept/sources.md` |

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

<CSharpRunner code={pitfalls} />

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

## Forward references

A forward reference is a term or code construct that a lesson or its example uses before the course explains it.

1. **Order first.** Use only terms and constructs that are explained in this lesson or an earlier one. Prefer an example that needs no forward reference.
2. **Unavoidable forward references** get a `<Forward>` note with a fixed German sentence. One concept: "Das Konzept von **X** wurde bisher nicht erläutert und folgt in Lektion „<title>“ / Modul N. Für dieses Beispiel ist das weitere Verständnis dafür nicht notwendig." Several concepts use the plural of the same sentence (the plural is the only grammatical change): "Die Konzepte von **X**, **Y** und **Z** wurden bisher nicht erläutert und folgen in … Für dieses Beispiel ist das weitere Verständnis dafür nicht notwendig." The note links the target lesson (`lesson`) or names the module (`module`).
3. **Several terms in one note.** Terms with the same target go into one note. One note per target: if one example has forward references with different targets, put one `<Forward>` per target directly after each other.
4. **Quizzes.** A quiz question never needs a concept that is only marked as a forward reference.

Syntax: exactly one of these two forms, on its own line with a blank line before and after.

```mdx
<Forward concepts={['Variablen', '`var`']} lesson="m01-l02" />
<Forward concepts={['Variablen', '`var`']} module="1" />
```

- `concepts`: non-empty list of German display names in single quotes. A name wrapped in backticks is shown as code.
- `lesson`: a `lesson_id` that exists; the build fails otherwise. `module`: module number from `concept/module-plan.md` (`0`–`19`), used when the target lesson does not exist yet.
- **Early implementations:** the lesson `m02-l01` has an id that is not stable (`concept/module-plan.md`, "Existing content"). Never use them as `lesson` target; use `module`.
- **Placement:** at the first place in the lesson where the concept appears: normally directly before the runner or code block whose example uses it, or directly after the sentence that names the term.
- **Concept ledger:** the glossary field `lesson` is the lesson that introduces a term. Terms that are first explained on "Wie dieser Kurs funktioniert" have no `lesson` field.
- **Code constructs** without a glossary entry (for example `var`, `typeof`, comments `//`) are checked in review: the PR lists, per example file, every construct used and where it is explained (lesson) or marked (`<Forward>`).

Example (a lesson in module 0 whose example uses `var`):

```mdx
<Forward concepts={['`var`']} module="1" />

<CSharpRunner code={firstProgram} />
```

## Project step (Projektschritt)

SecLog is the only example project (`concept/project-seclog.md`). New concepts are first shown in small, isolated examples (step 3); the project step comes after them. Step 5 may still contain other modify tasks. The optional extra exercise in step 10 is only for additional practice.

Fixed frame (German, learner-facing):

```mdx
## Projektschritt

**Stand bisher:** <1–2 sentences: what SecLog can do so far>

**Neue Anforderung:** <what SecLog should do after this step>

<CSharpRunner id="<lesson_id>-project" code={projectTask} tests={projectTests} />

<details>
<summary>Musterlösung anzeigen</summary>
<CodeBlock language="csharp" title="seclog-<name>-solution.cs">{projectSolution}</CodeBlock>
<explanation with subgoal labels>
</details>

**Neuer Stand:** <1–2 sentences: what SecLog can do now>
```

- If a lesson cannot add a real step, the section has one sentence instead: "In dieser Lektion kommt kein Projektschritt dazu, weil …". It does not invent an unrelated SecLog snippet.
- For ◻ (not runnable) lessons, the runner is replaced by the existing fallback: a `<CodeBlock>` and a collapsed model solution (the code comes from a CI project).
- File names: `website/examples/mXX/lYY/seclog-<name>-task.cs`, `seclog-<name>-task.tests.snippet.cs`, `seclog-<name>-solution.cs` and their `.out.txt` snapshots. `<name>` stays the same in a module.
- Continuity rule (start code byte-identical to the previous solution) and the sample data: `concept/project-seclog.md`.
- Module box: the first lesson of every module has `:::tip[Projekt: Was SecLog nach diesem Modul kann]` right after the learning goals, with 2–4 bullets from the milestone row of the module.
