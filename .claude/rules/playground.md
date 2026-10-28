---
paths:
  - "playground/**"
  - "website/src/components/CSharpRunner/**"
---

# Rules for the playground and the runner embedding

- The playground runs on a single thread in WebAssembly. Never block while waiting (`.Wait()`, `.Result`) – see `ResolveEntryPoint` in `playground/CSharpRunner.cs`.
- `PublishTrimmed` and AOT stay off (Roslyn uses reflection). `InvariantGlobalization` stays on.
- References for user code come only from the `ReferenceName` list and the packages decided in S6. No free NuGet install.
- Send nothing to a server: compiling and running stay local in the browser.
- `postMessage` only with an origin check (`window.location.origin`) in both directions.
- Do not break the `<base href>` replacement and the fingerprint check in `.github/workflows/pages.yml`; keep `index.html` free of fingerprint placeholders.
- UI texts are not hard-coded again; they will be translatable (backlog B5).
- The editor must never scroll vertically; it grows with its content. The runner reports its content height to the page with `{type: 'resize', id, height}` (origin check as for `run-result`). Do not add elements with a fixed height or an inner scroll area.
- Keep keyboard use (Tab, Esc, Ctrl/⌘+Enter) and the `aria-live` result area.
- After changes: `dotnet build playground/Playground.csproj` and `cd website && npm run build`.
