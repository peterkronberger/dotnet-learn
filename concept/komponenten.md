# Komponenten und Konventionen

Stand: 2026-10-22. Status: **vorhanden** (im MVP) · **geplant** · **🔒 Sx** (hängt von einem Spike ab). Alle Komponenten sind global in `website/src/theme/MDXComponents.js` registriert (kein Import in MDX nötig).

## Admonitions (Docusaurus, vorhanden)

| Zweck | Syntax |
| --- | --- |
| Lernziele | `:::note[Lernziele]` |
| Vorhersage | `:::info[Vorhersage]` |
| Fehlvorstellung | `:::warning[Achtung]` |
| Typische KI-Antwort (nur Modul 19) | `:::note[Typische KI-Antwort (konstruiertes Beispiel)]` |

Aufklappbares: `<details><summary>Lösung und Erklärung anzeigen</summary> … </details>` bzw. `Vertiefung: … (optional)` (Leerzeilen vor und nach dem Inhalt).

## `<CSharpRunner>` – vorhanden, wird erweitert

Heute: `code` (String), `height` (px, Standard 460). Bettet `/runner/` als iframe ein, Code als Base64url im URL-Fragment, `loading="lazy"`, Theme per `postMessage` (`{type: 'theme', value}`).

Vorhanden (S7):
- `id`: stabiler Schlüssel der Instanz (Standard: automatisch); ordnet `run-result` der richtigen Instanz zu.
- `predict` (bool) + `expected` (String, Ausgabe-Snapshot): ein Eingabefeld pro erwarteter Ausgabezeile; nach „Ausführen“ ✓/✗ pro Zeile, sachlich formuliert, Ergebnis in `aria-live`-Region. Geprüft wird gegen die echte Ausgabe.
- `tests` (String): C#-Prüfcode mit `public static class TaskTests` und `Run()` (Tupel `Name, Visible, Passed, Feedback`); der Runner führt dann nur die Tests aus. Ergebnis pro Test, versteckte Tests mit neutralem Namen.
- `onResult`: Callback mit der `run-result`-Nachricht.
- Demo: `/runner-demo`.

Geplant:
- `code` kommt aus einer `.cs`-Datei (`import x from '!!raw-loader!@site/examples/…'`) – 🔒 S1. Damit entfällt die README-Einschränkung „keine Backticks, Backslashes, `${`“.
- `stdin` (String): vorbefüllte Konsoleneingabe, zeilenweise von `Console.ReadLine()` gelesen. Im Runner als Eingabefeld unter dem Editor.
- Laden: weit weggescrollte Instanzen entladen, eingegebener Code bleibt erhalten; Runtime aus dem Browser-Cache.
- Sprache der Runner-Oberfläche (heute fest Deutsch in `App.razor`) über einen Parameter (z. B. `#lang=de`) – für i18n.

Nachrichtenprotokoll (S7, vorhanden; Ursprungsprüfung `window.location.origin` in beide Richtungen, Seite prüft zusätzlich `event.source` und `id`):
- Seite → Runner: `{type: 'theme', value}`; Konfiguration im Fragment: `#code=<b64>&id=<id>&tests=<b64>`
- Runner → Seite: `{type: 'run-result', id, success, output, diagnostics: [{isError, text, line}], tests?: [{name, visible, passed, feedback}]}`

## Playground (`playground/`) – vorhanden

- `<textarea>`-Editor; Tab, Esc, Strg/⌘+Enter vorhanden. Keine Autovervollständigung (entschieden).
- Prism-Overlay für Syntaxfarben, Zeilen-Hervorhebung, Fehlermarkierung – 🔒 S4.
- Schleifenwächter (Abbruch nach ca. 3 s mit deutscher Meldung und Zeilennummer) – 🔒 S3.
- Compilerfehler als Liste mit Zeile/Spalte (vorhanden) + deutsche Erklärung häufiger Fehler zusätzlich zur Originalmeldung (geplant).
- Schritt-für-Schritt-Visualisierung (Variablen, Heap-Modell), pro Runner per `trace` einschaltbar – vorhanden (S5).
- Fest vorgeladene Pakete (`Microsoft.Extensions.DependencyInjection`, `…Logging`) – 🔒 S6. Keine freie NuGet-Installation.
- `InvariantGlobalization=true`: Zahlen erscheinen immer mit Punkt (`3.5`). Die CI muss mit `InvariantCulture` laufen, damit gezeigte Ausgaben übereinstimmen.

## `<Quiz>` – vorhanden (Single Choice), wird zur Familie ausgebaut

Heute: `id`, `question`, `options`, `answer`, `children` (Erklärung); Fortschritt unter `dotnet-learn:quiz:<id>` in localStorage.

Geplant:
- **Fragen in Datendateien pro Sprache** statt inline in MDX, z. B. `website/src/data/quiz/de/m01.json`; in MDX nur `<Quiz id="m01-l02-q3" />`. Grund: Leitner-Dashboard und Einstufungstests brauchen alle Fragen zentral; Übersetzung pro Sprache. Bestehende Inline-Quizzes migrieren.
- Typen: `single`, `multi`, `output` (Ausgabe vorhersagen), `parsons` (mit Tastatur-Alternative), `cloze` (Code-Lückentext), `find-bug` (Zeile anklicken), `match` (Zuordnung), `explain` (In eigenen Worten erklären, Musterantwort aufdecken, Selbstbewertung).
- Jede Antwortoption kann eine `misconception`-Referenz tragen (Verknüpfung mit der „Achtung“-Box, DoD-Punkt 11).
- Feedback erklärt immer das *Warum*.

## Lernfortschritt – geplant

- Ein versionierter Speicher (z. B. `dotnet-learn:v1:progress`) in localStorage/IndexedDB, alle Zugriffe in `try/catch`; alte Schlüssel `dotnet-learn:quiz:*` werden migriert.
- **Leitner-System** mit 5 Boxen; Startwerte der Intervalle 1 · 2 · 4 · 8 · 16 Tage (Annahme, anpassbar).
- **„Heute fällig“-Dashboard** auf der Startseite.
- **Einstufungstest** pro Modul (8–12 Fragen, ≥ 80 % → „bekannt“, Fragen in höhere Box).
- **Export/Import** als JSON-Datei.

## Weitere Komponenten – geplant

| Komponente | Zweck |
| --- | --- |
| `<Since csharp="13" dotnet="9" />` | Versions-Badge für Features ab C# 12 / .NET 8, mit aufklappbarem „So schrieb man es vorher“ |
| Umsteiger-Box | Docusaurus `<Tabs groupId="from-language">` mit Java / Python / JS/TS / Go; Auswahl seitenübergreifend (Verhalten von `groupId` vor Umsetzung in der Docusaurus-Doku prüfen) |
| Feedback-Link | Pro Seite „Fehler melden / Feedback“ → vorausgefüllte GitHub-Issue-Vorlage mit Seiten-URL (Vorlage unter `.github/ISSUE_TEMPLATE/` anlegen) |
| `<CodeBlock>` | Offizielle Docusaurus-Komponente für nicht ausführbaren Code aus `.cs`-Dateien |

## Erweiterungen der Seite

- `@docusaurus/theme-mermaid` für Diagramme (mit Textbeschreibung).
- Lokale Volltextsuche (Paket bei Auswahl auf Docusaurus-3- und i18n-Kompatibilität prüfen).
- Playwright-E2E-Tests mit automatischem Barrierefreiheits-Check (z. B. axe-core) in CI.
- Später: `@docusaurus/plugin-pwa`.
