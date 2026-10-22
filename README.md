# dotnet-learn

Lernseite für C# und .NET auf GitHub Pages. Sie besteht aus zwei Teilen, die zusammen veröffentlicht werden:

| Teil | Ordner | Technik | URL |
| --- | --- | --- | --- |
| Lernseite (Kapitel, Quizzes, einklappbarer Code) | [`website/`](website) | Docusaurus 3 | `https://peterkronberger.github.io/dotnet-learn/` |
| C#-Playground (Code im Browser ausführen) | [`playground/`](playground) | Blazor WebAssembly + Roslyn (.NET 10) | `https://peterkronberger.github.io/dotnet-learn/runner/` |

Die Lernseite bettet den Playground per `<CSharpRunner code={...} />` als iframe ein. Das Konzept hinter dem Aufbau steht auf der Seite „Lernprinzipien“ (`website/docs/lernprinzipien.mdx`).

## Einmalige Einrichtung auf GitHub (manuell)

Diese Schritte lassen sich nicht über Code festlegen:

1. **Pages-Quelle umstellen:** *Settings → Pages → Build and deployment → Source: **GitHub Actions***.
2. **Environment prüfen (nur bei Fehlermeldung):** Meldet das Deployment, der Branch dürfe nicht nach `github-pages` deployen, unter *Settings → Environments → github-pages* den Branch `main` erlauben.
3. **Repository öffentlich** lassen. Mit GitHub Free ist Pages nur für öffentliche Repositories verfügbar.
4. **Eigene Domain (optional):** *Settings → Pages → Custom domain*, DNS beim Anbieter setzen, dann *Enforce HTTPS* aktivieren. Danach `url` und `baseUrl` in `website/docusaurus.config.js` sowie `SITE_BASE_PATH` in `.github/workflows/pages.yml` anpassen.

Die Seite wird bei jedem Push auf `main` gebaut und veröffentlicht. Pull-Requests werden nur gebaut, nicht veröffentlicht.

## Lokal entwickeln

Voraussetzungen: Node.js 20 oder neuer, .NET SDK 10.

```bash
# Lernseite (http://localhost:3000/dotnet-learn/)
cd website
npm install
npm start

# Playground (separates Terminal)
dotnet run --project playground
```

Lokal zeigt die eingebettete Vorschau den Playground unter dem Pfad `/dotnet-learn/runner/` der Docusaurus-Seite nur, wenn der Playground vorher veröffentlicht und nach `website/build/runner` kopiert wurde. Für die Entwicklung genügt es, den Playground direkt über `dotnet run` aufzurufen.

## Inhalte schreiben

- Kapitel liegen als `.mdx` unter `website/docs/`. Reihenfolge über `sidebar_position`.
- Code mit Syntax-Highlighting: Markdown-Codeblöcke mit ` ```csharp `.
- Einklappbar: `<details><summary>…</summary> … </details>` (Leerzeilen vor und nach dem Inhalt lassen).
- Quiz: `<Quiz id="…" question="…" options={[…]} answer={0}>Erklärung</Quiz>`.
- Ausführbarer Code: `<CSharpRunner code={`…`} />`. Der Code darf keine Backticks, keine Backslashes und keine Zeichenfolge `${` enthalten.

## Grenzen des Playgrounds

- Der erste Start lädt die .NET-Laufzeit und den Roslyn-Compiler (mehrere MB, danach im Browser-Cache) und kompiliert im ersten Durchlauf langsam.
- Der Code läuft im Hauptthread: Endlosschleifen blockieren den Tab. Eingaben über `Console.ReadLine` werden nicht unterstützt.
- Verfügbar sind die in `playground/Playground.csproj` aufgeführten Referenz-Assemblies (u. a. `System.Linq`, `System.Collections`, `System.Text.RegularExpressions`). Weitere lassen sich in der Liste `ReferenceName` ergänzen.
