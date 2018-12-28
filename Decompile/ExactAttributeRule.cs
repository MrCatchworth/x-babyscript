namespace XRebirthBabyScript.Decompile
{
    public class ExactAttributeRule : IAttributeRule
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public ExactAttributeRule(string name, string value) {
            Name = name;
            Value = value;
        }

        public bool Matches(string name, string value)
        {
            return name == Name && value == Value;
        }
    }
}