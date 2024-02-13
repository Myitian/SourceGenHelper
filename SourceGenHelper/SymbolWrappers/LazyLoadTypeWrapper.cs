using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myitian.SourceGenHelper.SymbolWrappers
{
    public class LazyLoadTypeWrapper : IEquatable<LazyLoadTypeWrapper>, IEquatable<Type>, ISymbolWrapper<ITypeSymbol>
    {
        protected LazyLoadTypeWrapper? baseType;
        protected AttributeWrapperList? attributes;
        protected LazyLoadTypeWrapperList? interfaces;
        protected LazyLoadTypeWrapperList? typeMembers;
        protected ReadOnlyList<EventWrapper>? events;
        protected LazyLoadTypeWrapperList? typeParameters;
        protected ReadOnlyList<FieldWrapper>? fields;
        protected ReadOnlyList<PropertyWrapper>? properties;
        protected ReadOnlyList<MethodWrapper>? methods;
        protected ReadOnlyList<MethodWrapper>? constructors;

        public LazyLoadTypeWrapper(ITypeSymbol symbol, bool lazy = true)
        {
            Symbol = symbol;
            if (!lazy)
            {
                Load();
            }
        }

        public bool Loaded { get; private set; } = false;
        public bool IsNamed { get; private set; } = false;
        public ITypeSymbol Symbol { get; private set; }
        public INamedTypeSymbol? NamedTypeSymbol => Symbol as INamedTypeSymbol;
        public LazyLoadTypeWrapper? BaseType
        {
            get
            {
                if (!Loaded)
                    Load();
                return baseType;
            }
        }
        public AttributeWrapperList Attributes
        {
            get
            {
                if (!Loaded)
                    Load();
                return attributes!;
            }
        }
        public LazyLoadTypeWrapperList Interfaces
        {
            get
            {
                if (!Loaded)
                    Load();
                return interfaces!;
            }
        }
        public LazyLoadTypeWrapperList TypeMembers
        {
            get
            {
                if (!Loaded)
                    Load();
                return typeMembers!;
            }
        }
        public ReadOnlyList<EventWrapper> Events
        {
            get
            {
                if (!Loaded)
                    Load();
                return events!;
            }
        }
        public LazyLoadTypeWrapperList TypeParameters
        {
            get
            {
                if (!Loaded)
                    Load();
                return typeParameters!;
            }
        }
        public ReadOnlyList<FieldWrapper> Fields
        {
            get
            {
                if (!Loaded)
                    Load();
                return fields!;
            }
        }
        public ReadOnlyList<PropertyWrapper> Properties
        {
            get
            {
                if (!Loaded)
                    Load();
                return properties!;
            }
        }
        public ReadOnlyList<MethodWrapper> Methods
        {
            get
            {
                if (!Loaded)
                    Load();
                return methods!;
            }
        }
        public ReadOnlyList<MethodWrapper> Constructors
        {
            get
            {
                if (!Loaded)
                    Load();
                return constructors!;
            }
        }
        public string Name => Symbol.Name;
        public string MetadataName => Symbol.MetadataName;
        public bool IsStatic => Symbol.IsStatic;
        public bool IsExtern => Symbol.IsExtern;
        public bool IsSealed => Symbol.IsSealed;
        public bool IsAbstract => Symbol.IsAbstract;
        public bool IsAnonymousType => Symbol.IsAnonymousType;
        public bool IsRecord => Symbol.IsRecord;
        public bool IsValueType => Symbol.IsValueType;
        public bool IsPartial => (Symbol.DeclaringSyntaxReferences[0].GetSyntax() is ClassDeclarationSyntax cds)
            && cds.Modifiers.Any(x => x.Text is "partial" or "Partial");
        public Accessibility DeclaredAccessibility => Symbol.DeclaredAccessibility;

        public void Load()
        {
            if (Symbol is INamedTypeSymbol named)
            {
                IsNamed = true;
                constructors = new(named.Constructors.Select(x => new MethodWrapper(x)));
            }
            else
            {
                constructors = new(Array.Empty<MethodWrapper>());
            }
            attributes = new(Symbol.GetAttributes());
            baseType = Symbol.BaseType is null ? null : new(Symbol.BaseType);
            interfaces = new(Symbol.Interfaces);
            typeMembers = new(Symbol.GetTypeMembers());
            List<EventWrapper> events = [];
            List<FieldWrapper> fields = [];
            List<PropertyWrapper> properties = [];
            List<MethodWrapper> methods = [];
            List<LazyLoadTypeWrapper> typeParameters = [];
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
                    case ITypeParameterSymbol typeParameterSymbol:
                        typeParameters.Add(new(typeParameterSymbol));
                        break;
                }
            }
            this.events = new(events);
            this.fields = new(fields);
            this.properties = new(properties);
            this.methods = new(methods);
            this.typeParameters = new(typeParameters);
            Loaded = true;
        }

        public override int GetHashCode()
            => Symbol.ToDisplayString().GetHashCode();
        public override bool Equals(object obj)
        {
            if (obj is LazyLoadTypeWrapper lltw)
                return Equals(lltw);
            if (obj is Type type)
                return Equals(type);
            return false;
        }
        public bool Equals(LazyLoadTypeWrapper other)
            => other is not null && Symbol.ToDisplayString() == other.Symbol.ToDisplayString();
        public bool Equals(Type other)
            => other is not null && Symbol.ToDisplayString() == other.FullName;

        public bool Implements(LazyLoadTypeWrapper other)
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
            StringBuilder sb = new();
            int depth_1 = depth + 1;
            sb.Append(indentChar, depth * indent).AppendLine($"Attributes ({Attributes.Count}):");
            foreach (AttributeWrapper attribute in Attributes)
            {
                sb.Append(indentChar, depth * indent).Append('[').Append(attribute.ToString()).AppendLine("]");
            }
            sb.Append(indentChar, depth * indent).AppendLine(Symbol.ToDisplayString(format));
            sb.Append(indentChar, depth * indent).AppendLine("{");
            sb.Append(indentChar, depth_1 * indent).AppendLine($"Fields ({Fields.Count}):");
            foreach (FieldWrapper field in Fields)
            {
                sb.Append(indentChar, depth_1 * indent).AppendLine(field.ToString(format));
            }
            sb.AppendLine();
            sb.Append(indentChar, depth_1 * indent).AppendLine($"Properties: ({Properties.Count}):");
            foreach (PropertyWrapper property in Properties)
            {
                sb.Append(indentChar, depth_1 * indent).AppendLine(property.ToString(format));
            }
            sb.AppendLine();
            sb.Append(indentChar, depth_1 * indent).AppendLine($"Constructors: ({Constructors.Count}):");
            foreach (MethodWrapper constructor in Constructors)
            {
                sb.Append(indentChar, depth_1 * indent).AppendLine(constructor.ToString(format));
            }
            sb.AppendLine();
            sb.Append(indentChar, depth_1 * indent).AppendLine($"Methods: ({Methods.Count}):");
            foreach (MethodWrapper method in Methods)
            {
                sb.Append(indentChar, depth_1 * indent).AppendLine(method.ToString(format));
            }
            sb.AppendLine();
            sb.Append(indentChar, depth_1 * indent).AppendLine($"Type Members: ({TypeMembers.Count}):");
            foreach (LazyLoadTypeWrapper member in TypeMembers)
            {
                sb.AppendLine(member.RecursiveToString(format, depth_1, indent));
            }
            sb.Append(indentChar, depth * indent).Append('}');
            return sb.ToString();
        }
    }
}