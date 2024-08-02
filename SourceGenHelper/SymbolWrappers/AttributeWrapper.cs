using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public readonly struct AttributeWrapper
{
    private readonly AttributeData attribute;
    private readonly TypeWrapper? attributeClass;

    public TypeWrapper? AttributeClass => attributeClass;
    public ReadOnlyDictionary<string, TypedConstant> NamedArguments { get; }
    public ImmutableArray<TypedConstant> ConstructorArguments => attribute.ConstructorArguments;

    public AttributeWrapper(AttributeData attribute)
    {
        this.attribute = attribute;
        attributeClass = attribute.AttributeClass is null ? null : new(attribute.AttributeClass);
        Dictionary<string, TypedConstant> namedArgs = [];
        foreach (KeyValuePair<string, TypedConstant> kvp in attribute.NamedArguments)
        {
            namedArgs.Add(kvp.Key, kvp.Value);
        }
        NamedArguments = new(namedArgs);
    }

    public override string ToString()
    {
        return attribute.ToString();
    }
}