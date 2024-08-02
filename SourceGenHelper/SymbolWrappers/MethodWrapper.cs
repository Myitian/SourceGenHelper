using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public readonly struct MethodWrapper(IMethodSymbol symbol) : IMemberSymbolWrapper<IMethodSymbol>
{
    private readonly ImmutableArray<AttributeWrapper> attributes = symbol.GetAttributes().ToImmutableAttributeWrapperArray();
    private readonly ImmutableArray<ParameterWrapper> parameters = symbol.Parameters.Select(x => new ParameterWrapper(x)).ToImmutableArray();
    private readonly TypeWrapper returnType = new(symbol.ReturnType);

    public IMethodSymbol Symbol { get; } = symbol;
    public TypeWrapper ReturnType => returnType;
    public ImmutableArray<AttributeWrapper> Attributes => attributes;
    public ImmutableArray<ParameterWrapper> Parameters => parameters;
    public string Name => Symbol.Name;
    public string MetadataName => Symbol.MetadataName;
    public bool IsStatic => Symbol.IsStatic;
    public bool IsExtern => Symbol.IsExtern;
    public bool IsVirtual => Symbol.IsVirtual;
    public bool IsAbstract => Symbol.IsAbstract;
    public bool IsOverride => Symbol.IsOverride;
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