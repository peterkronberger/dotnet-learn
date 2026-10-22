# 0001 – Roslyn auf 5.x heben

- Status: entschieden (Messung der Download-Größe und Deploy-Prüfung ausstehend)
- Datum: 2026-10-22
- Spike/Issue: S0 / #5

## Frage
Kompiliert der Playground C# 14?

## Ergebnis
- `Microsoft.CodeAnalysis.CSharp` 4.14.0 entspricht C# 13; erst 5.0 bringt C# 14 (Roslyn-Wiki, siehe Quellen).
- Die Version 5.9.0 lässt sich restoren; `dotnet build playground/Playground.csproj` lief im Zwischenstand fehlerfrei (0 Warnungen, 0 Fehler). Die gebaute DLL trägt laut Assembly-Metadaten die Version 5.9.0.
- Größe der DLL `Microsoft.CodeAnalysis.CSharp.dll` (nicht die Download-Größe): 6.697.768 B (4.14.0) vs. 7.079.208 B (5.9.0), also +381.440 B (ca. +0,36 MiB).
- **Download-Größe von `_framework` (MB) vorher/nachher: ausstehend.** Ein erster `dotnet publish` hat die `.wasm`-Dateien aus dem inkrementellen `obj/`-Cache wiederverwendet und war daher nicht belastbar. Für eine saubere Messung: `playground/obj`, `playground/bin` und `out/` löschen, dann je Version `dotnet publish playground/Playground.csproj -c Release -o out/playground` und `du -sb out/playground/wwwroot/_framework`.
- **Nicht geprüft:** Das C#-14-Beispiel (`field`, `extension`) aus dem Issue läuft im veröffentlichten Playground; die bestehenden Beispiele laufen weiter. Das erfolgt manuell nach dem Deploy.

## Entscheidung
Der Playground nutzt `Microsoft.CodeAnalysis.CSharp` 5.9.0. Die Entscheidung gilt vorbehaltlich der manuellen Deploy-Prüfung; schlägt sie fehl, gilt die Ausweichlösung (C#-14-Features nur als nicht ausführbare Beispiele).

## Folgen
- Inhalte mit C#-14-Features dürfen erst nach bestandener Deploy-Prüfung geschrieben werden.
- Die Download-Größe ist nachzutragen.

## Quellen
- Roslyn-Wiki, NuGet-Pakete: https://github.com/dotnet/roslyn/blob/main/docs/wiki/NuGet-packages.md
- NuGet: https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp
