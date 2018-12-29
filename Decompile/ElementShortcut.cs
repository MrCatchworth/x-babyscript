using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace XBabyScript.Decompile
{
    public class ElementShortcut
    {
        public IHeaderRule HeaderRule { get; set; } = null;

        public IList<IAttributeRule> AttributeRules { get; set; } = null;

        public Action<BabyScriptDecompiler> Apply;

        private bool _checkMatch(XmlReader reader) {
            // Header must match.
            if (HeaderRule != null && !HeaderRule.Matches(reader)) {
                return false;
            }

            // Attribute list must match.
            var satisfiedMatchers = new HashSet<IAttributeRule>(AttributeRules.Count);
            if (AttributeRules != null) {
                while (reader.MoveToNextAttribute())
                {
                    // Comment attribute gets a free pass.
                    if (reader.Name == "comment")
                    {
                        continue;
                    }

                    var satisfiedNow = AttributeRules.Where(matcher => matcher.Matches(reader.Name, reader.Value)).ToList();

                    // All attributes must satisfy at least one matcher.
                    if (!satisfiedNow.Any()) {
                        return false;
                    }

                    // Mark these matchers as satisfied.
                    satisfiedMatchers.UnionWith(satisfiedNow);
                }

                // All matchers must have been satisfied by now if we are to continue.
                if (satisfiedMatchers.Count != AttributeRules.Count)
                {
                    return false;
                }

            }
                
            return true;
        }

        public bool CheckMatch(XmlReader reader) {
            var success = _checkMatch(reader);

            reader.MoveToElement();
            return success;
        }
    }
}