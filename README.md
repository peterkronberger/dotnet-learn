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

Verbindlich sind die Regeln und die Definition of Done in `CLAUDE.md` sowie der Ablauf in `concept/lektions-template.md`.

- **Ablage:** Lektionen liegen unter `website/docs/mXX-<slug>/lYY-<slug>.mdx` (Slugs Englisch, siehe `concept/modulplan.md`). Front Matter: `title`, `sidebar_position`, `description`, `lesson_id` (`mXX-lYY`), `duration_min`.
- **Code** liegt unter `website/examples/mXX/lYY/<english-name>.cs` (Englisch, kebab-case, ein Beispiel pro Datei) und wird per `raw-loader` eingebunden. Backticks, Backslashes und `${` sind im Code erlaubt.
- Einklappbar: `<details><summary>…</summary> … </details>` (Leerzeilen vor und nach dem Inhalt lassen).
- Quiz: `<Quiz id="m01-l03-q1" />`. Die Frage steht in `website/src/data/quiz/de/mXX.json` (Format: [`website/src/data/quiz/README.md`](website/src/data/quiz/README.md)). Inline-Props sind nicht mehr erlaubt, der Build schlägt dann fehl.
- Fehlvorstellung: `<Misconception id="m01-l03-int-division-rounds" />` als erste Zeile in einer `:::warning[Achtung]`-Box; eine Antwortoption verweist per `misconception` darauf.
- Prüfung der Quizdaten und MDX-Verweise: `node tools/check-quiz.mjs`.
- Versions-Badge (nur Features ab C# 12 / .NET 8, sonst Build-Fehler): `<Since csharp="12" dotnet="8" />` inline; mit Kindern (Vorher-Code) als aufklappbares „So schrieb man es vorher“.
- Umsteiger-Box: `<FromLanguage>` mit `<TabItem value="java|python|js|go|cpp">` als Kindern (Beschriftung und Reihenfolge setzt die Komponente; Auswahl gilt per `groupId="from-language"` für alle Boxen). Beispiel: `website/src/pages/dev/components-demo.mdx`.

```mdx
import CodeBlock from '@theme/CodeBlock';
import pitfalls from '!!raw-loader!@site/examples/m01/l02/numeric-pitfalls.cs';

<CSharpRunner height={420} code={pitfalls} />

<CodeBlock language="csharp" title="Beispiel">{pitfalls}</CodeBlock>
```

- **Ausführbar:** `<CSharpRunner code={…} />`. Weitere Props (Details: `concept/komponenten.md`): `predict` + `expected` (Vorhersagefeld), `tests` (automatisch geprüfte Aufgabe), `trace` (Schrittansicht).
- **Nicht ausführbar:** `<CodeBlock language="csharp" title="…">{code}</CodeBlock>`.
- **Ausgaben:** Der Snapshot `<name>.out.txt` entsteht mit `bash tools/check-examples.sh --update` (Diff prüfen). `bash tools/check-examples.sh` kompiliert und vergleicht alle Beispiele. `*.snippet.cs` werden übersprungen. EF Core, Minimal API und xUnit liegen als Projekte unter `website/examples/_ci/`.

## Grenzen des Playgrounds

- Der erste Start lädt die .NET-Laufzeit und den Roslyn-Compiler (mehrere MB, danach im Browser-Cache) und kompiliert im ersten Durchlauf langsam.
- Der Code läuft im Hauptthread. Endlosschleifen werden nach ca. 3 s mit einer Meldung abgebrochen (S3). Der Wächter greift nicht in Operatoren, Accessors und Ausdruckskörpern; ein `catch (Exception)` im Nutzercode fängt ihn ab. Eingaben über `Console.ReadLine` werden nicht unterstützt.
- Verfügbar sind die in `playground/Playground.csproj` aufgeführten Referenz-Assemblies (u. a. `System.Linq`, `System.Collections`, `System.Text.RegularExpressions`). Weitere lassen sich in der Liste `ReferenceName` ergänzen.
- Fest geladen sind außerdem `Microsoft.Extensions.DependencyInjection` und `Microsoft.Extensions.Logging` (S6). Configuration und Hosting fehlen; eine freie NuGet-Installation gibt es nicht.
