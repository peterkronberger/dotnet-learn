# CLAUDE.md – dotnet-learn

Lernseite für C# und .NET: Docusaurus 3 (Lernpfad, Quizzes) + Blazor-WebAssembly-Playground mit Roslyn (.NET 10), gehostet auf GitHub Pages. Zielgruppe: Programmiereinsteiger **und** Umsteiger aus anderen Sprachen. Didaktik: beispielorientiert, wissenschaftlich fundiert, Fundamente werden erklärt.

Dieses Dokument ist verbindlich für jede Arbeit im Repo – von Menschen und von Claude (auch in den Workflows `claude-architect.yml` / `claude-implement.yml`).

## Repo-Struktur

| Pfad | Inhalt |
| --- | --- |
| `website/` | Docusaurus-Seite. Lektionen unter `website/docs/`, Komponenten unter `website/src/components/` |
| `website/examples/` | **Alle** C#-Beispiele als `.cs`-Dateien (eine Quelle für alle Sprachen, von CI geprüft) – siehe S1/S2 |
| `playground/` | Blazor WebAssembly + Roslyn; wird unter `/runner/` veröffentlicht und per iframe eingebettet |
| `concept/` | Lernkonzept (unten importiert) |
| `docs-dev/adr/` | Entscheidungsnotizen (ADR) zu Spikes und Architektur |
| `.github/workflows/` | `pages.yml` (Build + Deploy), `claude-*.yml` (Issue-Automatisierung) |

## Befehle

```bash
# Lernseite
cd website && npm install && npm run build   # Build-Prüfung (muss fehlerfrei sein)
cd website && npm start                       # Dev-Server: http://localhost:3000/dotnet-learn/

# Playground
dotnet build playground/Playground.csproj     # Build-Prüfung
dotnet run --project playground               # lokal starten

# Beispielcode prüfen (kompilieren + Ausgaben mit <name>.out.txt abgleichen), siehe ADR 0003
bash tools/check-examples.sh
bash tools/check-examples.sh --update   # Snapshots bewusst neu schreiben, Diff prüfen
```

Vor jedem Commit: beide Build-Prüfungen ausführen. Schlägt eine fehl, wird nicht committet.

## Unverhandelbare Regeln

1. **Code, Bezeichner und Code-Kommentare sind immer Englisch.** Fließtext ist Deutsch (weitere Sprachen später über Docusaurus-i18n). Fachbegriffe Englisch, beim ersten Auftreten mit deutscher Erklärung: „Value Type (Werttyp)“.
2. **Basis: C# 14 / .NET 10 (LTS).** Top-Level-Statements am Anfang, `Main` wird später erklärt. Nullable Reference Types aktiv. Features ab C# 12 / .NET 8 bekommen ein Versions-Badge (`<Since csharp="…" dotnet="…" />`).
3. **Code liegt in `.cs`-Dateien** unter `website/examples/mXX/lYY/` und wird per `raw-loader` in Codeblöcke und `<CSharpRunner>` eingebunden. Kein neuer Inline-Code in MDX (sobald S1 entschieden ist).
4. **Jede gezeigte Ausgabe ist echt**: Sie muss mit dem tatsächlichen Lauf übereinstimmen (CI-Abgleich, S2). Keine von Hand erfundenen Ausgaben.
5. **Nicht ausführbar ≠ gekennzeichnet**: Codebeispiele, die der Playground nicht ausführen kann (ASP.NET Core, EF Core, Unit-Tests), sind normale ` ```csharp title="…" `-Blöcke ohne Hinweis.
6. **Ausführbar** ist nur, was im `<CSharpRunner>` läuft. Praxisbeispiele folgen dem Muster *Vorhersage → Runner → Lösung und Erklärung → Vertiefung (optional)*.
7. **Fakten mit Quelle**: Technische Aussagen verweisen auf Microsoft Learn oder die C#-Spezifikation; didaktische Aussagen auf die Literatur in `concept/didaktik.md`. Nichts erfinden – im Zweifel als offene Frage markieren.
8. **Lektion = 15–25 min, ein Kernkonzept.** Länger → teilen.
9. **Barrierefreiheit WCAG 2.2 AA**: Tastaturbedienung für alle Interaktionen, Kontrast in Hell- und Dunkelmodus, Textalternative für Diagramme.
10. **Kein Tracking**, keine Konten. Lernfortschritt nur lokal (localStorage/IndexedDB) mit JSON-Export/-Import.
11. **Abhängige Inhalte erst nach entschiedenem Spike** (Tabelle unten). Nicht um einen offenen Spike herum bauen.
12. **Keine KI-Inhalte außerhalb von Modul 19** – einzige Ausnahme: ein Absatz auf der Seite „Wie dieser Kurs funktioniert“.

## Definition of Done – Lektion

Eine Lektion ist fertig, wenn alle Punkte erfüllt sind:

1. Jedes Lernziel ist durch mindestens eine Quiz- oder Übungsaufgabe abgedeckt.
2. Alle Codebeispiele bestehen den CI-Check (kompilieren, Ausgaben stimmen).
3. Die Pflichtmethoden sind enthalten: vorgerechnetes Beispiel mit Ausblenden, PRIMM-Praxisbeispiel, Abrufübung, Fragen für die verteilte Wiederholung.
4. Mindestens eine Fehlvorstellung wird behandelt (Box „Achtung“).
5. Features ab C# 12 / .NET 8 tragen das Versions-Badge.
6. Fachaussagen in „Vertiefung“-Bereichen verweisen auf Microsoft Learn oder die C#-Spezifikation.
7. Dauer 15–25 min (geschätzt über Wortzahl und Anzahl der Aufgaben).
8. Jedes vorgerechnete Beispiel hat Teilziel-Beschriftungen.
9. Mindestens eine Aufgabe hat einen echten Anwendungskontext (meist SecLog oder Phishing-Klassifikator).
10. Mindestens eine Übertragungsfrage („Wo würdest du das einsetzen?“) oder eine Projekt-Zusatzübung.
11. Jede Fehlvorstellungs-Box ist mit mindestens einer falschen Antwort im Quiz verknüpft.
12. Mindestens eine Aufgabe „In eigenen Worten erklären“.

Zusätzlich: `npm run build` fehlerfrei, keine toten Links, Abschnitt „Weiterlesen“ mit 1–3 Links auf Microsoft Learn bzw. die Spezifikation.

## Status der Spikes (Prototyp-Aufgaben)

Details, Akzeptanzkriterien und Ablauf: `concept/spikes.md`. Status: offen · läuft · entschieden (mit ADR-Link).

| # | Spike | Blockiert | Status |
| --- | --- | --- | --- |
| S0 | Roslyn auf 5.x heben (C# 14) | jeglichen Inhalt mit C#-14-Features | offen |
| S1 | Code aus `.cs`-Dateien in Codeblock und `<CSharpRunner>` einbinden | jeglichen neuen Inhalt | entschieden ([ADR 0002](docs-dev/adr/0002-code-import.md)) |
| S2 | CI: alle `.cs`-Dateien kompilieren und Ausgaben abgleichen | jeglichen neuen Inhalt | läuft ([ADR 0003](docs-dev/adr/0003-example-ci.md): Skript und Demos da, noch nicht ausgeführt; Workflow und Snapshots fehlen) |
| S3 | Roslyn-Schleifenwächter gegen Endlosschleifen | Modul 2 (Schleifen) | offen |
| S4 | Prism-Overlay im Editor | Modul 2 | offen |
| S5 | Schritt-für-Schritt-Visualisierung | Modul 4 | offen |
| S6 | `Microsoft.Extensions.*` im Playground | Modul 15 | offen |
| S7 | Runner ↔ Seite: Ausgabe/Testergebnis per `postMessage` | Vorhersagefeld, geprüfte Aufgaben | offen |

## Arbeitsweise

- Größere Aufgaben als Issue mit Label `claude:auto-architect`, kleine mit `claude:auto-implement` (siehe `.github/workflows/claude-*.yml`). Abhängigkeiten im Issue als eigene Zeile `Depends on #<nummer>`.
- Spikes enden immer mit einer ADR in `docs-dev/adr/` und einer Aktualisierung der Statustabelle oben.
- Änderungen an `.github/workflows/` brauchen einen Menschen (so in `claude-implement.yml` festgelegt).
- Halbjährliche Review (nächste: 2027-04): Versions- und Historienlektion (Modul 0), KI-Modul (Studienlage, Links), tote Links.

## Konzept (verbindlich)

@concept/didaktik.md
@concept/lektions-template.md
@concept/modulplan.md
@concept/komponenten.md
@concept/stil-und-sprache.md
@concept/ki-modul.md
@concept/spikes.md
