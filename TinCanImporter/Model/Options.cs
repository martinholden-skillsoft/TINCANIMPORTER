using CommandLine.Attributes;

namespace TinCanImporter.Model
{
    class Options
    {
        [RequiredArgument(0, "lrs", "The full URI to the LRS endpoint.")]
        public string lrs { get; set; }

        [RequiredArgument(1, "username", "The LRS username for the API")]
        public string username { get; set; }

        [RequiredArgument(2, "password", "The LRS username for the API")]
        public string password { get; set; }

        [RequiredArgument(3, "usagecsv", "The filename for the CSV usage data")]
        public string usagecsv { get; set; }

        [RequiredArgument(4, "assetcsv", "The filename for the CSV asset data")]
        public string assetcsv { get; set; }

        [OptionalArgument(20,"batchsize", "The batch size to use")]
        public int batchsize { get; set; }
    }

}
