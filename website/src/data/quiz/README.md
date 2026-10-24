# Quizdaten

Eine Datei pro Modul und Sprache: `de/mXX.json`. Neue Dateien werden in `index.js` explizit importiert. In MDX steht nur `<Quiz id="m01-l03-q1" />`.

## Gemeinsame Felder

| Feld | Pflicht | Beschreibung |
| --- | --- | --- |
| `id` | ja | Stabiler Schlüssel, beginnt mit `lesson` + `-q` (z. B. `m01-l03-q1`) |
| `lesson` | ja | `mXX-lYY` |
| `type` | ja | `single`, `multi`, `output`, `explain` (geplant: `parsons`, `cloze`, `find-bug`, `match`) |
| `question` | ja | Fragetext |
| `explanation` | `single`/`multi`/`output` | Erklärt das *Warum* |

Texte: Inline-Code mit Backticks, Absätze mit `\n\n`, sonst kein Markdown.

## Typen

- `single`: `options` mit genau einer Option `correct: true`.
- `multi`: `options` mit mindestens einer richtigen Option.
- Option: `text` (Pflicht), `correct`, `feedback` (Warum falsch/richtig), `misconception` (ID einer „Achtung“-Box, `<lesson_id>-<kebab-slug>`).
- `output`: `example` = Pfad relativ zu `website/examples/`. Code aus `<example>.cs`, erwartete Ausgabe aus `<example>.out.txt` (CI-Snapshot).
- `explain`: `modelAnswer` (Pflicht). Die Antwort der Lernenden wird nicht gespeichert; Selbstbewertung `correct`/`partial`/`wrong`.

## Beispiele

```json
{ "id": "m01-l03-q1", "lesson": "m01-l03", "type": "single", "question": "…",
  "options": [ { "text": "…", "misconception": "m01-l03-int-division-rounds" }, { "text": "…", "correct": true } ],
  "explanation": "…" }
{ "id": "m01-l03-q2", "lesson": "m01-l03", "type": "multi", "question": "…",
  "options": [ { "text": "…", "correct": true }, { "text": "…", "correct": true }, { "text": "…" } ], "explanation": "…" }
{ "id": "m01-l03-q3", "lesson": "m01-l03", "type": "output", "question": "…", "example": "m01/l03/quiz-remainder.cs", "explanation": "…" }
{ "id": "m01-l03-q4", "lesson": "m01-l03", "type": "explain", "question": "…", "modelAnswer": "Absatz 1\n\nAbsatz 2" }
```

Demo aller Typen: `/dev/quiz-demo`. Prüfung: `node tools/check-quiz.mjs`.
