using Cake.Core.Tooling;

namespace Cake.MonoApiTools
{
    public enum MonoApiHtmlOutputFormat
    {
        Html,
        Markdown
    }

    public class MonoApiHtmlSettings : ToolSettings
    {
        public string[] Ignore { get; set; }

        public string[] IgnoreAdded { get; set; }

        public string[] IgnoreRemoved { get; set; }

        public string[] IgnoreNew { get; set; }

        public bool IgnoreChangedParameterNames { get; set; }

        public bool IgnoreChangedPropertySetters { get; set; }

        public bool IgnoreChangedVirtual { get; set; }

        public bool IgnoreNonBreaking { get; set; }

        public bool IgnoreDuplicateXml { get; set; }

        public bool Colorize { get; set; }

        public bool Verbose { get; set; }

        public MonoApiHtmlOutputFormat OutputFormat { get; set; }
    }
}
