using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public class PropertyWrapper(IPropertySymbol symbol) : IContentSymbolWrapper<IPropertySymbol>, IMemberSymbolWrapper<IPropertySymbol>
{
    public IPropertySymbol Symbol { get; } = symbol;
    public TypeWrapper TypeSymbol { get; } = new(symbol.Type);
    public ImmutableArray<AttributeWrapper> Attributes { get; } = symbol.GetAttributes().ToImmutableAttributeWrapperArray();
    public string Name => Symbol.Name;
    public string MetadataName => Symbol.MetadataName;
    public bool IsStatic => Symbol.IsStatic;
    public bool IsExtern => Symbol.IsExtern;
    public bool IsVirtual => Symbol.IsVirtual;
    public bool IsAbstract => Symbol.IsAbstract;
    public bool IsOverride => Symbol.IsOverride;
    public bool IsRequired => Symbol.IsRequired;
    public bool IsReadOnly => Symbol.IsReadOnly;
    public bool IsWriteOnly => Symbol.IsWriteOnly;
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