# Module 19 – Programming with AI

Note: The course text for learners (MDX lessons) is German; this document is English.

As of: 2026-10-22 · next review: 2027-04

## Frame

- **Placement:** last module, after the final project. It is an add-on and assumes the basics. No AI content in other modules. The only exception is one paragraph on „Wie dieser Kurs funktioniert“ (How this course works).
- **Tool neutral:** no product names as a base. The focus is on the forms *code completion*, *chat*, and *agentic assistants*. No usage manuals.
- **Attitude:** neutral and based on science. Neither skeptical nor too positive.

## Neutrality checklist (mandatory for every lesson)

1. Every statement about productivity, learning, or security names the **group, task, tool generation/time, sample size, and effect with its uncertainty**.
2. If findings conflict, show them **together**.
3. **Disclose conflicts of interest** (authors work at the maker of the tool being studied).
4. **No judging adjectives** („erschreckend“ (shocking), „revolutionär“ (revolutionary)).
5. Label strictly: **study finding** (with source) or **best practice (not based on studies)**.
6. **Name the age of the study.** Findings apply to a specific tool generation.

## State of studies (verified, as of 2026-10-22)

| Study | Design / group | Task / tool | Result | Note |
| --- | --- | --- | --- | --- |
| Peng et al. 2023 | Controlled experiment, recruited developers | HTTP server in JavaScript; GitHub Copilot (Codex generation) | 55.8 % faster (95 % CI: 21–89 %) | Authors include Microsoft Research and GitHub; one narrow task |
| Paradis et al. 2025 | RCT, 96 Google engineers | Complex enterprise task; three internal AI features (summer 2024) | about 21 % faster, wide confidence interval | The authors say themselves: not easy to transfer to other tools or times; internal Google tools |
| METR 2025 | RCT, 16 experienced open-source developers, 246 tasks in their own large repos | Tools from early 2025 | 19 % **longer**; expected +24 % before, estimated +20 % after; < 44 % of suggestions accepted; 9 % of time spent on review and clean-up | Small sample; METR expects other results with newer tools |
| Perry et al. 2023 (CCS) | User study | Security-related tasks; assistant based on codex-davinci-002 | In 4 of 5 tasks, more insecure solutions, and these were more often judged secure; people who trusted less and wrote prompts more carefully had fewer vulnerabilities | Older model generation |
| Prather et al. 2024 (ICER) | 21 beginners, lab sessions (observation, interview, eye tracking) | Programming task with GenAI | For learners who struggled, AI made self-assessment problems worse; 11 of 21 showed no such problems | Qualitative, small sample |
| Shen & Tamkin 2026 | RCT, 52 engineers | Learn an unfamiliar Python library (Trio), with/without AI | Comprehension test significantly worse with AI (secondary source: about 50 % vs. 67 %); time saving not significant; biggest gap in finding errors; good results when used for understanding (follow-up questions, explanations) | Authors at Anthropic; small sample, short period |

Sources: Peng et al., arXiv:2302.06590 · Paradis et al., ICSE 2025 SEIP, arXiv:2410.12944 · METR (2025-07-10), „Measuring the Impact of Early-2025 AI on Experienced Open-Source Developer Productivity“ · Perry et al., CCS '23, 2785–2799, https://doi.org/10.1145/3576915.3623157 · Prather et al., ICER 2024, arXiv:2405.17739 · Shen & Tamkin, arXiv:2601.20245.

## Lessons

| # | Topic | Core content | Label |
| --- | --- | --- | --- |
| 1 | Realistic expectations | Productivity depends on task, experience, codebase, and tool; your own sense of time is not reliable (Peng, Paradis, METR together) | Study finding |
| 2 | Learning vs. handing over | Use AI to understand (follow-up questions, explanations, concept questions) instead of handing over tasks | Study finding (Shen & Tamkin) |
| 3 | Verify | Read AI code, predict the output, test; spot outdated idioms and invented APIs | Partly study finding (METR: review effort), partly best practice |
| 4 | Security | Typical vulnerabilities in AI code in the SecLog context; calibrate trust | Study finding (Perry) |
| 5 | Give context | Precise tasks, include helper functions, project instruction files | Only "more context → fewer vulnerabilities" is a study finding (Perry); the rest is best practice |
| 6 | Check self-assessment | Self-test: "Can I explain this without AI?" | Study finding (Prather) + best practice |
| 7 | Data and secrets | No secrets, credentials, or personal data in prompts | Best practice (not based on studies) |

## Exercise types (without real AI in the browser)

1. **AI code review** (`find-bug`): static C# answers with built-in errors or vulnerabilities.
2. **Improve the prompt** (`explain`): rewrite a weak prompt, compare with a model solution, rate yourself.
3. **"Is the AI suggestion right?"**: predict, then run it in `<CSharpRunner>`.

## AI examples

- Made up, but realistic. Always mark them with `:::note[Typische KI-Antwort (konstruiertes Beispiel)]` (typical AI answer, constructed example).
- They teach concrete knowledge (for example an invented API call, a missing input check).
- Code in them follows the same rules (English, `.cs` file, checked by CI – faulty code on purpose as a snippet, or with the expected error message).

## Currency

- Every lesson carries „Stand: YYYY-MM-DD“ (as of date).
- Twice-yearly review: state of studies (new RCTs, replications), dead links, wording against the neutrality checklist.
