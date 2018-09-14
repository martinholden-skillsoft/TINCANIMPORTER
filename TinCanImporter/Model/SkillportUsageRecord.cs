using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinCanImporter.Model
{
    public class SkillportUsageRecord
    {
       

        private static DateTime? GetDateTime(string input)
        {
            //We assume report uses GMT/UTC
            DateTime result;
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            if (DateTime.TryParse(input, culture, System.Globalization.DateTimeStyles.AssumeUniversal, out result))
            {
                return result;
            }
            return null;
        }

        private static TimeSpan? GetTimeSpan(string input)
        {
            //We assume report uses GMT/UTC
            TimeSpan result;
            if (TimeSpan.TryParse(input, out result))
            {
                return result;
            }
            return null;
        }

        private static Double? GetDouble(string input)
        {
            Double result;

            if (Double.TryParse(input, out result))
            {
                return result;
            }
            return null;
        }


        public string SKILLPORTSITE { get; set; }
        public string SKILLPORTID { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string ASSETID { get; set; }
        public string ASSETTYPE { get; set; }
        public string ASSETTITLE { get; set; }
        public string ASSETLANGUAGE { get; set; }
        public string ASSETUNIQUEPATH { get; set; }
        public string LASTACCESSDATE { get; set; }
        public string COMPLETEDDATE { get; set; }
        public string HIGHSCORE { get; set; }
        public string DURATION { get; set; }


        public string LOCALE { get; set; }
        public DateTime? LASTACCESSDATESTAMP()
        {
            return GetDateTime(this.LASTACCESSDATE);
        }
        
        public DateTime? COMPLETEDDATESTAMP()
        {
            return GetDateTime(this.COMPLETEDDATE);
        }

        public TimeSpan? DURATIONTIMESPAN()
        {
            return GetTimeSpan(this.DURATION);
        }


        public int SCOREMIN { get; private set; } = 0;
        public int SCOREMAX { get; private set; } = 100;
        public Double? SCORERAW()
        {
            return GetDouble(this.HIGHSCORE);
        }



        public Double? SCORESCALED()
        {
            if (SCORERAW() != null)
            {
                return SCORERAW() / SCOREMAX;
            }
            return null;

        }

        public string FULLNAME()
        {
            return FIRSTNAME + " " + LASTNAME;
        }
    }
}
