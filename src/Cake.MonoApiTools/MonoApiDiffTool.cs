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

        public void Execute(FilePath firstAssembly, FilePath secondAssembly, MonoApiDiffSettings settings)
        {
            if (firstAssembly == null)
                throw new ArgumentNullException(nameof(firstAssembly));
            if (secondAssembly == null)
                throw new ArgumentNullException(nameof(secondAssembly));

            settings = settings ?? new MonoApiDiffSettings();

            var processSettings = new ProcessSettings
            {
                Arguments = GetArguments(firstAssembly, secondAssembly, settings),
                RedirectStandardOutput = true
            };

            Run(settings, null, processSettings, process =>
            {
                if (settings.OutputPath != null)
                {
                    var contents = process.GetStandardOutput() ?? new string[0];

                    var file = fileSystem.GetFile(settings.OutputPath);
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
                }
            });
        }

        private ProcessArgumentBuilder GetArguments(FilePath firstAssembly, FilePath secondAssembly, MonoApiDiffSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.AppendQuoted(firstAssembly.MakeAbsolute(environment).FullPath);

            builder.AppendQuoted(secondAssembly.MakeAbsolute(environment).FullPath);

            return builder;
        }
    }
}
