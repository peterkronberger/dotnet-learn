# 0008 – Microsoft.Extensions.* (DI, Logging) im Playground

- Status: entschieden (Deploy-Prüfung ausstehend)
- Datum: 2026-10-22
- Spike/Issue: S6 / #12

## Frage
Kann Nutzercode im Playground `ServiceCollection` (Dependency Injection) und Logging verwenden?

## Ergebnis
- Die Pakete sind nicht Teil von `Microsoft.NETCore.App.Ref`, deshalb reicht die `ReferenceName`-Liste nicht. `playground/Playground.csproj` referenziert nun `Microsoft.Extensions.DependencyInjection` und `Microsoft.Extensions.Logging` (je `10.0.*`) und bettet deren aufgelöste Assemblies im Ziel `EmbedReferenceAssemblies` als `refs/…`-Ressourcen ein. `CSharpRunner.cs` bleibt unverändert, weil `LoadReferences` alle `refs/`-Ressourcen liest.
- **Feste Menge** (keine freie NuGet-Installation): `Microsoft.Extensions.DependencyInjection`, `….DependencyInjection.Abstractions`, `….Logging`, `….Logging.Abstractions`, `….Options`.
- **Konsolen-Logging:** `Microsoft.Extensions.Logging.Console` ist bewusst nicht enthalten. Sein Logger schreibt über einen Hintergrund-Thread, der in der einthreadigen WebAssembly-Laufzeit nicht zuverlässig läuft (nicht geprüft). Das Beispiel nutzt stattdessen einen kleinen eigenen `ILoggerProvider`, der in `Console` schreibt.
- **Beispiel:** `website/examples/s6/di-logging.snippet.cs` (Singleton/Scoped/Transient, Logging). Als `.snippet.cs` markiert, weil die Datei Pakete braucht und `dotnet run file.cs` sie nicht auflösen kann; die CI prüft sie über das Projekt `website/examples/_ci/di-logging/` (Ausgabe als Snapshot).
- **Größe von `wwwroot/_framework`** (unkomprimiert, `dotnet publish -c Release`, `du -sb`): 61.477.674 B (≈ 58,6 MiB) vorher vs. 62.084.297 B (≈ 59,2 MiB) nachher, also +606.623 B (≈ +0,58 MiB, +1,0 %). Der Zuwachs besteht überwiegend aus Laufzeit-DLLs von `Logging` und `Options`, die bisher nicht ausgeliefert wurden. Transfergröße (gzip) nicht gemessen.
- `dotnet build playground/Playground.csproj` und `cd website && npm run build` laufen fehlerfrei.
- **Nicht geprüft:** Das Beispiel kompiliert und läuft im veröffentlichten Playground. Ob Nutzercode weitere Typen braucht, deren Assemblies fehlen (z. B. `Microsoft.Extensions.Primitives`), zeigt sich erst dort. Das erfolgt manuell nach dem Deploy.

## Entscheidung
Der Playground liefert die feste Paketmenge oben als Compiler-Referenzen aus. Die Entscheidung gilt vorbehaltlich der manuellen Deploy-Prüfung; schlägt sie fehl, gilt die Ausweichlösung (Modul 15 als nicht ausführbare Beispiele).

## Folgen
- Modul 15 darf nach bestandener Deploy-Prüfung ausführbare Beispiele mit DI und eigenem Logging-Provider enthalten.
- Der Playground wächst um ca. 0,6 MiB.
- `Microsoft.Extensions.Configuration`/`Hosting` sind nicht enthalten; bei Bedarf in einer eigenen ADR nachrüsten.
