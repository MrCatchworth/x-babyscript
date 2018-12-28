using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace XRebirthBabyScript.Decompile
{
    public class ElementShortcut
    {
        public IHeaderRule HeaderMatcher { get; set; } = null;

        public IList<IAttributeRule> AttributeMatchers { get; set; } = null;

        public Action<BabyScriptDecompiler> Apply;

        private bool _checkMatch(XmlReader reader) {
            // Header must match.
            if (HeaderMatcher != null && !HeaderMatcher.Matches(reader)) {
                return false;
            }

            // Attribute list must match.
            var satisfiedMatchers = new HashSet<IAttributeRule>(AttributeMatchers.Count);
            if (AttributeMatchers != null) {
                while (reader.MoveToNextAttribute())
                {
                    // Comment attribute gets a free pass.
                    if (reader.Name == "comment")
                    {
                        continue;
                    }

                    var satisfiedNow = AttributeMatchers.Where(matcher => matcher.Matches(reader.Name, reader.Value)).ToList();

                    // All attributes must satisfy at least one matcher.
                    if (!satisfiedNow.Any()) {
                        return false;
                    }

                    // Mark these matchers as satisfied.
                    satisfiedMatchers.UnionWith(satisfiedNow);
                }

                // All matchers must have been satisfied by now if we are to continue.
                if (satisfiedMatchers.Count != AttributeMatchers.Count)
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