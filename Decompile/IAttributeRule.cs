namespace XRebirthBabyScript.Decompile
{
    public interface IAttributeRule
    {
        bool Matches(string name, string value);
    }
}