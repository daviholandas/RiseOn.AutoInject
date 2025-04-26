using System.Collections.Generic;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Run);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution(GenerateProjects = true)] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath PackagesDirectory => ArtifactsDirectory / "packages";

    Target RestoreToTests => _ => _
        .DependsOn(PackToTest)
        .Executes(() =>
        {
            DotNetRestore(config => config
                .SetConfigFile(RootDirectory / "nuget-tests.config")
                .SetProjectFile(Solution)
            );
        });

    Target PackToTest => _ => _
        .DependsOn(Building)
        .Executes(() =>
        {
            DotNetPack(config => config
                .SetConfiguration(Configuration)
                .SetOutputDirectory(PackagesDirectory)
                .SetVersion("0.0.1-beta")
                .SetProject(Solution)
                );
        });

    Target Building => _ => _
        .Executes(() =>
        {
            DotNetBuild(config => config
                .SetConfiguration(Configuration)
                .SetProjectFile(Solution)
            );
        });

    Target Run => _ => _
        .Executes();
}
