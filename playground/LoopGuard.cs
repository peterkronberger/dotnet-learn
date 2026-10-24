using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Playground;

/// <summary>
/// Before compiling, inserts a call to the guard class into every loop body and at the start of
/// every method. The insertions contain no line breaks, so line numbers stay the same;
/// columns can shift within the same line (see <see cref="CSharpRunner"/>).
/// </summary>
public sealed class LoopGuardRewriter : CSharpSyntaxRewriter
{
    public const string GuardTypeName = "__PlaygroundGuard";
    public const string ExceptionTypeName = "__PlaygroundTimeoutException";

    /// <summary>Source text of the guard class; compiled as its own syntax tree into the user code assembly.</summary>
    public const string GuardSource = $$"""
        internal sealed class {{ExceptionTypeName}} : System.Exception
        {
            public {{ExceptionTypeName}}(int line) : base("Execution limit reached") { Line = line; }
            public int Line { get; }
        }

        internal static class {{GuardTypeName}}
        {
            private const int CheckInterval = 256;
            private static long _deadline;
            private static int _counter;

            public static void Reset(long timeoutTicks)
            {
                _deadline = System.Diagnostics.Stopwatch.GetTimestamp() + timeoutTicks;
                _counter = 0;
            }

            public static void Loop(int line)
            {
                if (++_counter % CheckInterval == 0 && System.Diagnostics.Stopwatch.GetTimestamp() > _deadline)
                {
                    throw new {{ExceptionTypeName}}(line);
                }
            }

            public static void Method(int line)
            {
                if (!System.Runtime.CompilerServices.RuntimeHelpers.TryEnsureSufficientExecutionStack())
                {
                    throw new {{ExceptionTypeName}}(line);
                }

                Loop(line);
            }
        }
        """;

    private readonly SyntaxTree _tree;

    private LoopGuardRewriter(SyntaxTree tree) => _tree = tree;

    public static SyntaxNode Rewrite(SyntaxTree tree) => new LoopGuardRewriter(tree).Visit(tree.GetRoot());

    private StatementSyntax Call(string method, SyntaxNode at)
    {
        var line = _tree.GetLineSpan(at.Span).StartLinePosition.Line + 1;
        return SyntaxFactory.ParseStatement($"global::{GuardTypeName}.{method}({line});");
    }

    private StatementSyntax GuardLoopBody(StatementSyntax body, SyntaxNode loop)
    {
        var call = Call("Loop", loop);
        return body is BlockSyntax block
            ? block.WithStatements(block.Statements.Insert(0, call))
            : SyntaxFactory.Block(call, body);
    }

    private BlockSyntax GuardBlock(BlockSyntax block, SyntaxNode owner) =>
        block.WithStatements(block.Statements.Insert(0, Call("Method", owner)));

    // Expression bodies become block bodies so that recursion through them is guarded too.
    private BlockSyntax BlockFromExpression(ArrowExpressionClauseSyntax arrow, TypeSyntax returnType, SyntaxNode owner)
    {
        StatementSyntax statement = returnType is PredefinedTypeSyntax { Keyword.RawKind: (int)SyntaxKind.VoidKeyword }
            ? SyntaxFactory.ExpressionStatement(arrow.Expression)
            : SyntaxFactory.ReturnStatement(arrow.Expression);
        return SyntaxFactory.Block(Call("Method", owner), statement);
    }

    public override SyntaxNode? VisitWhileStatement(WhileStatementSyntax node)
    {
        var visited = (WhileStatementSyntax)base.VisitWhileStatement(node)!;
        return visited.WithStatement(GuardLoopBody(visited.Statement, node));
    }

    public override SyntaxNode? VisitDoStatement(DoStatementSyntax node)
    {
        var visited = (DoStatementSyntax)base.VisitDoStatement(node)!;
        return visited.WithStatement(GuardLoopBody(visited.Statement, node));
    }

    public override SyntaxNode? VisitForStatement(ForStatementSyntax node)
    {
        var visited = (ForStatementSyntax)base.VisitForStatement(node)!;
        return visited.WithStatement(GuardLoopBody(visited.Statement, node));
    }

    public override SyntaxNode? VisitForEachStatement(ForEachStatementSyntax node)
    {
        var visited = (ForEachStatementSyntax)base.VisitForEachStatement(node)!;
        return visited.WithStatement(GuardLoopBody(visited.Statement, node));
    }

    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        var visited = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node)!;
        if (visited.Body is not null)
        {
            return visited.WithBody(GuardBlock(visited.Body, node));
        }

        if (visited.ExpressionBody is not null)
        {
            return visited
                .WithBody(BlockFromExpression(visited.ExpressionBody, visited.ReturnType, node))
                .WithExpressionBody(null)
                .WithSemicolonToken(default);
        }

        return visited;
    }

    public override SyntaxNode? VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
    {
        var visited = (LocalFunctionStatementSyntax)base.VisitLocalFunctionStatement(node)!;
        if (visited.Body is not null)
        {
            return visited.WithBody(GuardBlock(visited.Body, node));
        }

        if (visited.ExpressionBody is not null)
        {
            return visited
                .WithBody(BlockFromExpression(visited.ExpressionBody, visited.ReturnType, node))
                .WithExpressionBody(null)
                .WithSemicolonToken(default);
        }

        return visited;
    }

    public override SyntaxNode? VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
    {
        var visited = (ConstructorDeclarationSyntax)base.VisitConstructorDeclaration(node)!;
        return visited.Body is null ? visited : visited.WithBody(GuardBlock(visited.Body, node));
    }

    public override SyntaxNode? VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
    {
        var visited = (ParenthesizedLambdaExpressionSyntax)base.VisitParenthesizedLambdaExpression(node)!;
        return visited.Block is null ? visited : visited.WithBlock(GuardBlock(visited.Block, node));
    }

    public override SyntaxNode? VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
    {
        var visited = (SimpleLambdaExpressionSyntax)base.VisitSimpleLambdaExpression(node)!;
        return visited.Block is null ? visited : visited.WithBlock(GuardBlock(visited.Block, node));
    }
}
