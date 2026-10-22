# 0007 – Schrittansicht im Playground

- Status: entschieden (Browser-Prüfung ausstehend)
- Datum: 2026-10-22
- Spike/Issue: S5 / #11

## Frage
Kann der Playground ein Programm Schritt für Schritt zeigen – aktuelle Zeile, Variablenwerte, Referenzen (Stack/Heap-Modell)?

## Ergebnis
- **Instrumentierung** (`playground/StepTraceRewriter.cs`, wie der Schleifenwächter aus S3 ein `CSharpSyntaxRewriter`): Vor jede Anweisung eines Blocks und jede Top-Level-Anweisung kommt ein Aufruf `__PlaygroundTrace.Step(line, names, values)`; nach der letzten Top-Level-Anweisung ein Schlussschritt (`Line = 0`, „Programmende“). Die Einfügungen enthalten keine Zeilenumbrüche, Zeilennummern bleiben gleich. Einzelne Anweisungen als Schleifenkörper werden dafür in einen Block gewickelt.
- **Verfolgte Variablen:** Parameter, lokale Variablen mit Initialisierer, `for`-/`foreach`-Variablen. Funktionskörper und Typen sehen die Top-Level-Variablen nicht (eigener Gültigkeitsbereich). **Nicht verfolgt:** Pattern-/`out`-Variablen, Dekonstruktion, `ref`-Locals, Ausdruckskörper, Anweisungen in `switch`-Abschnitten.
- **Aufzeichnung** (`playground/StepRecorder.cs`): Strings und Werttypen erscheinen inline, Referenztypen als nummeriertes Objekt (`#1`, `#2`, …; gleiche Instanz = gleiche Nummer, über alle Schritte stabil). Das Heap-Modell zeigt Arrays/Collections (max. 8 Elemente), sonst öffentliche Felder und Properties. Lazy Sequenzen (z. B. LINQ) werden **nicht** aufgezählt, damit kein Nutzercode doppelt läuft. Grenzen: 500 Schritte, 20 Objekte pro Schritt.
- **Einschalten pro Runner:** `<CSharpRunner trace />` hängt `&trace=1` an das Fragment. Ohne `trace` wird nichts instrumentiert (keine Mehrkosten). Mit `tests` wird `trace` ignoriert.
- **Rückfall:** Kompiliert die instrumentierte Variante nicht (z. B. `Span<T>`-Locals), läuft das Programm normal und der Runner zeigt „Schrittansicht nicht verfügbar“.
- **Ansicht** (`App.razor`): Quelltext mit markierter Zeile (Rahmen + ▶ + Text „aktuelle Zeile“, nicht nur Farbe), Variablenliste, Objektliste, bisherige Ausgabe. Referenzpfeile (SVG, `aria-hidden`) verbinden Variable und Objekt; die Textalternative ist „→ Objekt #n“ in der Variablenliste. Bedienung per Schaltflächen (Tab/Enter) und Pfeiltasten links/rechts, Pos1/Ende; die Schrittangabe steht in einer `aria-live`-Region.
- **Beispiele:** `website/examples/s5/loop-trace.cs` (v1: Schleife) und `reference-types.cs` (v2: Wert- vs. Referenztyp, zwei Variablen → dasselbe Objekt), Demo auf `/runner-demo`.
- **Gemessen/geprüft:** Ein Testlauf von `CSharpRunner.RunAsync(..., trace: true)` unter CoreCLR lieferte für beide Beispiele die erwarteten Schritte (z. B. `a=#1; b=#1` → `#1 int[]`). Der Browser (WebAssembly, Darstellung, Tastatur, Screenreader) wurde im Automatisierungslauf **nicht** geprüft; Laufzeit-/Größenmessung im Browser steht aus. Das Zeitlimit des Schleifenwächters (3 s) gilt auch mit Aufzeichnung.

## Entscheidung
Die Instrumentierung wird eingesetzt, vorbehaltlich der manuellen Browser-Prüfung. Ausweichlösung bleibt: statische Diagramme (Mermaid/SVG) mit Textbeschreibung.

## Folgen
- Modul 4 ist entsperrt (Wert- vs. Referenztypen mit `trace`).
- Oberflächentexte der Schrittansicht sind wie der restliche Runner noch deutsch fest kodiert (Backlog B5).
- Stack/Heap bleibt ein *Modell*: Die Ansicht zeigt nicht, wo die Laufzeit Objekte tatsächlich ablegt.
