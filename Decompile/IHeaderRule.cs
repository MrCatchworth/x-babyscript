using System.Xml;

namespace XBabyScript.Decompile
{
    public interface IHeaderRule
    {
        bool Matches(XmlReader reader);
    }
}