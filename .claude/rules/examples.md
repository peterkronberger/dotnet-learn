---
paths:
  - "website/examples/**/*.cs"
---

# Rules for example code

- C# 14 / .NET 10, top-level statements, type declarations at the end of the file, nullable on.
- Identifiers and comments in **English**. Program output is English too. Explanations belong in the lesson, not in the code.
- One example per file; file name in English, kebab-case, descriptive (`integer-division.cs`).
- Keep it short: as few lines as possible, only the current concept.
- Keep lines short: at most about 80 characters per line, so code blocks and the runner rarely need horizontal scrolling or line wrapping.
- Runnable examples must run on their own. Fragments that cannot run on their own end in `.snippet.cs` (CI skips them).
- Examples that need NuGet packages (EF Core, Minimal API, xUnit, DI/logging) are projects under `website/examples/_ci/<name>/` with `run.sh` and `expected.out.txt`.
- Output must not depend on culture (`InvariantCulture`): the playground uses `InvariantGlobalization`.
- If the example should be runnable, use only namespaces that the playground has (`playground/Playground.csproj`, `ReferenceName`).
- No endless loops, no real network or file access in runnable examples.
- SecLog sample data is made up: no real people, IPs or credentials.
