---
paths:
  - "website/docs/**/*.mdx"
  - "website/docs/**/*.md"
---

# Rules for writing lessons

- Follow `concept/lesson-template.md` exactly (10 steps, practice-example pattern in step 4).
- Before you finish, check the Definition of Done in `CLAUDE.md` point by point and tick it off in the PR.
- Front matter: `title`, `sidebar_position`, `description`, `lesson_id` (`mXX-lYY`; review modules `wXX-lYY`, see `concept/module-plan.md`), `duration_min`.
- Body text in German, informal "Du" form, matter-of-fact (`concept/style-and-language.md`). Code, identifiers and comments in English.
- Do not write code inline. Import it from `website/examples/mXX/lYY/*.cs`.
- Never type outputs by hand. Take them only from CI snapshots (`<name>.out.txt`, made by `bash tools/check-examples.sh`; rewrite them on purpose with `--update`).
- Do not set a height on `<CSharpRunner>` (no `height`, no `minHeight`); the height is automatic.
- Code that cannot run: a normal code block with a title, without a "not runnable" note.
- Write learning goals as observable behavior. A task checks every learning goal.
- Quiz questions live in `website/src/data/quiz/de/mXX.json` (format: `website/src/data/quiz/README.md`). In MDX only `<Quiz id="…" />`, no inline quiz.
- Quiz options are shown in random order. They must not refer to their position (no "Antwort A", "die erste Option", "beide oben"). Options like "Keine der genannten" need `"pin": "last"` (format: `website/src/data/quiz/README.md`).
- Order first: use only terms and constructs explained in this lesson or before. An unavoidable forward reference gets one `<Forward concepts={[…]} lesson="…" />` (or `module="N"`) at its first place (`concept/lesson-template.md`, "Forward references"). Never target an early-implementation lesson (`m01-l02`, `m01-l03`, `m02-l01`).
- Learn what earlier modules taught from `concept/module-digest.md`; do not read their lessons in full (`CLAUDE.md`, "Context budget (module work)").
- A quiz question must not need a concept that is only marked as a forward reference.
- The glossary `lesson` field is the lesson that introduces the term. `node tools/check-quiz.mjs` checks `<Term>` links and quiz texts against it.
- The PR lists, per example file, every construct used (keywords, operators, types, members, comments) and where it is explained (lesson) or marked (`<Forward>`).
- Every "Achtung" box (misconception) starts with `<Misconception id="…" />` as its first line and is linked to at least one wrong quiz answer.
- After changing quiz data or misconception boxes, run `node tools/check-quiz.mjs`.
- New technical terms: add an entry to `website/src/data/glossary/de.json` (format in its `README.md`) and link the first use in the lesson with `<Term id="…">`. Then run `node tools/check-quiz.mjs`.
- Every lesson has a "Projektschritt" with the fixed frame (`concept/lesson-template.md`), or one sentence why not. The start code is byte-identical to the previous solution (`concept/project-seclog.md`, continuity rule). Check it with `diff` and say so in the PR.
- The first lesson of a module starts with the box "Projekt: Was SecLog nach diesem Modul kann" (text from the milestone table).
- SecLog is the only example project. Do not invent other projects or unrelated SecLog snippets.
- Features from C# 12 / .NET 8 on: `<Since csharp="…" dotnet="…" />`.
- Sources (`concept/sources.md`):
  - No attribution phrases such as "laut Microsoft Learn", "die Quelle sagt" or "Microsoft beschreibt". State the fact directly. The forbidden list is in `concept/sources.md`, "Automatic check".
  - No English quotes in German text. Paraphrase in German.
  - At most one inline link per paragraph, on a descriptive noun phrase (never "hier").
  - The PR contains the section "Verification" (table: statement · URL with clause or heading · date checked).
- End with "Weiterlesen" (further reading): level 3 of `concept/sources.md` (1–3 links to Microsoft Learn or the C# specification). The links must exist.
- Before writing, check that nothing blocks the module: every component the lesson needs is "available" in `concept/components.md` (not "planned"), and no entry under "Open questions" in `concept/module-plan.md` concerns it (see section 5 of `concept/issue-blueprint.md`).
- No AI content outside module 19.
