using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TinCan;
using TinCanImporter.Extensions;

namespace TinCanImporter.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// 

    [Serializable]
    public class SkillportUsageRecord
    {
        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the time span.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the double.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static Double? GetDouble(string input)
        {
            Double result;

            if (Double.TryParse(input, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Build Share Link for the specified asset type.
        /// </summary>
        /// <returns></returns>
        private string GetShareLink()
        {
            string result;

            switch (this.ASSETTYPE.ToUpper())
            {
                case "LEARNING PORTALS":
                    result = String.Format("https://{0}/skillportfe/main.action?path=summary/PORTAL/{1}", this.SKILLPORTSITE, this.ASSETID);
                    break;
                case "COURSES":
                    result = String.Format("https://{0}/skillportfe/main.action?path=summary/COURSES/{1}", this.SKILLPORTSITE, this.ASSETID);
                    break;
                case "CUSTOM":
                    result = String.Format("https://{0}/skillportfe/main.action?path=summary/COURSES/{1}", this.SKILLPORTSITE, this.ASSETID);
                    break;
                case "BOOKS":
                    result = String.Format("https://{0}/skillportfe/main.action?path=summary/BOOKS/{1}", this.SKILLPORTSITE, this.ASSETID);
                    break;
                case "VIDEOS":
                    result = String.Format("https://{0}/skillportfe/main.action?path=summary/VIDEOS/{1}", this.SKILLPORTSITE, this.ASSETID);
                    break;
                case "RESOURCES":
                    result = String.Format("https://{0}/skillportfe/main.action?path=summary/RESOURCES/{1}", this.SKILLPORTSITE, this.ASSETID);
                    break;
                default:
                    result = String.Format("https://{0}/skillportfe/main.action?path=summary/COURSES/{1}", this.SKILLPORTSITE, this.ASSETID);
                    break;
            }

            return result;
        }

        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero)
            {
                return "PT0S";
            }

            var builder = new StringBuilder("P");

            if (timeSpan.TotalDays > 7)
            {
                var weeks = Math.Floor(timeSpan.TotalDays / 7);
                builder.Append($"{weeks}W");

                timeSpan = timeSpan.Subtract(TimeSpan.FromDays(weeks * 7));
            }

            if (timeSpan.Days > 0)
            {
                builder.Append($"{timeSpan.Days}D");

                timeSpan = timeSpan.Subtract(TimeSpan.FromDays(timeSpan.Days));
            }

            if (timeSpan.TotalSeconds > 0)
            {
                builder.Append("T");
            }

            if (timeSpan.Hours > 0)
            {
                builder.Append($"{timeSpan.Hours}H");

                timeSpan = timeSpan.Subtract(TimeSpan.FromHours(timeSpan.Hours));
            }

            if (timeSpan.Minutes > 0)
            {
                builder.Append($"{timeSpan.Minutes}M");

                timeSpan = timeSpan.Subtract(TimeSpan.FromMinutes(timeSpan.Minutes));
            }

            if (timeSpan.TotalSeconds > 0)
            {
                builder.Append($"{timeSpan.TotalSeconds}S");
            }

            var result = builder.ToString();
            if (result == "P")
            {
                return string.Empty;
            }

            return result;
        }



        /// <summary>
        /// Gets or sets the skillportsite.
        /// </summary>
        /// <value>
        /// The skillportsite.
        /// </value>
        public string SKILLPORTSITE { get; set; }
        /// <summary>
        /// Gets or sets the skillportid.
        /// </summary>
        /// <value>
        /// The skillportid.
        /// </value>
        public string SKILLPORTID { get; set; }
        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        /// <value>
        /// The firstname.
        /// </value>
        public string FIRSTNAME { get; set; }
        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        /// <value>
        /// The lastname.
        /// </value>
        public string LASTNAME { get; set; }
        /// <summary>
        /// Gets or sets the assetid.
        /// </summary>
        /// <value>
        /// The assetid.
        /// </value>
        public string ASSETID { get; set; }
        /// <summary>
        /// Gets or sets the assettype.
        /// </summary>
        /// <value>
        /// The assettype.
        /// </value>
        public string ASSETTYPE { get; set; }
        /// <summary>
        /// Gets or sets the assettitle.
        /// </summary>
        /// <value>
        /// The assettitle.
        /// </value>
        public string ASSETTITLE { get; set; }
        /// <summary>
        /// Gets or sets the assetdescripion.
        /// </summary>
        /// <value>
        /// The assetdescripion.
        /// </value>
        public string ASSETDESCRIPION { get; set; }
        /// <summary>
        /// Gets or sets the assetlanguage.
        /// </summary>
        /// <value>
        /// The assetlanguage.
        /// </value>
        public string ASSETLANGUAGE { get; set; }
        /// <summary>
        /// Gets or sets the lastaccessdate.
        /// </summary>
        /// <value>
        /// The lastaccessdate.
        /// </value>
        public string LASTACCESSDATE { get; set; }
        /// <summary>
        /// Gets or sets the completeddate.
        /// </summary>
        /// <value>
        /// The completeddate.
        /// </value>
        public string COMPLETEDDATE { get; set; }
        /// <summary>
        /// Gets or sets the highscore.
        /// </summary>
        /// <value>
        /// The highscore.
        /// </value>
        public string HIGHSCORE { get; set; }
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public string DURATION { get; set; }
        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        /// <value>
        /// The locale.
        /// </value>
        public string LOCALE { get; set; }

        /// <summary>
        /// Gets the lastaccessdatestamp.
        /// </summary>
        /// <value>
        /// The lastaccessdatestamp.
        /// </value>
        public DateTime? LASTACCESSDATESTAMP
        {
            get
            {
                return GetDateTime(this.LASTACCESSDATE);
            }
        }

        /// <summary>
        /// Gets the completeddatestamp.
        /// </summary>
        /// <value>
        /// The completeddatestamp.
        /// </value>
        public DateTime? COMPLETEDDATESTAMP
        {
            get
            {
                return GetDateTime(this.COMPLETEDDATE);
            }
        }

        /// <summary>
        /// Gets the durationtimespan.
        /// </summary>
        /// <value>
        /// The durationtimespan.
        /// </value>
        public TimeSpan? DURATIONTIMESPAN
        {
            get
            {
                return GetTimeSpan(this.DURATION);
            }
        }

        /// <summary>
        /// Gets the scoremin.
        /// </summary>
        /// <value>
        /// The scoremin.
        /// </value>
        public int SCOREMIN { get; private set; } = 0;
        /// <summary>
        /// Gets the scoremax.
        /// </summary>
        /// <value>
        /// The scoremax.
        /// </value>
        public int SCOREMAX { get; private set; } = 100;

        /// <summary>
        /// Gets the scoreraw.
        /// </summary>
        /// <value>
        /// The scoreraw.
        /// </value>
        public Double? SCORERAW
        {
            get
            {
                return GetDouble(this.HIGHSCORE);
            }
        }

        /// <summary>
        /// Gets the scorescaled.
        /// </summary>
        /// <value>
        /// The scorescaled.
        /// </value>
        public Double? SCORESCALED
        {
            get
            {
                if (SCORERAW != null)
                {
                    return SCORERAW / SCOREMAX;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the fullname.
        /// </summary>
        /// <value>
        /// The fullname.
        /// </value>
        public string FULLNAME
        {
            get
            {
                return FIRSTNAME + " " + LASTNAME;
            }
        }

        /// <summary>
        /// Gets the assetpath.
        /// </summary>
        /// <value>
        /// The assetpath.
        /// </value>
        public string ASSETPATH
        {
            get
            {
                return GetShareLink();
            }
        }

        public Guid STATEMENTID
        {
            get; set;
        }

        public DateTime STATEMENTTIMESTAMP
        {
            get
            {
                return this.COMPLETEDDATESTAMP == null ? (DateTime)this.LASTACCESSDATESTAMP : (DateTime)this.COMPLETEDDATESTAMP;
            }
        }

        public string STATEMENTTIMESTAMPFORMATTED
        {
            get
            {
                return STATEMENTTIMESTAMP.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
        }

        public string DURATIONTIMESPANFORMATTED
        {
            get
            {
                return XmlConvert.ToString((TimeSpan)DURATIONTIMESPAN);
            }
        }
    }
}
