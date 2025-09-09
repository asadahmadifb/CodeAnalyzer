using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SFM.Packages.CodeAnalyzer.Analyzers.Base;
using System.Collections.Immutable;

namespace SFM.Packages.CodeAnalyzer.Analyzers.Naming;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MethodNamingAnalyzer : BaseDiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(DiagnosticDescriptors.NAMING001);

    protected override void InitializeAnalyzer(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeLocalFunction, SyntaxKind.LocalFunctionStatement);
    }

    private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
    {
        var methodDeclaration = (MethodDeclarationSyntax)context.Node;
        AnalyzeMethodName(context, methodDeclaration.Identifier);
    }

    private void AnalyzeLocalFunction(SyntaxNodeAnalysisContext context)
    {
        var localFunction = (LocalFunctionStatementSyntax)context.Node;
        AnalyzeMethodName(context, localFunction.Identifier);
    }

    private void AnalyzeMethodName(SyntaxNodeAnalysisContext context, SyntaxToken identifier)
    {
        var methodName = identifier.Text;

        if (string.IsNullOrEmpty(methodName) || !methodName.StartsWith("Z"))
        {
            var diagnostic = Diagnostic.Create(
                DiagnosticDescriptors.NAMING001,
                identifier.GetLocation(),
                methodName);

            context.ReportDiagnostic(diagnostic);
        }
    }
}