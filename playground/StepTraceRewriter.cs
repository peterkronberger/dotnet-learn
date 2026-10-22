using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Playground;

/// <summary>
/// Spike S5: inserts a call to the trace hook before every statement of a block and
/// every top-level statement. The call receives the line and the locals declared so far.
/// Like <see cref="LoopGuardRewriter"/> the insertions contain no line breaks, so line numbers stay the same.
/// Tracked: parameters and locals with an initializer, <c>for</c>/<c>foreach</c> variables.
/// Not tracked: pattern/out variables, deconstruction, <c>ref</c> locals, expression-bodied members.
/// </summary>
public sealed class StepTraceRewriter : CSharpSyntaxRewriter
{
    public const string TraceTypeName = "__PlaygroundTrace";

    /// <summary>Line reported for the step after the last top-level statement.</summary>
    public const int EndLine = 0;

    public const string TraceSource = $$"""
        internal static class {{TraceTypeName}}
        {
            public static System.Action<int, string[], object?[]>? Hook;

            public static void Step(int line, string[] names, object?[] values) => Hook?.Invoke(line, names, values);
        }
        """;

    private readonly SyntaxTree _tree;
    private Stack<List<string>> _scopes = new();

    private StepTraceRewriter(SyntaxTree tree) => _tree = tree;

    public static SyntaxNode Rewrite(SyntaxTree tree) => new StepTraceRewriter(tree).Visit(tree.GetRoot())!;

    private StatementSyntax Call(int line)
    {
        var names = _scopes.Reverse().SelectMany(scope => scope).ToList();
        var nameList = string.Join(", ", names.Select(n => $"\"{n}\""));
        var valueList = string.Join(", ", names.Select(n => $"(object?){n}"));
        return SyntaxFactory.ParseStatement(
            $"global::{TraceTypeName}.Step({line}, new string[] {{ {nameList} }}, new object?[] {{ {valueList} }});");
    }

    private int LineOf(SyntaxNode node) => _tree.GetLineSpan(node.Span).StartLinePosition.Line + 1;

    private void Declare(StatementSyntax statement)
    {
        if (statement is not LocalDeclarationStatementSyntax declaration
            || declaration.UsingKeyword != default
            || declaration.Declaration.Type is RefTypeSyntax or ScopedTypeSyntax
            || declaration.Modifiers.Any(m => m.IsKind(SyntaxKind.RefKeyword)))
        {
            return;
        }

        foreach (var variable in declaration.Declaration.Variables.Where(v => v.Initializer is not null))
        {
            _scopes.Peek().Add(variable.Identifier.ValueText);
        }
    }

    // A new function body cannot see the locals of the surrounding top-level code.
    public override SyntaxNode? Visit(SyntaxNode? node)
    {
        ParameterListSyntax? parameters;
        switch (node)
        {
            case BaseTypeDeclarationSyntax:
                parameters = null;
                break;
            case BaseMethodDeclarationSyntax method:
                parameters = method.ParameterList;
                break;
            case LocalFunctionStatementSyntax local:
                parameters = local.ParameterList;
                break;
            default:
                return base.Visit(node);
        }

        var saved = _scopes;
        _scopes = new Stack<List<string>>();
        _scopes.Push(parameters is null ? [] : ParameterNames(parameters));
        try
        {
            return base.Visit(node);
        }
        finally
        {
            _scopes = saved;
        }
    }

    private static List<string> ParameterNames(ParameterListSyntax parameters) =>
        parameters.Parameters
            .Where(p => !p.Modifiers.Any(m => m.Kind() is SyntaxKind.RefKeyword or SyntaxKind.OutKeyword or SyntaxKind.InKeyword or SyntaxKind.ScopedKeyword))
            .Select(p => p.Identifier.ValueText)
            .ToList();

    public override SyntaxNode? VisitCompilationUnit(CompilationUnitSyntax node)
    {
        _scopes = new Stack<List<string>>();
        _scopes.Push([]);

        var members = new List<MemberDeclarationSyntax>();
        var endIndex = -1;

        foreach (var member in node.Members)
        {
            if (member is GlobalStatementSyntax { Statement: var statement } global)
            {
                if (statement is not LocalFunctionStatementSyntax)
                {
                    members.Add(SyntaxFactory.GlobalStatement(Call(LineOf(statement))));
                }

                members.Add((MemberDeclarationSyntax)Visit(global)!);
                Declare(statement);
                if (statement is not LocalFunctionStatementSyntax)
                {
                    endIndex = members.Count;
                }
            }
            else
            {
                members.Add((MemberDeclarationSyntax)Visit(member)!);
            }
        }

        if (endIndex >= 0)
        {
            members.Insert(endIndex, SyntaxFactory.GlobalStatement(Call(EndLine)));
        }

        return node.WithMembers(SyntaxFactory.List(members));
    }

    public override SyntaxNode? VisitBlock(BlockSyntax node)
    {
        _scopes.Push([]);
        var statements = new List<StatementSyntax>();

        foreach (var statement in node.Statements)
        {
            if (statement is not LocalFunctionStatementSyntax)
            {
                statements.Add(Call(LineOf(statement)));
            }

            statements.Add((StatementSyntax)Visit(statement)!);
            Declare(statement);
        }

        _scopes.Pop();
        return node.WithStatements(SyntaxFactory.List(statements));
    }

    // A single embedded statement becomes a block so that it is traced on every iteration.
    // Lines come from the original node: wrapping before visiting would detach it from the original tree.
    private StatementSyntax TraceEmbedded(StatementSyntax original, StatementSyntax visited) =>
        original is BlockSyntax ? visited : SyntaxFactory.Block(Call(LineOf(original)), visited);

    public override SyntaxNode? VisitWhileStatement(WhileStatementSyntax node)
    {
        var visited = (WhileStatementSyntax)base.VisitWhileStatement(node)!;
        return visited.WithStatement(TraceEmbedded(node.Statement, visited.Statement));
    }

    public override SyntaxNode? VisitDoStatement(DoStatementSyntax node)
    {
        var visited = (DoStatementSyntax)base.VisitDoStatement(node)!;
        return visited.WithStatement(TraceEmbedded(node.Statement, visited.Statement));
    }

    public override SyntaxNode? VisitForStatement(ForStatementSyntax node)
    {
        var scope = new List<string>();
        if (node.Declaration is { Type: not RefTypeSyntax } declaration)
        {
            scope.AddRange(declaration.Variables.Where(v => v.Initializer is not null).Select(v => v.Identifier.ValueText));
        }

        _scopes.Push(scope);
        try
        {
            var visited = (ForStatementSyntax)base.VisitForStatement(node)!;
            return visited.WithStatement(TraceEmbedded(node.Statement, visited.Statement));
        }
        finally
        {
            _scopes.Pop();
        }
    }

    public override SyntaxNode? VisitForEachStatement(ForEachStatementSyntax node)
    {
        var name = node.Identifier.ValueText;
        _scopes.Push(name == "_" ? [] : [name]);
        try
        {
            var visited = (ForEachStatementSyntax)base.VisitForEachStatement(node)!;
            return visited.WithStatement(TraceEmbedded(node.Statement, visited.Statement));
        }
        finally
        {
            _scopes.Pop();
        }
    }
}
