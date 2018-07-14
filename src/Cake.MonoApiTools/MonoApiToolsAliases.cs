using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.MonoApiTools
{
    /// <summary>
    /// Contains functionality to work with the Mono API Tools.
    /// </summary>
    [CakeAliasCategory("MonoApiTools")]
    public static class MonoApiToolsAliases
    {
        [CakeMethodAlias]
        public static void MonoApiInfo(this ICakeContext context, FilePath assembly, FilePath outputFile)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            if (outputFile == null)
                throw new ArgumentNullException(nameof(outputFile));

            MonoApiInfo(context, new[] { assembly }, new MonoApiInfoSettings { OutputPath = outputFile });
        }

        [CakeMethodAlias]
        public static void MonoApiInfo(this ICakeContext context, FilePath assembly, MonoApiInfoSettings settings)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            MonoApiInfo(context, new[] { assembly }, settings);
        }

        [CakeMethodAlias]
        public static void MonoApiInfo(this ICakeContext context, FilePath[] assemblies, FilePath outputFile)
        {
            if (outputFile == null)
                throw new ArgumentNullException(nameof(outputFile));

            MonoApiInfo(context, assemblies, new MonoApiInfoSettings { OutputPath = outputFile });
        }

        [CakeMethodAlias]
        public static void MonoApiInfo(this ICakeContext context, FilePath[] assemblies, MonoApiInfoSettings settings)
        {
            var tool = new MonoApiInfoTool(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            tool.Execute(assemblies, settings);
        }
    }
}
