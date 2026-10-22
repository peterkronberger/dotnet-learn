---
paths:
  - "website/examples/**/*.cs"
---

# Regeln für Beispielcode

- C# 14 / .NET 10, Top-Level-Statements, Typdeklarationen am Dateiende, Nullable aktiv.
- Bezeichner und Kommentare **Englisch**. Erklärungen gehören in die Lektion, nicht in den Code.
- Ein Beispiel pro Datei; Dateiname Englisch, kebab-case, beschreibend (`integer-division.cs`).
- Kurz halten: so wenig Zeilen wie möglich, nur das aktuelle Konzept.
- Ausführbare Beispiele müssen für sich lauffähig sein. Bruchstücke werden nach der in S2 festgelegten Konvention markiert (Vorschlag: `*.snippet.cs`).
- Ausgaben kultur-unabhängig (`InvariantCulture`): der Playground nutzt `InvariantGlobalization`.
- Nur Namespaces verwenden, die im Playground verfügbar sind (`playground/Playground.csproj`, `ReferenceName`), sofern das Beispiel ausführbar sein soll.
- Keine Endlosschleifen, keine echten Netzwerk- oder Dateizugriffe in ausführbaren Beispielen.
- SecLog-Beispieldaten sind erfunden; keine echten Personen, IPs oder Zugangsdaten.
