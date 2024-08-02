using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public class AttributeWrapper
{
    private readonly AttributeData attribute;

    public TypeWrapper? AttributeClass => attribute.AttributeClass is null ? null : new(attribute.AttributeClass);
    public ReadOnlyDictionary<string, TypedConstant> NamedArguments { get; }
    public ImmutableArray<TypedConstant> ConstructorArguments => attribute.ConstructorArguments;

    public AttributeWrapper(AttributeData attribute)
    {
        this.attribute = attribute;
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