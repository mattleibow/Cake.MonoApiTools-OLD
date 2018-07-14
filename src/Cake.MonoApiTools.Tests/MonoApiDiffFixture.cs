using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.MonoApiTools.Tests
{
    internal sealed class MonoApiDiffFixture : ToolFixture<MonoApiDiffSettings>
    {
        public MonoApiDiffFixture()
            : base("mono-api-diff.exe")
        {
            FirstInfo = "version-one.dll";
            SecondInfo = "version-two.dll";
            OutputPath = "diff.xml";
        }

        public FilePath FirstInfo { get; set; }

        public FilePath SecondInfo { get; set; }

        public FilePath OutputPath { get; set; }

        protected override void RunTool()
        {
            var tool = new MonoApiDiffTool(FileSystem, Environment, ProcessRunner, Tools);

            tool.Execute(FirstInfo, SecondInfo, OutputPath, Settings);
        }
    }
}
