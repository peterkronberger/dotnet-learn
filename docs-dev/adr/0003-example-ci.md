# 0003 – CI prüft Beispiele und gleicht Ausgaben ab

- Status: vorgeschlagen (nicht in der Umgebung ausgeführt, siehe „Ergebnis“)
- Datum: 2026-10-22
- Spike/Issue: S2 / #7

## Frage
Wie prüft CI alle `.cs`-Dateien unter `website/examples/` und stellt sicher, dass gezeigte Ausgaben echt sind (CLAUDE.md, Regel 4)?

## Ergebnis
Umgesetzt, aber **nicht ausgeführt**: In der Entwicklungsumgebung dieses PRs war `dotnet` nicht freigegeben. Das Skript, die drei Demoprojekte und die Paketversionen sind deshalb ungetestet, und es gibt noch keine Snapshots (eine von Hand getippte Ausgabe würde Regel 4 verletzen). Offen bis zum ersten echten Lauf:

- ob `dotnet run <datei>.cs` die Dateien wie erwartet baut (Dateibasierte Apps, .NET 10 SDK; Nullable und Implicit Usings sind dort standardmäßig aktiv – gegen die Microsoft-Doku prüfen),
- ob die Paketversionen (`10.0.*`, xunit 2.9, `xunit.runner.visualstudio` 2.8) auflösen,
- ob das Filtern der xUnit-Ausgabe in `_ci/xunit/run.sh` stabile Zeilen liefert.

## Entscheidung
- **Konsole:** `tools/check-examples.sh` führt jede `*.cs` per `dotnet run <datei>` aus (`DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1`, wie der Playground) und vergleicht mit `<name>.out.txt` daneben. Abweichung, Laufzeit-/Compilerfehler oder fehlender Snapshot → Exit-Code 1.
- **Bruchstücke:** Dateiendung `*.snippet.cs`. Sie werden übersprungen (allein nicht lauffähig, daher auch nicht kompilierbar) und tragen keinen Snapshot.
- **EF Core, Minimal API, xUnit:** je ein Demoprojekt unter `website/examples/_ci/<name>/` mit `run.sh` und `expected.out.txt`. EF Core nutzt SQLite im Speicher und `ToQueryString()`; die Minimal API läuft mit `TestServer` ohne Netzwerk; xUnit gibt die Testzeilen gefiltert aus.
- **Snapshot bewusst aktualisieren:** `tools/check-examples.sh --update`, danach Diff im PR prüfen.
- **Workflow:** Vorschlag in `docs-dev/ci/examples.yml`. Ein Mensch kopiert ihn nach `.github/workflows/examples.yml`.

## Folgen
- Lektionen binden Ausgaben nur aus `*.out.txt` ein (`raw-loader`).
- Bis die Snapshots erzeugt und der Workflow aktiv sind, bleibt der Status von S2 **offen**; Inhalte bleiben blockiert.
- `_ci/`-Projekte erzeugen `bin/`/`obj/` (in `.gitignore`).

## Quellen
- Microsoft Learn: Dateibasierte C#-Apps (`dotnet run file.cs`) – Details beim ersten Lauf prüfen.
- Microsoft Learn: `ToQueryString`, ASP.NET Core `TestServer`, xUnit mit `dotnet test`.
