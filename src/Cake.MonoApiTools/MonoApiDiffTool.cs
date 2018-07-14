using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.MonoApiTools
{
    public sealed class MonoApiDiffTool : Tool<MonoApiDiffSettings>
    {
        private ICakeEnvironment environment;
        private IFileSystem fileSystem;

        public MonoApiDiffTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            this.fileSystem = fileSystem;
            this.environment = environment;
        }

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "mono-api-diff.exe" };
        }

        protected override string GetToolName()
        {
            return "mono-api-diff";
        }

        public void Execute(FilePath firstInfo, FilePath secondInfo, FilePath outputPath, MonoApiDiffSettings settings)
        {
            if (firstInfo == null)
                throw new ArgumentNullException(nameof(firstInfo));
            if (secondInfo == null)
                throw new ArgumentNullException(nameof(secondInfo));
            if (outputPath == null)
                throw new ArgumentNullException(nameof(outputPath));

            settings = settings ?? new MonoApiDiffSettings();

            var processSettings = new ProcessSettings
            {
                Arguments = GetArguments(firstInfo, secondInfo, outputPath, settings),
                RedirectStandardOutput = true
            };

            Run(settings, null, processSettings, process =>
            {
                var contents = process.GetStandardOutput() ?? new string[0];

                var file = fileSystem.GetFile(outputPath.MakeAbsolute(environment));
                var dir = fileSystem.GetDirectory(file.Path.GetDirectory());

                if (!dir.Exists)
                    dir.Create();

                using (var stream = file.OpenWrite())
                using (var writer = new StreamWriter(stream))
                {
                    foreach (var line in contents)
                    {
                        writer.WriteLine(line);
                    }
                }
            });
        }

        private ProcessArgumentBuilder GetArguments(FilePath firstInfo, FilePath secondInfo, FilePath outputPath, MonoApiDiffSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.AppendQuoted(firstInfo.MakeAbsolute(environment).FullPath);

            builder.AppendQuoted(secondInfo.MakeAbsolute(environment).FullPath);

            return builder;
        }
    }
}
