using System;

namespace XRebirthBabyScript.Tree
{
    public class BabyElement : BabyNode
    {
        public ElementType Type { get; private set; }
        public bool IsComment { get; private set; }
        public string Name { get; private set; }
        public BabyAttribute[] Attributes { get; private set; }
        

        public static BabyElement CreateAssignment(string name, string exact)
        {
            BabyAttribute[] attributes = new BabyAttribute[]
            {
                new BabyAttribute("name", name),
                new BabyAttribute("exact", exact)
            };

            BabyElement retVal = new BabyElement("set_value", attributes, null);
            retVal.Type = ElementType.Assignment;
            return retVal;
        }

        public static BabyElement CreateIncrement(string leftHand)
        {
            return new BabyElement("set_value", new[]{
                new BabyAttribute("name", leftHand),
                new BabyAttribute("operation", "add")
            }, null)
            {
                Type = ElementType.Increment
            };
        }

        public static BabyElement CreateDecrement(string leftHand)
        {
            return new BabyElement("set_value", new[]{
                new BabyAttribute("name", leftHand),
                new BabyAttribute("operation", "subtract")
            }, null)
            {
                Type = ElementType.Decrement
            };
        }

        public BabyElement(string name, BabyAttribute[] attr, BabyNode[] children) : base(children)
        {
            Name = name;
            Attributes = attr;
            Type = ElementType.Regular;
            IsComment = false;
        }
    }
}