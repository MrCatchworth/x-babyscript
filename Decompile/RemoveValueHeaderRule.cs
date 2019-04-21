using System.Xml;

namespace XBabyScript.Decompile
{
    public class RemoveValueHeaderRule : IHeaderRule
    {
        public bool Matches(XmlReader reader)
        {
            return reader.Name == "remove_value" && reader.IsEmptyElement;
        }
    }
}