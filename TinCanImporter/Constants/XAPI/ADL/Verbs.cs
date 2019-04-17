using System;
using TinCan;

namespace TinCanImporter.Constants.XAPI.ADL
{
    public static class Verbs
    {
        private static String BASE_VERB_URI = "http://adlnet.gov/expapi/verbs/";
        private static Verb CreateVerb(String action, String description = null)
        {
            var verb = new Verb();
            verb.id = new Uri(BASE_VERB_URI + action);
            verb.display = new LanguageMap();
            verb.display.Add("en-US", description ?? action);
            return verb;
        }

        public static Verb Answered
        {
            get { return CreateVerb("answered"); }
        }
        public static Verb Asked
        {
            get { return CreateVerb("asked"); }
        }
        public static Verb Attempted
        {
            get { return CreateVerb("attempted"); }
        }
        public static Verb Attended
        {
            get { return CreateVerb("attended"); }
        }
        public static Verb Commented
        {
            get { return CreateVerb("commented"); }
        }
        public static Verb Completed
        {
            get { return CreateVerb("completed"); }
        }
        public static Verb Exited
        {
            get { return CreateVerb("exited"); }
        }
        public static Verb Experienced
        {
            get { return CreateVerb("experienced"); }
        }
        public static Verb Failed
        {
            get { return CreateVerb("failed"); }
        }
        public static Verb Imported
        {
            get { return CreateVerb("imported"); }
        }
        public static Verb Initialized
        {
            get { return CreateVerb("initialized"); }
        }
        public static Verb Interacted
        {
            get { return CreateVerb("interacted"); }
        }
        public static Verb Launched
        {
            get { return CreateVerb("launched"); }
        }
        public static Verb Mastered
        {
            get { return CreateVerb("mastered"); }
        }
        public static Verb Passed
        {
            get { return CreateVerb("passed"); }
        }
        public static Verb Preferred
        {
            get { return CreateVerb("preferred"); }
        }
        public static Verb Progressed
        {
            get { return CreateVerb("progressed"); }
        }
        public static Verb Registered
        {
            get { return CreateVerb("registered"); }
        }
        public static Verb Responded
        {
            get { return CreateVerb("responded"); }
        }
        public static Verb Resumed
        {
            get { return CreateVerb("resumed"); }
        }
        public static Verb Scored
        {
            get { return CreateVerb("scored"); }
        }
        public static Verb Shared
        {
            get { return CreateVerb("shared"); }
        }
        public static Verb Suspended
        {
            get { return CreateVerb("suspended"); }
        }
        public static Verb Terminated
        {
            get { return CreateVerb("terminated"); }
        }
        public static Verb Voided
        {
            get { return CreateVerb("voided"); }
        }
    }
}