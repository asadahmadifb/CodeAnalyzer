using Microsoft.CodeAnalysis.Diagnostics;

namespace SFM.Packages.CodeAnalyzer.Analyzers.Base;

public abstract class BaseDiagnosticAnalyzer : DiagnosticAnalyzer
{
    protected abstract void InitializeAnalyzer(AnalysisContext context);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        InitializeAnalyzer(context);
    }
}