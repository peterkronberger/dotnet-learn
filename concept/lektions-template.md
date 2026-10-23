# Lektions-Template

Jede Lektion folgt dieser Reihenfolge. Optionale Elemente stehen in Klammern. Dauer 15–25 min, ein Kernkonzept. Prüfung gegen die Definition of Done in `CLAUDE.md`.

## Ablauf

| # | Schritt | Form | Hinweis |
| --- | --- | --- | --- |
| 1 | Lernziele | `:::note[Lernziele]`, 2–4 Punkte | Als beobachtbares Verhalten: „Du kannst …“, „Du erkennst …“ |
| 2 | Aufwärmen | 2–3 `<Quiz>` zu früheren Lektionen | Überschrift „Rückblick“; Abrufübung, nicht neuer Stoff |
| 3 | Konzept | Kurzer Text + kleines, isoliertes Beispiel | Nur das aktuelle Konzept; kein Projektkontext |
| 4 | Praxisbeispiel | **Praxisbeispiel-Muster** (unten) | Pflicht, mindestens einmal |
| 5 | Lücken-Beispiel | Beispiel mit Lücken, Teilziel-Beschriftungen bleiben | Fading: Lücken sitzen im Code |
| 6 | Ändern und Selbermachen | Automatisch geprüfte Aufgabe | Automatisch geprüfte Aufgabe mit `<CSharpRunner tests={…}>`; eingeklappte Musterlösung nur als Fallback, z. B. für nicht ausführbare Themen |
| 7 | Fehlvorstellungen | `:::warning[Achtung]`, 1–3 kurze Boxen | Jede Box ist mit einem Quiz-Distraktor verknüpft |
| 8 | (Umsteiger-Box) | Tabs Java / Python / JS/TS / Go | Nur Unterschiede zu C#, nicht C# erneut erklären |
| 9 | (Vertiefung) | `<details>` „Vertiefung: … (optional)“ | Ersetzt „Unter der Haube“; Fachaussagen mit Microsoft-Learn-Quelle |
| 10 | Zusammenfassung | 3–5 Sätze | Keine neuen Inhalte |
| 11 | Abschluss | Quiz (→ Leitner-Pool), „In eigenen Worten erklären“, Transferfrage, (SecLog-Zusatzübung), „Weiterlesen“ | „Weiterlesen“: 1–3 Links auf Microsoft Learn / C#-Spezifikation |

## Praxisbeispiel-Muster (Pflicht in Schritt 4)

Entspricht PRIMM. So ist es im MVP bereits umgesetzt (`website/docs/grundlagen/variablen-und-typen.mdx`, Abschnitt „Zwei Stolperfallen“):

1. **Vorhersage** (`:::info[Vorhersage]`): Aufgabe, vorab zu überlegen, was herauskommt. Optional mit Eingabefeld pro Ausgabezeile; nach dem Ausführen nüchterne Prüfung ✓/✗ (kein „Überraschung“-Wording). Eingabefeld über `<CSharpRunner predict expected={…}>`, `expected` aus dem `.out.txt`-Snapshot per `raw-loader`.
2. **`<CSharpRunner>`** (Run)
3. **`<details>` „Lösung und Erklärung anzeigen“** (Investigate): Ausgabe, dann das *Warum*, gegliedert mit Teilziel-Beschriftungen. Wo passend, eine Frage „Was passiert, wenn …?“.
4. **`<details>` „Vertiefung: … (optional)“**

„Ändern“ und „Selbermachen“ folgen als eigene Aufgaben (Schritt 5/6).

## Teilziel-Beschriftungen

Vorgerechnete Beispiele gliedern die Lösung in benannte Schritte, z. B. bei LINQ: „1. Datenquelle wählen → 2. Filtern → 3. Projizieren → 4. Ergebnis materialisieren“. Die Beschriftungen stehen im Fließtext oder in der Lösung, **nicht** als Code-Kommentar auf Deutsch (Code-Kommentare sind Englisch).

## Gerüst

```mdx
---
title: Ganzzahldivision und Gleitkommazahlen
sidebar_position: 2
description: Warum 7 / 2 in C# 3 ergibt – und wann double und decimal helfen.
lesson_id: m01-l02
duration_min: 20
---

import CodeBlock from '@theme/CodeBlock';
import pitfalls from '!!raw-loader!@site/examples/m01/l02/numeric-pitfalls.cs';

# Ganzzahldivision und Gleitkommazahlen

:::note[Lernziele]
- Du sagst das Ergebnis einer Division zweier `int` korrekt voraus.
:::

## Rückblick
<Quiz id="m01-l01-q1" … />

## Zwei Stolperfallen

:::info[Vorhersage]
Was geben die vier Zeilen aus? Notiere deinen Tipp für jede Zeile und führe den Code dann aus.
:::

<CSharpRunner height={420} code={pitfalls} />

<details>
<summary>Lösung und Erklärung anzeigen</summary>
…
</details>

<details>
<summary>Vertiefung: Wert- und Referenztypen (optional)</summary>
…
</details>

## Weiterlesen
- Microsoft Learn: …
```

Nicht ausführbarer Code erscheint wie ein normaler Codeblock mit Titel. Weil der Code aus einer Datei kommt, wird dafür `<CodeBlock language="csharp" title="…">{code}</CodeBlock>` (offizielle Docusaurus-Komponente) verwendet – die Darstellung ist identisch mit ` ```csharp title="…" `.

## Zusatzübungen SecLog

Neue Konzepte werden **immer zuerst** an kleinen, eigenständigen Beispielen gezeigt. Das Übungsprojekt SecLog kommt danach als optionale Zusatzübung (Schritt 11). Details: `concept/modulplan.md`.
