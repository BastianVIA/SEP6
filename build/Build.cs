using System;
using System.IO;
using System.IO.Compression;
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
using static Nuke.Common.IO.FileSystemTasks;




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
    AbsolutePath PublishDirectory => OutputDirectory / "Publish";
    AbsolutePath ZipFilePath => PublishDirectory / "app.zip";
    


    Target Clean => _ => _.Before(Restore)
        .Executes(() =>
        {
            var projectsToClean = Solution.AllProjects.Where(project => project.Name != "_build");

            foreach (var project in projectsToClean)
            {
                // EnsureCleanDirectory(project.Directory / "bin");
                // EnsureCleanDirectory(project.Directory / "obj");
                // EnsureCleanDirectory(PublishDirectory);
                
                (project.Directory / "bin").CreateOrCleanDirectory();
                (project.Directory / "obj").CreateOrCleanDirectory();
                PublishDirectory.CreateOrCleanDirectory();
               
            }
        });
    
    
    Target Restore => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s.SetProjectFile(Solution));
        });
    
    // Target Compile => _ => _
    //     .DependsOn(Restore)
    //     .Executes(() =>
    //     {
    //         DotNetBuild(s => s.SetProjectFile(Solution)
    //             .SetConfiguration(Configuration)
    //             .EnableNoRestore());
    //     });
    //
    
    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            var projectsToCompile = Solution.AllProjects.Where(project => project.Name != "TestBackend");

            foreach (var project in projectsToCompile)
            {
                DotNetBuild(s => s
                    .SetProjectFile(project)
                    .SetConfiguration(Configuration)
                    .EnableNoRestore());
            }
        });
    
    Target PublishBackend => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            AbsolutePath backendPublishDirectory = OutputDirectory / "Publish" / "Backend";
            DotNetPublish(s => s.SetProject(Solution.GetProject("Backend"))
                .SetConfiguration(Configuration)
                .SetOutput(backendPublishDirectory)
                .EnableNoBuild());
        });
    
    Target ZipBackend => _ => _
        .DependsOn(PublishBackend)
        .After(PublishBackend)
        .Executes(() =>
        {
            AbsolutePath backendZipFilePath = OutputDirectory / "Publish" / "Backend.zip";
            CompressionLevel compressionLevel = IsLocalBuild ? CompressionLevel.Fastest : CompressionLevel.Optimal;
        
            if (File.Exists(backendZipFilePath))
            {
                File.Delete(backendZipFilePath);
            }
        
            ZipFile.CreateFromDirectory(PublishDirectory / "Backend", backendZipFilePath, compressionLevel,
                includeBaseDirectory: false);
        });
    
    Target DeployBackend => _ => _
        .DependsOn(ZipBackend)
        .Executes(() =>
        {
            var backendPublishDirectory = OutputDirectory / "Publish" / "Backend";
    
            var zipFilePath = backendPublishDirectory / "backend.zip";
    
            ProcessTasks.StartProcess("az",
                $"webapp deployment source config-zip --src \"{zipFilePath}\" --name {AzureAppServiceBackendName} --resource-group {AzureResourceGroupName} --subscription {AzureSubscriptionId}",
                workingDirectory: backendPublishDirectory);
        });
    
    
    
    Target PublishFrontend => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            AbsolutePath frontendPublishDirectory = OutputDirectory / "Publish" / "Frontend";
            DotNetPublish(s => s.SetProject(Solution.GetProject("Frontend"))
                .SetConfiguration(Configuration)
                .SetOutput(frontendPublishDirectory)
                .EnableNoBuild());
        });
    
 
    Target ZipFrontend => _ => _
        .DependsOn(PublishFrontend)
        .After(PublishFrontend)
        .Executes(() =>
        {
            AbsolutePath frontendZipFilePath = OutputDirectory / "Publish" / "Frontend.zip";
            CompressionLevel compressionLevel = IsLocalBuild ? CompressionLevel.Fastest : CompressionLevel.Optimal;

            if (File.Exists(frontendZipFilePath))
            {
                File.Delete(frontendZipFilePath);
            }

            ZipFile.CreateFromDirectory(PublishDirectory / "Frontend", frontendZipFilePath, compressionLevel,
                includeBaseDirectory: false);
        });
    
    
    Target DeployFrontend => _ => _
        .DependsOn(ZipFrontend)
        .Executes(() =>
        {
            var frontendPublishDirectory = OutputDirectory / "Publish" / "Frontend";
    
            var zipFilePath = frontendPublishDirectory / "frontend.zip";
    
            ProcessTasks.StartProcess("az",
                $"webapp deployment source config-zip --src \"{zipFilePath}\" --name {AzureAppServiceFrontendName} --resource-group {AzureResourceGroupName} --subscription {AzureSubscriptionId}",
                workingDirectory: frontendPublishDirectory);
        });
    
    
    Target Deploy => _ => _
        .DependsOn(ZipBackend,ZipFrontend).Executes(() =>
        {
            if (Directory.Exists(PublishDirectory / "Backend"))
            {
                // DeleteDirectory(PublishDirectory / "Backend");
                (PublishDirectory / "Backend").DeleteDirectory();

            }
            
            if (Directory.Exists(PublishDirectory / "Frontend"))
            {
                // DeleteDirectory(PublishDirectory / "Frontend");
                (PublishDirectory / "Frontend").DeleteDirectory();

            }

        });
    
    
    string AzureSubscriptionId => Environment.GetEnvironmentVariable("AZURE_SUBSCRIPTION_ID");
    string AzureResourceGroupName => Environment.GetEnvironmentVariable("AZURE_RESOURCE_GROUP_NAME");
    string AzureAppServiceBackendName => Environment.GetEnvironmentVariable("AZURE_APP_SERVICE_BACKEND_NAME");
    string AzureAppServiceFrontendName => Environment.GetEnvironmentVariable("AZURE_APP_SERVICE_FRONTEND_NAME");
}