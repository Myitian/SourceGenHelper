using Microsoft.CodeAnalysis;

namespace Myitian.SourceGenHelper.SymbolWrappers
{
    public interface ISymbolWrapper<T> where T : ISymbol
    {
        T Symbol { get; }
        AttributeWrapperList Attributes { get; }
        string Name { get; }
        string MetadataName { get; }
        string ToString(SymbolDisplayFormat? format);
    }
}