---
paths:
  - "website/docs/**/*.mdx"
  - "website/docs/**/*.md"
---

# Regeln beim Schreiben von Lektionen

- Aufbau exakt nach `concept/lektions-template.md` (11 Schritte, Praxisbeispiel-Muster in Schritt 4).
- Vor dem Abschluss die Definition of Done aus `CLAUDE.md` Punkt für Punkt prüfen und im PR abhaken.
- Front Matter: `title`, `sidebar_position`, `description`, `lesson_id` (`mXX-lYY`), `duration_min`.
- Fließtext Deutsch, Du-Form, sachlich (`concept/stil-und-sprache.md`). Code, Bezeichner und Kommentare Englisch.
- Code nicht inline schreiben, sondern aus `website/examples/mXX/lYY/*.cs` importieren (sobald S1 entschieden ist).
- Ausgaben nie von Hand tippen – nur aus CI-Snapshots (sobald S2 entschieden ist). Bis dahin: Ausgabe im Playground selbst prüfen.
- Nicht ausführbarer Code: normaler Codeblock mit Titel, ohne Hinweis „nicht ausführbar“.
- Lernziele beobachtbar formulieren; jedes Lernziel wird durch eine Aufgabe geprüft.
- Jede „Achtung“-Box (Fehlvorstellung) ist mit einem Quiz-Distraktor verknüpft.
- Features ab C# 12 / .NET 8: `<Since csharp="…" dotnet="…" />`.
- Am Ende „Weiterlesen“ mit 1–3 Links auf Microsoft Learn bzw. die C#-Spezifikation; Links müssen existieren.
- Prüfe vor dem Schreiben, ob das Modul durch einen offenen Spike blockiert ist (Tabelle in `CLAUDE.md`).
- Keine KI-Inhalte außerhalb von Modul 19.
