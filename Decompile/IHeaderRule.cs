using System.Xml;

namespace XRebirthBabyScript.Decompile
{
    public interface IHeaderRule
    {
        bool Matches(XmlReader reader);
    }
}