# Module digest – what earlier modules taught

As of 2026-11-02. One section per finished module, written by its module review. A task for module N reads this file instead of the lessons, examples and quiz files of earlier modules (`CLAUDE.md`, "Context budget"). It lists ids and paths, not text. Max. about 40 lines per module.

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
  - Klasse, Methode `Main` → M6 (`m00-l01`)
  - Typen, `int`/`string`/`bool`, `typeof` → M1 (`m00-l02`; taught in `m01-l02`)
  - Variablen, `var`, Division mit ganzen Zahlen, String-Interpolation → M1 (`m00-l05`)
  - `try`/`catch`, Exceptions → M7 (`m00-l05`)
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
  - Member (Mitglied) eines Typs → M6 (`m01-l06`)
  - Exceptions (Ausnahmen) → M7 (`m01-l07`, `m01-l08`)
  - `out` parameter → M3 (`m01-l08`, `m01-l09`)
  - `null`, nullable reference types (`string?`) → M4 (`m01-l09`)
- **SecLog end state:** `website/examples/m01/l09/seclog-event-solution.cs` – prints the profile, sample log event 2 from variables, failure rate, bob's failures and share, a formatted log line, and reads a failure count from console input with `TryParse`.
- **Notes for later modules:**
  - `if` is taught in `m02-l01` (M1 only pointed to it). In M1, `bool` only stores `true`/`false` and the result of `TryParse`.
  - `TryParse` and `out` are used as a recipe. The `bool` result is ignored in the `m01-l09` SecLog solution. Module 3 explains `out`.
  - `var name = Console.ReadLine()` (`m01-l09`) hides that the type can be `string?`. Module 4 must correct this.

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
- **Glossary terms:** selection-statement, boolean-expression, comparison-operator (`m02-l01`), logical-operator, short-circuit-evaluation (`m02-l02`), conditional-operator (`m02-l03`), switch-expression, pattern, discard, relational-pattern, constant-pattern, exhaustive (`m02-l04`), switch-statement, case-guard, fall-through (`m02-l05`), loop, iteration (`m02-l06`), loop-variable, off-by-one-error (`m02-l07`), jump-statement (`m02-l08`), nested-loop (`m02-l09`). `collection` has no `lesson` field yet (belongs to M5).
- **Misconception ids:** m02-l01-else-wrong-if, m02-l02-and-same-as-ampersand, m02-l03-both-branches-evaluated, m02-l04-missing-arm-gives-default, m02-l05-case-falls-through, m02-l06-condition-checked-once, m02-l07-off-by-one, m02-l08-continue-ends-loop, m02-l09-break-leaves-all-loops
- **Open forward references:**
  - `null` (der Wert „kein Wert“) → M4 (`m02-l06`; `Console.ReadLine()` can return `null`)
  - Sammlungen (Arrays, Listen) with `foreach` → M5 (`m02-l07`, plain text, no `<Forward>`)
- **SecLog end state:** `website/examples/m02/l09/seclog-analysis-solution.cs` – the M1 end state plus (each lesson only adds code): alert for many failed logins, `suspicious` flag, singular/plural word, severity (`switch` expression), action per event type (`switch` statement), failure streak read with `while`, per-attempt lines (`for`) and dot count (`foreach`), date part (`break`) and digit count (`continue`), bar chart of failures per user (nested loops).
- **Notes for later modules:**
  - SecLog still holds data in single variables; the bar chart of `m02-l09` takes names and counts from `switch` expressions as a stand-in for arrays (M5).
  - `foreach` is taught only over a `string`. `switch` on `string` uses constant labels; type, property and list patterns are not taught.
  - `Console.ReadLine()` loops (`m02-l06`) compare with `==` and ignore `null`; module 4 must correct this.
