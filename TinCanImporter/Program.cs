using CommandLine;
using Common.Logging;
using ExcelDataReader;
using MoreLinq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinCan;
using TinCan.LRSResponses;
using TinCanImporter.Model;

namespace TinCanImporter
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        /// <summary>
        /// The log
        /// </summary>
        public static ILog log;

        /// <summary>
        /// The locale lookup
        /// </summary>
        private static Hashtable localeLookup = GetLanguageHashtable();

        /// <summary>
        /// Gets the language hashtable.
        /// </summary>
        /// <returns></returns>
        private static Hashtable GetLanguageHashtable()
        {
            // Create and return new Hashtable.
            Hashtable hashtable = new Hashtable();
            hashtable.Add("ARABIC", "ar");
            hashtable.Add("BOKMAL", "nb-no");
            hashtable.Add("CHINESE CANTONESE", "zh-yue");
            hashtable.Add("CHINESE MANDARIN", "zh");
            hashtable.Add("CHINESE SIMPLIFIED", "az");
            hashtable.Add("CHINESE TRADITIONAL", "zh-tw");
            hashtable.Add("CZECH", "cs");
            hashtable.Add("DANISH", "da");
            hashtable.Add("DUTCH", "nl");
            hashtable.Add("ENGLISH", "en-us");
            hashtable.Add("ENGLISH (ALL)", "en");
            hashtable.Add("ENGLISH (INDIA)", "en-in");
            hashtable.Add("FINNISH", "fi");
            hashtable.Add("FRENCH", "fr");
            hashtable.Add("GERMAN", "de");
            hashtable.Add("GREEK", "el");
            hashtable.Add("HINDI", "hi");
            hashtable.Add("HUNGARIAN", "hu");
            hashtable.Add("INDONESIAN", "id");
            hashtable.Add("ITALIAN", "it");
            hashtable.Add("JAPANESE", "ja");
            hashtable.Add("KOREAN", "ko");
            hashtable.Add("MALAY", "ms-my");
            hashtable.Add("NOT SPECIFIED", "en-us");
            hashtable.Add("POLISH", "pl");
            hashtable.Add("PORTUGUESE", "pt");
            hashtable.Add("ROMANIAN", "ro");
            hashtable.Add("RUSSIAN", "ru");
            hashtable.Add("SPANISH", "es");
            hashtable.Add("SWEDISH", "sv");
            hashtable.Add("THAI", "th");
            hashtable.Add("TURKISH", "tr");
            hashtable.Add("UK ENGLISH", "en-gb");
            hashtable.Add("VIETNAMESE", "vi");
            return hashtable;
        }

        /// <summary>
        /// Gets the statement.
        /// </summary>
        /// <param name="usageRecord">The usage record.</param>
        /// <returns></returns>
        private static Statement GetStatement(SkillportUsageRecord usageRecord)
        {

            var agent = new Agent();
            agent.name = usageRecord.FULLNAME();
            agent.account = new AgentAccount();
            agent.account.name = usageRecord.SKILLPORTID;
            agent.account.homePage = new Uri(string.Format("https://{0}", usageRecord.SKILLPORTSITE));

            var verb = new Verb();
            verb.id = new Uri("http://adlnet.gov/expapi/verbs/launched");
            verb.display = new LanguageMap();
            verb.display.Add("en-US", "launched");


            //This will be used for generating a UUID for the statement
            //We incorporate the site, userid, assetpath, verb id, lastaccessdate
            //if completed we also add completed date
            //The dates are included so we can support multiple completions.
            string uniqueString = String.Join("~", usageRecord.SKILLPORTSITE, usageRecord.SKILLPORTID, usageRecord.ASSETUNIQUEPATH, verb.id, usageRecord.LASTACCESSDATESTAMP()).ToUpperInvariant();

            Result result = null;

            DateTime? timeStampToUse = usageRecord.LASTACCESSDATESTAMP();

            if (usageRecord.COMPLETEDDATESTAMP() != null)
            {
                verb.id = new Uri("http://adlnet.gov/expapi/verbs/completed");
                verb.display = new LanguageMap();
                verb.display.Add("en-US", "completed");

                result = new Result();
                result.completion = true;
                result.success = true;
                result.duration = usageRecord.DURATIONTIMESPAN();

                if (usageRecord.SCORERAW() != null)
                {
                    result.score = new Score();
                    result.score.min = usageRecord.SCOREMIN;
                    result.score.max = usageRecord.SCOREMAX;
                    result.score.raw = usageRecord.SCORERAW();
                    result.score.scaled = usageRecord.SCORESCALED();
                }

                timeStampToUse = usageRecord.COMPLETEDDATESTAMP();

                uniqueString = String.Join("~", uniqueString, usageRecord.COMPLETEDDATESTAMP()).ToUpperInvariant();
            }

            var activity = new Activity();
            activity.id = usageRecord.ASSETUNIQUEPATH;
            activity.definition = new ActivityDefinition();
            activity.definition.type = new Uri("http://adlnet.gov/expapi/activities/course");
            activity.definition.name = new LanguageMap();
            activity.definition.name.Add(usageRecord.LOCALE, usageRecord.ASSETTITLE);

            var statement = new Statement();
            statement.actor = agent;
            statement.verb = verb;
            statement.target = activity;
            statement.timestamp = timeStampToUse;
            if (result != null)
            {
                statement.result = result;
            }





            //Now we need to create a unique ID for this user/activityid so we can avoid duplicating
            //We will do this by concatenating
            Guid namespaceGuid = Helpers.GuidUtility.Create(Helpers.GuidUtility.UrlNamespace, usageRecord.SKILLPORTSITE);
            Guid statementGuid = Helpers.GuidUtility.Create(namespaceGuid, uniqueString);

            statement.id = statementGuid;

            if (log.IsTraceEnabled)
            {
                log.TraceFormat("Statement: {0}", statement.ToJSON().Replace("\"", "\"\""));
            }

            return statement;
        }

        /// <summary>
        /// Gets the data table from CSV.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        private static DataTable GetDataTableFromCSV(string fileName, ExcelReaderConfiguration config = null)
        {
            ExcelDataSetConfiguration FirstRowColumnNamesConfiguration = new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (reader) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            };

            DataSet ds;
            using (var filestream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var excelReader = ExcelReaderFactory.CreateCsvReader(filestream, config))
                {
                    ds = excelReader.AsDataSet(FirstRowColumnNamesConfiguration);
                }
            }
            if (ds.Tables != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the source data.
        /// </summary>
        /// <param name="userFilename">The user filename.</param>
        /// <param name="assetFilename">The asset filename.</param>
        /// <returns></returns>
        static IEnumerable<SkillportUsageRecord> GetSourceData(string userFilename, string assetFilename)
        {
            log.Info("Loading Source Data");
            var usage = GetDataTableFromCSV(userFilename).AsEnumerable();
            //SKILLPORTSITE	SKILLPORTID	FIRSTNAME	LASTNAME	ASSETID	ASSETTITLE	LASTACCESSDATE	COMPLETEDDATE	HIGHSCORE

            var assets = GetDataTableFromCSV(assetFilename).AsEnumerable();
            //ASSETID	ASSETTYPE	ASSETTITLE	LANGUAGE	UNIQUEPATH

            IEnumerable<SkillportUsageRecord> importData = null;

            if (assets != null && usage != null)
            {
                log.DebugFormat("Usage Record Count: {0} Assets Record Count: {1}", usage.Count(), assets.Count());


                importData = usage.Join(assets, a => a.Field<string>("ASSETID"), u => u.Field<string>("ASSETID"),
                    (u, a) => new SkillportUsageRecord()
                    {
                        SKILLPORTSITE = u.Field<string>("SKILLPORTSITE"),
                        SKILLPORTID = u.Field<string>("SKILLPORTID"),
                        FIRSTNAME = u.Field<string>("FIRSTNAME"),
                        LASTNAME = u.Field<string>("LASTNAME"),
                        ASSETID = a.Field<string>("ASSETID"),
                        ASSETTYPE = a.Field<string>("ASSETTYPE"),
                        ASSETTITLE = a.Field<string>("ASSETTITLE"),
                        ASSETLANGUAGE = a.Field<string>("LANGUAGE"),
                        ASSETUNIQUEPATH = a.Field<string>("UNIQUEPATH"),
                        LASTACCESSDATE = u.Field<string>("LASTACCESSDATE"),
                        COMPLETEDDATE = u.Field<string>("COMPLETEDDATE"),
                        HIGHSCORE = u.Field<string>("HIGHSCORE"),
                        DURATION = u.Field<string>("DURATION"),
                        LOCALE = (string)localeLookup[a.Field<string>("LANGUAGE").ToUpper()],
                    });

                log.DebugFormat("Consolidated Record Count: {0}", importData.Count());

            }
            return importData;
        }

        private static void Process(Options options)
        {
            var importData = GetSourceData(options.usagecsv, options.assetcsv);

            var lrsStore = new RemoteLRS(
                options.lrs,
                options.username,
               options.password
            );

            log.DebugFormat("Processing Records in Batches of {0}", options.batchsize);
            int batchCounter = 0;
            foreach (var batch in importData.Batch(options.batchsize))
            {
                batchCounter++;
                List<Statement> statementBatch = new List<Statement>();
                foreach (var item in batch)
                {
                    Statement statement = GetStatement(item);
                    statementBatch.Add(statement);

                }
                log.DebugFormat("Created xAPI Statement Batch: {0} Record Count: {1}", batchCounter, statementBatch.Count());

                StatementsResultLRSResponse lrsResponse = lrsStore.SaveStatements(statementBatch);

                if (lrsResponse.success)
                {
                    log.Info("LRS Batch Save Successful");
                }
                else
                {
                    // Do something with failure
                    log.ErrorFormat("LRS Batch Save Failed. {0}", lrsResponse.errMsg);
                }
                
            }
        }


        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            log = LogManager.GetLogger("TINCANIMPORTER");
            log.Info("Processing Started");
            Helpers.Networking.EnableTLS12();

            if (!Parser.TryParse(args, out Options options))
            {
                return;
            }
            try
            {
                Process(options);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Issue while Processing. Exception: {0}", ex, ex.ToString());
            }

        }
    }
}
