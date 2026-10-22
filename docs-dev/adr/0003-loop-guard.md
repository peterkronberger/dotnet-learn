# 0003 – Schleifenwächter gegen Endlosschleifen

- Status: entschieden (Browser-Prüfung ausstehend)
- Datum: 2026-10-22
- Spike/Issue: S3 / #8

## Frage
Kann Roslyn vor dem Ausführen in jede Schleife und Methode eine Zeitprüfung einfügen, damit Endlosschleifen den Tab nicht dauerhaft blockieren?

## Ergebnis
- `LoopGuardRewriter` (`playground/LoopGuard.cs`, ein `CSharpSyntaxRewriter`) fügt als erste Anweisung ein:
  - in `while`, `do`, `for`, `foreach`: `Loop(line)`; Körper ohne Block werden in einen Block gewickelt.
  - in Methoden, lokale Funktionen, Konstruktoren und Lambdas mit Block: `Method(line)`. Methoden und lokale Funktionen mit Ausdruckskörper (`=> …`) werden dafür in einen Blockkörper umgewandelt.
- Die Wächterklasse und die Exception (`__PlaygroundGuard`, `__PlaygroundTimeoutException`) sind ein eigener, interner Syntaxbaum in der Nutzercode-Assembly; der Editor und Compilermeldungen enthalten sie nicht.
- `Loop` prüft nur bei jedem 256. Aufruf die Uhr (`Stopwatch`); nach 3 s wird die Exception geworfen. `Method` prüft zusätzlich `RuntimeHelpers.TryEnsureSufficientExecutionStack()`, damit endlose Rekursion vor einem Stacküberlauf abbricht.
- Der Runner meldet: „Dein Programm läuft vermutlich endlos – prüfe die Abbruchbedingung in Zeile X.“ Die Zeile ist die der Schleife bzw. Methode im Originalcode.
- **Zeilen und Spalten in Compilermeldungen:** Die Einfügungen enthalten keine Zeilenumbrüche, Spalten können sich aber verschieben. Deshalb wird zuerst die Variante mit Wächter kompiliert; nur wenn sie Fehler oder Warnungen meldet (oder scheitert), wird der Originalcode erneut kompiliert und **dessen** Meldungen angezeigt. Im Normalfall (keine Meldungen) entsteht keine zweite Kompilierung. Scheitert nur die Variante mit Wächter, läuft der Originalcode ohne Wächter.
- **Messung (native .NET 10 auf einem Linux-Runner, nicht in WebAssembly):** `for` mit 1.000.000 Durchläufen: 48 ms gesamt inkl. Kompilierung; `foreach` über 3 Elemente: 45 ms; Programm mit Warnung (zweite Kompilierung): 18 ms. Abbruch: `for (;;) { }` nach 3033 ms, `do … while (true)` nach 3047 ms, endlose Rekursion nach ca. 0,5 s (Abbruch über die Stacktiefe). Der erste Lauf (`while (true)`) dauerte 4398 ms wegen Aufwärmen. **Nicht gemessen:** Zeiten in WebAssembly und die Beispiele aus `website/docs/` einzeln; WebAssembly ist erwartbar langsamer.
- Gemessene Abweichung: Die Fehlerzeile bei endloser Rekursion nennt die Methode, nicht die rekursive Aufrufstelle.

## Entscheidung
Der Schleifenwächter wird eingesetzt. Er gilt vorbehaltlich einer manuellen Prüfung im veröffentlichten Playground (insbesondere Verhalten der Stacktiefenprüfung in WebAssembly).

## Folgen und Grenzen
- Die 3 s zählen ab Programmstart auf Wanduhrzeit. Programme, die mit `await Task.Delay(…)` länger warten und danach Schleifen ausführen, können fälschlich abbrechen; der Playground ist einthreadig und kann Wartezeit nicht abziehen.
- Fängt Nutzercode die Exception mit `catch (Exception)`, wirft die nächste Prüfung erneut.
- Schleifen in Operatoren, Property-Accessors und Ausdruckskörpern anderer Member erhalten keinen Wächter; blockierende Aufrufe wie `Thread.Sleep` oder Endlosschleifen in Bibliotheksaufrufen ebenfalls nicht.
- Der Wächter kostet pro Iteration einen Aufruf mit Zähler.
- Modul 2 ist von S3 aus entsperrt (S4 bleibt offen).

## Quellen
- Roslyn `CSharpSyntaxRewriter`: https://learn.microsoft.com/dotnet/api/microsoft.codeanalysis.csharp.csharpsyntaxrewriter
- `RuntimeHelpers.TryEnsureSufficientExecutionStack`: https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.runtimehelpers.tryensuresufficientexecutionstack
