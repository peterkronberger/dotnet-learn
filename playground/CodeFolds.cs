namespace Playground;

/// <summary>One hidden block: its number (1, 2, ...), the indentation of its start marker, the hidden code and its line count.</summary>
public sealed record Fold(int Index, string Indent, string Code, int LineCount);

/// <summary>The code as the editor shows it (placeholders instead of folds) and the folds.</summary>
public sealed record FoldedCode(string Visible, IReadOnlyList<Fold> Folds)
{
    public bool HasFolds => Folds.Count > 0;
}

/// <summary>Result of putting the folds back. <c>MapLine</c> maps a line of the expanded code to a line of the typed code.</summary>
public sealed record ExpandResult(bool Success, string Code, string? Error, Func<int, int> MapLine);

/// <summary>
/// Hides unchanged parts of a long program. The <c>.cs</c> file marks them with <c>// @fold-start</c> and <c>// @fold-end</c>;
/// the editor shows one placeholder line per fold and the hidden code is put back before compiling.
/// </summary>
public static class CodeFolds
{
    public const string StartMarker = "// @fold-start";
    public const string EndMarker = "// @fold-end";

    private static string Normalize(string code) => code.Replace("\r\n", "\n");

    public static FoldedCode Parse(string code)
    {
        var text = Normalize(code);
        var lines = text.Split('\n');
        var visible = new List<string>();
        var folds = new List<Fold>();
        List<string>? block = null;
        var indent = string.Empty;

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            if (trimmed == StartMarker)
            {
                if (block is not null)
                {
                    return new FoldedCode(text, []); // nested start
                }

                block = [];
                indent = line[..(line.Length - line.TrimStart().Length)];
            }
            else if (trimmed == EndMarker)
            {
                if (block is null)
                {
                    return new FoldedCode(text, []); // end without start
                }

                var fold = new Fold(folds.Count + 1, indent, string.Join('\n', block), block.Count);
                folds.Add(fold);
                visible.Add(indent + Placeholder(fold));
                block = null;
            }
            else if (block is not null)
            {
                block.Add(line);
            }
            else
            {
                visible.Add(line);
            }
        }

        if (block is not null)
        {
            return new FoldedCode(text, []); // start without end
        }

        return folds.Count == 0 ? new FoldedCode(text, []) : new FoldedCode(string.Join('\n', visible), folds);
    }

    public static string Placeholder(Fold fold) => $"// ⋯ Block {fold.Index}: {fold.LineCount} Zeilen ausgeblendet ⋯";

    public static ExpandResult Expand(string typedCode, IReadOnlyList<Fold> folds)
    {
        var typedLines = Normalize(typedCode).Split('\n');
        var byText = folds.ToDictionary(Placeholder);
        var seen = new HashSet<int>();
        var duplicated = new HashSet<int>();
        var expanded = new List<string>();
        var map = new List<int>(); // index = expanded line - 1, value = typed line

        for (var i = 0; i < typedLines.Length; i++)
        {
            if (byText.TryGetValue(typedLines[i].Trim(), out var fold))
            {
                if (!seen.Add(fold.Index))
                {
                    duplicated.Add(fold.Index);
                }

                if (fold.LineCount > 0)
                {
                    foreach (var hidden in fold.Code.Split('\n'))
                    {
                        expanded.Add(hidden);
                        map.Add(i + 1);
                    }
                }
            }
            else
            {
                expanded.Add(typedLines[i]);
                map.Add(i + 1);
            }
        }

        var broken = folds.Select(f => f.Index).Where(k => !seen.Contains(k) || duplicated.Contains(k)).ToList();

        if (broken.Count > 0)
        {
            return new ExpandResult(false, string.Empty,
                $"Der Platzhalter für Block {broken[0]} fehlt oder steht doppelt. „Zurücksetzen“ stellt den Code wieder her.",
                line => line);
        }

        return new ExpandResult(true, string.Join('\n', expanded), null,
            line => map.Count == 0 ? line : map[Math.Clamp(line, 1, map.Count) - 1]);
    }
}
