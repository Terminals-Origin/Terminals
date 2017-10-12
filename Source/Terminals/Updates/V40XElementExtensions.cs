using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Terminals.Updates
{
    internal static class V40XElementExtensions
    {
        internal static void ReplaceByNewElement(this XElement favorite, string toReplace, string replacedBy)
        {
            favorite.Remove(toReplace);

            XName puttyName = favorite.Name.Namespace + replacedBy;
            var puttyOptions = new XElement(puttyName);
            favorite.Add(puttyOptions);
        }

        internal static void Remove(this XElement parent, string toReplace)
        {
            var toRemove = parent.FindByLocalName(toReplace).FirstOrDefault();

            if (toRemove != null)
                toRemove.Remove();
        }

        internal static IEnumerable<XElement> FindByLocalName(this XElement parent, string localName)
        {
            return parent.Descendants().Where(f => f.Name.LocalName == localName);
        }
    }
}