using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public readonly struct EventWrapper(IEventSymbol symbol) : IContentSymbolWrapper<IEventSymbol>, IMemberSymbolWrapper<IEventSymbol>
{
    private readonly ImmutableArray<AttributeWrapper> attributes = symbol.GetAttributes().ToImmutableAttributeWrapperArray();
    private readonly TypeWrapper typeSymbol = new(symbol.Type);

    public IEventSymbol Symbol { get; } = symbol;
    public TypeWrapper TypeSymbol => typeSymbol;
    public ImmutableArray<AttributeWrapper> Attributes => attributes;
    public string Name => Symbol.Name;
    public string MetadataName => Symbol.MetadataName;
    public bool IsStatic => Symbol.IsStatic;
    public bool IsExtern => Symbol.IsExtern;
    public bool IsVirtual => Symbol.IsVirtual;
    public bool IsAbstract => Symbol.IsAbstract;
    public bool IsOverride => Symbol.IsOverride;
    public Accessibility DeclaredAccessibility => Symbol.DeclaredAccessibility;

    public override string ToString()
    {
        return Symbol.ToDisplayString();
    }
    public string ToString(SymbolDisplayFormat? format)
    {
        return Symbol.ToDisplayString(format);
    }
}