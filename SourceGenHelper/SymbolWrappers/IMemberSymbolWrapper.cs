using Microsoft.CodeAnalysis;

namespace Myitian.SourceGenHelper.SymbolWrappers
{
    public interface IMemberSymbolWrapper<T> : ISymbolWrapper<T> where T : ISymbol
    {
        bool IsStatic { get; }
        bool IsExtern { get; }
        bool IsVirtual { get; }
        bool IsAbstract { get; }
        bool IsOverride { get; }
        public Accessibility DeclaredAccessibility { get; }
    }
}