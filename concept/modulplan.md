# Module plan

As of 2026-10-23. 20 modules (0–19) plus 4 mixed review modules. Planning assumption: 60–80 h for beginners (not derived empirically). Lessons are proposals. The order inside a module may change when it is written. The order of modules may not.

Lesson titles and text are German. German titles below are the real learner-facing names and stay as they are.

**End goal:** C#/.NET basics up to OOP, collections, generics, LINQ, exceptions, async/await and unit tests; plus ASP.NET Core (Minimal APIs) and EF Core. Not included: Blazor, Unity.

## Location and naming

- Folder: `website/docs/mXX-<slug>/`, lessons `lYY-<slug>.mdx`. Examples in parallel under `website/examples/mXX/lYY/`.
- Slugs in English (Docusaurus does not translate slugs, so URLs stay the same in all languages). Titles and body text in German.
- `lesson_id` in the front matter (`m04-l02`) is the stable key for quiz questions and learning progress.

## Existing MVP → target structure

| MVP file | Target | Status |
| --- | --- | --- |
| `docs/intro.mdx` | M0 · L1 "Dein erstes Programm" (switch identifiers/comments to English) | moved to `docs/m00-orientation/l01-first-program.mdx` |
| `docs/lernprinzipien.mdx` | M0 · page "Wie dieser Kurs funktioniert" (add literature from `didaktik.md` and the AI paragraph) | open (B10) |
| `docs/grundlagen/variablen-und-typen.mdx` | M1 · L2/L3 (code to `examples/`, English, fix the solution text to `3.5` – the playground uses `InvariantGlobalization`, so the output is always `3.5`) | moved to `docs/m01-variables-and-types/l02-variables-and-types.mdx` |
| `docs/grundlagen/kontrollfluss.mdx` | M2 (split into several lessons) | moved to `docs/m02-control-flow/l01-control-flow.mdx` (split still open) |

## Modules

Legend: ▶ runnable in the playground · ◻ not runnable (normal code block)

### M0 Orientierung (orientation) ▶
1. Dein erstes Programm (run it right away)
2. C# vs. .NET: language vs. platform – runtime (CLR), standard library (BCL), SDK, compiler (Roslyn)
3. The .NET family: .NET Framework (Windows only, no further development), .NET Core (cross-platform, open source), from .NET 5 on ".NET"; briefly Mono/Xamarin, .NET Standard. **Name clarification**: ".NET Core" ≠ "Entity Framework Core"; ".NET Framework Core" does not exist.
4. History and versions: timeline, yearly release rhythm (November), LTS (even versions, 3 years) vs. STS, support length. With an "as of" date; review twice a year.
5. From code to running program: compiler → IL → JIT → CLR (medium depth, no machine code)
- Page "Wie dieser Kurs funktioniert": methods, limits, how to use it (Leitner, export/import), **one** paragraph on AI use while learning.
- Facts already verified for L4: C#/.NET mapping C# 9/.NET 5, 10/.NET 6, 11/.NET 7, 12/.NET 8, 13/.NET 9, 14/.NET 10 (Roslyn wiki); .NET 10 = LTS, released 2025-11-11, support until November 2028. Check everything else (older years, .NET Framework versions) against Microsoft Learn when writing.

### M1 Erste Programme, Variablen, Typen (first programs, variables, types) ▶
Statements and output · variables and built-in types · integer, floating point, `decimal` · strings and interpolation (*use* objects: `Length`, `ToUpper`) · type conversion and `Parse` · console input (through the runner's input field).
Expected misconceptions: `=` as comparison; integer division rounds; `"3" + 4` gives 7; a variable "remembers" earlier values.

### M2 Kontrollfluss (control flow) ▶
`if`/`else`, comparison and logic operators · `switch` expressions and simple patterns · `while`/`do` · `for`/`foreach` · `break`/`continue`.
Misconceptions: off-by-one; the loop condition is checked only once; `else` belongs to the wrong `if`.

### M3 Methoden (methods) ▶
Definition, parameters, return · `return` vs. output · scope · overloading · local functions · optional/named parameters · `ref`/`out` (short).
Misconceptions: `return` prints something; a method changes the caller's variable.

### M4 Typsystem-Fundamente (type system basics) ▶
Value vs. reference types · stack/heap as a model (mark it as a *model*, a runtime detail) · `null`, nullable reference types, `Nullable<T>` · string immutability · equality `==` vs. `Equals` · (deep dive: boxing).
Misconceptions: assignment copies the object; value types always live on the stack.

### W1 Wiederholung (review) M1–M4 ▶ (mixed practice)

### M5 Arrays, Strings, Collections ▶
Arrays · `List<T>` · `Dictionary<TKey,TValue>` · `HashSet<T>` · `StringBuilder` · iterating and changing.
Misconceptions: the index starts at 1; changing a list during `foreach`.

### M6 OOP I ▶
Classes and objects · fields and properties · constructors · encapsulation · `static`.

### M7 Exceptions ▶
`try`/`catch`/`finally` · `throw` · custom exceptions · validating business rules · `using`/`IDisposable` (short).

### M8 OOP II ▶
Interfaces · inheritance, `abstract`, polymorphism · `record` · `struct` · equality of objects.

### W2 Wiederholung M5–M8 ▶

### M9 Generics ▶
Generic methods and classes · constraints · understanding generic collections.

### M10 Delegates, Lambdas, Events ▶
`Func`/`Action` · lambdas · closures · events.
Misconception: a lambda captures the value instead of the variable.

### M11 LINQ ▶
Method syntax (query syntax briefly) · filter, project, sort · group, aggregate · deferred execution.
Misconception: the query already runs when it is defined.

### W3 Wiederholung M9–M11 ▶

### M12 Pattern Matching (vertieft, in depth) ▶
Type, property, relational and list patterns · `switch` expressions · `record` with `with`.

### M13 async/await ▶
`Task` · `async`/`await` · exceptions in asynchronous code · cancellation (`CancellationToken`).
Misconception: `async` starts a new thread automatically. Note: the playground runs on one thread (WebAssembly). True parallelism only as an example that cannot run.

### M14 Testen (testing) ◻
xUnit: `[Fact]`, `[Theory]`, `Assert` · Arrange-Act-Assert · testable code. The test runner output is produced in CI (S2).

### W4 Wiederholung M12–M14 ▶

### M15 Dependency Injection und Hosting ▶
Dependencies through interfaces · `ServiceCollection` · lifetimes · logging · configuration/options. Runnable for DI and logging. Configuration/options and hosting cannot run (missing in the playground, see S6).

### M16 EF Core ◻
`DbContext`, model, relations (1:n, n:m) · LINQ → SQL (shown SQL comes from a CI run) · change tracking · migrations.

### M17 ASP.NET Core Minimal APIs ◻
Routing, handlers · binding and validation · status codes · middleware pipeline · DI in endpoints. HTTP requests and responses are produced in CI (S2).

### M18 Abschlussprojekt (final project)
Full SecLog: domain logic, LINQ analysis and classifier ▶ automatically checked; EF Core and API parts ◻ as code-review tasks ("Finde 3 Probleme in diesem Endpoint" – find 3 problems in this endpoint, "Welches SQL entsteht?" – which SQL is produced?).

### M19 Programmieren mit KI (programming with AI) ◻/▶
See `concept/ki-modul.md`.

## Practice project SecLog

- **Domain (purely defensive):** analyze login logs, detect brute-force patterns, manage incidents. No attack techniques.
- **From M11:** phishing classifier (Naive Bayes in plain C#) on synthetic sample texts.
- **Form:** optional extra exercises at the end of a lesson. New concepts are always shown first in small, self-contained examples.
- **Data:** made-up sample data only. Mark rules like "5 failed attempts in 10 minutes" as a teaching simplification.
- **Path:** M1–M5 console analysis with arrays/collections → M6–M8 domain model → M7 business rules as exceptions → M11 LINQ analysis + classifier → M14 tests → M16 persistence → M17 incident API → M18 final project.
