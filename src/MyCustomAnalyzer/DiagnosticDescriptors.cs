using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace SFM.Packages.CodeAnalyzer;

internal static class DiagnosticDescriptors
{
    private const string CodeSmellCategory = "CodeSmell";
    private const string PerformanceCategory = "Performance";
    private const string ConcurrencyCategory = "Concurrency";

    private const string AsyncCategory = "Async";

    private const string ErrorHandlingCategory = "ErrorHandling";
    private static readonly string[] UnnecessaryTag = new[] { WellKnownDiagnosticTags.Unnecessary };

    public static readonly DiagnosticDescriptor NAMING001 = new DiagnosticDescriptor(
    id: nameof(NAMING001),
    title: "Method name must start with 'Z'",
    messageFormat: "Method name '{0}' must start with uppercase 'Z'",
    category: "Naming",
    defaultSeverity: DiagnosticSeverity.Error,
    isEnabledByDefault: true,
    description: "All methods must start with the letter 'Z'.", // اضافه کردن این دو خط
    helpLinkUri: "https://github.com/yourcompany/analyzers/docs/NAMING001",
    customTags: new[] { WellKnownDiagnosticTags.Compiler, WellKnownDiagnosticTags.NotConfigurable });

    public static readonly DiagnosticDescriptor AsyncMethodMustEndWithAsync = new DiagnosticDescriptor(
      nameof(AsyncMethodMustEndWithAsync),
      title: "Async method name must end with 'Async'",
      messageFormat: "Async method '{0}' must end with 'Async'",
      category: "Naming",
      defaultSeverity: DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "All async methods must end with 'Async'.");
    /// <nodoc />
   
    /// <nodoc />
    public static readonly DiagnosticDescriptor EPC37 = new DiagnosticDescriptor(
        nameof(EPC37),
        title: "Do not validate arguments in async methods eagerly",
        messageFormat: "Argument validation in async method '{0}' will not fail eagerly. Consider using a wrapper method or Task.FromException.",
        category: AsyncCategory,
        // Info by default, since it might generate quite a bit of warnings for a codebase.
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "Argument validation in async methods does not throw exceptions eagerly. The exception is thrown when the task is awaited, which can lead to unexpected behavior.",
        helpLinkUri: GetHelpUri(nameof(EPC37)));

    public static string GetHelpUri(string ruleId)
    {
        return $"https://github.com/SergeyTeplyakov/ErrorProne.NET/tree/master/docs/Rules/{ruleId}.md";
    }
}