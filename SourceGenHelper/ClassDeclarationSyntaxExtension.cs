using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Myitian.SourceGenHelper
{
    public static class ClassDeclarationSyntaxExtension
    {
        public static INamedTypeSymbol? ToSymbol(this ClassDeclarationSyntax clazz, GeneratorExecutionContext context)
        {
            return context.Compilation.GetSemanticModel(clazz.SyntaxTree).GetDeclaredSymbol(clazz);
        }
    }
}