---
paths:
  - "playground/**"
  - "website/src/components/CSharpRunner/**"
---

# Regeln für Playground und Runner-Einbettung

- Der Playground läuft einthreadig in WebAssembly. Nichts blockierend warten (`.Wait()`, `.Result`) – siehe `ResolveEntryPoint` in `playground/CSharpRunner.cs`.
- `PublishTrimmed` und AOT bleiben aus (Roslyn nutzt Reflection). `InvariantGlobalization` bleibt an.
- Referenzen für Nutzercode kommen ausschließlich aus der `ReferenceName`-Liste bzw. den in S6 festgelegten Paketen. Keine freie NuGet-Installation.
- Nichts an einen Server senden: Kompilieren und Ausführen bleiben lokal im Browser.
- `postMessage` nur mit Ursprungsprüfung (`window.location.origin`) in beide Richtungen.
- Die `<base href>`-Ersetzung und die Fingerprint-Prüfung in `.github/workflows/pages.yml` nicht brechen; `index.html` ohne Fingerprint-Platzhalter lassen.
- Oberflächentexte nicht neu hart kodieren; sie werden übersetzbar (Backlog B5).
- Tastaturbedienung (Tab, Esc, Strg/⌘+Enter) und `aria-live`-Ergebnisbereich erhalten.
- Nach Änderungen: `dotnet build playground/Playground.csproj` und `cd website && npm run build`.
