---
paths:
  - "website/examples/**/*.cs"
---

# Rules for example code

- C# 15 / .NET 11, top-level statements, type declarations at the end of the file, nullable on.
- Identifiers and comments in **English**. Program output is English too. Explanations belong in the lesson, not in the code.
- One example per file; file name in English, kebab-case, descriptive (`integer-division.cs`).
- An example may use only constructs that are explained up to its lesson or marked with a `<Forward>` note in the lesson. Code comments are a construct too (taught in module 1): do not put comments into examples of module 0.
- Keep it short: as few lines as possible, only the current concept.
- Keep lines short: at most about 80 characters per line, so code blocks and the runner rarely need horizontal scrolling or line wrapping.
- Fold markers (`// @fold-start` / `// @fold-end`) hide unchanged code in the runner. SecLog task files with more than 40 lines get them from module 1 on; rules in `concept/project-seclog.md`, "Short task files".
- Runnable examples must run on their own. Fragments that cannot run on their own end in `.snippet.cs` (CI skips them).
- Examples that need NuGet packages (EF Core, Minimal API, xUnit, DI/logging) are projects under `website/examples/_ci/<name>/` with `run.sh` and `expected.out.txt`.
- Runnable package examples (M15): a DI or logging example that a lesson shows in `<CSharpRunner>` is one project `_ci/mXX-lYY-<slug>/` with exactly one code file `Program.cs`. The `.csproj` follows `_ci/di-logging/di-logging.csproj`: `ImplicitUsings` disabled, `<Using Include="…" />` items for exactly the six global usings of the playground (`System`, `System.Collections.Generic`, `System.IO`, `System.Linq`, `System.Threading`, `System.Threading.Tasks`), `InvariantGlobalization` true, `Nullable` enable, and only the packages of S6. `run.sh` is `dotnet run --project <name>.csproj --verbosity quiet`. The lesson imports `Program.cs` and `expected.out.txt` with `raw-loader`. Helper code that is not the topic (for example a small synchronous console logger provider, because `AddConsole` writes on a background thread and its lines can change order) goes at the end of `Program.cs` between `// @fold-start` and `// @fold-end`. Check the program in the browser (`/dev/runner-demo`): a program that compiles in CI can still miss a reference in the playground.
- Hosting and configuration projects (◻): `_ci/mXX-lYY-<slug>/` with `Microsoft.Extensions.Hosting` (11.0.*), shown only as code blocks. The output must be stable on every run: `builder.Logging.ClearProviders()` (no host lifetime messages), the program ends by itself (for example `IHostApplicationLifetime.StopApplication()` in a `BackgroundService`), and environment variables and command-line arguments are set in `run.sh`. See `_ci/config-hosting/`.
- Test projects print their results with `tools/run-tests.sh` (add `--allow-failures` when a lesson shows failing tests on purpose). The isolated test examples of a lesson are one project `_ci/mXX-lYY-<slug>/`; SecLog as a project in ◻ modules: `concept/project-seclog.md`, "◻ modules: SecLog as a project".
- Input for `Console.ReadLine()` goes into `<name>.in.txt` next to the example. CI feeds it, and the lesson passes it as `stdin` to `<CSharpRunner>`.
- Output must not depend on culture (`InvariantCulture`): the playground uses `InvariantGlobalization`.
- If the example should be runnable, use only namespaces that the playground has (`playground/Playground.csproj`, `ReferenceName`).
- No endless loops and no real network access in runnable examples. File access only as in module 7: a fresh directory from Directory.CreateTempSubdirectory, the example writes every file it reads, deletes the directory at the end, and never prints a full path (see website/examples/dev/file-io-check.cs).
- SecLog sample data comes from the canonical data set in `concept/project-seclog.md`. It is made up: no real people, no credentials; IP addresses only from RFC 5737 (`192.0.2.0/24`, `198.51.100.0/24`, `203.0.113.0/24`).
