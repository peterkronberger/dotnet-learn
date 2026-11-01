# Quiz data

One file per module and language: `de/mXX.json`. New files must be imported in `index.js` by hand. MDX contains only `<Quiz id="m01-l03-q1" />`.

## Common fields

| Field | Required | Description |
| --- | --- | --- |
| `id` | yes | Stable key, starts with `lesson` + `-q` (e.g. `m01-l03-q1`) |
| `lesson` | yes | `mXX-lYY` |
| `type` | yes | `single`, `multi`, `output`, `explain`, `parsons`, `cloze`, `find-bug`, `match` |
| `question` | yes | Question text |
| `placement` | no | `true` marks a placement question of its module (see "Placement test") |
| `placementLessons` | no | Only with `placement: true`: extra lessons of the same module that the question also covers, e.g. `["m01-l05"]` |
| `explanation` | `single`/`multi`/`output`/`parsons`/`cloze`/`find-bug`/`match` | Explains the *why* |

Texts: inline code with backticks, paragraphs with `\n\n`, no other Markdown.

## Placement test

`"placement": true` marks a question as a placement question of its module (the module is the prefix of `lesson`, `m01-l03` → `m01`). Placement questions stay normal lesson questions and are still shown in their lesson with `<Quiz id>`. `placementLessons` lists extra lessons of the **same module** that the question also covers (use it when a module has more than 8 lessons). The result page links `lesson` plus all `placementLessons`.

Rules for every module with at least one placement question (checked by `node tools/check-quiz.mjs` and, for the fields, at build time):

- Exactly 8 placement questions.
- No `explain` question.
- Every lesson of the module (every `lesson_id` in `website/docs` with the prefix `<module>-l`) is covered by the `lesson` or the `placementLessons` of a placement question.
- Every id in `placementLessons` exists, belongs to the same module and is not the question's own `lesson`.
- Choose self-contained questions: they must be answerable without the lesson page or a runner.

A module without placement questions has no placement test. The pass rule is set by the component: 8 questions, at most 1 wrong. `getPlacementQuestions(locale, moduleKey)` in `index.js` returns the questions of one module in lesson order.

## Answer order

The options of `single`/`multi`, the pool of lines of `parsons` and the right entries of `match` are shown in a new random order on every view and on every "Nochmal versuchen" (try again). Write options in any order (do not put the correct one first). `find-bug`, `cloze`, `output` and `explain` keep their order.

Rule: option texts, option `feedback`, `explanation` and `wrongMatches[].feedback` must not refer to a position. Examples of forbidden text: "Antwort A", "die erste Option", "beide oben", "die obige Antwort". `node tools/check-quiz.mjs` checks this.

## Types

- `single`: `options` (at least 2) with exactly one option `correct: true`.
- `multi`: `options` (at least 2) with at least one correct option.
- Option: `text` (required), `correct`, `feedback` (why it is wrong or right), `misconception` (ID of an "Achtung" box, `<lesson_id>-<kebab-slug>`), `pin` (`"last"`, optional, only on `single`/`multi` options: the option is always shown after all shuffled options; several pinned options keep their data order). Use it for options that only make sense at the end, e.g. "Keine der genannten" or "Alle genannten". Such summary options must have `"pin": "last"`.
- `output`: `example` = path relative to `website/examples/`. The code comes from `<example>.cs`, the expected output from `<example>.out.txt` (CI snapshot).
- `explain`: `modelAnswer` (required). The learner's answer is not stored; self-rating `correct`/`partial`/`wrong`.
- `parsons`: put lines in order. `example` = path as for `output`; the correct order is the non-empty lines of the file (indentation is shown but not graded). Optional: `distractors` (lines that are not part of the solution, not identical to a solution line) and `distractorMisconceptions` (object `line text` → misconception ID; every key must be a distractor). Usage rule (`concept/didactics.md`): evidence on distractors is mixed; use few of them in early modules and let each target a known misconception. The pool of available lines (solution lines and distractors) is shown in random order (new on every view and every retry). Controlled with buttons (add, move up/down, remove); changes are announced in a live region.
- `cloze`: code with blanks. `example` = path as for `output` (complete, runnable code). `blanks` = the blanks in code order (at least one). Each blank is the first unused occurrence of `text`, searched from the end of the previous blank; if `text` is not found, the build and `check-quiz.mjs` fail. Per blank: `text` (solution), optional `accept` (accepted inputs, default `[text]`, compared with all whitespace removed), `hint` (shown when the blank is wrong), `wrongAnswers` (list of `text` plus `feedback` and/or `misconception`). Each blank has the label "Lücke N von M" (blank N of M); the result is shown per blank as text (✓/✗) next to the field.
- `find-bug`: select the faulty line(s). `example` = path as for `output`; for code that does not compile, a `*.snippet.cs` file (without `.out.txt`). `bugLines` = line numbers (1-based, at least one, inside the file, no duplicates). Exactly one line: radio buttons, otherwise checkboxes. The answer is correct when the selected set is exactly `bugLines`. Optional `lineFeedback`: object `line number` → `{feedback, misconception}`, shown for selected lines. Each line has the label "Zeile N: <code>" (line N: <code>); the result is shown as text (✓/✗) next to the line.
- `match`: match terms. `pairs` = 2 to 8 objects `{left, right}`; every `left` and every `right` is unique. One `<select>` per left term (label = the left term) with the right entries in random order (new on every view and every retry) and the empty option "Bitte wählen" (please choose). The answer is correct when all matches are right. The result is shown as text (✓/✗) per row, with the correct answer on errors. Optional `wrongMatches`: list of `{left, right, feedback, misconception}` for typical wrong matches (`left`/`right` must appear in `pairs`, and the pair must not be a correct one).

## Examples

```json
{ "id": "m01-l03-q1", "lesson": "m01-l03", "type": "single", "question": "…",
  "options": [ { "text": "…", "misconception": "m01-l03-int-division-rounds" }, { "text": "…", "correct": true } ],
  "explanation": "…" }
{ "id": "m01-l03-q2", "lesson": "m01-l03", "type": "multi", "question": "…",
  "options": [ { "text": "…", "correct": true }, { "text": "…", "correct": true }, { "text": "…" } ], "explanation": "…" }
{ "id": "m01-l03-q3", "lesson": "m01-l03", "type": "output", "question": "…", "example": "m01/l03/quiz-remainder.cs", "explanation": "…" }
{ "id": "m01-l03-q4", "lesson": "m01-l03", "type": "explain", "question": "…", "modelAnswer": "…\n\n…" }
```

Demo of all types: `/dev/quiz-demo`. Check: `node tools/check-quiz.mjs`.
