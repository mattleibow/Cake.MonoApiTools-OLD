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

            MonoApiInfo(context, new[] { assembly }, outputFile, null);
        }

        [CakeMethodAlias]
        public static void MonoApiInfo(this ICakeContext context, FilePath assembly, FilePath outputFile, MonoApiInfoSettings settings)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            MonoApiInfo(context, new[] { assembly }, outputFile, settings);
        }

        [CakeMethodAlias]
        public static void MonoApiInfo(this ICakeContext context, FilePath[] assemblies, FilePath outputFile)
        {
            MonoApiInfo(context, assemblies, outputFile, null);
        }

        [CakeMethodAlias]
        public static void MonoApiInfo(this ICakeContext context, FilePath[] assemblies, FilePath outputFile, MonoApiInfoSettings settings)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));
            if (outputFile == null)
                throw new ArgumentNullException(nameof(outputFile));

            var tool = new MonoApiInfoTool(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            tool.Execute(assemblies, outputFile, settings);
        }

        [CakeMethodAlias]
        public static void MonoApiDiff(this ICakeContext context, FilePath firstInfo, FilePath secondInfo, FilePath outputFile)
        {
            MonoApiDiff(context, firstInfo, secondInfo, outputFile, null);
        }

        [CakeMethodAlias]
        public static void MonoApiDiff(this ICakeContext context, FilePath firstInfo, FilePath secondInfo, FilePath outputFile, MonoApiDiffSettings settings)
        {
            if (firstInfo == null)
                throw new ArgumentNullException(nameof(firstInfo));
            if (secondInfo == null)
                throw new ArgumentNullException(nameof(secondInfo));
            if (outputFile == null)
                throw new ArgumentNullException(nameof(outputFile));

            var tool = new MonoApiDiffTool(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            tool.Execute(firstInfo, secondInfo, outputFile, settings);
        }
    }
}
