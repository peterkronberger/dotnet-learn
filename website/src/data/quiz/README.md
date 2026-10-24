# Quiz data

One file per module and language: `de/mXX.json`. New files must be imported in `index.js` by hand. MDX contains only `<Quiz id="m01-l03-q1" />`.

## Common fields

| Field | Required | Description |
| --- | --- | --- |
| `id` | yes | Stable key, starts with `lesson` + `-q` (e.g. `m01-l03-q1`) |
| `lesson` | yes | `mXX-lYY` |
| `type` | yes | `single`, `multi`, `output`, `explain`, `parsons`, `cloze`, `find-bug`, `match` |
| `question` | yes | Question text |
| `explanation` | `single`/`multi`/`output`/`parsons`/`cloze`/`find-bug`/`match` | Explains the *why* |

Texts: inline code with backticks, paragraphs with `\n\n`, no other Markdown.

## Types

- `single`: `options` (at least 2) with exactly one option `correct: true`.
- `multi`: `options` (at least 2) with at least one correct option.
- Option: `text` (required), `correct`, `feedback` (why it is wrong or right), `misconception` (ID of an "Achtung" box, `<lesson_id>-<kebab-slug>`).
- `output`: `example` = path relative to `website/examples/`. The code comes from `<example>.cs`, the expected output from `<example>.out.txt` (CI snapshot).
- `explain`: `modelAnswer` (required). The learner's answer is not stored; self-rating `correct`/`partial`/`wrong`.
- `parsons`: put lines in order. `example` = path as for `output`; the correct order is the non-empty lines of the file (indentation is shown but not graded). Optional: `distractors` (lines that are not part of the solution, not identical to a solution line) and `distractorMisconceptions` (object `line text` → misconception ID; every key must be a distractor). Lines are shown in a fixed shuffle derived from the question ID. Controlled with buttons (add, move up/down, remove); changes are announced in a live region.
- `cloze`: code with blanks. `example` = path as for `output` (complete, runnable code). `blanks` = the blanks in code order (at least one). Each blank is the first unused occurrence of `text`, searched from the end of the previous blank; if `text` is not found, the build and `check-quiz.mjs` fail. Per blank: `text` (solution), optional `accept` (accepted inputs, default `[text]`, compared with all whitespace removed), `hint` (shown when the blank is wrong), `wrongAnswers` (list of `text` plus `feedback` and/or `misconception`). Each blank has the label "Lücke N von M" (blank N of M); the result is shown per blank as text (✓/✗) next to the field.
- `find-bug`: select the faulty line(s). `example` = path as for `output`; for code that does not compile, a `*.snippet.cs` file (without `.out.txt`). `bugLines` = line numbers (1-based, at least one, inside the file, no duplicates). Exactly one line: radio buttons, otherwise checkboxes. The answer is correct when the selected set is exactly `bugLines`. Optional `lineFeedback`: object `line number` → `{feedback, misconception}`, shown for selected lines. Each line has the label "Zeile N: <code>" (line N: <code>); the result is shown as text (✓/✗) next to the line.
- `match`: match terms. `pairs` = 2 to 8 objects `{left, right}`; every `left` and every `right` is unique. One `<select>` per left term (label = the left term) with the right entries in shuffled order (seed = question ID) and the empty option "Bitte wählen" (please choose). The answer is correct when all matches are right. The result is shown as text (✓/✗) per row, with the correct answer on errors. Optional `wrongMatches`: list of `{left, right, feedback, misconception}` for typical wrong matches (`left`/`right` must appear in `pairs`, and the pair must not be a correct one).

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
