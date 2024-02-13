using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Myitian.SourceGenHelper.SymbolWrappers
{
    public class AttributeWrapperList : ReadOnlyList<AttributeWrapper>
    {
        public AttributeWrapperList(IEnumerable<AttributeWrapper> attributes) : base(attributes)
        {
        }
        public AttributeWrapperList(IEnumerable<AttributeData> attributes) : base(attributes.Select(x => new AttributeWrapper(x)))
        {
        }
    }
}