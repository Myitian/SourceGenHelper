using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public class ParameterWrapper(IParameterSymbol symbol) : IContentSymbolWrapper<IParameterSymbol>
{
    public IParameterSymbol Symbol { get; } = symbol;
    public TypeWrapper TypeSymbol { get; } = new(symbol.Type);
    public ImmutableArray<AttributeWrapper> Attributes { get; } = symbol.GetAttributes().ToImmutableAttributeWrapperArray();
    public string Name => Symbol.Name;
    public string MetadataName => Symbol.MetadataName;
    public bool IsOptional => Symbol.IsOptional;
    public RefKind RefKind => Symbol.RefKind;
    public bool HasExplicitDefaultValue => Symbol.HasExplicitDefaultValue;
    public object? ExplicitDefaultValue => Symbol.ExplicitDefaultValue;

    public override string ToString()
    {
        return Symbol.ToDisplayString();
    }
    public string ToString(SymbolDisplayFormat? format)
    {
        return Symbol.ToDisplayString(format);
    }
}