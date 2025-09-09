using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SFM.Packages.CodeAnalyzer.Analyzers.Base;
using System.Collections.Immutable;

namespace SFM.Packages.CodeAnalyzer.Analyzers.Naming;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class AsyncNamingAnalyzer : BaseDiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(DiagnosticDescriptors.AsyncMethodMustEndWithAsync);

    protected override void InitializeAnalyzer(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeLocalFunction, SyntaxKind.LocalFunctionStatement);
    }

    private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
    {
        var methodDeclaration = (MethodDeclarationSyntax)context.Node;
        if (IsAsyncMethod(methodDeclaration))
        {
            AnalyzeAsyncMethodName(context, methodDeclaration.Identifier);
        }
    }

    private void AnalyzeLocalFunction(SyntaxNodeAnalysisContext context)
    {
        var localFunction = (LocalFunctionStatementSyntax)context.Node;
        if (IsAsyncMethod(localFunction))
        {
            AnalyzeAsyncMethodName(context, localFunction.Identifier);
        }
    }

    private bool IsAsyncMethod(MethodDeclarationSyntax methodDeclaration)
    {
        return methodDeclaration.Modifiers.Any(SyntaxKind.AsyncKeyword);
    }

    private bool IsAsyncMethod(LocalFunctionStatementSyntax localFunction)
    {
        return localFunction.Modifiers.Any(SyntaxKind.AsyncKeyword);
    }

    private void AnalyzeAsyncMethodName(SyntaxNodeAnalysisContext context, SyntaxToken identifier)
    {
        var methodName = identifier.Text;

        if (string.IsNullOrEmpty(methodName) || !methodName.EndsWith("Async"))
        {
            var diagnostic = Diagnostic.Create(
                DiagnosticDescriptors.AsyncMethodMustEndWithAsync,
                identifier.GetLocation(),
                methodName);

            context.ReportDiagnostic(diagnostic);
        }
    }
}