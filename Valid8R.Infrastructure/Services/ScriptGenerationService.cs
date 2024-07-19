// -----------------------------------------------------
// Copyright (c)  Christo du Toit - All rights reserved.
// -----------------------------------------------------

using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV3s;

namespace Valid8R.Infrastructure.Services
{
    internal class ScriptGenerationService
    {
        private readonly ADotNetClient adotNetClient;

        public ScriptGenerationService() =>
            adotNetClient = new ADotNetClient();

        public void GenerateBuildScript()
        {
            string branchName = "main";
            string projectName = "Valid8R";

            var githubPipeline = new GithubPipeline
            {
                Name = "Build",

                OnEvents = new Events
                {
                    Push = new PushEvent
                    {
                        Branches = new string[] { branchName }
                    },

                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] { branchName }
                    }
                },

                Jobs = new Dictionary<string, Job>
                {
                    {
                        "build",
                        new Job
                        {
                            RunsOn = BuildMachines.UbuntuLatest,

                            Steps = new List<GithubTask>
                            {
                                new CheckoutTaskV3
                                {
                                    Name = "Check out"
                                },

                                new SetupDotNetTaskV3
                                {
                                    Name = "Setup .NET",

                                     With = new TargetDotNetVersionV3
                                     {
                                        DotNetVersion = "8.0.303"
                                     }
                                },

                                new RestoreTask
                                {
                                    Name = "Restoring Packages"
                                },

                                new DotNetBuildTask
                                {
                                    Name = "Building Solution"
                                },

                                new TestTask
                                {
                                    Name = "Running Tests"
                                }
                            }
                        }
                    },
                    {
                        "tagAndRelease",
                        new TagJob(
                            runsOn: BuildMachines.UbuntuLatest,
                            dependsOn: "build",
                            projectRelativePath: $"{projectName}/{projectName}.csproj",
                            githubToken: "${{ secrets.PAT_FOR_TAGGING }}",
                            branchName: branchName)
                    },
                }
            };

            string yamlRelativeFilePath = "../../../../.github/workflows/build.yml";
            string yamlFullPath = Path.GetFullPath(yamlRelativeFilePath);
            FileInfo yamlDefinition = new FileInfo(yamlFullPath);

            if (!yamlDefinition.Directory.Exists)
            {
                yamlDefinition.Directory.Create();
            }

            adotNetClient.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: yamlRelativeFilePath);
        }
    }
}

