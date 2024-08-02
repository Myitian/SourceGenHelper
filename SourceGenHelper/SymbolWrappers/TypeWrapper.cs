using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Myitian.SourceGenHelper.SymbolWrappers;

public class TypeWrapper : IEquatable<TypeWrapper>, IEquatable<Type>, IEquatable<string>, ISymbolWrapper<ITypeSymbol>
{
    protected TypeWrapper? baseType;
    protected TypeWrapper? containingType;
    protected ImmutableArray<AttributeWrapper> attributes;
    protected ImmutableArray<TypeWrapper> interfaces;
    protected ImmutableArray<TypeWrapper> typeMembers;
    protected ImmutableArray<EventWrapper> events;
    protected ImmutableArray<TypeWrapper> typeParameters;
    protected ImmutableArray<FieldWrapper> fields;
    protected ImmutableArray<PropertyWrapper> properties;
    protected ImmutableArray<MethodWrapper> methods;
    protected ImmutableArray<MethodWrapper> constructors;
    protected bool isPartial;

    public TypeWrapper(ITypeSymbol symbol, bool lazy = true)
    {
        Symbol = symbol;
        isPartial = (symbol.DeclaringSyntaxReferences[0].GetSyntax() is BaseTypeDeclarationSyntax dec)
            && dec.Modifiers.Any(x => x.Text is "partial" or "Partial");
        if (!lazy)
        {
            Load();
        }
    }

    public bool Loaded { get; private set; } = false;
    public bool IsNamed { get; private set; } = false;
    public ITypeSymbol Symbol { get; private set; }
    public INamedTypeSymbol? NamedTypeSymbol => Symbol as INamedTypeSymbol;
    public TypeWrapper? BaseType
    {
        get
        {
            if (!Loaded)
                Load();
            return baseType;
        }
    }
    public TypeWrapper? ContainingType
    {
        get
        {
            if (!Loaded)
                Load();
            return containingType;
        }
    }
    public ImmutableArray<AttributeWrapper> Attributes
    {
        get
        {
            if (!Loaded)
                Load();
            return attributes;
        }
    }
    public ImmutableArray<TypeWrapper> Interfaces
    {
        get
        {
            if (!Loaded)
                Load();
            return interfaces;
        }
    }
    public ImmutableArray<TypeWrapper> TypeMembers
    {
        get
        {
            if (!Loaded)
                Load();
            return typeMembers;
        }
    }
    public ImmutableArray<EventWrapper> Events
    {
        get
        {
            if (!Loaded)
                Load();
            return events;
        }
    }
    public ImmutableArray<TypeWrapper> TypeParameters
    {
        get
        {
            if (!Loaded)
                Load();
            return typeParameters;
        }
    }
    public ImmutableArray<FieldWrapper> Fields
    {
        get
        {
            if (!Loaded)
                Load();
            return fields;
        }
    }
    public ImmutableArray<PropertyWrapper> Properties
    {
        get
        {
            if (!Loaded)
                Load();
            return properties;
        }
    }
    public ImmutableArray<MethodWrapper> Methods
    {
        get
        {
            if (!Loaded)
                Load();
            return methods;
        }
    }
    public ImmutableArray<MethodWrapper> Constructors
    {
        get
        {
            if (!Loaded)
                Load();
            return constructors;
        }
    }
    public string Name => Symbol.Name;
    public string MetadataName => Symbol.MetadataName;
    public string? Namespace
    {
        get
        {
            if (Symbol.ContainingNamespace?.IsGlobalNamespace != false)
                return null;
            return Symbol.ContainingNamespace.ToDisplayString();
        }
    }
    public bool IsStatic => Symbol.IsStatic;
    public bool IsExtern => Symbol.IsExtern;
    public bool IsSealed => Symbol.IsSealed;
    public bool IsAbstract => Symbol.IsAbstract;
    public bool IsAnonymousType => Symbol.IsAnonymousType;
    public bool IsRecord => Symbol.IsRecord;
    public bool IsValueType => Symbol.IsValueType;
    public bool IsGenericType => NamedTypeSymbol is not null && NamedTypeSymbol.IsGenericType;
    public bool IsPartial => isPartial;
    public Accessibility DeclaredAccessibility => Symbol.DeclaredAccessibility;

    public void Load()
    {
        if (Symbol is INamedTypeSymbol named)
        {
            IsNamed = true;
            constructors = named.Constructors.Select(x => new MethodWrapper(x)).ToImmutableArray();
        }
        else
        {
            constructors = [];
        }
        attributes = Symbol.GetAttributes().ToImmutableAttributeWrapperArray();
        baseType = Symbol.BaseType is null ? null : new(Symbol.BaseType);
        containingType = Symbol.ContainingType is null ? null : new(Symbol.ContainingType);
        interfaces = Symbol.Interfaces.ToImmutableTypeWrapperArray();
        typeMembers = Symbol.GetTypeMembers().ToImmutableTypeWrapperArray();
        typeParameters = NamedTypeSymbol?.TypeParameters.ToImmutableTypeWrapperArray() ?? [];
        List<EventWrapper> events = [];
        List<FieldWrapper> fields = [];
        List<PropertyWrapper> properties = [];
        List<MethodWrapper> methods = [];
        foreach (ISymbol child in Symbol.GetMembers())
        {
            switch (child)
            {
                case IEventSymbol eventSymbol:
                    events.Add(new(eventSymbol));
                    break;
                case IFieldSymbol fieldSymbol:
                    fields.Add(new(fieldSymbol));
                    break;
                case IMethodSymbol methodSymbol:
                    methods.Add(new(methodSymbol));
                    break;
                case IPropertySymbol propertySymbol:
                    properties.Add(new(propertySymbol));
                    break;
            }
        }
        this.events = [.. events];
        this.fields = [.. fields];
        this.properties = [.. properties];
        this.methods = [.. methods];
        Loaded = true;
    }

    public override int GetHashCode()
        => Symbol.ToDisplayString().GetHashCode();
    public override bool Equals(object obj)
    {
        if (obj is TypeWrapper wrapper)
            return Equals(wrapper);
        if (obj is Type type)
            return Equals(type);
        if (obj is string str)
            return Equals(str);
        return false;
    }
    public bool Equals(TypeWrapper other)
        => other is not null && Symbol.ToDisplayString() == other.Symbol.ToDisplayString();
    public bool Equals(Type other)
        => other is not null && Symbol.ToDisplayString() == other.FullName;
    public bool Equals(string other)
        => other is not null && Symbol.ToDisplayString() == other;

    public bool Implements(TypeWrapper other)
    {
        if (other is null)
            return false;
        if (Equals(other))
            return true;
        if (BaseType?.Equals(other) == true)
            return true;
        foreach (var @interface in Symbol.AllInterfaces)
            if (@interface.Equals(other))
                return true;
        return false;
    }
    public bool Implements(Type other)
    {
        if (other is null)
            return false;
        if (Equals(other))
            return true;
        if (BaseType?.Equals(other) == true)
            return true;
        foreach (var @interface in Symbol.AllInterfaces)
            if (@interface.Equals(other))
                return true;
        return false;
    }

    public override string ToString()
        => Symbol.ToDisplayString();
    public string ToString(SymbolDisplayFormat? format)
        => Symbol.ToDisplayString(format);
    public string ToStringWithMembers(SymbolDisplayFormat? format)
    {
        StringBuilder sb = new(Symbol.ToDisplayString(format));
        foreach (ISymbol child in Symbol.GetMembers())
        {
            sb.Append("\n - ");
            sb.Append(child.ToDisplayString(format));
        }
        return sb.ToString();
    }
    public string RecursiveToString(SymbolDisplayFormat? format = null, int depth = 0, int indent = 4, char indentChar = ' ')
    {
        return RecursiveToString(format, depth, indent, indentChar, new()).ToString();
    }
    public StringBuilder RecursiveToString(SymbolDisplayFormat? format = null, int depth = 0, int indent = 4, char indentChar = ' ', StringBuilder? sb = null)
    {
        sb ??= new();
        int depth_1 = depth + 1;
        sb.Append(indentChar, depth * indent).AppendLine($"Attributes ({Attributes.Length}):");
        foreach (AttributeWrapper attribute in Attributes)
        {
            sb.Append(indentChar, depth * indent).Append('[').Append(attribute.ToString()).AppendLine("]");
        }
        sb.Append(indentChar, depth * indent).AppendLine(Symbol.ToDisplayString(format));
        sb.Append(indentChar, depth * indent).AppendLine("{");
        sb.Append(indentChar, depth_1 * indent).AppendLine($"Fields ({Fields.Length}):");
        foreach (FieldWrapper field in Fields)
        {
            sb.Append(indentChar, depth_1 * indent).AppendLine(field.ToString(format));
        }
        sb.AppendLine();
        sb.Append(indentChar, depth_1 * indent).AppendLine($"Properties: ({Properties.Length}):");
        foreach (PropertyWrapper property in Properties)
        {
            sb.Append(indentChar, depth_1 * indent).AppendLine(property.ToString(format));
        }
        sb.AppendLine();
        sb.Append(indentChar, depth_1 * indent).AppendLine($"Constructors: ({Constructors.Length}):");
        foreach (MethodWrapper constructor in Constructors)
        {
            sb.Append(indentChar, depth_1 * indent).AppendLine(constructor.ToString(format));
        }
        sb.AppendLine();
        sb.Append(indentChar, depth_1 * indent).AppendLine($"Methods: ({Methods.Length}):");
        foreach (MethodWrapper method in Methods)
        {
            sb.Append(indentChar, depth_1 * indent).AppendLine(method.ToString(format));
        }
        sb.AppendLine();
        sb.Append(indentChar, depth_1 * indent).AppendLine($"Type Members: ({TypeMembers.Length}):");
        foreach (TypeWrapper member in TypeMembers)
        {
            member.RecursiveToString(format, depth_1, indent, indentChar, sb).AppendLine();
        }
        sb.Append(indentChar, depth * indent).Append('}');
        return sb;
    }
}