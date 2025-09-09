using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace SFM.Packages.CodeAnalyzer;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AsyncAwaitCodeFixProvider))]
public class AsyncAwaitCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(DiagnosticDescriptors.NAMING001.Id);

    public sealed override FixAllProvider GetFixAllProvider() =>
        WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var declaration = root.FindToken(diagnosticSpan.Start).Parent;

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Add async modifier",
                createChangedSolution: c => AddAsyncModifierAsync(context.Document, declaration, c),
                equivalenceKey: "Add async modifier"),
            diagnostic);
    }

    private async Task<Solution> AddAsyncModifierAsync(Document document, SyntaxNode declaration, CancellationToken cancellationToken)
    {
        SyntaxNode newDeclaration = null;

        if (declaration is MethodDeclarationSyntax methodDeclaration)
        {
            newDeclaration = methodDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
        }
        else if (declaration is LocalFunctionStatementSyntax localFunction)
        {
            newDeclaration = localFunction.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
        }

        if (newDeclaration == null)
            return document.Project.Solution;

        var root = await document.GetSyntaxRootAsync(cancellationToken);
        var newRoot = root.ReplaceNode(declaration, newDeclaration);

        return document.Project.Solution.WithDocumentSyntaxRoot(document.Id, newRoot);
    }
}