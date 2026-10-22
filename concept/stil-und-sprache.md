# Stil und Sprache

## Sprachen

- **Fließtext:** Deutsch. Mehrsprachigkeit über die eingebaute Docusaurus-i18n (`i18n.locales`, derzeit nur `de`). Übersetzungen später unter `website/i18n/<locale>/…`; MDX-Seiten werden als Ganzes übersetzt.
- **Code, Bezeichner, Code-Kommentare:** immer Englisch – auch in Beispielen, Quizzes und im Playground-Startcode. Erklärungen gehören in den Fließtext oder in die Lösung, nicht in deutsche Kommentare.
- **Fachbegriffe:** Englisch, beim ersten Auftreten mit deutscher Erklärung: „Value Type (Werttyp)“. Jeder Begriff kommt ins Glossar.
- **Texte der Oberfläche** (React-Komponenten): über `<Translate>`/`code.json`, nicht hart kodiert.
- **Quiz-Texte, Lernziele, Hinweise:** in sprachspezifischen Datendateien (siehe `komponenten.md`).
- **Slugs:** Englisch, damit URLs in allen Sprachen gleich bleiben.

## Ton

- Du-Form, sachlich, freundlich. Kurze Sätze. Ein Gedanke pro Absatz.
- Keine wertenden Superlative („genial“, „erschreckend“). Keine Floskeln.
- Fehler gelten als normaler Lernschritt; Feedback erklärt, statt zu urteilen.
- Prüfungen nüchtern: ✓/✗, „Stimmt“ / „Stimmt nicht – erwartet war …“.

## Code-Stil

- C# 14 / .NET 10, Top-Level-Statements (Typdeklarationen am Dateiende), Nullable aktiv, `var` wo der Typ offensichtlich ist.
- Moderne Idiome bevorzugen; ältere Schreibweisen nur zeigen, wo man ihnen in fremdem Code oft begegnet – dann mit `<Since>`-Badge und „So schrieb man es vorher“.
- Namenskonventionen nach Microsoft Learn (PascalCase für Typen/Member, camelCase für lokale Variablen).
- Jedes Beispiel ist für sich lauffähig oder ausdrücklich als Bruchstück markiert (Markierung legt S2 fest).
- Ausgaben kultur-unabhängig formulieren (Playground nutzt `InvariantGlobalization`): `3.5`, nicht `3,5`.

## Code-Ablage

- Pfad: `website/examples/mXX/lYY/<english-name>.cs`, ein Beispiel pro Datei.
- Einbindung per `raw-loader` (offizielle Docusaurus-Doku „Importing code snippets“).
- Gezeigte Ausgaben, SQL-Statements, HTTP-Antworten und Testrunner-Ausgaben stammen aus echten CI-Läufen (S2).

## Quellen und Aktualität

- Technische Aussagen → Microsoft Learn oder C#-Spezifikation; am Lektionsende „Weiterlesen“ mit 1–3 Links.
- Didaktische Literatur nur auf der Seite „Wie dieser Kurs funktioniert“, nicht in jeder Lektion.
- Versions-, Historien- und KI-Inhalte tragen „Stand: YYYY-MM-DD“ und werden halbjährlich geprüft.
- Nichts erfinden: Fehlt ein Beleg, wird die Aussage weggelassen oder als offene Frage markiert.

## Barrierefreiheit (WCAG 2.2 AA)

- Alle Interaktionen per Tastatur; sichtbarer Fokus.
- Kontrast Fließtext ≥ 4,5:1 in Hell- und Dunkelmodus.
- Diagramme und Visualisierungen mit Textbeschreibung; Farbe nie als einziges Signal.
- Live-Regionen (`aria-live`) für Ergebnisse von Runner und Quiz.
