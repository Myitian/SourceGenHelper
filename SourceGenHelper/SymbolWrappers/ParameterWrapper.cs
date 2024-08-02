using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public readonly struct ParameterWrapper(IParameterSymbol symbol) : IContentSymbolWrapper<IParameterSymbol>
{
    private readonly ImmutableArray<AttributeWrapper> attributes = symbol.GetAttributes().ToImmutableAttributeWrapperArray();
    private readonly TypeWrapper typeSymbol = new(symbol.Type);

    public IParameterSymbol Symbol { get; } = symbol;
    public TypeWrapper TypeSymbol => typeSymbol;
    public ImmutableArray<AttributeWrapper> Attributes => attributes;
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