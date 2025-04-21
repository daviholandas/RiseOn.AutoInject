using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Run);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution(GenerateProjects = true)] readonly Solution Solution;

    Target PackToTest => _ => _
        .DependsOn(Building)
        .Executes(() =>
        {
            DotNetPack(config => config
                .SetProject(Solution.AutoInject_SourceGenerator)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(Solution.AutoInject_SourceGenerator.Directory + "/artifacts")
                .SetVersion("0.0.1-beta")
                );
        });

    Target Building => _ => _
        .Executes(() =>
        {
            DotNetBuild(config => config
                .SetProjectFile(Solution.AutoInject_SourceGenerator)
                .SetConfiguration(Configuration)
            );
        });

    Target Run => _ => _
        .Executes();
}
