using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Myitian.SourceGenHelper;

public static class BaseTypeDeclarationSyntaxExtension
{
    public static INamedTypeSymbol? ToSymbol(this BaseTypeDeclarationSyntax dec, GeneratorExecutionContext context)
    {
        return context.Compilation.GetSemanticModel(dec.SyntaxTree).GetDeclaredSymbol(dec);
    }
}