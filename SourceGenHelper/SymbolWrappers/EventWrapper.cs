using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public class EventWrapper(IEventSymbol symbol) : IContentSymbolWrapper<IEventSymbol>, IMemberSymbolWrapper<IEventSymbol>
{
    public IEventSymbol Symbol { get; } = symbol;
    public TypeWrapper TypeSymbol { get; } = new(symbol.Type);
    public ImmutableArray<AttributeWrapper> Attributes { get; } = symbol.GetAttributes().ToImmutableAttributeWrapperArray();
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