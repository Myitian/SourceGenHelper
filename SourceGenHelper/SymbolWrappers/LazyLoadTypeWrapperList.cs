using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Myitian.SourceGenHelper.SymbolWrappers
{
    public class LazyLoadTypeWrapperList : ReadOnlyList<LazyLoadTypeWrapper>
    {
        public LazyLoadTypeWrapperList(IEnumerable<LazyLoadTypeWrapper> typeSymbols) : base(typeSymbols)
        {
        }
        public LazyLoadTypeWrapperList(IEnumerable<ITypeSymbol> typeSymbols) : base(typeSymbols.Select(x => new LazyLoadTypeWrapper(x)))
        {
        }
    }
}