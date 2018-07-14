using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.MonoApiTools
{
    public class MonoApiInfoSettings : ToolSettings
    {
        public bool GenerateAbi { get; set; }

        public bool GenerateContractApi { get; set; }

        public bool FollowForwarders { get; set; }

        public DirectoryPath[] SearchPaths { get; set; }

        public FilePath[] ResolvePaths { get; set; }

        public FilePath OutputPath { get; set; }
    }
}
