# 0006 – Runner meldet Ergebnisse per postMessage

- Status: entschieden (Browser-Prüfung ausstehend)
- Datum: 2026-10-22
- Spike/Issue: S7 / #10

## Frage
Kann der Runner (iframe) Ausgabe und Testergebnisse an die Lernseite melden, damit Vorhersagefelder und automatisch geprüfte Aufgaben möglich werden?

## Ergebnis
- **Nachricht Runner → Seite** (nach jedem Lauf, `pgBridge.report` in `playground/wwwroot/index.html`):
  `{type: 'run-result', id, success, output, diagnostics: [{isError, text, line}], tests?: [{name, visible, passed, feedback}]}`.
  Gesendet wird mit `targetOrigin = window.location.origin`.
- **Zuordnung bei mehreren Runnern:** `<CSharpRunner>` hängt seine `id` (Prop oder `useId()`) als `&id=…` ans Fragment (`#code=<b64>&id=…&tests=<b64>`). Der Runner liest sie in `App.razor.cs` und trägt sie in jede Nachricht ein. Die Seite verarbeitet nur Nachrichten, deren `event.source` das eigene iframe und deren `id` die eigene ist.
- **Ursprungsprüfung beidseitig:** Seite → Runner (`theme`) wie bisher mit `targetOrigin` und `event.origin`-Prüfung im Runner; Runner → Seite mit `targetOrigin` beim Senden und `event.origin`-Prüfung in der Seite.
- **Vorhersagefeld** (`predict` + `expected`): `expected` (Ausgabe-Snapshot `*.out.txt`) bestimmt nur die Anzahl der Felder. Verglichen wird mit der **echten** Ausgabe des Laufs (Zeile für Zeile, Leerraum am Zeilenende ignoriert). Anzeige „✓ Zeile n: Stimmt.“ bzw. „✗ Zeile n: Stimmt nicht – erwartet war …“; leere Felder werden nicht bewertet. Felder sind native `<input>` mit `<label>` (Tastatur ohne Zusatzcode), das Ergebnis steht in einer `aria-live="polite"`-Region; ✓/✗ sind zusätzlich als Text formuliert.
- **Geprüfte Aufgabe** (`tests`-Prop): C#-Prüfcode wird mit dem Lernendencode kompiliert (zweite Syntaxstruktur `Tests.cs`) und deklariert `public static class TaskTests` mit `public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()`. Mit Tests läuft **nur** `Run()`, nicht der Einstiegspunkt. Die Aufgabe verlangt eine Typdeklaration (z. B. `static class Palindrome`), weil Methoden in Top-Level-Statements lokale Funktionen sind. Der Schleifenwächter (S3) wirkt auch in den Tests. Versteckte Tests erscheinen als „Versteckter Test n“, behalten aber ihr erklärendes Feedback. Compilerfehler im Prüfcode erscheinen ohne Zeilennummer als „(Prüfcode)“.
- Beispiele: `website/examples/s7/` (`predict-demo.cs`, `palindrome-task.cs`, `palindrome-task.tests.snippet.cs`), Demo-Seite `/runner-demo` (`website/src/pages/runner-demo.js`).
- **Grenzen / nicht geprüft:** `dotnet build playground/Playground.csproj` und `npm run build` sind fehlerfrei. Das Verhalten im Browser (Nachrichtenfluss, mehrere Runner, Tastatur, Screenreader) wurde im Automatisierungslauf **nicht** geprüft. Die Snapshots `predict-demo.out.txt` (14 / 5 / 2) und `palindrome-task.out.txt` (`False`) wurden von Hand geschrieben, weil `dotnet run` im Lauf nicht erlaubt war – bitte mit `bash tools/check-examples.sh` bestätigen.
- **Sicherheit/Fairness:** Der Prüfcode steht im URL-Fragment des iframes und ist damit für technisch Versierte einsehbar; „versteckt“ heißt nur „nicht angezeigt“. Es ist kein Schutz gegen Mogeln und nicht als solcher gedacht (kein Tracking, keine Konten).

## Entscheidung
Das Protokoll und die Tests-Konvention werden eingesetzt, vorbehaltlich der manuellen Browser-Prüfung. Ausweichlösung bleibt „Aufgabe + eingeklappte Musterlösung“.

## Folgen
- Vorvorhersagefelder (`predict`) und geprüfte Aufgaben (`tests`) sind entsperrt.
- Der Leitner-/Fortschrittsspeicher kann `onResult` nutzen.
- Der Fragment-Parameter `tests` kann bei sehr langem Prüfcode die URL-Länge belasten.
- UI-Texte der Komponente laufen über `<Translate>`; die Runner-Meldungen für Tests sind (wie bisher die Runner-Oberfläche) noch deutsch fest kodiert (Backlog B5).

## Quellen
- `Window.postMessage`: https://developer.mozilla.org/docs/Web/API/Window/postMessage
- `aria-live`: https://developer.mozilla.org/docs/Web/Accessibility/ARIA/Attributes/aria-live
