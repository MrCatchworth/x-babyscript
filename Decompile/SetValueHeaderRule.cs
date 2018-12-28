using System.Xml;

namespace XRebirthBabyScript.Decompile
{
    public class SetValueHeaderRule : IHeaderRule
    {
        public bool Matches(XmlReader reader)
        {
            return reader.Name == "set_value" && reader.IsEmptyElement;
        }
    }
}