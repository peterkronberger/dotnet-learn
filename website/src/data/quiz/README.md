# Quizdaten

Eine Datei pro Modul und Sprache: `de/mXX.json`. Neue Dateien werden in `index.js` explizit importiert. In MDX steht nur `<Quiz id="m01-l03-q1" />`.

## Gemeinsame Felder

| Feld | Pflicht | Beschreibung |
| --- | --- | --- |
| `id` | ja | Stabiler Schlüssel, beginnt mit `lesson` + `-q` (z. B. `m01-l03-q1`) |
| `lesson` | ja | `mXX-lYY` |
| `type` | ja | `single`, `multi`, `output`, `explain`, `parsons`, `cloze` (geplant: `find-bug`, `match`) |
| `question` | ja | Fragetext |
| `explanation` | `single`/`multi`/`output`/`parsons`/`cloze` | Erklärt das *Warum* |

Texte: Inline-Code mit Backticks, Absätze mit `\n\n`, sonst kein Markdown.

## Typen

- `single`: `options` mit genau einer Option `correct: true`.
- `multi`: `options` mit mindestens einer richtigen Option.
- Option: `text` (Pflicht), `correct`, `feedback` (Warum falsch/richtig), `misconception` (ID einer „Achtung“-Box, `<lesson_id>-<kebab-slug>`).
- `output`: `example` = Pfad relativ zu `website/examples/`. Code aus `<example>.cs`, erwartete Ausgabe aus `<example>.out.txt` (CI-Snapshot).
- `explain`: `modelAnswer` (Pflicht). Die Antwort der Lernenden wird nicht gespeichert; Selbstbewertung `correct`/`partial`/`wrong`.
- `parsons`: Zeilen ordnen. `example` = Pfad wie bei `output`; die richtige Reihenfolge sind die nicht-leeren Zeilen der Datei (Einrückung wird angezeigt, aber nicht bewertet). Optional: `distractors` (Zeilen, die nicht in die Lösung gehören, nicht identisch mit Lösungszeilen) und `distractorMisconceptions` (Objekt `Zeilentext` → Misconception-ID). Anzeige in fester, aus der Frage-ID abgeleiteter Mischung. Bedienung per Knöpfen (Hinzufügen, Nach oben/unten, Entfernen); Änderungen werden in einer Live-Region angesagt.
- `cloze`: Code-Lückentext. `example` = Pfad wie bei `output` (vollständiger, lauffähiger Code). `blanks` = Lücken in der Reihenfolge des Codes. Jede Lücke ist das erste noch nicht vergebene Vorkommen von `text`, gesucht ab dem Ende der vorherigen Lücke; kommt `text` nicht vor, schlagen Build und `check-quiz.mjs` fehl. Pro Lücke: `text` (Lösung), optional `accept` (akzeptierte Eingaben, Standard `[text]`, Vergleich ohne Leerzeichen), `hint` (bei falscher Lücke), `wrongAnswers` (Liste aus `text` und `feedback` und/oder `misconception`). Jede Lücke hat das Label „Lücke N von M“; das Ergebnis steht pro Lücke als Text (✓/✗) neben dem Feld.

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
