using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using System.Linq;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Run);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("API key for pushing NuGet packages")] 
    readonly string NuGetApiKey;

    [Solution(GenerateProjects = true)] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath PackagesDirectory => ArtifactsDirectory / "packages";

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(config => config
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(config => config
                .SetConfiguration(Configuration)
                .SetProjectFile(Solution)
                .EnableNoRestore()
            );
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(config => config
                .SetProjectFile(Solution.AutoInject_Tests.Path)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
            );
        });

    Target Pack => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPack(config => config
                .SetConfiguration(Configuration)
                .SetProject(Solution)
                .SetOutputDirectory(PackagesDirectory)
                .EnableNoBuild()
                .EnableNoRestore()
            );
        });

    Target Push => _ => _
        .DependsOn(Pack)
        .Requires(() => NuGetApiKey)
        .Executes(() =>
        {
            var packages = System.IO.Directory.GetFiles(PackagesDirectory, "*.nupkg", System.IO.SearchOption.AllDirectories);
            foreach (var package in packages)
            {
                DotNetNuGetPush(s => s
                    .SetTargetPath(package)
                    .SetSource("https://api.nuget.org/v3/index.json")
                    .SetApiKey(NuGetApiKey)
                );
            }
        });

    Target Run => _ => _
        .DependsOn(Push)
        .Executes();
}
