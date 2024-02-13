using Microsoft.CodeAnalysis;

namespace Myitian.SourceGenHelper.SymbolWrappers
{
    public interface IContentSymbolWrapper<T> : ISymbolWrapper<T> where T : ISymbol
    {
        LazyLoadTypeWrapper TypeSymbol { get; }
    }
}