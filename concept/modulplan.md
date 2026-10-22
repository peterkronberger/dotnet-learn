# Modulplan

Stand: 2026-10-22. 20 Module (0–19) plus 4 gemischte Wiederholungsmodule. Planungsannahme: 60–80 h für Einsteiger (nicht empirisch abgeleitet). Lektionen sind Vorschläge; Reihenfolge innerhalb eines Moduls darf bei der Ausarbeitung angepasst werden, die Modulreihenfolge nicht.

**Endkompetenz:** C#-/.NET-Grundlagen bis OOP, Collections, Generics, LINQ, Exceptions, async/await und Unit-Tests; dazu ASP.NET Core (Minimal APIs) und EF Core. Nicht enthalten: Blazor, Unity.

## Ablage und Benennung

- Ordner: `website/docs/mXX-<slug>/`, Lektionen `lYY-<slug>.mdx`; Beispiele parallel unter `website/examples/mXX/lYY/`.
- Slugs auf Englisch (Docusaurus übersetzt Slugs nicht; so bleiben URLs in allen Sprachen gleich). Titel und Fließtext auf Deutsch.
- `lesson_id` im Front Matter (`m04-l02`) ist der stabile Schlüssel für Quizfragen und Lernfortschritt.

## Bestehender MVP → Zielstruktur

| MVP-Datei | Ziel |
| --- | --- |
| `docs/intro.mdx` | M0 · L1 „Dein erstes Programm“ (Bezeichner/Kommentare auf Englisch umstellen) |
| `docs/lernprinzipien.mdx` | M0 · Seite „Wie dieser Kurs funktioniert“ (um Literatur aus `didaktik.md` und KI-Absatz ergänzen) |
| `docs/grundlagen/variablen-und-typen.mdx` | M1 · L2/L3 (Code nach `examples/`, Englisch, Lösungstext zu `3.5` korrigieren – der Playground nutzt `InvariantGlobalization`, die Ausgabe ist immer `3.5`) |
| `docs/grundlagen/kontrollfluss.mdx` | M2 (aufteilen in mehrere Lektionen) |

## Module

Legende: ▶ ausführbar im Playground · ◻ nicht ausführbar (normaler Codeblock) · 🔒 Spike muss entschieden sein

### M0 Orientierung ▶
1. Dein erstes Programm (sofort ausführen)
2. C# vs. .NET: Sprache vs. Plattform – Laufzeit (CLR), Standardbibliothek (BCL), SDK, Compiler (Roslyn)
3. Die .NET-Familie: .NET Framework (nur Windows, keine Weiterentwicklung), .NET Core (plattformübergreifend, Open Source), ab .NET 5 „.NET“; kurz Mono/Xamarin, .NET Standard. **Namensklärung**: „.NET Core“ ≠ „Entity Framework Core“; „.NET Framework Core“ gibt es nicht.
4. Historie und Versionen: Zeitleiste, jährlicher Release-Rhythmus (November), LTS (gerade Versionen, 3 Jahre) vs. STS, Support-Dauer. Mit „Stand“-Datum; halbjährliche Review.
5. Vom Code zum laufenden Programm: Compiler → IL → JIT → CLR (mittlere Tiefe, kein Maschinencode)
- Seite „Wie dieser Kurs funktioniert“: Methoden, Grenzen, Bedienung (Leitner, Export/Import), **ein** Absatz zur KI-Nutzung beim Lernen.
- Bereits verifizierte Fakten für L4: C#-/.NET-Zuordnung C# 9/.NET 5, 10/.NET 6, 11/.NET 7, 12/.NET 8, 13/.NET 9, 14/.NET 10 (Roslyn-Wiki); .NET 10 = LTS, erschienen 2025-11-11, Support bis November 2028. Alles Weitere (ältere Jahreszahlen, .NET-Framework-Versionen) beim Schreiben gegen Microsoft Learn prüfen.

### M1 Erste Programme, Variablen, Typen ▶
Anweisungen und Ausgabe · Variablen und eingebaute Typen · Ganzzahl, Gleitkomma, `decimal` · Strings und Interpolation (Objekte *nutzen*: `Length`, `ToUpper`) · Typumwandlung und `Parse` · Konsoleneingabe (über das Eingabefeld des Runners).
Erwartete Fehlvorstellungen: `=` als Vergleich; Ganzzahldivision rundet; `"3" + 4` ergibt 7; eine Variable „merkt sich“ frühere Werte.

### M2 Kontrollfluss ▶ 🔒 S3, S4
`if`/`else`, Vergleichs- und Logikoperatoren · `switch`-Ausdrücke und einfache Muster · `while`/`do` · `for`/`foreach` · `break`/`continue`.
Fehlvorstellungen: Off-by-one; die Schleifenbedingung wird nur einmal geprüft; `else` gehört zum falschen `if`.

### M3 Methoden ▶
Definition, Parameter, Rückgabe · `return` vs. Ausgabe · Gültigkeitsbereich · Überladung · lokale Funktionen · optionale/benannte Parameter · `ref`/`out` (kurz).
Fehlvorstellungen: `return` gibt etwas aus; eine Methode ändert die Variable des Aufrufers.

### M4 Typsystem-Fundamente ▶ 🔒 S5
Wert- vs. Referenztypen · Stack/Heap als Modell (als *Modell* kennzeichnen, Laufzeitdetail) · `null`, Nullable Reference Types, `Nullable<T>` · String-Unveränderlichkeit · Gleichheit `==` vs. `Equals` · (Vertiefung: Boxing).
Fehlvorstellungen: Zuweisung kopiert das Objekt; Werttypen liegen immer auf dem Stack.

### W1 Wiederholung M1–M4 ▶ (gemischtes Üben)

### M5 Arrays, Strings, Collections ▶
Arrays · `List<T>` · `Dictionary<TKey,TValue>` · `HashSet<T>` · `StringBuilder` · Iterieren und Ändern.
Fehlvorstellungen: Index beginnt bei 1; Liste während `foreach` ändern.

### M6 OOP I ▶
Klassen und Objekte · Felder und Properties · Konstruktoren · Kapselung · `static`.

### M7 Exceptions ▶
`try`/`catch`/`finally` · `throw` · eigene Exceptions · Geschäftsregeln validieren · `using`/`IDisposable` (kurz).

### M8 OOP II ▶
Interfaces · Vererbung, `abstract`, Polymorphie · `record` · `struct` · Gleichheit von Objekten.

### W2 Wiederholung M5–M8 ▶

### M9 Generics ▶
Generische Methoden und Klassen · Constraints · generische Collections verstehen.

### M10 Delegates, Lambdas, Events ▶
`Func`/`Action` · Lambdas · Closures · Events.
Fehlvorstellung: Lambda erfasst den Wert statt der Variable.

### M11 LINQ ▶
Methodensyntax (Query-Syntax kurz) · Filtern, Projizieren, Sortieren · Gruppieren, Aggregieren · verzögerte Ausführung.
Fehlvorstellung: Die Abfrage läuft schon beim Definieren.

### W3 Wiederholung M9–M11 ▶

### M12 Pattern Matching (vertieft) ▶
Typ-, Property-, Relational-, List-Patterns · `switch`-Ausdrücke · `record` mit `with`.

### M13 async/await ▶
`Task` · `async`/`await` · Exceptions in asynchronem Code · Abbruch (`CancellationToken`).
Fehlvorstellung: `async` startet automatisch einen neuen Thread. Hinweis: Der Playground läuft einthreadig (WebAssembly); echte Parallelität nur als nicht ausführbares Beispiel.

### M14 Testen ◻
xUnit: `[Fact]`, `[Theory]`, `Assert` · Arrange-Act-Assert · testbarer Code. Testrunner-Ausgabe wird in CI erzeugt (S2).

### W4 Wiederholung M12–M14 ▶

### M15 Dependency Injection und Hosting ▶ 🔒 S6
Abhängigkeiten über Interfaces · `ServiceCollection` · Lebensdauern · Logging · Konfiguration/Options. Fallback ohne S6: nicht ausführbar.

### M16 EF Core ◻
`DbContext`, Modell, Beziehungen (1:n, n:m) · LINQ → SQL (gezeigtes SQL aus CI-Lauf) · Change Tracking · Migrationen.

### M17 ASP.NET Core Minimal APIs ◻
Routing, Handler · Bindung und Validierung · Statuscodes · Middleware-Pipeline · DI in Endpunkten. HTTP-Anfragen und -Antworten werden in CI erzeugt (S2).

### M18 Abschlussprojekt
SecLog vollständig: Domänenlogik, LINQ-Analyse und Klassifikator ▶ automatisch geprüft; EF-Core- und API-Teile ◻ als Code-Review-Aufgaben („Finde 3 Probleme in diesem Endpoint“, „Welches SQL entsteht?“).

### M19 Programmieren mit KI ◻/▶
Siehe `concept/ki-modul.md`.

## Übungsprojekt SecLog

- **Domäne (rein defensiv):** Login-Protokolle analysieren, Brute-Force-Muster erkennen, Vorfälle (Incidents) verwalten. Keine Angriffstechniken.
- **Ab M11:** Phishing-Klassifikator (Naive Bayes in reinem C#) auf synthetischen Beispieltexten.
- **Form:** optionale Zusatzübungen am Lektionsende; neue Konzepte immer zuerst in kleinen, eigenständigen Beispielen.
- **Daten:** ausschließlich erfundene Beispieldaten; Regeln wie „5 Fehlversuche in 10 Minuten“ als didaktische Vereinfachung kennzeichnen.
- **Verlauf:** M1–M5 Konsolenauswertung mit Arrays/Collections → M6–M8 Domänenmodell → M7 Geschäftsregeln als Exceptions → M11 LINQ-Auswertung + Klassifikator → M14 Tests → M16 Persistenz → M17 Incident-API → M18 Abschluss.
