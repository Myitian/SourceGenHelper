using Microsoft.CodeAnalysis;
using System.Linq;

namespace Myitian.SourceGenHelper.SymbolWrappers
{
    public class MethodWrapper(IMethodSymbol symbol) : IMemberSymbolWrapper<IMethodSymbol>
    {
        public IMethodSymbol Symbol { get; } = symbol;
        public LazyLoadTypeWrapper ReturnType { get; } = new(symbol.ReturnType);
        public AttributeWrapperList Attributes { get; } = new(symbol.GetAttributes());
        public ReadOnlyList<ParameterWrapper> Parameters { get; } = new(symbol.Parameters.Select(x => new ParameterWrapper(x)));
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
}