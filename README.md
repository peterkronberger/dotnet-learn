# dotnet-learn

A learning site for C# and .NET on GitHub Pages. It has two parts that are published together:

| Part | Folder | Technology | URL |
| --- | --- | --- | --- |
| Learning site (chapters, quizzes, collapsible code) | [`website/`](website) | Docusaurus 3 | `https://peterkronberger.github.io/dotnet-learn/` |
| C# playground (run code in the browser) | [`playground/`](playground) | Blazor WebAssembly + Roslyn (.NET 10) | `https://peterkronberger.github.io/dotnet-learn/runner/` |

The learning site embeds the playground as an iframe with `<CSharpRunner code={...} />`. The ideas behind the design are on the page "Lernprinzipien" (learning principles) (`website/docs/learning-principles.mdx`; it is planned to rename it to "Wie dieser Kurs funktioniert", backlog B10).

## Repository structure

The table is in [`CLAUDE.md`](CLAUDE.md#repo-structure). In short: lessons in `website/docs/`, all C# examples in `website/examples/`, quiz questions in `website/src/data/quiz/`, the playground in `playground/`, the learning concept in `concept/`, checks in `tools/`.

## Working with Claude

Issues can start the Claude workflows through labels: `claude:auto-architect` for larger tasks and `claude:auto-implement` for small ones. Write a dependency as its own line `Depends on #<number>`. Changes to `.github/workflows/` need a human. Details: "Way of working" in [`CLAUDE.md`](CLAUDE.md).

## One-time setup on GitHub (manual)

You cannot set these steps in code:

1. **Switch the Pages source:** *Settings → Pages → Build and deployment → Source: **GitHub Actions***.
2. **Check the environment (only if there is an error):** If the deployment says the branch may not deploy to `github-pages`, allow the branch `main` under *Settings → Environments → github-pages*.
3. **Keep the repository public.** With GitHub Free, Pages is only available for public repositories.
4. **Custom domain (optional):** *Settings → Pages → Custom domain*, set DNS at your provider, then turn on *Enforce HTTPS*. After that, change `url` and `baseUrl` in `website/docusaurus.config.js` and `SITE_BASE_PATH` in `.github/workflows/pages.yml`.

The site is built and published on every push to `main`. Pull requests are only built, not published.

## Develop locally

Requirements: Node.js 20 or newer (`engines` in `website/package.json`), .NET SDK 10.

```bash
# Learning site (http://localhost:3000/dotnet-learn/)
cd website
npm install
npm start

# Playground (separate terminal)
dotnet run --project playground
```

Checks to run before a commit (the first two are the build checks from `CLAUDE.md`):

```bash
cd website && npm run build                # learning site
dotnet build playground/Playground.csproj  # playground
bash tools/check-examples.sh               # compile all examples, compare outputs
node tools/check-quiz.mjs                  # quiz data and links
```

Locally, the embedded preview shows the playground at the path `/dotnet-learn/runner/` of the Docusaurus site only if the playground was published first and copied to `website/build/runner`. For development it is enough to open the playground directly with `dotnet run`.

## Writing content

The rules and the Definition of Done in `CLAUDE.md` and the flow in `concept/lesson-template.md` are binding.

- **Location:** Lessons live in `website/docs/mXX-<slug>/lYY-<slug>.mdx` (slugs in English, see `concept/module-plan.md`). Front matter: `title`, `sidebar_position`, `description`, `lesson_id` (`mXX-lYY`), `duration_min`.
- **Code** lives in `website/examples/mXX/lYY/<english-name>.cs` (English, kebab-case, one example per file) and is included with `raw-loader`. Backticks, backslashes and `${` are allowed in the code.
- Collapsible: `<details><summary>…</summary> … </details>` (leave blank lines before and after the content).
- Quiz: `<Quiz id="m01-l03-q1" />`. The question is stored in `website/src/data/quiz/de/mXX.json` (format: [`website/src/data/quiz/README.md`](website/src/data/quiz/README.md)). Inline props are no longer allowed; the build fails.
- Misconception: `<Misconception id="m01-l03-int-division-rounds" />` as the first line in a `:::warning[Achtung]` box; one answer option points to it with `misconception`.
- Reference lesson (pattern for new lessons): `website/docs/m01-variables-and-types/l03-numbers.mdx`.
- Check quiz data and MDX references: `node tools/check-quiz.mjs`.
- Version badge (only features from C# 12 / .NET 8, otherwise build error): `<Since csharp="12" dotnet="8" />` inline; with children (old code) as a collapsible "So schrieb man es vorher".
- Switcher box: `<FromLanguage>` with `<TabItem value="java|python|js|go|cpp">` as children (the component sets labels and order; the choice applies to all boxes via `groupId="from-language"`). Example: `website/src/pages/dev/components-demo.mdx`.

```mdx
import CodeBlock from '@theme/CodeBlock';
import pitfalls from '!!raw-loader!@site/examples/m01/l03/numeric-pitfalls.cs';

<CSharpRunner height={420} code={pitfalls} />

<CodeBlock language="csharp" title="Beispiel">{pitfalls}</CodeBlock>
```

- **Runnable:** `<CSharpRunner code={…} />`. More props (details: `concept/components.md`): `predict` + `expected` (prediction field), `tests` (automatically checked task), `trace` (step view).
- **Not runnable:** `<CodeBlock language="csharp" title="…">{code}</CodeBlock>`.
- **Outputs:** The snapshot `<name>.out.txt` is created with `bash tools/check-examples.sh --update` (check the diff). `bash tools/check-examples.sh` compiles and compares all examples. `*.snippet.cs` files are skipped. EF Core, Minimal API and xUnit are projects under `website/examples/_ci/`.

## Limits of the playground

- The first start loads the .NET runtime and the Roslyn compiler (several MB, then kept in the browser cache). The first compile is slow.
- The code runs on the main thread. Endless loops are stopped after about 3 s with a message (S3). The guard does not apply inside operators, accessors and expression bodies. A `catch (Exception)` in user code catches it. Input with `Console.ReadLine` is not supported.
- The reference assemblies listed in `playground/Playground.csproj` are available (among others `System.Linq`, `System.Collections`, `System.Text.RegularExpressions`). You can add more to the `ReferenceName` list.
- `Microsoft.Extensions.DependencyInjection` and `Microsoft.Extensions.Logging` are also fixed and preloaded (S6). Configuration and Hosting are missing. There is no free NuGet install.
