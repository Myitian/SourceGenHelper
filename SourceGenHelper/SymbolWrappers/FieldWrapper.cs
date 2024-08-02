using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public readonly struct FieldWrapper(IFieldSymbol symbol) : IContentSymbolWrapper<IFieldSymbol>, IMemberSymbolWrapper<IFieldSymbol>
{
    private readonly ImmutableArray<AttributeWrapper> attributes = symbol.GetAttributes().ToImmutableAttributeWrapperArray();
    private readonly TypeWrapper typeSymbol = new(symbol.Type);

    public IFieldSymbol Symbol { get; } = symbol;
    public TypeWrapper TypeSymbol => typeSymbol;
    public ImmutableArray<AttributeWrapper> Attributes => attributes;
    public string Name => Symbol.Name;
    public string MetadataName => Symbol.MetadataName;
    public bool IsStatic => Symbol.IsStatic;
    public bool IsExtern => Symbol.IsExtern;
    public bool IsVolatile => Symbol.IsVolatile;
    public bool IsVirtual => Symbol.IsVirtual;
    public bool IsAbstract => Symbol.IsAbstract;
    public bool IsOverride => Symbol.IsOverride;
    public bool IsRequired => Symbol.IsRequired;
    public bool IsReadOnly => Symbol.IsReadOnly;
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