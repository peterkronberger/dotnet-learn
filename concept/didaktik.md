# Didactics – learning methods and evidence

As of 2026-10-22. This file sets **which** learning methods the course uses, **why** (evidence) and **how** they are built. Learners see this reasoning on the page "Wie dieser Kurs funktioniert" (how this course works) (so far `website/docs/lernprinzipien.mdx`).

## Two target groups, one core path

- **Beginners** need detailed worked examples. **Switchers** (people coming from other languages) are slowed down by them (expertise reversal effect, [Kalyuga 2003]).
- So there is one shared core path plus:
  - **A placement test per module** (8–12 questions). At 80 % or more, the module counts as "known". Topics answered wrongly are recommended as single lessons. The module questions go straight into a higher Leitner box. The 80 % is a decision, not an empirical value.
  - **Switcher boxes** "Kommst du aus …?" (do you come from …?) with tabs for Java, Python, JavaScript/TypeScript and Go (C++ only in places, e.g. memory, `struct`). The language is chosen once and stored locally.
  - **Optional deep dives** (collapsible), so beginners are not overloaded.

## Required methods

| Method | Implementation | Evidence |
| --- | --- | --- |
| Worked examples with fading | Full example → example with gaps → free task | [Sweller & Cooper 1985], [Kalyuga 2003] |
| Subgoal labels | Every worked example is split into named steps. Gap examples keep the labels | [Margulieux 2020], [Margulieux & Catrambone 2016] |
| PRIMM (Predict, Run, Investigate, Modify, Make) | Practice-example pattern: prediction → runner → solution and explanation → modify/make it yourself | [Sentance 2019] |
| Explicit prediction | Optional input field per output line, then a sober ✓/✗ check | [Brod 2021], [Brod 2022] |
| Retrieval practice | Warm-up questions at the start, closing quiz in every lesson | [Roediger & Karpicke 2006], [Dunlosky 2013] |
| Spaced repetition | Leitner system, "Heute fällig" (due today) dashboard | [Cepeda 2006], [Dunlosky 2013] |

## Additional methods

| Method | Implementation | Evidence |
| --- | --- | --- |
| Mixed practice (interleaving) | Review modules after about 3–4 modules. First recognize which concept is asked for | [Dunlosky 2013]; evidence mostly from mathematics, not tested directly for programming |
| Parsons problems | Put code lines in order (with a keyboard alternative) | Practice in programming education; add evidence before using it on the page "Wie dieser Kurs funktioniert" |
| Read/trace before writing | First read code and predict the output, then write. Task "In eigenen Worten erklären" (explain in your own words) | [Lopez 2008]: tracing **and** explaining together predict the ability to write |
| Address misconceptions on purpose | Box "Achtung"; quiz distractors taken from typical misconceptions | [Qian & Lehman 2017] |
| Notional machine | Simplified but correct model of execution: memory visualizations, step-by-step view (where useful) | [Sorva 2013], [Guo 2013] |
| Explanatory feedback | Every quiz/test response explains the *why* | [Hattie & Timperley 2007] |
| Enhanced compiler messages | German explanation of common errors **in addition to** the original message | [Becker 2016]: fewer errors and repeated errors in a control-group study |

## Review grid: Merrill's First Principles

Every lesson is checked against these five points [Merrill 2002]: real task (SecLog/classifier), activate prior knowledge (warm-up), demonstrate (example), apply with feedback (exercise), transfer (transfer question).

## Order of content

- "Objects first" or "objects later" is largely neutral in empirical terms. Course goal and quality of material matter more [Ehlert & Schulte 2009]. Chosen: procedural first, own classes from module 6. Objects (string methods, `List<T>`) are still *used* from the start.
- The type system (value/reference) comes **before** collections, because arrays and lists are reference types.
- Basis: overview work [Luxton-Reilly 2018].

## Limits (state them openly)

- Much of the evidence comes from memory, school or mathematics settings. Transfer to programming is plausible but not directly shown for every method.
- The platform does not measure its own learning success (no tracking).

## AI and learning (for the paragraph on "Wie dieser Kurs funktioniert")

Solve exercises without AI first. Reason: in an RCT, developers who learned a new library with AI help did worse on the comprehension test than those without AI [Shen & Tamkin 2026]. Details and neutral presentation: `concept/ki-modul.md`.

## References

Verified (primary source or publisher/repository entry checked, as of 2026-10-22):

- [Becker 2016] Becker, B. A., Glanville, G., Iwashima, R., McDonnell, C., Goslin, K., & Mooney, C. (2016). Effective compiler error message enhancement for novice programming students. *Computer Science Education, 26*(2–3), 148–175.
- [Brod 2021] Brod, G. (2021). Predicting as a learning strategy. *Psychonomic Bulletin & Review*. https://doi.org/10.3758/s13423-021-01904-1
- [Brod 2022] Brod, G., Greve, A., Jolles, D., Theobald, M., & Galeano-Keiner, E. M. (2022). Explicitly predicting outcomes enhances learning of expectancy-violating information. *Psychonomic Bulletin & Review, 29*, 2192–2201. https://doi.org/10.3758/s13423-022-02124-x – Limitation: studies fact learning.
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
- [Shen & Tamkin 2026] Shen, J. H., & Tamkin, A. (2026). How AI impacts skill formation. arXiv:2601.20245 (Anthropic; disclose the conflict of interest).
- [Sorva 2013] Sorva, J. (2013). Notional machines and introductory programming education. *ACM Transactions on Computing Education, 13*(2). https://doi.org/10.1145/2483710.2483713

Not verified by us (check before use):

- [Sweller & Cooper 1985] Sweller, J., & Cooper, G. A. (1985). The use of worked examples as a substitute for problem solving in learning algebra. *Cognition and Instruction, 2*(1), 59–89. – Details taken from the MVP; DOI not checked.
- Taken from `lernprinzipien.mdx` (MVP), not checked: Kornell, Hays & Bjork (2009); Sweller (1988); Mayer (2009); Shute (2008, Focus on formative feedback).
