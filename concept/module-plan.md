# Module plan

As of 2026-10-25. 20 modules (0–19) plus 4 mixed review modules. Planning assumption: 60–80 h for beginners (not derived empirically). The spec review of 2026-10-25 adds 43 lesson proposals and 30 smaller parts inside lessons. Lessons only: 43 × 15–25 min = 645–1,075 min ≈ +11–18 h, so the new working assumption is about 70–100 h (still not derived empirically; the smaller parts are assumed to fit into the existing 15–25 min per lesson). Lessons are proposals. The order inside a module may change when it is written. The order of modules may not.

Lesson titles and text are German. German titles below are the real learner-facing names and stay as they are.

**End goal:** C#/.NET basics up to OOP, collections, generics, LINQ, exceptions, async/await and unit tests; plus ASP.NET Core (Minimal APIs) and EF Core. Not included: Blazor, Unity. What else is left out on purpose, and why: see "Deliberately not covered".

## Location and naming

- Folder: `website/docs/mXX-<slug>/`, lessons `lYY-<slug>.mdx`. Examples in parallel under `website/examples/mXX/lYY/`.
- Slugs in English (Docusaurus does not translate slugs, so URLs stay the same in all languages). Titles and body text in German.
- `lesson_id` in the front matter (`m04-l02`) is the stable key for quiz questions and learning progress.

## Existing MVP → target structure

| MVP file | Target | Status |
| --- | --- | --- |
| `docs/intro.mdx` | M0 · L1 "Dein erstes Programm" (switch identifiers/comments to English) | moved to `docs/m00-orientation/l01-first-program.mdx` |
| `docs/learning-principles.mdx` | M0 · page "Wie dieser Kurs funktioniert" (add literature from `didactics.md` and the AI paragraph) | open (B10) |
| `docs/grundlagen/variablen-und-typen.mdx` | M1 · L2/L3 (code to `examples/`, English, fix the solution text to `3.5` – the playground uses `InvariantGlobalization`, so the output is always `3.5`) | moved to `docs/m01-variables-and-types/l02-variables-and-types.mdx`; L3 moved out to `docs/m01-variables-and-types/l03-numbers.mdx` |
| `docs/grundlagen/kontrollfluss.mdx` | M2 (split into several lessons) | moved to `docs/m02-control-flow/l01-control-flow.mdx` (split still open) |

Known gaps (not errors):
- M1 has no L1 yet. The planned lesson is "Statements and output"; the existing lessons start at `l02`.
- M2 `l01-control-flow.mdx` still needs to be split into several lessons.

## Reference: which source counts as "the specification"

DoD point 13 asks to validate every C# statement against the language specification. There are three layers, and a lesson must link the right one:

1. **ECMA-334 (published standard).** The latest published edition on the Ecma site is the 7th edition (December 2023); Ecma maps it to C# 7.
2. **C# standard draft on Microsoft Learn** (`/dotnet/csharp/language-reference/language-specification/<slug>`). Working text of the next edition(s). It already contains later features (for example records, record structs, file-scoped namespaces, global using directives). Clause numbers below follow this draft; check them when linking, they can move between drafts.
3. **Feature specifications** (`/dotnet/csharp/language-reference/proposals/…`, overview page "Latest feature specifications"). The normative-like source for features that are not in the draft yet – in practice everything from about C# 12 on (collection expressions, primary constructors, `params` collections, C# 14 extension members, `field`, …).

Rule: link layer 2 when the clause exists there, otherwise layer 3. Library behavior (BCL, ASP.NET Core, EF Core) is not part of the language specification; link the Microsoft Learn conceptual docs or API reference instead.

Clauses of the draft and where they are taught:

| Clause (draft) | Slug | Covered in |
| --- | --- | --- |
| §6 Lexical structure | `lexical-structure` | M1 (literals, comments, identifiers), M14 (preprocessing directives, only the ones listed there) |
| §7 Basic concepts | `basic-concepts` | M6 (application startup, accessibility), M3 (declarations, scopes, signatures), M4 (automatic memory management, deep dive) |
| §8 Types | `types` | M1, M4, M8, M9, M16 (expression tree types) |
| §9 Variables | `variables` | M1 (definite assignment), M3 (parameter kinds), M4 (default values) |
| §10 Conversions | `conversions` | M1 (numeric), M4 (boxing, nullable), M5 (collection expressions), M8 (reference, user-defined), M10 (anonymous function and method group) |
| §11 Patterns and pattern matching | `patterns` | M2 (simple), M12 (all) |
| §12 Expressions | `expressions` | M1, M2, M4, M5, M10, M11, M12, M13 |
| §13 Statements | `statements` | M2, M3, M7, M11, M13 |
| §14 Namespaces | `namespaces` | M6 |
| §15 Classes | `classes` | M6, M8, M9, M10, M11, M13 |
| §16 Structs | `structs` | M8 |
| §17 Arrays | `arrays` | M5 |
| §18 Interfaces | `interfaces` | M8, M9 |
| §19 Enums | `enums` | M8 |
| §20 Delegates | `delegates` | M10 |
| §21 Exceptions | `exceptions` | M7 |
| §22 Attributes | `attributes` | M14 |
| §23 Unsafe code | `unsafe-code` | not covered (see "Deliberately not covered") |
| Annex: Documentation comments | – | M6 (short) |

## Modules

Legend: ▶ runnable in the playground · ◻ not runnable (normal code block) · *(new)* added by the spec review 2026-10-25 · **Spec** = specification content the module must cover · **Badge** = features from C# 12 / .NET 8 on that need `<Since>`.

### M0 Orientierung (orientation) ▶
1. Dein erstes Programm (run it right away)
2. C# vs. .NET: language vs. platform – runtime (CLR), standard library (BCL), SDK, compiler (Roslyn)
3. The .NET family: .NET Framework (Windows only, no further development), .NET Core (cross-platform, open source), from .NET 5 on ".NET"; briefly Mono/Xamarin, .NET Standard. **Name clarification**: ".NET Core" ≠ "Entity Framework Core"; ".NET Framework Core" does not exist.
4. History and versions: timeline, yearly release rhythm (November), LTS (even versions, 3 years) vs. STS (odd versions, **24 months since .NET 9**, before 18 months), support length. With an "as of" date; review twice a year.
5. From code to running program: compiler → IL → JIT → CLR (medium depth, no machine code)
- Page "Wie dieser Kurs funktioniert": methods, limits, how to use it (Leitner, export/import), **one** paragraph on AI use while learning.
- Facts already verified for L4: C#/.NET mapping C# 9/.NET 5, 10/.NET 6, 11/.NET 7, 12/.NET 8, 13/.NET 9, 14/.NET 10 (Roslyn wiki); .NET 10 = LTS, released 2025-11-11, support until November 2028. Check everything else (older years, .NET Framework versions) against Microsoft Learn when writing.
- **Spec:** language vs. implementation: the standard describes the language, not the runtime or the library (draft: Scope, Conformance, Annex "Standard library"); the three source layers from "Reference" above, in learner words.
- Facts checked 2026-10-25 (support policy page, .NET Blog):
  - STS support is 24 months, starting with .NET 9 (announced 2025-09-16). LTS stays 3 years.
  - .NET 8 (LTS) and .NET 9 (STS) both reach end of support on **2026-11-10**. L4 must reflect this at the first review after that date.
  - .NET 11 is planned for 2026-11-10 together with C# 15 (secondary source: Visual Studio Magazine, 2026-09-14). By the odd/even rule it is an STS release. The course base stays C# 14 / .NET 10 (LTS) – see `CLAUDE.md`.
  - **Open question:** end of support for .NET 10 is 2028-11-14 on the .NET support policy page, but 2028-11-10 on the EF Core 10 "What's new" page. Use the support policy page as the primary source; recheck before writing L4.

### M1 Erste Programme, Variablen, Typen (first programs, variables, types) ▶
Statements and output · variables and built-in types · integer, floating point, `decimal` · strings and interpolation (*use* objects: `Length`, `ToUpper`) · type conversion and `Parse` · console input (through the runner's input field).
Expected misconceptions: `=` as comparison; integer division rounds; `"3" + 4` gives 7; a variable "remembers" earlier values.
- *(new)* Lesson "Comments, names and literals" (can merge into L1 "Statements and output").
- *(new)* Lesson "Operators and expressions": arithmetic and compound assignment, increment/decrement in prefix and postfix form, precedence and associativity.
- *(new)* Lesson "Constants and implicit typing".
- *(new)* Lesson "Overflow and explicit conversions" (after L3 numbers).
- **Spec:**
  - §6: comments, identifiers and the difference between keywords and contextual keywords; integer literals (decimal, hexadecimal, binary, digit separators, type suffixes), real literals, character literals and escape sequences, regular, verbatim, interpolated and raw string literals (C# 11).
  - §7/§13: a program as a sequence of statements; expression statements; local variable and local constant declarations.
  - §8: the simple types and their ranges and sizes; `string` as a reference type that is *used* here, explained in M4.
  - §9: definite assignment – a local variable must be assigned before it is read (compiler error, not a default value).
  - §12: numeric promotion, integer division and remainder (incl. negative operands), floating point special values (infinity, NaN), `decimal` precision, string concatenation with non-string operands, operator precedence and associativity, constant expressions.
  - §10/§12: implicit vs. explicit numeric conversions, loss of data on explicit conversion, overflow in checked vs. unchecked context (default is unchecked for non-constant expressions; constant expressions are checked at compile time).
  - Implicitly typed local variables: the type is fixed at compile time (no dynamic typing).
- **Library (not spec):** `Console` output and input, `int.Parse` vs. `TryParse` (TryParse needs `out` – forward reference to M3, use it as a recipe here), `Math`, composite formatting and format strings; culture dependency of formatting and parsing (playground: `InvariantGlobalization`).
- More misconceptions: postfix increment returns the new value; `var` makes a variable dynamic; a cast rounds instead of truncating.
- **Dependency:** console input needs the planned `stdin` parameter of `<CSharpRunner>` (`components.md`). Until then, the lesson part "console input" cannot have a runnable task.

### M2 Kontrollfluss (control flow) ▶
`if`/`else`, comparison and logic operators · `switch` expressions and simple patterns · `while`/`do` · `for`/`foreach` · `break`/`continue`.
Misconceptions: off-by-one; the loop condition is checked only once; `else` belongs to the wrong `if`.
- *(new)* The switch **statement** next to the switch expression (people meet both).
- *(new)* The conditional operator (inline choice of a value).
- *(new)* Nested loops (base for 2D arrays in M5).
- **Spec:**
  - §12: relational and equality operators, conditional logical operators with short-circuit evaluation vs. the non-short-circuit logical operators, the conditional operator (both branches need a common type), boolean expressions.
  - §13: selection statements; switch statement rules: no implicit fall-through between sections, case guards, the default section, a switch section must not reach its end; the four iteration statements and when each one evaluates its condition (before vs. after the body); jump statements that leave or restart a loop; scope of loop variables.
  - §11 (simple part): constant patterns, relational patterns and the discard pattern in switch expressions; non-exhaustive switch expressions give a compiler warning and a runtime exception.
- More misconceptions: `&&` and `&` are the same; a `case` falls through like in C/Java.

### M3 Methoden (methods) ▶
Definition, parameters, return · `return` vs. output · scope · overloading · local functions · optional/named parameters · `ref`/`out` (short).
Misconceptions: `return` prints something; a method changes the caller's variable.
- *(new)* Lesson "Recursion and the call stack" (incl. stack overflow as a symptom).
- *(new)* Lesson "Tuples: returning several values" (tuple types, element names, deconstruction).
- *(new)* Expression-bodied members (short form of a method).
- *(new)* Parameter arrays (`params`).
- **Spec:**
  - §7: declarations and declaration spaces, scopes, name hiding through nesting; signatures and overloading (return type is not part of the signature).
  - §15 (methods): method declarations, value parameters, reference, output and input parameters (`in`, short), parameter arrays, optional parameters and default values (must be compile-time constants), named arguments, expression bodies, local functions incl. static local functions.
  - §9: definite assignment of output parameters before the method returns.
  - §12: invocation, argument evaluation order (left to right), overload resolution in simple cases (better conversion), tuple expressions and deconstruction.
  - §8: value tuple types; tuple element names are not part of the runtime type (deep dive).
- **Badge:** `params` collections (C# 13 / .NET 9); optional parameters in lambdas come in M10.
- More misconceptions: a recursive method needs no base case; a tuple is a reference type.

### M4 Typsystem-Fundamente (type system basics) ▶
Value vs. reference types · stack/heap as a model (mark it as a *model*, a runtime detail) · `null`, nullable reference types, `Nullable<T>` · string immutability · equality `==` vs. `Equals` · (deep dive: boxing).
Misconceptions: assignment copies the object; value types always live on the stack.
- *(new)* Lesson "Passing arguments": value parameter of a reference type vs. reference parameter (connects M3 and M4).
- *(new)* Lesson "Null-safe operators": null-conditional and null-coalescing operators, null-coalescing assignment.
- **Spec:**
  - §8: value types vs. reference types, the simple types as structs, `object` as the root, boxing and unboxing; nullable value types and lifted operators; nullable reference types: annotations, the nullable context, flow analysis and warnings (they are compile-time warnings, not runtime checks), the null-forgiving operator.
  - §9: default values of variables (fields and array elements get them, locals do not – link M1).
  - §12: reference equality vs. value equality for predefined types; string equality compares content; null-conditional member and element access; null-coalescing operator and assignment.
  - §7 (deep dive): automatic memory management – the standard does not say where values live; stack/heap is an implementation model.
  - Project setting: nullable context on/off (link to M14).
- **Badge:** null-conditional assignment (C# 14 / .NET 10).
- More misconceptions: "reference types are passed by reference"; a nullable annotation prevents `null` at runtime.

### W1 Wiederholung (review) M1–M4 ▶ (mixed practice)

### M5 Arrays, Strings, Collections ▶
Arrays · `List<T>` · `Dictionary<TKey,TValue>` · `HashSet<T>` · `StringBuilder` · iterating and changing.
Misconceptions: the index starts at 1; changing a list during `foreach`.
- *(new)* Lesson "Multi-dimensional and jagged arrays".
- *(new)* Lesson "Index from end and ranges".
- *(new)* Lesson "Collection expressions".
- *(new)* Lesson "Which collection when?": queue, stack, the collection interfaces, rough cost of lookups (constant vs. linear) as a decision aid.
- *(new)* Lesson "Working with strings": searching, splitting, joining, comparing with ordinal vs. culture-sensitive rules.
- *(new)* Lesson "Date and time" (needed for SecLog timestamps such as "5 failed attempts in 10 minutes").
- **Spec:**
  - §17: array types, rank, single-dimensional, multi-dimensional and jagged arrays, array creation and initializers, element access and bounds checking (exception on a wrong index), the `System.Array` base type; array covariance and its runtime check (deep dive).
  - §12: element access with index-from-end and ranges (C# 8), the `foreach` contract (pattern-based enumeration, the iteration variable is read-only).
  - §10/§12: collection expressions and the spread element (target-typed; which target types are allowed).
  - §8/§12: object and collection initializers.
- **Library (not spec):** generic collection interfaces (`IEnumerable<T>`, `ICollection<T>`, `IList<T>`, the read-only variants, `IDictionary<TKey,TValue>`); the enumerator invalidation rule when a collection changes during enumeration; dictionary keys need consistent equality and hash code (forward reference M8); `StringBuilder`; `DateTime` vs. `DateTimeOffset`, `TimeSpan`, `DateOnly`/`TimeOnly`, time zones only as a warning (UTC for logs).
- **Badge:** collection expressions (C# 12 / .NET 8).
- More misconceptions: `string.Replace` changes the string; `DateTime.Now` is fine for log timestamps across time zones.

### M6 OOP I ▶
Classes and objects · fields and properties · constructors · encapsulation · `static`.
- *(new)* Lesson "Properties in depth": auto-properties, init-only setters, required members, properties with a backing field and the `field` keyword.
- *(new)* Lesson "Object initializers and target-typed creation".
- *(new)* Lesson "Primary constructors".
- *(new)* Lesson "Namespaces and using directives": file-scoped namespaces, global using directives, implicit usings of the SDK, using alias and `using static`.
- *(new)* Lesson "Where the program starts": entry point, top-level statements as generated `Main`, command-line arguments, exit code. (`CLAUDE.md` says "`Main` is explained later" – this is that place.)
- *(new)* Short part "Documentation comments" for public members.
- **Spec:**
  - §15: class declarations and modifiers (`static`, `sealed`, `partial` only by name), members, accessibility (`public`, `private`, `internal`, `protected` – the last one in M8; `protected internal`/`private protected` only by name), instance and static fields, readonly fields vs. constants, methods, properties and accessors (incl. accessor accessibility), instance and static constructors, constructor overloading and chaining with `this`, `this` access; primary constructors: parameters are in scope for the whole class and are captured, they are **not** properties (unlike records).
  - §7: accessibility domains; `internal` means "same assembly" (link M14 projects); application startup and termination, entry point rules, top-level statements.
  - §14: compilation units, namespace declarations (block and file-scoped), using directives (namespace, alias, static), global using directives; fully qualified names and the global namespace alias (short).
  - §12: object creation, target-typed object creation, object initializers, member access.
  - Annex "Documentation comments": XML documentation comments (short, for library code).
- **Badge:** primary constructors for classes (C# 12 / .NET 8); `field` keyword (C# 14 / .NET 10).
- More misconceptions: a primary constructor parameter is a property; `static` means "cannot change"; `readonly` makes the referenced object immutable.

### M7 Exceptions ▶
`try`/`catch`/`finally` · `throw` · custom exceptions · validating business rules · `using`/`IDisposable` (short).
- *(new)* Lesson "Reading a stack trace" (debugging skill; runtime errors vs. compile errors vs. logic errors).
- *(new)* Lesson "Exception or not?": exceptions vs. the Try-pattern vs. return values (Microsoft design guidelines, not spec).
- *(new)* Lesson "Reading and writing files" (◻ or ▶, see note).
- **Spec:**
  - §21: causes of exceptions, the `System.Exception` class, how an exception is handled (search for a matching catch clause outward through the call stack, finally blocks run on the way), common exception classes defined by the standard.
  - §13: try statement forms, order of catch clauses (more specific first, compile error otherwise), exception filters, rethrow without losing the stack trace vs. throwing the caught object again, the using statement and using declarations (C# 8), disposal order.
  - §12: throw expressions (for example in a null-coalescing expression).
  - §15 (deep dive): finalizers and why application code rarely needs them; the dispose pattern.
  - Link to M1: overflow exceptions in checked context.
- **Library (not spec):** argument guard helpers (throw helpers on exception types), `IDisposable`, best practices for exceptions (Microsoft Learn), `System.IO` (`File`, `Path`, streams, text encoding), `IOException` family.
- **Note:** check whether the playground (Blazor WebAssembly) supports an in-memory file system well enough for runnable file examples. If not, the file lesson uses normal code blocks with CI-produced output.
- More misconceptions: `finally` runs only when there was no exception; catching `Exception` everywhere is safe practice.

### M8 OOP II ▶
Interfaces · inheritance, `abstract`, polymorphism · `record` · `struct` · equality of objects.
- *(new)* Lesson "Virtual, override and hiding" (incl. `sealed`, `base`, constructor chaining to the base class).
- *(new)* Lesson "Type tests and casts" (type test, safe cast, pattern-based test; link M12).
- *(new)* Lesson "Enums" (named constants, underlying values, flags, parsing).
- *(new)* Lesson "Comparing and sorting objects" (comparison interfaces, comparer objects).
- *(new)* Short part "Composition vs. inheritance" (design guideline, not spec).
- **Spec:**
  - §15: base classes, inheritance of members, protected accessibility, virtual, override, sealed and abstract members, member hiding and the warning for it, base access, abstract classes; operator overloading and user-defined conversions (short or deep dive); indexers (deep dive).
  - §18: interface declarations and members, implementation and explicit interface member implementation, default interface members (C# 8, deep dive), interfaces as types.
  - §16: struct semantics (copy on assignment, value of `default`, no inheritance from other structs), readonly structs and readonly members, record structs (C# 10), boxing when a struct is used through an interface.
  - Records (in the draft): record class vs. record struct, positional records, compiler-generated members (value-based equality, `ToString`, deconstruction, copy constructor), records and inheritance.
  - §19: enum declarations, underlying type, enum members and their values, enum operations and conversions; any value of the underlying type is valid, so switches over enums are not exhaustive.
  - §12: type-testing operator, `as` operator, explicit reference conversions and their runtime check.
  - §8/§10: implicit and explicit reference conversions.
- **Library (not spec):** the virtual members of `object` and the equality contract (`Equals` and `GetHashCode` must agree), `IEquatable<T>`, `IComparable<T>`/`IComparer<T>`, flags attribute for enums, `Enum.Parse`/`TryParse`.
- More misconceptions: a record is always immutable; an enum variable can only hold its named values; overriding `Equals` without `GetHashCode` is harmless.

### W2 Wiederholung M5–M8 ▶

### M9 Generics ▶
Generic methods and classes · constraints · understanding generic collections.
- *(new)* Generic interfaces and generic delegates (as base for M10/M11).
- *(new)* Default value of a type parameter.
- **Spec:**
  - §15: type parameters and type parameter constraints (class, struct, notnull, base class, interface, parameterless constructor; unmanaged only by name), generic methods.
  - §8: constructed types, open and closed types, type arguments; type parameters and nullable annotations.
  - §12: type inference for generic method calls (simple cases), default value expressions.
  - §18 (deep dive): variance of interfaces and delegates (`in`/`out`) – why `IEnumerable<string>` converts to `IEnumerable<object>` but `List<string>` does not convert to `List<object>`.
- More misconceptions: `List<object>` accepts a `List<string>`; generics cost boxing for value types.

### M10 Delegates, Lambdas, Events ▶
`Func`/`Action` · lambdas · closures · events.
Misconception: a lambda captures the value instead of the variable.
- *(new)* Method groups as delegate values.
- *(new)* Multicast delegates and unsubscribing from events.
- **Spec:**
  - §20: delegate declarations, compatibility, instantiation, invocation, multicast invocation lists (order, return value of the last target).
  - §10: anonymous function conversions and method group conversions.
  - §12: anonymous functions (lambda expressions), captured outer variables and their lifetime, static anonymous functions, natural type of lambdas (C# 10), discard parameters.
  - §15: events, field-like events, event accessors (short); the standard event pattern with sender and event args (library convention).
  - Loop capture: each `foreach` iteration has its own variable; a `for` loop variable is one shared variable.
- **Badge:** optional parameters and parameter arrays in lambdas (C# 12 / .NET 8); modifiers on simple lambda parameters (C# 14 / .NET 10, deep dive only).
- More misconceptions: a lambda runs when it is defined; an event can be raised from outside the class.

### M11 LINQ ▶
Method syntax (query syntax briefly) · filter, project, sort · group, aggregate · deferred execution.
Misconception: the query already runs when it is defined.
- *(new)* Lesson "Extension methods" (first lesson of the module – LINQ operators are extension methods).
- *(new)* Lesson "Iterators": writing your own sequence with an iterator block; this explains deferred execution.
- *(new)* Joins, set operations, quantifiers, element operators and what they do when no element exists.
- *(new)* Anonymous types in projections (vs. tuples and records).
- **Spec:**
  - §15: extension methods (static class, first parameter with `this`, lookup rules: instance methods win); iterators: iterator blocks, enumerable and enumerator objects, lazy execution, yield return and yield break.
  - C# 14 extension members (feature specification): extension blocks with properties and static members.
  - §12: anonymous object creation expressions; query expressions and their syntactic translation into method calls (this is why query syntax and method syntax are the same).
  - §13: the yield statement and its restrictions.
- **Library (not spec):** `IEnumerable<T>` contract, deferred vs. immediate operators, multiple enumeration, `Enumerable` operator families; newer operators from .NET 9/10 only with a badge (check the exact list when writing).
- **Badge:** extension members (C# 14 / .NET 10).
- More misconceptions: `First` returns `null` when nothing matches; a query result is a copy that does not change when the source changes.

### W3 Wiederholung M9–M11 ▶

### M12 Pattern Matching (vertieft, in depth) ▶
Type, property, relational and list patterns · `switch` expressions · `record` with `with`.
- *(new)* Logical patterns (combinators) and parenthesized patterns.
- *(new)* Positional patterns and deconstruction.
- *(new)* "Patterns or polymorphism?" (design choice, not spec).
- **Spec:**
  - §11: all pattern kinds – declaration, type, constant, relational, logical, property (incl. extended property patterns, C# 10), positional, var, discard, list and slice patterns (C# 11); the null test with patterns; subsumption (an unreachable arm is an error) and exhaustiveness (warning, runtime exception).
  - §12: switch expression typing, the is-pattern expression, with expressions – for records, and since C# 10 also for structs and anonymous types (check in the feature spec).
  - Deconstruction: user-defined deconstruct methods and how positional patterns use them.
- More misconceptions: `with` changes the original; the order of switch arms does not matter.

### M13 async/await ▶
`Task` · `async`/`await` · exceptions in asynchronous code · cancellation (`CancellationToken`).
Misconception: `async` starts a new thread automatically. Note: the playground runs on one thread (WebAssembly). True parallelism only as an example that cannot run.
- *(new)* Lesson "Waiting for several tasks" (all, any; timeouts).
- *(new)* Lesson "Async streams" (asynchronous iteration).
- *(new)* Async disposal.
- *(new)* Part "Pitfalls": async void (only for event handlers), blocking on a task (deadlock risk, ◻), I/O-bound vs. CPU-bound work.
- *(new)* Deep dive ◻ "Shared data and locks" (thread safety only as a concept).
- **Spec:**
  - §15: async functions, the allowed return types (task types, void, async iterators), evaluation of an async function (runs synchronously until the first await of an incomplete task).
  - §12: await expressions, the awaitable pattern; an exception in an async method is stored in the task and rethrown at the await.
  - §13: asynchronous foreach and asynchronous using (C# 8); the lock statement (deep dive).
- **Library (not spec):** `Task`/`Task<T>`, `ValueTask` (only by name), `Task.WhenAll`/`WhenAny`, cancellation tokens and their sources, `IAsyncEnumerable<T>`, `IAsyncDisposable`, synchronization context and `ConfigureAwait` (deep dive, library code).
- **Badge:** the `System.Threading.Lock` type with the lock statement (C# 13 / .NET 9, deep dive only).
- More misconceptions: `await` blocks the thread; an exception in an awaited task is lost.

### M14 Testen (testing) ◻
xUnit: `[Fact]`, `[Theory]`, `Assert` · Arrange-Act-Assert · testable code. The test runner output is produced in CI (S2).
- *(new)* Lesson "From the playground to a project" (first lessons of the module): the SDK and the `dotnet` CLI, file-based apps (one `.cs` file, .NET 10) as the bridge from the playground, SDK-style project file (target framework, nullable, implicit usings, warnings as errors), solutions and project references, NuGet package references, build configurations, assemblies (link `internal` from M6).
- *(new)* Lesson "Using a debugger" (breakpoints, stepping, watching variables; tool-neutral, ◻).
- *(new)* Lesson "Attributes" (▶ possible: obsolete warnings, caller info) – needed for `[Fact]`, ASP.NET Core and EF Core.
- *(new)* Lesson "Test doubles": hand-written fakes first; mocking libraries only by name.
- *(new)* Lesson "Testing time-dependent code" (time provider abstraction and its fake; SecLog time windows).
- *(new)* Testing exceptions and async code.
- **Spec:**
  - §22: attribute classes, attribute usage, positional and named parameters, attribute targets, reserved attributes (conditional, obsolete, caller info attributes, attribute usage).
  - §6: preprocessing directives – only conditional compilation symbols (Debug/Release), the nullable directive and the directives of file-based apps; nothing else.
  - §7: assemblies as the unit of deployment and accessibility (`internal`).
- **Library / tooling (not spec):** xUnit.net (check current major version, v3 at the time of writing), test platforms VSTest vs. Microsoft.Testing.Platform (MTP); MSTest, NUnit and TUnit only by name; data-driven tests; `TimeProvider` (.NET 8) and `FakeTimeProvider` (package `Microsoft.Extensions.TimeProvider.Testing`).
- **Badge:** `TimeProvider` (.NET 8); file-based apps (.NET 10 SDK).
- More misconceptions: a test that never fails is a good test; 100 % coverage means no bugs.

### W4 Wiederholung M12–M14 ▶

### M15 Dependency Injection und Hosting ▶
Dependencies through interfaces · `ServiceCollection` · lifetimes · logging · configuration/options. Runnable for DI and logging. Configuration/options and hosting cannot run (missing in the playground, see S6).
- *(new)* Captive dependencies and scope validation (a singleton must not hold a scoped service).
- *(new)* Structured logging (message templates, log levels, categories).
- *(new)* Configuration sources and their order (JSON files, environment variables, command line), environments; secrets do not go into source code (user secrets / environment variables).
- *(new)* Generic host and background services (◻, short).
- **Spec:** no new language content; uses interfaces (M8), generics (M9) and lambdas (M10).
- **Library (not spec):** `Microsoft.Extensions.DependencyInjection` (service lifetimes, constructor injection, service provider), `Microsoft.Extensions.Logging`, `Microsoft.Extensions.Configuration`, options pattern (`IOptions<T>` and its variants), `Microsoft.Extensions.Hosting`.
- **Badge:** keyed services (.NET 8 – check on Microsoft Learn when writing).
- More misconceptions: transient means "disposed right away"; logging with string interpolation is the same as a message template.

### M16 EF Core ◻
`DbContext`, model, relations (1:n, n:m) · LINQ → SQL (shown SQL comes from a CI run) · change tracking · migrations.
- *(new)* Lesson "IQueryable vs. IEnumerable": why a lambda becomes SQL (expression trees); what happens with code that cannot be translated.
- *(new)* Lesson "Loading related data" (explicit include, the N+1 problem).
- *(new)* Tracking vs. no-tracking queries; saving as one unit of work (transactions, short).
- *(new)* Lesson "Raw SQL safely": parameterized queries vs. string concatenation (SQL injection, defensive).
- *(new)* Async queries and the context lifetime in DI (scoped).
- *(new)* Configuration by convention, by data annotations and by fluent API; keys.
- **Spec:**
  - §8/§10: expression tree types – a lambda converted to an expression tree is data, not code (this is the language basis of LINQ to SQL).
  - §12: query expressions again (translated to `Queryable` calls).
- **Library (not spec):** EF Core 10 (LTS, released November 2025, needs .NET 10), provider in CI: SQLite (check the CI project `website/examples/_ci/ef-core`), migrations tooling.
- More misconceptions: `ToList()` at the end changes nothing; EF Core sends every LINQ method to the database.

### M17 ASP.NET Core Minimal APIs ◻
Routing, handlers · binding and validation · status codes · middleware pipeline · DI in endpoints. HTTP requests and responses are produced in CI (S2).
- *(new)* Lesson "HTTP basics" first: methods, status code classes, headers, request/response body, idempotency.
- *(new)* Lesson "JSON with System.Text.Json" (▶ possible – check in the playground): serialization, naming policy, nullable members.
- *(new)* Lesson "Errors as problem details" (standard error format, central error handling).
- *(new)* Lesson "Describing the API with OpenAPI" (built-in document generation since .NET 9).
- *(new)* Lesson "Who may call the endpoint?": authentication vs. authorization as a concept, defensive (SecLog incident API).
- *(new)* Integration tests for endpoints (link M14, CI only).
- *(new)* Cancellation in request handlers (link M13).
- **Spec:** no new language content; uses lambdas (M10), attributes (M14), records (M8), async (M13).
- **Library (not spec):** ASP.NET Core 10 Minimal APIs; built-in validation (`AddValidation`, data annotations, `IValidatableObject`; some validation attributes are experimental in .NET 10); typed results; middleware order; configuration and environments (link M15); `Microsoft.AspNetCore.OpenApi`.
- More misconceptions: validation runs automatically without registration; middleware order does not matter.

### M18 Abschlussprojekt (final project)
Full SecLog: domain logic, LINQ analysis and classifier ▶ automatically checked; EF Core and API parts ◻ as code-review tasks ("Finde 3 Probleme in diesem Endpoint" – find 3 problems in this endpoint, "Welches SQL entsteht?" – which SQL is produced?).
- *(new)* Review checklist that covers the whole course: nullability warnings, exception handling, disposal, async pitfalls, DI lifetimes, SQL injection, input validation, time zones, tests for business rules.

### M19 Programmieren mit KI (programming with AI) ◻/▶
See `concept/ai-module.md`.

## Deliberately not covered

Content of the specification or platform that the course leaves out, and why. "Only by name" means: one sentence and a link, no exercise.

| Content | Spec / area | Reason |
| --- | --- | --- |
| Unsafe code, pointers, fixed buffers, stack allocation into pointers | §23 | Not needed for the end goal; high risk of memory errors; rarely used in application code. |
| `goto` and labeled statements | §13 | Rarely used, makes control flow hard to read; the switch statement rules are enough. Only by name in M2. |
| Dynamic binding (`dynamic`) | §8, §12 | Mainly for COM and dynamic languages interop; works against the static typing the course teaches. |
| `ref struct`, `Span<T>`, ref returns and ref locals, inline arrays, C# 14 implicit span conversions | §16, feature specs | Performance topics. They need a solid model of memory first and are not needed for the end goal. Only by name in M5 ("further topics"). |
| Generic math, static abstract interface members | feature spec (C# 11) | Mainly for library authors; high abstraction level. Deep dive at most in M9. |
| Reflection, source generators, interceptors, partial events and partial constructors (C# 14) | library, feature specs | Infrastructure for frameworks and generators, not for beginners' application code. Attributes are taught (M14), reading them via reflection is not. |
| Interop (P/Invoke, COM), `extern` members, extern alias | §15, §14 | Platform-specific; not needed for cross-platform web and console apps. |
| Preprocessing directives beyond those in M14 (`#region`, `#pragma`, `#line`, …) | §6 | Tool and editor concerns; no effect on program logic. |
| UTF-8 string literals, interpolated string handlers, overload resolution priority, user-defined compound assignment (C# 14), `nameof` of unbound generic types (C# 14) | feature specs | Niche or library-author features; little value for the learning goal. Revisit at the half-yearly review. |
| Multithreading and parallel programming (threads, parallel loops, PLINQ, channels, concurrent collections) | library | The playground runs on one thread (WebAssembly); the topic needs its own course. Only the concept "shared data needs synchronization" in the M13 deep dive. |
| Garbage collector internals, finalization details, memory profiling | runtime | Implementation detail, not specified by the standard (§7 only requires automatic memory management). Only a deep dive in M4/M7. |
| Regular expressions | library | Useful for log parsing, but a separate language. Optional deep dive in M5 at most; SecLog uses string methods. |
| Cryptography, password hashing, writing own security code | library | The SecLog domain is defensive and analytic; writing security primitives yourself is a known source of vulnerabilities. Only the advice "use vetted platform APIs". |
| Native AOT, trimming, containers, cloud deployment | tooling | Deployment is outside the end goal. Only by name in M14. |
| Blazor, MAUI, WPF, WinForms, Unity, gRPC, SignalR | frameworks | Outside the end goal (see top of file). |
| .NET Framework-only technology (AppDomains, remoting, WCF server) | platform | Not available in modern .NET; only mentioned as history in M0. |
| C# 15 / .NET 11 features | – | .NET 11 is an STS release (planned 2026-11-10). The course base is C# 14 / .NET 10 LTS. Review at the next half-yearly review (2027-04). |

## Practice project SecLog

- **Domain (purely defensive):** analyze login logs, detect brute-force patterns, manage incidents. No attack techniques.
- **From M11:** phishing classifier (Naive Bayes in plain C#) on synthetic sample texts.
- **Form:** optional extra exercises at the end of a lesson. New concepts are always shown first in small, self-contained examples.
- **Data:** made-up sample data only. Mark rules like "5 failed attempts in 10 minutes" as a teaching simplification.
- **Path:** M1–M5 console analysis with arrays/collections (from M5 with real timestamps) → M6–M8 domain model (incl. enums for event types and severity) → M7 business rules as exceptions, reading a log file → M11 LINQ analysis + classifier → M14 tests (incl. time windows with a fake time provider) → M16 persistence → M17 incident API → M18 final project.

## Sources of the spec review (as of 2026-10-25)

Primary sources:
- Microsoft Learn, C# language specification (draft, overview and table of contents): https://learn.microsoft.com/dotnet/csharp/specification – updated 2026-05-01.
- Microsoft Learn, C# standard foreword (features added compared with the previous edition): https://learn.microsoft.com/dotnet/csharp/language-reference/language-specification/foreword
- Ecma International, ECMA-334 (latest published edition: 7th, December 2023): https://ecma-international.org/publications-and-standards/standards/ecma-334
- Microsoft Learn, What's new in C# 14: https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14 – updated 2026-09-03.
- Microsoft Learn, The history of C# (features per version): https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-version-history
- Microsoft Learn, C# language versioning: https://learn.microsoft.com/dotnet/csharp/language-reference/configure-language-version – updated 2026-01-20.
- .NET support policy: https://dotnet.microsoft.com/platform/support/policy/dotnet-core
- .NET Blog, ".NET STS releases supported for 24 months" (2025-09-16): https://devblogs.microsoft.com/dotnet/dotnet-sts-releases-supported-for-24-months/
- Microsoft Learn, File-based apps (.NET 10 SDK): https://learn.microsoft.com/dotnet/core/sdk/file-based-apps – updated 2026-09-14.
- Microsoft Learn, What is TimeProvider?: https://learn.microsoft.com/dotnet/standard/datetime/timeprovider-overview – updated 2026-01-20.
- Microsoft Learn, Testing in .NET (frameworks and test platforms): https://learn.microsoft.com/dotnet/core/testing – updated 2026-04-27.
- Microsoft Learn, What's new in EF Core 10: https://learn.microsoft.com/ef/core/what-is-new/ef-core-10.0/whatsnew
- Microsoft Learn, Validation in ASP.NET Core: https://learn.microsoft.com/aspnet/core/validation/overview – updated 2026-10-16.
- .NET Blog, OpenAPI document generation in .NET 9 (2024-11-25): https://devblogs.microsoft.com/dotnet/dotnet9-openapi/

Secondary source:
- Visual Studio Magazine, ".NET 11 RC1 gets go-live support ahead of November launch" (2026-09-14): https://visualstudiomagazine.com/articles/2026/09/14/net-11-rc1-gets-go-live-support-ahead-of-november-launch.aspx

Open questions from the review:
- End of support of .NET 10: 2028-11-14 (support policy) vs. 2028-11-10 (EF Core 10 page).
- Keyed services in .NET 8: only checked in secondary sources; check on Microsoft Learn before writing M15.
- With expressions for structs and anonymous types since C# 10: check in the feature specification before writing M12.
- Playground support for files (M7) and System.Text.Json (M17): check before deciding ▶ or ◻.
