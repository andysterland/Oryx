﻿// --------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// --------------------------------------------------------------------------------------------

using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Oryx.Common;

namespace Microsoft.Oryx.BuildScriptGeneratorCli
{
    [Command("oryx", Description = "Generates and runs build scripts for multiple languages.")]
    [Subcommand(typeof(BuildCommand))]
    [Subcommand(typeof(PlatformsCommand))]
    [Subcommand(typeof(BuildScriptCommand))]
    [Subcommand(typeof(RunScriptCommand))]
    [Subcommand(typeof(InstallRunTimeCommand))]
    [Subcommand(typeof(ExecCommand))]
    [Subcommand(typeof(BuildpackDetectCommand))]
    [Subcommand(typeof(BuildpackBuildCommand))]
    [Subcommand(typeof(DockerfileCommand))]
    internal class Program
    {
        public const string GitCommit = "GitCommit";
        public const string ReleaseTagName = "RELEASE_TAG_NAME";

        [Option(CommandOptionType.NoValue, Description = "Print version information.")]
        public bool Version { get; set; }

        internal static string GetVersion()
        {
            var informationalVersion = Assembly.GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (informationalVersion != null)
            {
                return informationalVersion.InformationalVersion;
            }

            return null;
        }

        internal static string GetMetadataValue(string name)
        {
            var commitMetadata = Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>()
                    .Where(attr => attr.Key.Equals(name))
                    .FirstOrDefault();
            if (commitMetadata != null)
            {
                return commitMetadata.Value;
            }

            return null;
        }

        internal int OnExecute(CommandLineApplication app, IConsole console)
        {
            if (Version)
            {
                var version = GetVersion();
                var commit = GetMetadataValue(GitCommit);
                var releaseTagName = GetMetadataValue(ReleaseTagName);
                console.WriteLine($"Version: {version}, Commit: {commit}, ReleaseTagName: {releaseTagName}");

                return ProcessConstants.ExitSuccess;
            }

            app.ShowHelp();

            return ProcessConstants.ExitSuccess;
        }

        private static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);
    }
}