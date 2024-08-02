using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public interface ISymbolWrapper<T> where T : ISymbol
{
    T Symbol { get; }
    ImmutableArray<AttributeWrapper> Attributes { get; }
    string Name { get; }
    string MetadataName { get; }
    string ToString(SymbolDisplayFormat? format);
}