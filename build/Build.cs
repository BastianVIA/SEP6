using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;

[UnsetVisualStudioEnvironmentVariables]
[DotNetVerbosityMapping]
class Build : NukeBuild
{
 
    public static int Main() => Execute<Build>(x => x.Deploy);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target Clean => _ => _.Before(Compile)
        .Executes(() =>
        {
            var projectsToClean = Solution.AllProjects.Where(project => project.Name != "_build");

            foreach (var project in projectsToClean)
            {
                EnsureCleanDirectory(project.Directory / "bin");
                EnsureCleanDirectory(project.Directory / "obj");
            }
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s.SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s.SetProjectFile(Solution)
                              .SetConfiguration(Configuration)
                              .EnableNoRestore());
        });

    Target Publish => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var publishDirectory = OutputDirectory / "Publish";

            DotNetPublish(s => s.SetProject(Solution)
                                .SetConfiguration(Configuration)
                                .SetOutput(publishDirectory)
                                .EnableNoBuild());
        });

    Target Deploy => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {
            var publishDirectory = OutputDirectory / "Publish";   

            var zipFilePath = publishDirectory / "app.zip";

            ProcessTasks.StartProcess("az", $"webapp deployment source config-zip --src \"{zipFilePath}\" --name {AzureAppServiceName} --resource-group {AzureResourceGroupName} --subscription {AzureSubscriptionId}", workingDirectory: publishDirectory);
        });
    
    string AzureSubscriptionId => Environment.GetEnvironmentVariable("AZURE_SUBSCRIPTION_ID");
    string AzureResourceGroupName => Environment.GetEnvironmentVariable("AZURE_RESOURCE_GROUP_NAME");
    string AzureAppServiceName => Environment.GetEnvironmentVariable("AZURE_APP_SERVICE_NAME");
}
