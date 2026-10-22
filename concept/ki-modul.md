# Modul 19 – Programmieren mit KI

Stand: 2026-10-22 · nächste Review: 2027-04

## Rahmen

- **Platzierung:** letztes Modul, nach dem Abschlussprojekt. Ergänzend, setzt die Grundlagen voraus. Keine KI-Inhalte in anderen Modulen; einzige Ausnahme: ein Absatz auf „Wie dieser Kurs funktioniert“.
- **Werkzeugneutral:** keine Produktnamen als Basis. Es geht um die Formen *Code-Vervollständigung*, *Chat* und *agentische Assistenten*. Keine Bedienungsanleitungen.
- **Haltung:** neutral und wissenschaftlich fundiert – weder skeptisch noch übermäßig positiv.

## Neutralitäts-Prüfliste (verbindlich für jede Lektion)

1. Jede Aussage zu Produktivität, Lernen oder Sicherheit nennt **Gruppe, Aufgabe, Werkzeug-Generation/Zeitpunkt, Stichprobengröße und Effekt mit Unsicherheit**.
2. Gibt es gegensätzliche Befunde, werden sie **gemeinsam** gezeigt.
3. **Interessenkonflikte** offenlegen (Autoren beim Hersteller des untersuchten Werkzeugs).
4. **Keine wertenden Adjektive** („erschreckend“, „revolutionär“).
5. Strikt kennzeichnen: **Studienbefund** (mit Quelle) oder **Best Practice (nicht studienbasiert)**.
6. **Alter der Studie** nennen; Befunde gelten für eine bestimmte Werkzeug-Generation.

## Studienlage (verifiziert, Stand 2026-10-22)

| Studie | Design / Gruppe | Aufgabe / Werkzeug | Ergebnis | Hinweis |
| --- | --- | --- | --- | --- |
| Peng et al. 2023 | Kontrolliertes Experiment, angeworbene Entwickler | HTTP-Server in JavaScript; GitHub Copilot (Codex-Generation) | 55,8 % schneller (95-%-KI: 21–89 %) | Autoren u. a. Microsoft Research und GitHub; eine eng umrissene Aufgabe |
| Paradis et al. 2025 | RCT, 96 Google-Engineers | Komplexe Enterprise-Aufgabe; drei interne KI-Funktionen (Sommer 2024) | ca. 21 % schneller, großes Konfidenzintervall | Autoren schreiben selbst: nicht ohne Weiteres auf andere Tools/Zeitpunkte übertragbar; interne Google-Tools |
| METR 2025 | RCT, 16 erfahrene Open-Source-Entwickler, 246 Aufgaben in eigenen großen Repos | Werkzeuge Anfang 2025 | 19 % **länger**; Erwartung vorher +24 %, Schätzung nachher +20 %; < 44 % der Vorschläge übernommen; 9 % der Zeit für Prüfen/Aufräumen | Kleine Stichprobe; METR erwartet bei neueren Tools andere Ergebnisse |
| Perry et al. 2023 (CCS) | Nutzerstudie | Sicherheitsrelevante Aufgaben; Assistent auf Basis codex-davinci-002 | In 4 von 5 Aufgaben häufiger unsichere Lösungen, zugleich häufiger für sicher gehalten; wer weniger vertraute und Prompts sorgfältiger formulierte, hatte weniger Schwachstellen | Ältere Modellgeneration |
| Prather et al. 2024 (ICER) | 21 Anfänger, Laborsitzungen (Beobachtung, Interview, Eye-Tracking) | Programmieraufgabe mit GenAI | Bei Lernenden mit Schwierigkeiten verstärkte KI Probleme der Selbsteinschätzung; 11 von 21 zeigten keine solchen Probleme | Qualitativ, kleine Stichprobe |
| Shen & Tamkin 2026 | RCT, 52 Engineers | Unbekannte Python-Bibliothek (Trio) lernen, mit/ohne KI | Verständnistest signifikant schlechter mit KI (Sekundärquelle: ca. 50 % vs. 67 %); Zeitgewinn nicht signifikant; größte Lücke beim Fehlerfinden; gute Ergebnisse bei Nutzung zum Verstehen (Nachfragen, Erklärungen) | Autoren bei Anthropic; kleine Stichprobe, kurzer Zeitraum |

Quellen: Peng et al., arXiv:2302.06590 · Paradis et al., ICSE 2025 SEIP, arXiv:2410.12944 · METR (2025-07-10), „Measuring the Impact of Early-2025 AI on Experienced Open-Source Developer Productivity“ · Perry et al., CCS '23, 2785–2799, https://doi.org/10.1145/3576915.3623157 · Prather et al., ICER 2024, arXiv:2405.17739 · Shen & Tamkin, arXiv:2601.20245.

## Lektionen

| # | Thema | Kerninhalt | Kennzeichnung |
| --- | --- | --- | --- |
| 1 | Realistische Erwartungen | Produktivität hängt von Aufgabe, Erfahrung, Codebasis und Werkzeug ab; das eigene Zeitgefühl ist unzuverlässig (Peng, Paradis, METR gemeinsam) | Studienbefund |
| 2 | Lernen vs. Abgeben | KI zum Verstehen nutzen (Nachfragen, Erklärungen, konzeptionelle Fragen) statt Aufgaben abzugeben | Studienbefund (Shen & Tamkin) |
| 3 | Verifizieren | KI-Code lesen, Ausgabe vorhersagen, testen; veraltete Idiome und erfundene APIs erkennen | Teils Studienbefund (METR: Prüfaufwand), teils Best Practice |
| 4 | Sicherheit | Typische Schwachstellen in KI-Code am SecLog-Kontext; Vertrauen kalibrieren | Studienbefund (Perry) |
| 5 | Kontext geben | Präzise Aufgaben, Hilfsfunktionen mitliefern, Projekt-Anweisungsdateien | Nur „mehr Kontext → weniger Schwachstellen“ ist Studienbefund (Perry); Rest Best Practice |
| 6 | Selbsteinschätzung prüfen | Selbsttest: „Kann ich das ohne KI erklären?“ | Studienbefund (Prather) + Best Practice |
| 7 | Daten und Geheimnisse | Keine Secrets, Zugangsdaten oder personenbezogenen Daten in Prompts | Best Practice (nicht studienbasiert) |

## Übungsformen (ohne echte KI im Browser)

1. **KI-Code-Review** (`find-bug`): statische C#-Antworten mit eingebauten Fehlern oder Schwachstellen.
2. **Prompt verbessern** (`explain`): schwachen Prompt überarbeiten, mit Musterlösung vergleichen, selbst bewerten.
3. **„Stimmt der KI-Vorschlag?“**: Vorhersage, dann im `<CSharpRunner>` ausführen.

## KI-Beispiele

- Konstruiert, aber realistisch. Immer mit `:::note[Typische KI-Antwort (konstruiertes Beispiel)]` kennzeichnen.
- Sie dienen dazu, konkretes Wissen zu vermitteln (z. B. ein erfundener API-Aufruf, eine fehlende Eingabeprüfung).
- Code darin folgt denselben Regeln (Englisch, `.cs`-Datei, CI-geprüft – fehlerhafter Code bewusst als Bruchstück bzw. mit erwarteter Fehlermeldung).

## Aktualität

- Jede Lektion trägt „Stand: YYYY-MM-DD“.
- Halbjährliche Review: Studienlage (neue RCTs, Replikationen), tote Links, Formulierungen gegen die Neutralitäts-Prüfliste.
