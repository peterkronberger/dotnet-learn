# Project SecLog – the one example project

As of 2026-10-28. This file is binding for all lessons, quizzes and issues. SecLog is the **only** example project of the course. It grows in every lesson. There is no second project (the former phishing classifier is the SecLog feature "message rating", see M11).

## Vision

At the end of M18, SecLog is a small, purely defensive tool for security logs:

- It reads login log lines (from text, a file or a database) and rejects invalid lines.
- It counts failures per user and per IP address and finds brute-force patterns ("5 failed attempts in 10 minutes", a teaching simplification).
- It manages incidents (found patterns) with a severity and a status.
- It rates the texts of reported messages as "possible phishing" (**message rating**, German: "Meldungsbewertung"; Naive Bayes in plain C# on made-up texts).
- It has unit tests for its rules and offers an incident API.

No attack techniques are shown. Writing security primitives is out of scope (`concept/module-plan.md`, "Deliberately not covered").

## How the project appears in lessons

- **Announcement.** The first lesson of the course (m00-l01) announces the project and its end goal.
- **Module box.** The first lesson of every module has the box below, right after the learning goals. The bullets (2–4, German) come from the milestone row of the module.

  ```mdx
  :::tip[Projekt: Was SecLog nach diesem Modul kann]
  - <bullet>
  :::
  ```

- **Project step ("Projektschritt").** Every lesson has a section "Projektschritt" (step 6 of the lesson template) with the fixed frame from `concept/lesson-template.md`, section "Project step (Projektschritt)". If a lesson cannot add a real step, the section has one sentence: "In dieser Lektion kommt kein Projektschritt dazu, weil …". It does not invent an unrelated SecLog snippet.
- **New concepts first.** A concept is shown in a small, isolated example (step 3) before the project step uses it.
- **File names.** `website/examples/mXX/lYY/seclog-<name>-task.cs`, `seclog-<name>-task.tests.snippet.cs`, `seclog-<name>-solution.cs` and their `.out.txt` snapshots. `<name>` is chosen per module and stays the same inside the module (for example `profile` in M0).
- **Review modules (W1–W4) and M19** add no step. Their tasks may use the current state of SecLog, but they do not change it.

## Continuity rule

The start code of a project task (`seclog-<name>-task.cs`) is **byte-identical** to the solution file of the previous project step (`seclog-<name>-solution.cs` of the previous lesson that had a step). The new requirement is only in the lesson text ("Neue Anforderung"), not in a code comment.

Check: `diff <previous solution> <new task>` is empty, and `diff <new task> <new solution>` shows only the new requirement. The PR says that the check was done.

Exceptions:

1. The very first step of the course (m00-l01) has no previous solution.
2. When a lesson teaches a concept that restructures code (methods in M3, classes in M6, a project in M14), the start code may be the restructured previous state. "Stand bisher" then says this in one sentence.
3. ◻ modules (M14–M17) continue in the CI projects under `website/examples/_ci/`. The rule applies to their files in the same way.

The first project step of a module starts from the last solution of the previous module.

## Milestones

| Module | After this module SecLog can … | Concepts |
| --- | --- | --- |
| M0 | print its project profile: purpose, language and platform, .NET variant, version and end of support, execution model | statements, `Console.WriteLine`, string literals |
| M1 | show one log event with user, IP and result, compute the failure rate in percent, read a number from text | variables, types, arithmetic, interpolation, `Parse` |
| M2 | rate a number of failed attempts as a severity and go through several events with loops | `if`, `switch`, loops |
| M3 | split the analysis into methods (check a line, read the user, return user and IP together) | methods, parameters, tuples |
| M4 | handle missing values (no user, no IP) safely | value/reference types, nullable, null-safe operators |
| W1 | no new step (review) | – |
| M5 | hold the whole sample log, count failures per user, list distinct IPs, read timestamps, find "5 failed attempts in 10 minutes" | arrays, `List<T>`, `Dictionary`, `HashSet`, date and time |
| M6 | model events and incidents as classes | classes, properties, constructors, namespaces |
| M7 | reject invalid log lines with its own exception and read the log from a file | exceptions, `using`, file I/O |
| M8 | use enums for event type and severity, records for events, and exchangeable detection rules | enums, records, interfaces, inheritance |
| W2 | no new step (review) | – |
| M9 | use generic helpers for counting and storing | generics |
| M10 | plug in detection rules as lambdas and raise an event when an incident is found | delegates, lambdas, events |
| M11 | analyze the log with LINQ (top IPs, failures per hour) and rate reported messages as "possible phishing" (message rating) | LINQ, extension methods, iterators |
| W3 | no new step (review) | – |
| M12 | parse log lines with patterns and update incidents with `with` | pattern matching, records |
| M13 | read several log sources asynchronously with cancellation (part A); analyze many log files in parallel and collect results safely (part B, ◻) | async/await, concurrency |
| M14 | be a real project with unit tests for its rules (incl. time windows with a fake time provider) and be published | projects, xUnit, attributes, publishing |
| W4 | no new step (review) | – |
| M15 | get its services and thresholds through DI, logging and configuration | DI, logging, options |
| M16 | store events and incidents in a database | EF Core |
| M17 | offer an incident API | ASP.NET Core Minimal APIs |
| M18 | be complete: final project with the review checklist | everything |
| M19 | no new step; its security lesson reviews made-up AI-written SecLog code (`concept/ai-module.md`) | – |

## Canonical sample data

All lessons use this one data set. It is made up. The data set starts with M1; module 0 does not use it.

Rules:

- **Users:** `alice`, `bob`, `carol`, `dave`. Made-up names, no real people, no passwords or other credentials.
- **IP addresses** only from the documentation ranges of [RFC 5737](https://www.rfc-editor.org/rfc/rfc5737): `192.0.2.0/24`, `198.51.100.0/24`, `203.0.113.0/24`.
- **Domains** only `example.com` and `example.org` ([RFC 2606](https://www.rfc-editor.org/rfc/rfc2606)). No real brands, no real links.
- **Line format**, timestamps in UTC: `2026-03-02T08:00:12Z login-failed user=bob ip=203.0.113.7`
- **Event types:** `login-success`, `login-failed`, `logout`.
- **Teaching simplification:** rules such as "5 failed attempts in 10 minutes" are simplified for teaching. Real detection needs more context. Lessons say so.

### Log lines

| # | Line | Note |
| --- | --- | --- |
| 1 | `2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10` | normal |
| 2 | `2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7` | brute force, attempt 1 |
| 3 | `2026-03-02T08:02:05Z login-failed user=bob ip=203.0.113.7` | attempt 2 |
| 4 | `2026-03-02T08:03:31Z login-success user=carol ip=198.51.100.23` | normal |
| 5 | `2026-03-02T08:04:02Z login-failed user=bob ip=203.0.113.7` | attempt 3 |
| 6 | `2026-03-02T08:05:15Z login-failed user=bob ip=203.0.113.7` | attempt 4 |
| 7 | `2026-03-02T08:06:48Z login-failed user=bob ip=203.0.113.7` | attempt 5: five failures within 10 minutes (08:01:40 to 08:06:48) |
| 8 | `2026-03-02T08:07:20Z login-success user=dave ip=198.51.100.8` | normal |
| 9 | `2026-03-02T08:15:03Z login-failed user=alice ip=192.0.2.10` | one typing error |
| 10 | `2026-03-02T08:15:30Z login-success user=alice ip=192.0.2.10` | normal |
| 11 | `2026-03-02T08:42:11Z logout user=carol ip=198.51.100.23` | normal |
| 12 | `2026-03-02T09:10:09Z login-failed user=dave ip=198.51.100.8` | one typing error |
| 13 | `2026-03-02T09:10:41Z login-success user=dave ip=198.51.100.8` | normal |
| 14 | `2026-03-02T09:30:00Z logout user=alice ip=192.0.2.10` | normal |

Totals: 14 events, 7 `login-failed` (bob 5, alice 1, dave 1), 5 `login-success`, 2 `logout`.

### Reported messages (M11 message rating)

Made-up texts for the message rating. The rating is the expected result of the teaching example.

| # | Text | Expected rating |
| --- | --- | --- |
| 1 | `Your account is locked. Confirm your password now at http://example.com/verify or it will be deleted.` | possible phishing |
| 2 | `Urgent: unusual sign-in. Open http://example.org/secure and enter your login data within 24 hours.` | possible phishing |
| 3 | `You won a prize! Click http://example.com/claim to get your reward today.` | possible phishing |
| 4 | `The team meeting moves to 10:00 tomorrow. The agenda is in the shared folder.` | harmless |
| 5 | `Reminder: the quarterly report is due on Friday. Please send your part to the team lead.` | harmless |
| 6 | `Thanks for your help with the server update. The maintenance window ended without problems.` | harmless |

## Module 0: project profile ("SecLog-Steckbrief")

Module 0 has no data set. Its project step is one program that prints the profile of SecLog and grows by one group of lines per lesson:

1. L1: purpose of SecLog
2. L2: language and platform
3. L3: .NET variant
4. L4: version and end of support
5. L5: execution model

The exact lines are chosen in the module 0 issue.

## Evidence and limits

- Basis: whole-task learning (4C/ID) [Vandewaetere 2015], real task as the first principle [Merrill 2002], advance organizer [Ausubel 1960].
- The evidence for whole tasks comes from medical and professional education. The evidence for advance organizers is mixed. Transfer to a programming course is plausible but not shown directly.
- Details and references: `concept/didactics.md`.
