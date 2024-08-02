using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public static class ImmutableArrayExtensions
{
    public static ImmutableArray<AttributeWrapper> ToImmutableAttributeWrapperArray(this IEnumerable<AttributeWrapper> attributes)
    {
        return attributes.ToImmutableArray();
    }
    public static ImmutableArray<AttributeWrapper> ToImmutableAttributeWrapperArray(this IEnumerable<AttributeData> attributes)
    {
        return attributes.Select(x => new AttributeWrapper(x)).ToImmutableArray();
    }
    public static ImmutableArray<TypeWrapper> ToImmutableTypeWrapperArray(this IEnumerable<TypeWrapper> typeSymbols)
    {
        return typeSymbols.ToImmutableArray();
    }
    public static ImmutableArray<TypeWrapper> ToImmutableTypeWrapperArray(this IEnumerable<ITypeSymbol> typeSymbols)
    {
        return typeSymbols.Select(x => new TypeWrapper(x)).ToImmutableArray();
    }
}