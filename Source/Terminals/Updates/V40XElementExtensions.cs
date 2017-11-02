using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Terminals.Updates
{
    internal static class V40XElementExtensions
    {
        internal static void ReplaceByNewElement(this XElement favorite, string toReplace, string replacedBy)
        {
            var toRemove = favorite.FindByLocalName(toReplace).FirstOrDefault();

            if (toRemove != null)
            {
                toRemove.Remove();
                favorite.Add(XElement.Parse(replacedBy));
            }
        }

        internal static IEnumerable<XElement> FindByLocalName(this XElement parent, string localName)
        {
            return parent.Descendants().Where(f => f.Name.LocalName == localName);
        }
    }
}