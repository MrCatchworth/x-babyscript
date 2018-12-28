namespace XRebirthBabyScript.Decompile
{
    public class NameAttributeRule : IAttributeRule
    {
        public string Name { get; private set; }

        public NameAttributeRule(string name)
        {
            Name = name;
        }

        public bool Matches(string name, string value)
        {
            return Name == name;
        }
    }
}