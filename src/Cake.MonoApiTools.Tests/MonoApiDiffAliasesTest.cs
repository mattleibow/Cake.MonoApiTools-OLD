using Cake.Core.IO;
using Cake.Testing;
using System;
using Xunit;

namespace Cake.MonoApiTools.Tests
{
    public class MonoApiDiffAliasesTest
    {
        [Theory]
        [InlineData("/bin/tools/mono-api-diff.exe", "/bin/tools/mono-api-diff.exe")]
        [InlineData("./tools/mono-api-diff.exe", "/Working/tools/mono-api-diff.exe")]
        public void Should_Use_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
        {
            // Given
            var fixture = new MonoApiDiffFixture();
            fixture.Settings.ToolPath = toolPath;
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Path.FullPath);
        }

        [Fact]
        public void Should_Throw_If_First_Input_File_Is_Null()
        {
            // Given
            var fixture = new MonoApiDiffFixture();
            fixture.FirstAssembly = null;

            // When + Then
            var result = Assert.Throws<ArgumentNullException>("firstAssembly", () => fixture.Run());
        }

        [Fact]
        public void Should_Throw_If_Second_Input_File_Is_Null()
        {
            // Given
            var fixture = new MonoApiDiffFixture();
            fixture.SecondAssembly = null;

            // When + Then
            var result = Assert.Throws<ArgumentNullException>("secondAssembly", () => fixture.Run());
        }

        [Fact]
        public void Should_Find_Executable_If_Tool_Path_Was_Not_Provided()
        {
            // Given
            var fixture = new MonoApiDiffFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("/Working/tools/mono-api-diff.exe", result.Path.FullPath);
        }

        [Fact]

        public void Should_Create_Correct_Command_Line_Arguments_When_Only_Assembly_Paths()
        {
            // Given
            var fixture = new MonoApiDiffFixture();
            fixture.OutputPath = null;

            // When
            var result = fixture.Run();

            // Then
            var args =
                "\"/Working/version-one.dll\" " +
                "\"/Working/version-two.dll\"";
            Assert.Equal(args, result.Args);
        }

        [Fact]
        public void Should_Create_Correct_Command_Line_Arguments_For_Assembly_And_Output()
        {
            // Given
            var fixture = new MonoApiDiffFixture();

            // When
            var result = fixture.Run();

            // Then
            var args =
                "\"/Working/version-one.dll\" " +
                "\"/Working/version-two.dll\"";
            Assert.Equal(args, result.Args);
        }
    }
}
