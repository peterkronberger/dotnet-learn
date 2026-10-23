# Spikes (Prototyp-Aufgaben) und technischer Backlog

Stand: 2026-10-23. Der Status steht in der Tabelle in `CLAUDE.md`.

Alle Spikes sind entschieden; die Ergebnisse stehen in `CLAUDE.md` unter „Entscheidungen in Kurzform“.

## Ablauf

1. **Ein GitHub Issue pro Spike**, Label `spike` (einmalig anlegen) plus Start-Label der vorhandenen Automatisierung:
   - kleine Spikes (S0, S1, S6): `claude:auto-implement`
   - größere Spikes (S2, S3, S5, S7): `claude:auto-architect`
   - Abhängigkeiten als eigene Zeile `Depends on #<nummer>` (die Workflows setzen solche Issues auf `claude:queued`).
2. **Issue-Aufbau:** Frage · Hypothese · Akzeptanzkriterien · Zeitlimit · Ausweichlösung · abhängige Inhalte (Abschnitte unten übernehmen).
3. **Umsetzung** in einem eigenen Branch (`claude/issue-<nr>`) per Pull Request.
4. **Ergebnis festhalten** unter „Entscheidungen in Kurzform“ in `CLAUDE.md` – auch wenn die Hypothese falsch war.
5. **Statustabelle in `CLAUDE.md`** im selben PR aktualisieren (Status).
6. Inhalte, die von einem Spike abhängen, werden erst danach geschrieben.

Reihenfolge: S0 → S1 → S2 → S3 → S4 → S7 → S5 → S6. S3, S4, S5 und S7 ändern alle den Playground (`App.razor`, `index.html`); deshalb nacheinander (`Depends on`), nicht parallel.

Zeitlimits sind Schätzungen. Wird eines überschritten: abbrechen, Zwischenstand in `CLAUDE.md` festhalten, Mensch entscheidet.

## S0 – Roslyn auf 5.x heben

- **Frage:** Kompiliert der Playground C# 14?
- **Befund aus dem MVP-Abgleich:** `playground/Playground.csproj` nutzt `Microsoft.CodeAnalysis.CSharp` 4.14.0. Laut Roslyn-Wiki entspricht 4.14 **C# 13** (VS 2022 17.14 / .NET 9); erst 5.0 bringt C# 14 (VS 2026 18.0 / .NET 10). `LanguageVersion.Latest` liefert mit 4.14 also höchstens C# 13.
- **Hypothese:** Version 5.x läuft unverändert in Blazor WebAssembly.
- **Akzeptanzkriterien:** Beispiel mit `field`-Schlüsselwort und Extension Members kompiliert und läuft im veröffentlichten Playground; bestehende Beispiele laufen weiter; Download-Größe vorher/nachher in „Entscheidungen in Kurzform“.
- **Zeitlimit:** 2 h · **Ausweichlösung:** C#-14-Features nur als nicht ausführbare Beispiele.
- **Blockiert:** jeden Inhalt mit C#-14-Features.

## S1 – Code aus `.cs`-Dateien einbinden

- **Frage:** Lässt sich Code per `raw-loader` (offiziell dokumentiert: Docusaurus „Importing code snippets“) in `<CSharpRunner code={…}>` und `<CodeBlock>` einbinden?
- **Hypothese:** Ja; der Runner erhält einen normalen String.
- **Akzeptanzkriterien:** Pilot: `variablen-und-typen.mdx` nutzt nur noch Dateien unter `website/examples/`; Code mit Backslash, Backtick und `${` funktioniert; `npm run build` fehlerfrei; Darstellung des nicht ausführbaren Blocks identisch mit ` ```csharp title="…" `.
- **Zeitlimit:** 2 h · **Ausweichlösung:** eigener Webpack-Loader oder Remark-Plugin.
- **Blockiert:** jeden neuen Inhalt.

## S2 – CI: Beispiele kompilieren und Ausgaben abgleichen

- **Frage:** Wie prüft CI alle `.cs`-Dateien und stellt sicher, dass gezeigte Ausgaben echt sind?
- **Hypothese:** Ausführbare Beispiele werden in CI ausgeführt (z. B. als dateibasierte Apps des .NET-10-SDK – vor Umsetzung prüfen); die Ausgabe wird als Snapshot (`<name>.out.txt`) neben der Datei abgelegt, die Lektion bindet den Snapshot ein. Weicht ein Lauf vom Snapshot ab, schlägt CI fehl. Für EF Core (SQL), ASP.NET Core (HTTP) und xUnit (Testrunner-Ausgabe) gibt es eigene CI-Projekte, die die Snapshots erzeugen.
- **Offene Teilfragen:** Markierung von Bruchstücken unsichtbar für Lernende (Vorschlag: Dateiendung `.snippet.cs` = nur kompilieren bzw. überspringen); `InvariantCulture` erzwingen (Playground nutzt `InvariantGlobalization`).
- **Akzeptanzkriterien:** CI-Job schlägt bei Compilerfehler bzw. Ausgabe-Abweichung fehl; ein Beispiel je Art (Konsole, EF Core, Minimal API, xUnit) erzeugt einen Snapshot.
- **Zeitlimit:** 1 Tag · **Ausweichlösung:** nur kompilieren, Ausgaben manuell prüfen (Regel 4 in `CLAUDE.md` gilt trotzdem).
- **Hinweis:** ändert `.github/workflows/` → laut `claude-implement.yml` braucht das einen Menschen.
- **Blockiert:** jeden neuen Inhalt.

## S3 – Schleifenwächter

- **Frage:** Kann Roslyn vor dem Ausführen in jede Schleife und Methode eine Zeitprüfung einfügen?
- **Hypothese:** Ein `CSharpSyntaxRewriter` fügt einen Aufruf ein, der nach ca. 3 s eine eigene Exception wirft; der Runner zeigt „Dein Programm läuft vermutlich endlos – prüfe die Abbruchbedingung in Zeile X“.
- **Akzeptanzkriterien:** `while (true) { }` und endlose Rekursion brechen nach ca. 3 s ab; Zeilennummern in Compilermeldungen bleiben unverändert; normale Beispiele laufen ohne spürbare Verzögerung (Messung in „Entscheidungen in Kurzform“).
- **Zeitlimit:** 1 Tag · **Ausweichlösung:** Hinweis wie bisher; Schleifen-Übungen so gestalten, dass Endlosschleifen unwahrscheinlich sind. (Web Worker wird nicht weiter verfolgt.)
- **Blockiert:** Modul 2.

## S4 – Syntaxfarben-Overlay im Editor (umgesetzt ohne Prism, siehe Entscheidung)

- **Frage:** Lässt sich hinter dem `<textarea>` ein deckungsgleicher, gefärbter Codeblock anzeigen?
- **Akzeptanzkriterien:** Deckungsgleich bei langen Zeilen, Umbrüchen, Scrollen, Mobilgeräten und IME-Eingabe; aktuelle Zeile und Fehlerzeilen hervorhebbar; flüssig bis ca. 200 Zeilen; Tab/Esc/Strg+Enter funktionieren weiter.
- **Zeitlimit:** 1 Tag · **Ausweichlösung:** reines `<textarea>` (Status quo) plus Fehlerliste.
- **Blockiert:** Modul 2.

## S7 – Runner ↔ Seite: Ergebnis per `postMessage`

- **Frage:** Kann der Runner Ausgabe und Testergebnisse an die Lernseite melden?
- **Akzeptanzkriterien:** `{type: 'run-result', id, success, output, diagnostics, tests?}` kommt an; Vorhersagefeld zeigt ✓/✗ pro Zeile; eine Aufgabe mit sichtbaren und versteckten Tests meldet Ergebnis pro Test; Ursprungsprüfung (`origin`) beidseitig.
- **Zeitlimit:** 2 Tage · **Ausweichlösung:** Aufgaben mit eingeklappter Musterlösung.
- **Blockiert:** Vorhersagefeld, automatisch geprüfte Aufgaben.

## S5 – Schritt-für-Schritt-Visualisierung

- **Frage:** Kann der Playground Variablenwerte pro Schritt und Referenzen (Stack/Heap) anzeigen?
- **Hypothese:** Roslyn-Instrumentierung (wie S3) protokolliert pro Anweisung Zeile und lokale Variablen; eine Ansicht zeigt Schritte vor/zurück (Vorbild: Online Python Tutor).
- **Akzeptanzkriterien:** v1: ein 15-Zeilen-Beispiel mit Schleife zeigt pro Schritt Zeile, Variablen und Ausgabe; v2: Referenzpfeile für ein Wert-/Referenztyp-Beispiel aus Modul 4; tastaturbedienbar, mit Textalternative.
- **Zeitlimit:** 2 Tage für v1 · **Ausweichlösung:** statische Diagramme (Mermaid/SVG) mit Textbeschreibung.
- **Blockiert:** Modul 4.

## S6 – `Microsoft.Extensions.*` im Playground

- **Frage:** Kann Nutzercode `ServiceCollection` und Logging verwenden?
- **Hinweis:** Die Blazor-App lädt `Microsoft.Extensions.DependencyInjection` bereits zur Laufzeit (`Program.cs`); für den Compiler fehlen die Referenzen – die Pakete sind nicht Teil von `Microsoft.NETCore.App.Ref`, also nicht über `ReferenceName` einbindbar.
- **Akzeptanzkriterien:** Beispiel mit `ServiceCollection`, Lebensdauern und Konsolen-Logging läuft; Download-Größe in „Entscheidungen in Kurzform“.
- **Zeitlimit:** 0,5 Tag · **Ausweichlösung:** Modul 15 als nicht ausführbare Beispiele.
- **Blockiert:** Modul 15.

## Technischer Backlog aus dem MVP-Abgleich (keine Spikes)

| # | Aufgabe | Abhängig von | Status |
| --- | --- | --- | --- |
| B1 | MVP-Lektionen umstellen: Bezeichner und Kommentare Englisch, neue Ordnerstruktur, Lösungstext „`3,5` bzw. `3.5`“ korrigieren (Ausgabe ist immer `3.5`) | S1 | erledigt |
| B2 | Startcode Englisch: `playground/Samples.cs`, `website/src/pages/playground.js` | – | erledigt |
| B3 | Quizfragen in Datendateien pro Sprache, Quiz-Typen, versionierter Fortschrittsspeicher, Leitner, Dashboard, Export/Import, Migration der alten Schlüssel | – | offen |
| B4 | README-Abschnitt „Inhalte schreiben“ an die neuen Regeln anpassen | S1 | erledigt |
| B5 | Runner-Oberfläche übersetzbar (Sprachparameter) | – | offen |
| B6 | Konsoleneingabe: `stdin`-Prop, Eingabefeld, `Console.SetIn` | – | offen |
| B7 | `package-lock.json` committen, in `pages.yml` auf `npm ci` umstellen | – (Workflow → Mensch) | erledigt |
| B8 | Issue-Vorlage „Fehler melden / Feedback“ + Link pro Seite | – | offen |
| B9 | Mermaid, lokale Suche, Playwright + Barrierefreiheits-Check | – | offen |
| B10 | `lernprinzipien.mdx` → „Wie dieser Kurs funktioniert“: Literatur aus `didaktik.md`, KI-Absatz, Bedienung | B3 | offen |
| B11 | `<Since>`-Badge, Umsteiger-Tabs | – | offen |
