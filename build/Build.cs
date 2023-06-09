using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.IO.FileSystemTasks;
using Nuke.Common.Tools.Xunit;
using Nuke.Common.Tools.ReportGenerator;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;



[UnsetVisualStudioEnvironmentVariables]
[DotNetVerbosityMapping]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.LocalBuild);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [PathExecutable] readonly Tool ReportGenerator;

    
    [Parameter("The build id used as the 4. number in the .Net version number")] 
    readonly int BuildId = 0;
    Version Version => new Version(1,0 , 0, BuildId);
    
    Target CiBuild => _ => _
        .DependsOn(Clean, Restore, Compile,Test,ZipBackend,PublishFrontend,ZipFrontend,CleanPublishFolder);
    Target LocalBuild => _ => _
        .DependsOn(Clean, Restore, Compile,Test,PublishBackend,PublishFrontend,ZipBackend,ZipFrontend);
    
    AbsolutePath OutputDirectory => RootDirectory / "output";
    AbsolutePath PublishDirectory => OutputDirectory / "Publish";
    


    Target Clean => _ => _.Before(Restore)
        .Executes(() =>
        {
            EnsureExistingDirectory(OutputDirectory);
        
            var projectsToClean = Solution.AllProjects.Where(project => project.Name != "_build");

            foreach (var project in projectsToClean)
            {
                EnsureCleanDirectory(project.Directory / "bin");
                EnsureCleanDirectory(project.Directory / "obj");
                EnsureCleanDirectory(PublishDirectory);
            }
        });
    
    
    
    
    Target Restore => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s.SetProjectFile(Solution));
            DotNetRestore(s => s.SetProjectFile(Solution.GetProject("TestBackend")));

        });
    
    
    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {

            var projectsToCompile = Solution.AllProjects;
            foreach (var project in projectsToCompile)
            {
                DotNetBuild(s => s
                    .SetProjectFile(project)
                    .SetConfiguration(Configuration)
                    .EnableNoRestore());
            }
        });
    
 
    
    // Target Test => _ => _
    //     .DependsOn(Compile)
    //     .After(Compile)
    //     .Before(PublishBackend,PublishFrontend)
    //     .Executes(() =>
    //     {
    //         DotNetTest(_ => _
    //             .SetProjectFile(Solution.GetProject("TestBackend"))
    //             .SetConfiguration(Configuration)
    //
    //             .EnableNoBuild());
    //     });


    Target Test => _ => _
        .DependsOn(Compile)
        .After(Compile)
        .Before(PublishBackend, PublishFrontend)
        .Executes(() =>
        {
    
            EnsureCleanDirectory(PublishDirectory);
    
            DotNetTest(_ => _
                    .SetProjectFile(Solution.GetProject("TestBackend"))
                    .SetConfiguration(Configuration)
                    .EnableNoBuild()
                    .SetProcessArgumentConfigurator(a => a.Add("--logger:trx")) // Set logger to trx for XML report
                    .SetResultsDirectory(PublishDirectory) 
            );
        });
    

    public void CreateInfoFile(Version version, GitRepository gitRepository, string versionInfoPath)
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append("Git repository " + gitRepository.HttpsUrl + "\n");
        stringBuilder.Append("Git sha key " + gitRepository.Commit + "\n");
        stringBuilder.Append("Build version " + version + "\n");
        stringBuilder.Append("Date and Time " + DateTime.Now);

        File.WriteAllText(versionInfoPath, stringBuilder.ToString());
    }
    
    
    Target VerifyOutput => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            ControlFlow.Assert(Directory.Exists(OutputDirectory), $"Output directory does not exist: {OutputDirectory}");
            // TODO check if i can set the movie.db with the backend or not
        });
    
   
    
    Target PublishBackend => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        { 
            CreateInfoFile(Version, GitRepository, PublishDirectory / "VersionInfo.txt");

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
    
    Target CleanPublishFolder => _ => _
        .After(ZipBackend,ZipFrontend).Executes(() =>
        {
            if (Directory.Exists(PublishDirectory / "Backend"))
            {
                (PublishDirectory / "Backend").DeleteDirectory();

            }
            
            if (Directory.Exists(PublishDirectory / "Frontend"))
            {
                (PublishDirectory / "Frontend").DeleteDirectory();

            }

        });
    
}