using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.MonoApiTools.Tests
{
    internal sealed class MonoApiDiffFixture : ToolFixture<MonoApiDiffSettings>
    {
        public MonoApiDiffFixture()
            : base("mono-api-diff.exe")
        {
            FirstAssembly = "version-one.dll";
            SecondAssembly = "version-two.dll";
            OutputPath = "diff.xml";
        }

        public FilePath FirstAssembly { get; set; }

        public FilePath SecondAssembly { get; set; }

        public FilePath OutputPath { get; set; }

        protected override void RunTool()
        {
            var tool = new MonoApiDiffTool(FileSystem, Environment, ProcessRunner, Tools);

            if (OutputPath != null)
                Settings.OutputPath = OutputPath;

            tool.Execute(FirstAssembly, SecondAssembly, Settings);
        }
    }
}
