# 0002 – Code aus `.cs`-Dateien einbinden

- Status: entschieden
- Datum: 2026-10-22
- Spike/Issue: S1 / #6

## Frage
Lässt sich C#-Code per `raw-loader` in `<CSharpRunner code={…}>` und in nicht ausführbare Codeblöcke (`<CodeBlock>`) einbinden?

## Ergebnis
Ja. Mit `raw-loader` (`^4.0.2`) und dem Import `import x from '!!raw-loader!@site/examples/…/x.cs'` erhält MDX den Dateiinhalt als normalen String.

- Pilot `website/docs/grundlagen/variablen-und-typen.mdx` nutzt keinen Inline-Code mehr; die Beispiele liegen unter `website/examples/m01/l02/`.
- `<CSharpRunner code={x} />` bekommt den String unverändert. `CSharpRunner` kodiert ihn als Base64url (UTF-8); die README-Einschränkung „keine Backticks, Backslashes, `${`“ entfällt, weil der Code nicht mehr in einem JS-Template-Literal steht. Die Datei `special-characters.cs` enthält `${`, Backslashes und einen Backtick als Testfall.
- Nicht ausführbare Blöcke: `<CodeBlock language="csharp" title="…">{x}</CodeBlock>` (`@theme/CodeBlock`). Das ist dieselbe Komponente, die Docusaurus für ` ```csharp title="…" ` verwendet.
- `npm run build` läuft fehlerfrei durch.
- Nicht geprüft: der Lauf von `special-characters.cs` im Browser-Runner (der Playground-Build war nicht Teil dieses Issues, `CSharpRunner` blieb unverändert). Vor dem ersten Inhalt mit solchen Zeichen einmal im veröffentlichten Runner gegenprüfen.

## Entscheidung
Alle C#-Beispiele liegen als `.cs`-Dateien unter `website/examples/mXX/lYY/` und werden per `raw-loader` importiert. Keine Ausweichlösung (eigener Loader, Remark-Plugin) nötig.

## Folgen
- Neuer Inhalt kann Code aus Dateien einbinden (S2 – CI-Prüfung – bleibt offen).
- Die übrigen MVP-Lektionen (`kontrollfluss.mdx`, `intro.mdx`) sind noch nicht umgestellt (Backlog B1); B4 (README) bleibt offen.
- `package-lock.json` wurde bewusst nicht committet (Backlog B7).

## Quellen
- Docusaurus: „Importing code snippets“, https://docusaurus.io/docs/markdown-features/react
- `raw-loader`: https://github.com/webpack-contrib/raw-loader
