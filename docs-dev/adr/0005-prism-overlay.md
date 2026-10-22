# 0005 – Syntaxfarben im Playground-Editor (Overlay)

- Status: entschieden (Browser-Prüfung ausstehend)
- Datum: 2026-10-22
- Spike/Issue: S4 / #9

## Frage
Lässt sich hinter dem `<textarea>` ein deckungsgleicher, gefärbter Codeblock anzeigen?

## Ergebnis
- Aufbau: `<div class="pg-editor-wrap">` mit einem `<pre id="editor-highlight" aria-hidden="true">` (absolut, `inset: 0`) hinter dem `<textarea>`. Das `<textarea>` bleibt das einzige bedienbare Element (Tab, Esc, Strg/⌘+Enter, IME, Auswahl, Mobilgeräte unverändert); sein Text ist transparent, der Cursor (`caret-color`) sichtbar.
- Deckungsgleichheit: identische Schrift, Größe, Zeilenhöhe, Innenabstand, Rahmen, `tab-size`, `white-space: pre-wrap` und `overflow-wrap` bei `<pre>` und `<textarea>`; `scrollbar-gutter: stable` auf beiden, damit die Umbruchbreite gleich bleibt; die Scrollposition wird bei `scroll` übertragen.
- **Kein Prism im Playground.** Die Lernseite nutzt Prism zur Build-Zeit; im Playground wäre eine Bibliothek ein zusätzlicher Download. Stattdessen färbt `playground/wwwroot/js/editor.js` (ca. 130 Zeilen, kein Framework) mit einem regulären Ausdruck: Kommentare, Strings (inkl. `@""`, `$""`, `"""`), Zeichen, Zahlen, Präprozessor, Schlüsselwörter. Typnamen werden nicht gefärbt (ohne Semantik wären sie unzuverlässig). Mehrzeilige Token werden pro Zeile zerlegt.
- Zeilenmarkierung: `DiagnosticLine` trägt jetzt `Line`; `App.razor.cs` übergibt nach jedem Rendern Fehler- und Warnzeilen per `pgEditor.refresh`. Fehlerzeilen haben Hintergrund **und** einen Randbalken, die Liste darunter nennt die Zeile ebenfalls (Farbe ist nicht das einzige Signal). Die aktuelle Zeile (Cursor) wird bei Fokus hervorgehoben. Sobald der Code editiert wird, entfallen Fehlermarken (Zeilennummern wären veraltet).
- Ausweichverhalten: Das Overlay wird erst aktiv (`.pg-overlay`), wenn das Skript gerendert hat; ohne Skript bleibt das `<textarea>` normal lesbar. Im Modus `forced-colors` wird das Overlay ausgeblendet.
- Kontrast: Die Tokenfarben wurden nach Abschätzung gewählt (Hell z. B. `#5b21b6`, `#166534`; Dunkel z. B. `#c4b5fd`, `#86efac`). **Nicht mit einem Werkzeug nachgemessen.**
- Messung: **Nicht durchgeführt.** Im Automatisierungslauf konnte weder ein Browser noch Node gestartet werden. Die Erwartung, dass Tokenisierung und `innerHTML` für 200 Zeilen pro Tastendruck im niedrigen Millisekundenbereich liegen, ist eine Annahme, keine Messung.
- **Nicht geprüft:** Deckungsgleichheit in echten Browsern (lange Zeilen, Mobilgeräte, IME), Flüssigkeit bei 200 Zeilen, Kontrastwerte. Dafür ist eine manuelle Prüfung im veröffentlichten Playground nötig.

## Entscheidung
Das Overlay mit eigenem Tokenizer wird eingesetzt, vorbehaltlich der manuellen Browser-Prüfung. Bei Abweichungen gilt die Ausweichlösung (reines `<textarea>` plus Fehlerliste); sie ist durch das Fallback-Verhalten bereits eingebaut (Skript-Einbindung entfernen genügt).

## Folgen
- Modul 2 ist von S4 aus entsperrt (nach manueller Prüfung).
- Das Overlay rendert bei jedem `input` komplett neu; für sehr lange Dateien (> ca. 500 Zeilen) wäre ein inkrementelles Verfahren nötig.
- S3/S5 können Zeilen über `pgEditor.refresh(errorLines, warningLines)` markieren.

## Quellen
- `scrollbar-gutter`: https://developer.mozilla.org/docs/Web/CSS/scrollbar-gutter
- C#-Lexik: https://learn.microsoft.com/dotnet/csharp/language-reference/language-specification/lexical-structure
