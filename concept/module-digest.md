# Module digest – what earlier modules taught

As of 2026-11-06. One section per finished module, written by its module review. A task for module N reads this file instead of the lessons, examples and quiz files of earlier modules (`CLAUDE.md`, "Context budget"). It lists ids and paths, not text. Max. about 40 lines per module.

## Format

Each module section has these fields:

- **Lessons:** one line per lesson: `lesson_id`, German title, core concept in English.
- **Constructs taught:** keywords, operators, types, members, literal forms and comments, each with the lesson that teaches it. An example of a later module may use them without a `<Forward>` note.
- **Glossary terms:** ids whose `"lesson"` field is in this module.
- **Misconception ids:** all `<Misconception id="…" />` ids of the module.
- **Open forward references:** concepts marked with `<Forward … module="X">` in this module where X is a later module. A target inside the same module is not listed.
- **SecLog end state:** the last solution file of the module and what SecLog can do.
- **Notes for later modules:** optional, max. 3 bullets: decisions a later module must know, and constructs used without explanation.

## M0 Orientierung

- **Lessons:**
  - `m00-l01` Dein erstes Programm – a program is a list of statements that run top to bottom.
  - `m00-l02` C# und .NET: Sprache und Plattform – C# is the language, .NET the platform with runtime and libraries.
  - `m00-l03` Die .NET-Familie – .NET Framework, .NET Core, .NET (5+), Mono and what they are for.
  - `m00-l04` Geschichte und Versionen – LTS and STS releases and their support dates.
  - `m00-l05` Vom Code zum laufenden Programm – compile to IL, JIT, compile errors vs. run-time errors.
- **Constructs taught:** top-level statements (`m00-l01`), statement end `;` (`m00-l01`), string literal `"…"` (`m00-l01`), `Console.WriteLine(…)` and `Console.Write(…)` (`m00-l01`), empty `Console.WriteLine()` (`m00-l03`), method call with arguments such as `Math.Max(3, 5)` and `"…".ToUpper()` as examples of library calls (`m00-l02`).
- **Glossary terms:** top-level-statements, literal, compiler, seclog, statement, console-output, keyword, csharp, method, dotnet, roslyn, clr, bcl, sdk, language-specification, dotnet-framework, dotnet-core, dotnet-standard, mono, ef-core, lts, sts, end-of-support, il, jit, ready-to-run, native-aot, compile-error, runtime-error
- **Misconception ids:** m00-l01-text-without-quotes, m00-l01-statement-order, m00-l02-csharp-is-dotnet, m00-l02-console-is-language, m00-l03-dotnet-core-is-ef-core, m00-l03-framework-is-current, m00-l04-sts-is-preview, m00-l04-newest-has-longest-support, m00-l05-compiles-to-machine-code, m00-l05-compile-error-at-runtime
- **Open forward references:**
  - Typen, `int`/`string`/`bool`, `typeof` → M1 (`m00-l02`; taught in `m01-l02`)
  - Variablen, `var`, Division mit ganzen Zahlen, String-Interpolation → M1 (`m00-l05`)
- **SecLog end state:** `website/examples/m00/l05/seclog-profile-solution.cs` – prints a project profile of 8 lines (purpose, language, platform, variant, version, end of support, execution model).
- **Notes for later modules:**
  - `how-this-course-works.mdx` is not a lesson. No later module depends on it.
  - `m00-l05/statements-before-error.cs` uses `var`, `10 / divisor`, `$"…"`, `try`/`catch (DivideByZeroException ex)` and `ex.Message`. The Forward notes name `var`, division, interpolation, `try`/`catch` and Exceptions, but not `DivideByZeroException` and `ex.Message`.

## M1 Erste Programme, Variablen, Typen

- **Lessons:**
  - `m01-l01` Anweisungen, Kommentare und Literale – comments, escape sequences, verbatim strings, number and char literals.
  - `m01-l02` Variablen und eingebaute Typen – a variable has a fixed type; declaration, assignment, identifiers.
  - `m01-l03` Ganzzahlen, Gleitkommazahlen und decimal – integer, floating-point and decimal types, integer division, round-off error.
  - `m01-l04` Operatoren und Ausdrücke – operators, precedence, compound assignment, `++`/`--`.
  - `m01-l05` Konstanten und implizite Typisierung – `const` and `var`.
  - `m01-l06` Zeichenketten und Interpolation – concatenation, interpolation, format strings, raw strings, `Length`, `ToUpper()`.
  - `m01-l07` Explizite Umwandlungen und Überlauf – implicit and explicit conversions, casts, overflow, `checked`.
  - `m01-l08` Text in Zahlen umwandeln: Parse und TryParse – `int.Parse`, `int.TryParse`.
  - `m01-l09` Konsoleneingabe – `Console.ReadLine()` returns one line of text.
- **Constructs taught:** `//` and `/* */` comments (`m01-l01`), escapes `\t \n \" \\` (`m01-l01`), `@"…"` (`m01-l01`), literals `0x`, `0b`, `_`, `1e3`, `'A'` (`m01-l01`), `int double bool char string` (`m01-l02`), `=` and declaration (`m01-l02`), `true`/`false` (`m01-l02`), `long` with `L`, `decimal` with `m`, `f`/`d` suffixes (`m01-l03`), `+ - * / %` (`m01-l03`), `1.0 / 0` and NaN (`m01-l03`), `Math.Round(x, n)`, `Math.Abs` (`m01-l03`), parentheses (`m01-l04`), `+= -= *= /= %=` (`m01-l04`), `x++`, `++x` (`m01-l04`), `const` (`m01-l05`), `var` (`m01-l05`), `"a" + b` (`m01-l06`), `$"…{x}"`, `{x:F1}` (`m01-l06`), `"""…"""` (`m01-l06`), `.Length`, `.ToUpper()` (`m01-l06`), `(int)x`, `(byte)x` (`m01-l07`), `int.MaxValue`, `int.MinValue` (`m01-l07`), `checked(…)` (`m01-l07`), `Math.Round(x)` (`m01-l07`), `int.Parse`, `double.Parse` (`m01-l08`), `int.TryParse(s, out int n)` as a recipe (`m01-l08`), `Console.ReadLine()` (`m01-l09`)
- **Glossary terms:** comment, escape-sequence, verbatim-string-literal, identifier, declaration, assignment, definite-assignment, variable, type, static-typing, built-in-type, integer-division, integral-type, floating-point-type, operand, decimal-type, numeric-promotion, nan, round-off-error, exception, expression, operator, operator-precedence, compound-assignment, increment-operator, type-inference, constant, constant-expression, implicitly-typed-variable, string-interpolation, string-concatenation, format-string, raw-string-literal, culture, cast, implicit-conversion, explicit-conversion, overflow, checked-context, parsing, console-input
- **Misconception ids:** m01-l01-comment-is-executed, m01-l01-escape-in-verbatim, m01-l02-assignment-is-comparison, m01-l02-variable-remembers-old-values, m01-l03-integer-division-rounds, m01-l03-double-is-exact, m01-l04-postfix-returns-new-value, m01-l04-left-to-right-always, m01-l05-var-is-dynamic, m01-l05-const-from-variable, m01-l06-string-plus-number-adds, m01-l06-toupper-changes-variable, m01-l07-cast-rounds, m01-l07-overflow-always-error, m01-l08-parse-accepts-decimals, m01-l08-tryparse-keeps-old-value, m01-l09-readline-reads-all, m01-l09-readline-returns-number
- **Open forward references:**
- **SecLog end state:** `website/examples/m01/l09/seclog-event-solution.cs` – prints the profile, sample log event 2 from variables, failure rate, bob's failures and share, a formatted log line, and reads a failure count from console input with `TryParse`.
- **Notes for later modules:**
  - `if` is taught in `m02-l01` (M1 only pointed to it). In M1, `bool` only stores `true`/`false` and the result of `TryParse`.
  - `TryParse` and `out` are used as a recipe. The `bool` result is ignored in the `m01-l09` SecLog solution. `out` is explained in `m03-l08`.

## M2 Kontrollfluss

- **Lessons:**
  - `m02-l01` Entscheidungen mit if und else – comparisons give `bool`; `if`, `else`, `else if`; the `else` belongs to the nearest `if`.
  - `m02-l02` Bedingungen verknüpfen: logische Operatoren – `&&`, `||`, `!`, precedence, short-circuit, `&`/`|` on `bool`.
  - `m02-l03` Der bedingte Operator – `?:` chooses a value; one branch is evaluated; common type.
  - `m02-l04` switch-Ausdrücke und einfache Muster – `switch` expression with constant, relational and discard patterns.
  - `m02-l05` Die switch-Anweisung – sections, several labels, `when` guard, no fall-through, `default`.
  - `m02-l06` Schleifen mit while und do – `while` checks before, `do` after the body.
  - `m02-l07` Zählschleifen mit for und Durchlaufen mit foreach – `for` header parts, off-by-one, `foreach` over a `string`.
  - `m02-l08` Schleifen steuern: break und continue – leave a loop or skip one iteration.
  - `m02-l09` Verschachtelte Schleifen – inner block runs outer x inner times; `break` leaves only the inner loop.
- **Constructs taught:** `==` `!=` `<` `>` `<=` `>=` (`m02-l01`), `if`/`else`/`else if` and `{ }` blocks (`m02-l01`), `&&` `||` `!` and short-circuit (`m02-l02`), `&` `|` on `bool` (`m02-l02`), `?:` (`m02-l03`), `switch` expression with constant, `<`-relational and `_` patterns (`m02-l04`), `switch` statement with `case`, `default`, several labels, `when` (`m02-l05`), `while` (`m02-l06`), `do … while` (`m02-l06`), `for` (`m02-l07`), `foreach` over a `string` (`m02-l07`), `break` (`m02-l05`, `m02-l08`), `continue` (`m02-l08`), nested loops (`m02-l09`).
- **Glossary terms:** selection-statement, boolean-expression, comparison-operator (`m02-l01`), logical-operator, short-circuit-evaluation (`m02-l02`), conditional-operator (`m02-l03`), switch-expression, pattern, discard, relational-pattern, constant-pattern, exhaustive (`m02-l04`), switch-statement, case-guard, fall-through (`m02-l05`), loop, iteration (`m02-l06`), loop-variable, off-by-one-error (`m02-l07`), jump-statement (`m02-l08`), nested-loop (`m02-l09`). `collection` has `lesson` `m05-l04`.
- **Misconception ids:** m02-l01-else-wrong-if, m02-l02-and-same-as-ampersand, m02-l03-both-branches-evaluated, m02-l04-missing-arm-gives-default, m02-l05-case-falls-through, m02-l06-condition-checked-once, m02-l07-off-by-one, m02-l08-continue-ends-loop, m02-l09-break-leaves-all-loops
- **Open forward references:**
- **SecLog end state:** `website/examples/m02/l09/seclog-analysis-solution.cs` – the M1 end state plus (each lesson only adds code): alert for many failed logins, `suspicious` flag, singular/plural word, severity (`switch` expression), action per event type (`switch` statement), failure streak read with `while`, per-attempt lines (`for`) and dot count (`foreach`), date part (`break`) and digit count (`continue`), bar chart of failures per user (nested loops).
- **Notes for later modules:**
  - SecLog still holds data in single variables; the bar chart of `m02-l09` takes names and counts from `switch` expressions as a stand-in for arrays (M5).
  - `foreach` is taught only over a `string`. `switch` on `string` uses constant labels; type, property and list patterns are not taught.

## M3 Methoden

- **Lessons:**
  - `m03-l01` Methoden definieren und aufrufen – a declaration runs nothing; a call jumps into the body; parameter vs. argument; arguments are evaluated left to right.
  - `m03-l02` return oder Ausgabe? – `return` hands a value to the caller, `Console.WriteLine` prints; `void`; early return; expression body `=>`.
  - `m03-l03` Gültigkeitsbereich und Wertparameter – scope and declaration space; a value parameter is a copy.
  - `m03-l04` Lokale Funktionen – a method inside a method; capture; `static` local functions; declaration order does not matter.
  - `m03-l05` Methoden überladen – signature, overloading, simple overload resolution; the return type is not part of the signature.
  - `m03-l06` Optionale und benannte Argumente – default values are constants; named arguments in any order.
  - `m03-l07` Beliebig viele Argumente: params – parameter array, must be last; `params` collections (C# 13).
  - `m03-l08` ref, out und in – reference, output and input parameters; `out` must be assigned before return.
  - `m03-l09` Tupel: mehrere Werte zurückgeben – tuple types, element names, deconstruction; tuples are value types.
  - `m03-l10` Rekursion und der Aufrufstapel – base case, recursive case, call stack, stack overflow; recursion vs. loop.
- **Constructs taught:** method declaration with parameters and return type, call, argument order, `void` (`m03-l01`), `return`, `=>` expression body (`m03-l02`), scope, value parameters (`m03-l03`), local functions, `static` local functions (`m03-l04`), overloads (`m03-l05`), default values, `name: value` arguments (`m03-l06`), `params` (`m03-l07`), `ref` / `out` / `in`, `out int x`, `out _` (`m03-l08`), tuple types `(T A, T B)`, tuple literals, `var (a, b) = …` (`m03-l09`), recursive calls (`m03-l10`).
- **Glossary terms:** parameter, argument, return-type, void, method-call, return-value, return-statement, expression-body, scope, local-variable, value-parameter, declaration-space, local-function, static-local-function, capture, signature, overloading, overload-resolution, optional-parameter, default-argument, named-argument, parameter-array, reference-parameter, output-parameter, input-parameter, tuple, tuple-element, deconstruction, recursion, base-case, call-stack, stack-overflow
- **Misconception ids:** m03-l01-declaration-runs-code, m03-l02-return-prints, m03-l03-method-changes-caller-variable, m03-l04-declare-before-call, m03-l05-return-type-overloads, m03-l06-default-from-variable, m03-l07-params-not-last, m03-l08-out-may-stay-unassigned, m03-l09-tuple-is-reference-type, m03-l10-no-base-case
- **Open forward references:**
- **SecLog end state:** `website/examples/m03/l10/seclog-methods-solution.cs` – the M2 end state split into methods: check a line, read the user (`TryReadUser` with `out`), return user and IP together as a tuple (`ReadUserAndIp`), overloaded report lines, optional IP argument, static local helper, recursion. Each task file equals the previous solution.
- **Notes for later modules:**
  - Methods in examples are local functions in top-level code. Overloads live in a `static class` (Forward to M6).
  - `TryParse` can now be used with its `bool` result (`m03-l08`).

## M4 Typsystem-Fundamente

- **Lessons:**
  - `m04-l01` Werttypen und Referenztypen – a variable holds a value or a reference; assignment copies the value or the reference; `int` is an alias of `System.Int32`; `object` is the root.
  - `m04-l02` Stack und Heap – ein Modell – stack and heap as a model, not a promise of the language; garbage collector.
  - `m04-l03` Argumente übergeben – two questions: value or reference type, value or `ref` parameter; changing the object vs. replacing the reference.
  - `m04-l04` null und Nullable-Verweistypen – `null` = no object; `string?`, nullable context, null-state warnings, `!`.
  - `m04-l05` Nullable-Werttypen und Standardwerte – `int?`, `HasValue`, `Value`, lifted operators; default values (locals get none).
  - `m04-l06` Null-sichere Operatoren – `?.`, `?[]`, `??`, `??=`, null-conditional assignment (C# 14).
  - `m04-l07` Strings sind unveränderlich – string methods return a new string; `+` vs. `StringBuilder`.
  - `m04-l08` Gleichheit: == und Equals – reference vs. value equality; `string` compares content; `ReferenceEquals`.
  - `m04-l09` Boxing und Unboxing – `object` holds a copy of a value in a new object; unboxing needs the exact type.
- **Constructs taught:** value and reference type, `new StringBuilder(…)` as recipe (`m04-l01`), `ref` with reference types (`m04-l03`), `null`, `string?`, `!` (`m04-l04`), `int?`, `HasValue`, `Value`, default values (`m04-l05`), `?.` `?[]` `??` `??=` (`m04-l06`), `Trim()`, `ToLower()`, `Append` (`m04-l07`), `Equals`, `object.ReferenceEquals` (`m04-l08`), boxing, unboxing cast `(int)obj` (`m04-l09`).
- **Glossary terms:** value-type, reference-type, object, simple-type, string-builder, stack, heap, garbage-collector, automatic-memory-management, pass-by-reference, null, nullable-reference-type, nullable-context, null-state-analysis, null-forgiving-operator, nullable-value-type, lifted-operator, default-value, null-conditional-operator, null-coalescing-operator, null-coalescing-assignment, null-conditional-assignment, immutable, reference-equality, value-equality, boxing, unboxing
- **Misconception ids:** m04-l01-assignment-copies-object, m04-l02-value-types-on-stack, m04-l03-reference-types-passed-by-reference, m04-l04-annotation-prevents-null, m04-l05-local-gets-default, m04-l06-coalesce-always-evaluates, m04-l07-string-method-changes-string, m04-l08-string-equality-compares-references, m04-l09-box-shares-value
- **Open forward references:**
  - Projektdatei (nullable setting) → M14 (`m04-l04`)
- **SecLog end state:** `website/examples/m04/l08/seclog-types-solution.cs` – the M3 end state plus: findings in one `StringBuilder` passed to a method (`l03`), `FindUser` returns `string?` (`l04`), `FailuresOrNull` returns `int?` (`l05`), `??`, `?.`, `??=` for missing values (`l06`), `Trim().ToLower()` for user names (`l07`), `==`/`Equals`/`ReferenceEquals` (`l08`). `l02` and `l09` have no step. Chain: `m03/l10` → `m04/l01` → `l03` → `l04` → `l05` → `l06` → `l07` → `l08`.
- **Notes for later modules:**
  - `StringBuilder` is the only mutable reference type used so far. Examples compile without nullable warnings.
  - `Console.ReadLine()` is `string?`; pass it on only after a null check or with `??`.

## M5 Arrays, Strings, Collections

- **Lessons:**
  - `m05-l01` Arrays – fixed length, index from 0, `Length`, `for`/`foreach`; index out of range.
  - `m05-l02` Mehrdimensionale und verzweigte Arrays – `int[,]` (rectangular) vs. `int[][]` (jagged).
  - `m05-l03` Index vom Ende und Bereiche – `^n`, `a..b` (end exclusive), `Index`/`Range`.
  - `m05-l04` Listen: List<T> – growing list, `Add`, `Insert`, `Remove`, `IndexOf`, `Count`; a list is a reference type.
  - `m05-l05` Sammlungen durchlaufen und ändern – enumerator, no change during `foreach`, remove backwards.
  - `m05-l06` Wörterbücher: Dictionary<TKey,TValue> – key to value, `Add`, indexer, `TryGetValue`, `ContainsKey`; key equality (→ M8).
  - `m05-l07` Mengen: HashSet<T> – no duplicates, `Add` returns `bool`, set operations.
  - `m05-l08` Collection-Ausdrücke und Initialisierer – `[1, 2, 3]`, spread `..`, object/collection initializers (C# 12).
  - `m05-l09` Welche Sammlung wann? – `Queue<T>`, `Stack<T>`, `IReadOnlyList<T>`, cost of `Contains`.
  - `m05-l10` Mit Strings arbeiten – `Split`, `Join`, `IndexOf`, `Replace`, `StringComparison`, ordinal vs. culture.
  - `m05-l11` Datum und Uhrzeit – `DateTimeOffset`, `TimeSpan`, `DateOnly`, `TimeOnly`, UTC; time zones as a warning.
- **Constructs taught:** `T[]`, `new T[n]`, `{ … }` initializer, `Length`, `[i]` (`m05-l01`), `T[,]`, `T[][]`, `GetLength` (`m05-l02`), `^n`, `..`, `Index`, `Range` (`m05-l03`), `List<T>` and its members (`m05-l04`), enumerator, `GetEnumerator`, `MoveNext` (`m05-l05`), `Dictionary<TKey,TValue>`, `TryGetValue` (`m05-l06`), `HashSet<T>`, `UnionWith`, `IntersectWith`, `ExceptWith` (`m05-l07`), collection expressions, `..` spread, object and collection initializers (`m05-l08`), `Queue<T>`, `Stack<T>`, `IReadOnlyList<T>` (`m05-l09`), `Split`, `Join`, `Contains`, `StartsWith`, `StringComparison` (`m05-l10`), `DateTimeOffset`, `TimeSpan`, `DateOnly`, `TimeOnly`, `DateTimeOffset.Parse(text, CultureInfo.InvariantCulture)` (`m05-l11`). `Span<T>` and `ref struct` are named only.
- **Glossary terms:** collection (`m05-l04`); see the `"lesson": "m05-l…"` fields: array-initializer, bounds-check, rank, multi-dimensional-array, jagged-array, index-from-end, list, type-argument, enumerator, iteration-variable, dictionary, key, key-value-pair, hash-set, set-operation, collection-expression, spread-element, collection-initializer, target-typed, queue, stack-collection, collection-interface, ordinal-comparison, culture-sensitive-comparison, separator, utc, timestamp, time-span, date-time-offset
- **Misconception ids:** m05-l01-index-starts-at-one, m05-l02-jagged-same-as-multidimensional, m05-l03-range-end-inclusive, m05-l04-insert-overwrites, m05-l05-change-list-in-foreach, m05-l06-missing-key-gives-default, m05-l07-add-duplicate-throws, m05-l08-collection-expression-is-array, m05-l09-contains-same-cost, m05-l10-replace-changes-string, m05-l11-now-for-log-timestamps
- **Open forward references:**
  - `Span<T>`, `ref struct` → named only, not taught
- **SecLog end state:** `website/examples/m05/l11/seclog-collections-solution.cs` – the M4 end state plus: whole sample log in an array/list, failures per user in a `Dictionary`, distinct IPs in a `HashSet`, timestamps as `DateTimeOffset`, alert "5 failed attempts in 10 minutes". New local functions: `PrintAlerts`, `EventTypeOf`, `TimestampOf`. Chain: `m04/l08` → `m05/l01` → … → `m05/l11`. M5 milestone reached.
- **Notes for later modules:**
  - No current time in runnable examples. Parse timestamps with `DateTimeOffset` and `CultureInfo.InvariantCulture`.
  - The order of `HashSet` and `Dictionary` elements is not promised. Do not print them unsorted in outputs.
  - Compare texts with an explicit `StringComparison` (ordinal for logs).

## M6 OOP I

- **Lessons:**
  - `m06-l01` Klassen und Objekte – a class describes a type with fields and methods; `new` creates an object.
  - `m06-l02` Konstruktoren – constructors, `this(…)` chaining, `readonly` fields.
  - `m06-l03` Kapselung und Zugriffsmodifizierer – `private`/`public`/`internal`, accessibility domain, documentation comments.
  - `m06-l04` Eigenschaften – property with `get`/`set` and a backing field, validation in `set`.
  - `m06-l05` Objektinitialisierer und zieltypisiertes `new` – `new T(…) { P = v }`, `new(…)`.
  - `m06-l06` Eigenschaften im Detail – auto-property, `init`, `required`, `field` (C# 14), computed property.
  - `m06-l07` Primärkonstruktoren – `class C(string p)`, capture (C# 12).
  - `m06-l08` Statische Member und statische Klassen – `static` fields/methods/properties/constructors, `static class`.
  - `m06-l09` Namespaces und `using`-Direktiven – `namespace`, `using`, alias, `global using`, implicit usings.
  - `m06-l10` Wo das Programm startet – entry point, top-level statements, `args`, exit code.
- **Constructs taught:** `class`, field, `new T()`, `.` member access, instance method (`m06-l01`), constructor, `this(…)`, `this.x`, `readonly` (`m06-l02`), `private`, `public`, `internal`, `///` with `<summary>`, `<param>`, `<returns>` (`m06-l03`), `get`, `set`, `value`, property (`m06-l04`), object initializer, `new(…)` (`m06-l05`), `{ get; }`, `{ get; set; }`, `init`, `required`, `field`, `=>` property (`m06-l06`), primary constructor (`m06-l07`), `static` member, `static class`, static constructor (`m06-l08`), `namespace`, `using`, alias, `global using` (`m06-l09`), `Main`, `args`, `return` exit code (`m06-l10`). `sealed` and `partial` are named only.
- **Glossary terms:** see the `"lesson": "m06-l…"` fields: class, instance, field, member, instance-method, object-creation, constructor, default-constructor, constructor-chaining, this-access, readonly-field, encapsulation, access-modifier, accessibility-domain, documentation-comment, property, accessor, backing-field, computed-property, object-initializer, auto-property, init-accessor, required-member, field-keyword, primary-constructor, static-member, instance-member, static-class, static-constructor, namespace, fully-qualified-name, using-directive, using-alias, global-using, implicit-usings, entry-point, main-method, command-line-argument, exit-code
- **Misconception ids:** m06-l01-class-declaration-creates-object, m06-l02-readonly-object-immutable, m06-l03-private-per-object, m06-l04-property-is-field, m06-l05-initializer-before-constructor, m06-l06-init-settable-later, m06-l07-primary-parameter-is-property, m06-l08-static-means-unchangeable, m06-l09-using-adds-library, m06-l10-top-level-has-no-main
- **Open forward references:**
  - `with` expression → M12 (`m06-l06`)
  - `async`/`await` → M13 (`m06-l10`)
  - Projects and assemblies, project file, attributes → M14 (`m06-l03`, `m06-l06`, `m06-l09`)
- **SecLog end state:** `website/examples/m06/l10/seclog-classes-solution.cs` – the M5 end state plus: `LogEvent` (events from the sample log, `Describe()`) and `Incident` (failed logins of one user, validated `Status`, `required Source`, static `Count`). Both live in the namespace `SecLog.Model` at the end of the file. Chain: `m05/l11` → `m06/l01` → … → `m06/l10`. M6 milestone reached.
- **Notes for later modules:**
  - New top-level blocks go **before** `return 0;` (since `m06-l10`).
  - M6 rejects invalid values without exceptions (`Add` returns `bool`, `Status` ignores bad values); M7 may change this.

## M7 Exceptions

- **Lessons:**
  - `m07-l01` Exceptions abfangen mit try und catch – `try`/`catch` handles an exception; code after the error in the `try` block is skipped.
  - `m07-l02` Mehrere catch-Klauseln und Ausnahmefilter – catch order (specific first), `when` filter, propagation.
  - `m07-l03` Stack Traces lesen – read a stack trace; runtime vs. compile vs. logic errors (no project step on purpose).
  - `m07-l04` Aufräumen mit finally – `finally` runs with and without an exception.
  - `m07-l05` Exceptions auslösen mit throw – `throw`, `throw;` vs. `throw ex;`, throw expression, guard clauses and throw helpers.
  - `m07-l06` Eigene Exceptions und Geschäftsregeln – own exception class, `InnerException`, business rules in one place.
  - `m07-l07` Exception oder nicht? – exception vs. Try pattern vs. return value; when to catch.
  - `m07-l08` Ressourcen freigeben: using und IDisposable – `using` statement and declaration, `Dispose`, disposal order.
  - `m07-l09` Dateien lesen und schreiben – `System.IO` (`File`, `Path`, streams), `IOException` family.
- **Constructs taught:** `try`, `catch`, `catch (T ex)`, `Message`, common exception types (`m07-l01`), catch order, `when` (`m07-l02`), stack trace (`m07-l03`), `finally` (`m07-l04`), `throw`, `throw;`, throw expression with `??`, `ArgumentNullException.ThrowIfNull` and other throw helpers (`m07-l05`), custom class `: Exception` with `base(…)` (used, inheritance marked), `InnerException` (`m07-l06`), Try pattern `bool TryX(…, out T)` (`m07-l07`), `using (…) { }`, `using T x = …;`, `IDisposable`, `Dispose()` (`m07-l08`), `File.*`, `Path.*`, `StreamReader`, `StreamWriter`, `Directory.CreateTempSubdirectory`, `FileNotFoundException`, `IOException` (`m07-l09`). Finalizers and the dispose pattern: deep dive only (`m07-l08`).
- **Glossary terms:** see the `"lesson": "m07-l…"` fields: try-statement, catch-clause, throw-exception, exception-type, exception-filter, exception-propagation, stack-trace, stack-frame, logic-error, unhandled-exception, finally-block, throw-statement, throw-expression, rethrow, guard-clause, throw-helper, custom-exception, inner-exception, business-rule, try-pattern, swallowing-exceptions, resource, dispose, using-statement, using-declaration, finalizer, file-path, stream, text-encoding, io-exception
- **Misconception ids:** m07-l01-resume-after-error, m07-l02-catch-order-irrelevant, m07-l03-first-line-is-the-bug, m07-l04-finally-only-without-exception, m07-l05-throw-ex-keeps-stack-trace, m07-l06-own-exception-for-every-error, m07-l07-catch-exception-everywhere-is-safe, m07-l08-dispose-frees-memory, m07-l09-writealltext-appends
- **Open forward references:** none (inheritance, `base` and interfaces are taught in M8).
- **SecLog end state:** `website/examples/m07/l09/seclog-exceptions-solution.cs` – the M6 end state plus: `InvalidLogLineException` (line number, `InnerException`), `ParseLine` (throws) and `TryParseLine` (Try pattern), `Incident.Status` throws `ArgumentException` for unknown values, and the sample log is written to a temp file and read with `StreamReader` in `using`; bad lines are reported, not fatal. Chain: `m06/l10` → `m07/l01` → `l02` → `l04` → … → `l09` (`l03` has no step). M7 milestone reached.
- **Notes for later modules:**
  - `Incident.Status` throws now (M6 ignored bad values); `ParseLine` throws, `TryParseLine` returns `false`.
  - File examples create a temp directory, write every file they read, delete it at the end and never print a full path (`.claude/rules/examples.md`).
  - Exceptions in async code are not covered → M13.

## M8 OOP II

- **Lessons:**
  - `m08-l01` Vererbung und Basisklassen – `class D : B`, inherited members, `protected`, `base(…)` constructor call; constructors are not inherited.
  - `m08-l02` Virtuelle Methoden, override und Verdecken – `virtual`/`override`, polymorphism, `new` hiding, `base.M()`.
  - `m08-l03` Abstrakte Klassen und Polymorphie – `abstract` class and members; no object of an abstract class.
  - `m08-l04` Versiegelte Klassen und Member – `sealed` class, `sealed override`, CA1852; sealed is not immutable.
  - `m08-l05` Interfaces – `interface`, implementation, explicit implementation, default interface members (C# 8), composition vs. inheritance.
  - `m08-l06` Typprüfungen und Casts – `is`, `is T x`, `as`, cast, `InvalidCastException`, boxing named, user-defined conversions (short).
  - `m08-l07` Enums – `enum`, underlying type, `[Flags]`, `Enum.Parse`/`TryParse`; any underlying value is valid.
  - `m08-l08` Structs – value semantics, `readonly struct`, `readonly` members, `default`, boxing, no inheritance.
  - `m08-l09` Gleichheit von Objekten – `Equals`/`GetHashCode` contract, `IEquatable<T>`, `operator ==`/`!=`.
  - `m08-l10` Records – `record`, positional record, `record struct`, `readonly record struct`, generated members.
  - `m08-l11` Objekte vergleichen und sortieren – `IComparable<T>`, `IComparer<T>`, `List<T>.Sort`; order vs. equality.
- **Constructs taught:** `: Base`, `protected`, `base(…)` (`m08-l01`), `virtual`, `override`, `new` (hiding), `base.M()` (`m08-l02`), `abstract` (`m08-l03`), `sealed`, `sealed override` (`m08-l04`), `interface`, explicit interface implementation, default interface members (`m08-l05`), `is`, `is T x`, `as`, explicit cast, `implicit operator`/`explicit operator` (`m08-l06`), `enum`, `[Flags]`, `Enum.Parse`, `Enum.TryParse` (`m08-l07`), `struct`, `readonly struct`, `readonly` member, `default` (`m08-l08`), `Equals`, `GetHashCode`, `IEquatable<T>`, `operator ==`/`!=` (`m08-l09`), `record`, `record struct`, `readonly record struct` (`m08-l10`), `IComparable<T>`, `CompareTo`, `IComparer<T>`, `Compare`, `Sort` (`m08-l11`).
- **Glossary terms:** see the `"lesson": "m08-l…"` fields: inheritance, base-class, derived-class, protected-access, base-constructor-call, virtual-member, override, polymorphism, member-hiding, base-access, abstract-class, abstract-member, sealed-class, sealed-member, code-analysis-rule, interface, interface-implementation, explicit-interface-implementation, default-interface-member, composition, type-test, as-operator, reference-conversion, declaration-pattern, enum, enum-member, underlying-type, flags-enum, struct, readonly-struct, readonly-member, equality-contract, hash-code, operator-overloading, record, positional-record, record-struct, compiler-generated-member, comparable, comparer, sort-order
- **Misconception ids:** m08-l01-constructors-are-inherited, m08-l02-new-overrides-method, m08-l03-abstract-class-only-abstract-members, m08-l04-sealed-means-immutable, m08-l05-matching-methods-implement-interface, m08-l06-cast-changes-object, m08-l07-enum-only-named-values, m08-l08-struct-copy-changes-original, m08-l09-equals-without-gethashcode, m08-l10-record-always-immutable, m08-l11-sort-knows-order
- **Open forward references:**
  - More patterns (`m08-l06`), `with` expressions (`m08-l10`) → M12
  - LINQ, `OrderBy` (`m08-l11`) → M11
  - Attributes (`m08-l07`), assemblies (`m08-l04`) → M14
- **SecLog end state:** `website/examples/m08/l11/seclog-oop-solution.cs` – the M7 end state plus: `IDetectionRule`, abstract `DetectionRule`, sealed `FailedLoginRule`/`WatchListRule`/`StatusRule`, `Detector`, enums `EventType`/`Severity`, `readonly struct TimeWindow`, `sealed record SecurityEvent`, `LogEvent` with value equality, `IncidentComparer : IComparer<Incident>`. Chain: `m07/l09` → `m08/l01` → … → `m08/l11`. M8 milestone reached.
- **Notes for later modules:**
  - `LogEvent` keeps its string `EventType`; `SecurityEvent` is the record form. `LogEvent` has value equality since `m08-l09`.
  - `IDetectionRule` and `Detector` are the place for new rules.
  - `Sort` is not stable: outputs must not depend on the order of equal elements.

## M9 Generics

- **Lessons:**
  - `m09-l01` Generische Methoden – `Method<T>`, type parameter vs. type argument, type inference, explicit type arguments; no operators on an unconstrained `T`.
  - `m09-l02` Generische Klassen und konstruierte Typen – `class`/`struct`/`record` with `<T>`, open vs. closed and constructed types, static fields per constructed type.
  - `m09-l03` Einschränkungen für Typparameter – `where` with `class`, `struct`, `notnull`, base class, interface, `new()`; `unmanaged` named only.
  - `m09-l04` Standardwerte und Nullable bei Typparametern – `default(T)`, `default`, `T?` with and without constraint, `out T?` in Try methods.
  - `m09-l05` Generische Sammlungen verstehen – `List<T>`/`Dictionary<TKey,TValue>` as generic classes, `IEnumerable<T>` … `IList<T>`, no boxing in `List<int>`.
  - `m09-l06` Generische Interfaces, Delegates und Varianz – generic interface, `delegate` declaration, `Func`/`Action`/`Predicate` (read only), invariance vs. covariance, `in`/`out` (deepening).
- **Constructs taught:** `<T>` on methods and local functions, explicit type arguments (`m09-l01`), generic class/struct/record, constructed types (`m09-l02`), `where` with `class`/`struct`/`notnull`/base class/interface/`new()` (`m09-l03`), `default(T)`, `default`, `T?`, `out T?` (`m09-l04`), `IEnumerable<T>`, `ICollection<T>`, `IReadOnlyList<T>`, `IList<T>` (`m09-l05`), generic interface, `delegate` declaration, `Func`, `Action`, `Predicate`, `in`/`out` (`m09-l06`).
- **Glossary terms:** see the `"lesson": "m09-l…"` fields: generics, type-parameter, generic-method, generic-type, constructed-type, open-type, closed-type, type-parameter-constraint, constructor-constraint, default-value-expression, default-literal, non-generic-collection, enumerable, generic-interface, delegate, generic-delegate, variance, covariance, contravariance, invariance
- **Misconception ids:** m09-l01-type-inferred-from-return-type, m09-l02-constructed-types-share-statics, m09-l03-unconstrained-t-has-all-members, m09-l04-default-t-is-null, m09-l05-generics-box-value-types, m09-l06-list-object-accepts-list-string
- **Open forward references:**
  - Iterators and `yield` (`m09-l05`) → M11
  - Nullable analysis attributes (`m09-l04`) → M14
- **SecLog end state:** `website/examples/m09/l06/seclog-generics-solution.cs` – the M8 end state plus: `Store<T> : IStore<T>` (`Add`, `Get`, `GetOrDefault`, `TryGet`, `Items`, `Count`), `Counter<TKey>` (`Add`, `CountOf`, `SortedKeys`), generic `PrintEach<T>(title, IEnumerable<T>)`. Stores for events, incidents and rules; counters for users and IPs. Chain: `m08/l11` → `m09/l01` → … → `m09/l06`. M9 milestone reached.
- **Notes for later modules:**
  - Delegates are introduced minimally in `m09-l06` (created from named methods); `m10-l01` to `m10-l08` teach them in full.
  - `Counter<TKey>` and `Store<T>`/`IStore<T>` are the reusable helpers.
  - Generic math and static abstract interface members are not covered.

## M10 Delegates, Lambdas, Events

- **Lessons:**
  - `m10-l01` Delegates und Methodengruppen – delegate type, method group, method group conversion, `Invoke`, `?.Invoke`, compatibility.
  - `m10-l02` Func und Action: Verhalten als Parameter – `Func`, `Action`, `Predicate<T>`, `Comparison<T>`, callback, `List<T>` methods with delegates.
  - `m10-l03` Lambda-Ausdrücke – `=>`, expression and block body, runs when called, `Comparer<T>.Create`.
  - `m10-l04` Lambdas im Detail – natural type, `var` with a lambda, explicit return type, discard parameters, optional and `params` parameters, modifiers on simple parameters.
  - `m10-l05` Closures: erfasste Variablen – captured variables and their lifetime, `for` vs. `foreach` capture, `static` lambda.
  - `m10-l06` Multicast-Delegates – `+=`/`-=` on delegates, invocation list order, return value of the last target, `GetInvocationList`.
  - `m10-l07` Events – `event` vs. public delegate field, field-like event, `add`/`remove` accessors, `?.Invoke`, publisher and subscriber.
  - `m10-l08` Das Standard-Event-Muster – `EventHandler<TEventArgs>`, `EventArgs`, `sender`, `On…` method, unsubscribing.
- **Constructs taught:** method group conversion, `Invoke`, `?.Invoke` (`m10-l01`), `Func`, `Action`, `Predicate<T>`, `Comparison<T>` as parameters (`m10-l02`), lambda with expression or block body, `Comparer<T>.Create` (`m10-l03`), natural type, explicit return type, discard parameters `_`, optional and `params` lambda parameters (C# 12), modifiers on simple parameters (C# 14) (`m10-l04`), captured variables, `static` lambda (`m10-l05`), `+=`/`-=` on delegates, `GetInvocationList` (`m10-l06`), `event`, `add`/`remove` accessors (`m10-l07`), `EventHandler<TEventArgs>`, `EventArgs`, `On…` method (`m10-l08`).
- **Glossary terms:** see the `"lesson": "m10-l…"` fields: method-group, method-group-conversion, delegate-invocation, callback, func-delegate, action-delegate, predicate-delegate, lambda-expression, anonymous-function, expression-lambda, statement-lambda, anonymous-function-conversion, natural-type, discard-parameter, closure, captured-variable, static-anonymous-function, multicast-delegate, invocation-list, delegate-combination, event, field-like-event, event-accessor, publisher, subscriber, event-args, event-handler, standard-event-pattern
- **Misconception ids:** m10-l01-method-group-calls-method, m10-l02-func-last-type-is-parameter, m10-l03-lambda-runs-when-defined, m10-l04-lambda-has-no-type, m10-l05-lambda-captures-value, m10-l06-multicast-returns-all-results, m10-l07-event-raised-from-outside, m10-l08-subscribers-removed-automatically
- **Open forward references:**
  - LINQ (`Where`, `Select`) (`m10-l03`) → M11
  - Asynchronous event handlers (`m10-l08`) → M13
  - Attributes on lambdas → M14, expression trees → M16 (not marked in a lesson; not taught)
- **SecLog end state:** `website/examples/m10/l08/seclog-delegates-solution.cs` – the M9 end state plus: `delegate bool IncidentCheck(Incident)`, `FuncRule : IDetectionRule` (name plus `Func<Incident, bool>`), rules written as lambdas, `FailuresAtLeast(int)` returning a lambda (closure), alert chain with a multicast `Action`, `IncidentMonitor` with `event EventHandler<IncidentFoundEventArgs>? IncidentFound`, `IncidentFoundEventArgs : EventArgs`. Chain: `m09/l06` → `m10/l01` → … → `m10/l08`. M10 milestone reached.
- **Notes for later modules:**
  - `FuncRule` is the place for rules written as lambdas.
  - `IncidentMonitor.IncidentFound` follows the standard event pattern; observers must unsubscribe with `-=`.
  - Lambda results are used with `List<T>` methods only; LINQ starts in M11.

## W1 Wiederholung M1–M4

- **Lessons:**
  - `w01-l01` Code lesen und vorhersagen – mixed reading and tracing set for M1–M4 (quiz ids `w01-l01-q1` … `w01-l01-q28`).
  - `w01-l02` Code ergänzen und schreiben – mixed writing set and final set for M1–M4 (quiz ids `w01-l02-q1` … `w01-l02-q24`).
