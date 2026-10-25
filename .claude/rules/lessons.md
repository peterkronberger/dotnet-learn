---
paths:
  - "website/docs/**/*.mdx"
  - "website/docs/**/*.md"
---

# Rules for writing lessons

- Follow `concept/lesson-template.md` exactly (11 steps, practice-example pattern in step 4).
- Before you finish, check the Definition of Done in `CLAUDE.md` point by point and tick it off in the PR.
- Front matter: `title`, `sidebar_position`, `description`, `lesson_id` (`mXX-lYY`; review modules `wXX-lYY`, see `concept/module-plan.md`), `duration_min`.
- Body text in German, informal "Du" form, matter-of-fact (`concept/style-and-language.md`). Code, identifiers and comments in English.
- Do not write code inline. Import it from `website/examples/mXX/lYY/*.cs`.
- Never type outputs by hand. Take them only from CI snapshots (`<name>.out.txt`, made by `bash tools/check-examples.sh`; rewrite them on purpose with `--update`).
- Code that cannot run: a normal code block with a title, without a "not runnable" note.
- Write learning goals as observable behavior. A task checks every learning goal.
- Quiz questions live in `website/src/data/quiz/de/mXX.json` (format: `website/src/data/quiz/README.md`). In MDX only `<Quiz id="…" />`, no inline quiz.
- Every "Achtung" box (misconception) starts with `<Misconception id="…" />` as its first line and is linked to at least one wrong quiz answer.
- After changing quiz data or misconception boxes, run `node tools/check-quiz.mjs`.
- New technical terms: add an entry to `website/src/data/glossary/de.json` (format in its `README.md`) and link the first use in the lesson with `<Term id="…">`. Then run `node tools/check-quiz.mjs`.
- Features from C# 12 / .NET 8 on: `<Since csharp="…" dotnet="…" />`.
- End with "Weiterlesen" (further reading): 1–3 links to Microsoft Learn or the C# specification. The links must exist.
- Before writing, check that nothing blocks the module: every component the lesson needs is "available" in `concept/components.md` (not "planned"), and no entry under "Open questions" in `concept/module-plan.md` concerns it (see section 5 of `concept/issue-blueprint.md`).
- No AI content outside module 19.
