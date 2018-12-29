using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBabyScript
{
    public class ConversionProperties
    {
        public readonly Dictionary<string, IList<string>> ImpliedAttributeNames = new Dictionary<string, IList<string>>
        {
            //Control flow
            { "do_if",                      new[]{"value"} },
            { "do_elseif",                  new[]{"value"} },
            { "do_else",                    new[]{"value"} },
            { "do_all",                     new[]{"counter", "exact"} },
            { "do_while",                   new[]{"value"} },

            { "label",                      new[]{"name"} },
            { "resume",                     new[]{"label"} },

            //Cue-related elements
            { "param",                      new[]{"name", "value"} },
            { "reset_cue",                  new[]{"cue"} },
            { "signal_cue",                 new[]{"cue"} },
            { "cancel_cue",                 new[]{"cue"} },
            { "signal_cue_instantly",       new[]{"cue"} },
            { "cue",                        new[]{"name"} },
            { "check_value",                new[]{"value"} },
            { "library",                    new[]{"name"} },
            { "delay",                      new[]{"exact"} },
            { "remove_mission",             new[]{"cue"} },
            { "include_actions",            new[]{"ref"} },

            //List operations
            { "create_list",                new[]{"name"} },
            { "append_to_list",             new[]{"name", "exact"} },

            //Group operations
            { "create_group",               new[]{"groupname"} },
            { "add_to_group",               new[]{"groupname", "object"} },
            { "remove_from_group",          new[]{"group", "object"} },

            //Variable operations
            { "set_value",                  new[]{"name"} },
            { "remove_value",               new[]{"name"} },

            //String manipulations
            { "replace",                    new[]{"string", "with"} },

            //Conversation/menu actions
            { "add_conversation_view",      new[]{"view"} },

            //General actions
            //TODO: extremely incomplete!
            { "debug_text",                 new[]{"text"} },
            { "wait",                       new[]{"exact"} },
            { "speak",                      new[]{"actor", "line"} },
            { "start_script",               new[]{"name", "object"} },
            { "reward_player",              new[]{"money"} },
            { "destroy_object",             new[]{"object"} },
            { "set_job_active",             new[]{"job", "activate"} },
            { "set_object_active",          new[]{"object", "activate"} },
            { "set_known",                  new[]{"object", "known"} },
            { "signal_objects",             new[]{"object", "param", "param2", "param3"} }

        };

        public readonly Dictionary<string, string> ShortElementNames = new Dictionary<string, string>
        {
            { "do_if", "if" },
            { "do_elseif", "elseif" },
            { "do_else", "else" },
            { "do_while", "while" },
            { "set_value", "set" },
            { "remove_value", "rem" },
        };

        public string GetShortElementName(string fullName)
        {
            var success = ShortElementNames.TryGetValue(fullName, out var value);

            return success ? value : null;
        }

        public string GetFullElementName(string shortName)
        {
            var matchedPairs = ShortElementNames.Where(pair => pair.Value == shortName);
            if (matchedPairs.Any())
            {
                return matchedPairs.First().Key;
            }
            return null;
        }

        public string FileName { get; set; }

        public Stream InputStream { get; set; }

        public Stream OutputStream { get; set; }

        public BabyScriptLogger Logger { get; set; }

        public Options Options { get; set; }
    }
}