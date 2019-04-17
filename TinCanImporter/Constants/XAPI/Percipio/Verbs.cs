using System;
using TinCan;

namespace TinCanImporter.Constants.XAPI.Percipio
{
    public static class Verbs
    {
        private static String BASE_VERB_URI = "https://xapi.percipio.com/xapi/verbs/";
        private static Verb CreateVerb(String action, String description = null)
        {
            var verb = new Verb();
            verb.id = new Uri(BASE_VERB_URI + action);
            verb.display = new LanguageMap();
            verb.display.Add("en-US", description ?? action);
            return verb;
        }

        public static Verb Arrived
        {
            get { return CreateVerb("arrived", "arrived at"); }
        }
       
    }
}