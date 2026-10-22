# Didaktik – Lernmethoden und Belege

Stand: 2026-10-22. Diese Datei legt fest, **welche** Lernmethoden der Kurs nutzt, **warum** (Belege) und **wie** sie umgesetzt werden. Lernende sehen diese Begründung auf der Seite „Wie dieser Kurs funktioniert“ (bisher `website/docs/lernprinzipien.mdx`).

## Zwei Zielgruppen, ein Kernpfad

- **Einsteiger** brauchen ausführliche, vorgerechnete Beispiele. **Umsteiger** werden davon gebremst (Expertise-Reversal-Effekt, [Kalyuga 2003]).
- Deshalb: ein gemeinsamer Kernpfad plus
  - **Einstufungstest pro Modul** (8–12 Fragen). Ab 80 % gilt das Modul als „bekannt“; falsch beantwortete Themen werden als Einzellektionen empfohlen; die Modulfragen landen direkt in einer höheren Leitner-Box. Die 80 % sind eine Festlegung, kein empirischer Wert.
  - **Umsteiger-Boxen** „Kommst du aus …?“ mit Tabs für Java, Python, JavaScript/TypeScript und Go (C++ nur punktuell, z. B. Speicher, `struct`). Die Sprache wird einmal gewählt und lokal gespeichert.
  - **Optionale Vertiefungen** (aufklappbar), damit Einsteiger nicht überladen werden.

## Pflichtmethoden

| Methode | Umsetzung | Beleg |
| --- | --- | --- |
| Vorgerechnete Beispiele mit Ausblenden (Worked Examples → Fading) | Vollständiges Beispiel → Beispiel mit Lücken → freie Aufgabe | [Sweller & Cooper 1985], [Kalyuga 2003] |
| Teilziel-Beschriftungen (Subgoal Labels) | Jedes vorgerechnete Beispiel ist in benannte Teilschritte gegliedert; Lücken-Beispiele behalten die Beschriftungen | [Margulieux 2020], [Margulieux & Catrambone 2016] |
| PRIMM (Predict, Run, Investigate, Modify, Make) | Praxisbeispiel-Muster: Vorhersage → Runner → Lösung und Erklärung → Ändern/Selbermachen | [Sentance 2019] |
| Explizites Vorhersagen | Optionales Eingabefeld pro Ausgabezeile, danach nüchterne Prüfung ✓/✗ | [Brod 2021], [Brod 2022] |
| Abrufübungen (Retrieval Practice) | Aufwärmfragen zu Beginn, Abschlussquiz jeder Lektion | [Roediger & Karpicke 2006], [Dunlosky 2013] |
| Verteilte Wiederholung (Spaced Repetition) | Leitner-System, „Heute fällig“-Dashboard | [Cepeda 2006], [Dunlosky 2013] |

## Ergänzende Methoden

| Methode | Umsetzung | Beleg |
| --- | --- | --- |
| Gemischtes Üben (Interleaving) | Wiederholungsmodule nach etwa 3–4 Modulen; erst erkennen, welches Konzept gefragt ist | [Dunlosky 2013]; Belege überwiegend aus Mathematik, für Programmierung nicht direkt geprüft |
| Parsons-Aufgaben | Codezeilen ordnen (mit Tastatur-Alternative) | Programmierdidaktik-Praxis; Beleg vor Verwendung in der Seite „Wie dieser Kurs funktioniert“ ergänzen |
| Lesen/Nachvollziehen vor Schreiben | Code erst lesen und Ausgabe vorhersagen, dann schreiben; Aufgabe „In eigenen Worten erklären“ | [Lopez 2008]: Nachvollziehen **und** Erklären zusammen sagen Schreibfähigkeit voraus |
| Fehlvorstellungen gezielt adressieren | Box „Achtung“; Quiz-Distraktoren aus typischen Fehlvorstellungen | [Qian & Lehman 2017] |
| Notional Machine | Vereinfachtes, aber korrektes Modell der Ausführung: Speicher-Visualisierungen, Schritt-für-Schritt-Ansicht (wo sinnvoll) | [Sorva 2013], [Guo 2013] |
| Erklärendes Feedback | Jede Quiz-/Testrückmeldung erklärt das *Warum* | [Hattie & Timperley 2007] |
| Erweiterte Compilermeldungen | Deutsche Erklärung häufiger Fehler **zusätzlich** zur Originalmeldung | [Becker 2016]: weniger Fehler und Wiederholungsfehler in Kontrollgruppenstudie |

## Prüfraster: Merrills First Principles

Jede Lektion wird gegen diese fünf Punkte geprüft [Merrill 2002]: echte Aufgabe (SecLog/Klassifikator), Vorwissen aktivieren (Aufwärmen), Vorführen (Beispiel), Anwenden mit Feedback (Übung), Übertragen (Transferfrage).

## Reihenfolge der Inhalte

- „Objekte zuerst“ oder „Objekte später“ ist empirisch weitgehend neutral; Kursziel und Materialqualität zählen mehr [Ehlert & Schulte 2009]. Gewählt: erst prozedural, eigene Klassen ab Modul 6 – Objekte (String-Methoden, `List<T>`) werden aber von Anfang an *genutzt*.
- Typsystem (Wert/Referenz) **vor** Collections, weil Arrays und Listen Referenztypen sind.
- Grundlage: Überblicksarbeit [Luxton-Reilly 2018].

## Grenzen (offen kommunizieren)

- Viele Belege stammen aus Gedächtnis-, Schul- oder Mathematikkontexten. Die Übertragung aufs Programmieren ist plausibel, aber nicht für jede Methode direkt belegt.
- Die Plattform misst ihren eigenen Lernerfolg nicht (kein Tracking).

## KI und Lernen (für den Absatz auf „Wie dieser Kurs funktioniert“)

Übungen zuerst ohne KI lösen. Begründung: In einem RCT schnitten Entwickler, die eine neue Bibliothek mit KI-Hilfe lernten, im Verständnistest schlechter ab als ohne KI [Shen & Tamkin 2026]. Details und neutrale Darstellung: `concept/ki-modul.md`.

## Literatur

Verifiziert (Primärquelle oder Verlags-/Repositoriumseintrag geprüft, Stand 2026-10-22):

- [Becker 2016] Becker, B. A., Glanville, G., Iwashima, R., McDonnell, C., Goslin, K., & Mooney, C. (2016). Effective compiler error message enhancement for novice programming students. *Computer Science Education, 26*(2–3), 148–175.
- [Brod 2021] Brod, G. (2021). Predicting as a learning strategy. *Psychonomic Bulletin & Review*. https://doi.org/10.3758/s13423-021-01904-1
- [Brod 2022] Brod, G., Greve, A., Jolles, D., Theobald, M., & Galeano-Keiner, E. M. (2022). Explicitly predicting outcomes enhances learning of expectancy-violating information. *Psychonomic Bulletin & Review, 29*, 2192–2201. https://doi.org/10.3758/s13423-022-02124-x – Einschränkung: untersucht Faktenlernen.
- [Cepeda 2006] Cepeda, N. J., Pashler, H., Vul, E., Wixted, J. T., & Rohrer, D. (2006). Distributed practice in verbal recall tasks. *Psychological Bulletin, 132*(3), 354–380. https://doi.org/10.1037/0033-2909.132.3.354
- [Dunlosky 2013] Dunlosky, J., Rawson, K. A., Marsh, E. J., Nathan, M. J., & Willingham, D. T. (2013). Improving students' learning with effective learning techniques. *Psychological Science in the Public Interest, 14*(1), 4–58. https://doi.org/10.1177/1529100612453266
- [Ehlert & Schulte 2009] Ehlert, A., & Schulte, C. (2009). Empirical comparison of objects-first and objects-later. *ICER '09*, 15–26.
- [Guo 2013] Guo, P. J. (2013). Online Python Tutor: Embeddable web-based program visualization for CS education. *SIGCSE '13*.
- [Hattie & Timperley 2007] Hattie, J., & Timperley, H. (2007). The power of feedback. *Review of Educational Research, 77*(1), 81–112. https://doi.org/10.3102/003465430298487
- [Kalyuga 2003] Kalyuga, S., Ayres, P., Chandler, P., & Sweller, J. (2003). The expertise reversal effect. *Educational Psychologist, 38*(1), 23–31.
- [Lopez 2008] Lopez, M., Whalley, J., Robbins, P., & Lister, R. (2008). Relationships between reading, tracing and writing skills in introductory programming. *ICER '08*.
- [Luxton-Reilly 2018] Luxton-Reilly, A., Simon, et al. (2018). Introductory programming: A systematic literature review. *ITiCSE 2018 Companion*, 55–106. https://doi.org/10.1145/3293881.3295779
- [Margulieux & Catrambone 2016] Margulieux, L. E., & Catrambone, R. (2016). Improving problem solving with subgoal labels in expository text and worked examples. *Learning and Instruction, 42*, 58–71. https://doi.org/10.1016/j.learninstruc.2015.12.002
- [Margulieux 2020] Margulieux, L. E., Morrison, B. B., & Decker, A. (2020). Reducing withdrawal and failure rates in introductory programming with subgoal labeled worked examples. *International Journal of STEM Education*.
- [Merrill 2002] Merrill, M. D. (2002). First principles of instruction. *Educational Technology Research and Development, 50*(3), 43–59.
- [Qian & Lehman 2017] Qian, Y., & Lehman, J. (2017). Students' misconceptions and other difficulties in introductory programming: A literature review. *ACM Transactions on Computing Education, 18*(1), 1–24.
- [Roediger & Karpicke 2006] Roediger, H. L., & Karpicke, J. D. (2006). Test-enhanced learning. *Psychological Science, 17*(3), 249–255. https://doi.org/10.1111/j.1467-9280.2006.01693.x
- [Sentance 2019] Sentance, S., Waite, J., & Kallia, M. (2019). Teaching computer programming with PRIMM: A sociocultural perspective. *Computer Science Education, 29*(2–3), 136–176. https://doi.org/10.1080/08993408.2019.1608781
- [Shen & Tamkin 2026] Shen, J. H., & Tamkin, A. (2026). How AI impacts skill formation. arXiv:2601.20245 (Anthropic; Interessenkonflikt offenlegen).
- [Sorva 2013] Sorva, J. (2013). Notional machines and introductory programming education. *ACM Transactions on Computing Education, 13*(2). https://doi.org/10.1145/2483710.2483713

Nicht selbst verifiziert (vor Verwendung prüfen):

- [Sweller & Cooper 1985] Sweller, J., & Cooper, G. A. (1985). The use of worked examples as a substitute for problem solving in learning algebra. *Cognition and Instruction, 2*(1), 59–89. – Angaben aus dem MVP; DOI nicht geprüft.
- Aus `lernprinzipien.mdx` (MVP) übernommen, nicht geprüft: Kornell, Hays & Bjork (2009); Sweller (1988); Mayer (2009); Shute (2008, Focus on formative feedback).
