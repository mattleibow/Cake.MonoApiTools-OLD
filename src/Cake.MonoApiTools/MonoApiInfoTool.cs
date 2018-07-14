﻿using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.MonoApiTools
{
    public sealed class MonoApiInfoTool : Tool<MonoApiInfoSettings>
    {
        private ICakeEnvironment environment;

        public MonoApiInfoTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            this.environment = environment;
        }

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "mono-api-info.exe" };
        }

        protected override string GetToolName()
        {
            return "mono-api-info";
        }

        public void Execute(FilePath[] assemblies, MonoApiInfoSettings settings)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));
            if (assemblies.Length == 0)
                throw new ArgumentException("At least one assembly must be provided.", nameof(assemblies));

            settings = settings ?? new MonoApiInfoSettings();

            Run(settings, GetArguments(assemblies, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath[] assemblies, MonoApiInfoSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (settings.GenerateAbi)
                builder.Append("--abi");

            if (settings.FollowForwarders)
                builder.Append("--follow-forwarders");

            if (settings.SearchPaths != null)
            {
                foreach (var path in settings.SearchPaths)
                {
                    builder.AppendSwitchQuoted("--search-directory", "=", path.MakeAbsolute(environment).FullPath);
                }
            }

            if (settings.ResolvePaths != null)
            {
                foreach (var path in settings.ResolvePaths)
                {
                    builder.AppendSwitchQuoted("-r", "=", path.MakeAbsolute(environment).FullPath);
                }
            }

            if (settings.OutputPath != null)
                builder.AppendSwitchQuoted("-o", "=", settings.OutputPath.MakeAbsolute(environment).FullPath);

            if (settings.GenerateContractApi)
                builder.Append("--contract-api");

            foreach (var assembly in assemblies)
            {
                builder.AppendQuoted(assembly.MakeAbsolute(environment).FullPath);
            }

            return builder;
        }
    }
}
