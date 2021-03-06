﻿// --------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.Oryx.Tests.Common;
using Xunit;

namespace Microsoft.Oryx.BuildScriptGeneratorCli.Tests
{
    using FlagGetter = Func<CliEnvironmentSettings, bool>;

    public class CliEnvironmentSettingsTest
    {
        public static IEnumerable<object[]> DisableVariableNamesAndGetters = new[]
        {
            new object[] { "DISABLE_CHECKERS", (FlagGetter)(s => s.DisableCheckers) },
            new object[] { "DISABLE_DOTNETCORE_BUILD", (FlagGetter)(s => s.DisableDotNetCore) },
            new object[] { "DISABLE_PYTHON_BUILD", (FlagGetter)(s => s.DisablePython) },
            new object[] { "DISABLE_NODEJS_BUILD", (FlagGetter)(s => s.DisableNodeJs) },
            new object[] { "ENABLE_MULTIPLATFORM_BUILD", (FlagGetter)(s => !s.DisableMultiPlatformBuild) }
        };

        [Theory]
        [MemberData(nameof(DisableVariableNamesAndGetters))]
        public void DisableFeature_DontDisable_IfSetToFalse(string envVariableName, FlagGetter valueGetter)
        {
            // Arrange
            var testEnvironment = new TestEnvironment();
            testEnvironment.Variables[envVariableName] = "false";
            var settingsProvider = new CliEnvironmentSettings(testEnvironment);

            // Act
            var value = valueGetter(settingsProvider);

            // Assert
            Assert.False(value);
        }

        [Theory]
        [MemberData(nameof(DisableVariableNamesAndGetters))]
        public void DisableFeature_DontDisable_IfSetToNonBoolean(string envVariableName, FlagGetter valueGetter)
        {
            // Arrange
            var testEnvironment = new TestEnvironment();
            testEnvironment.Variables[envVariableName] = "abc";
            var settingsProvider = new CliEnvironmentSettings(testEnvironment);

            // Act
            var value = valueGetter(settingsProvider);

            // Assert
            Assert.False(value);
        }

        #pragma warning disable
        [Theory]
        [MemberData(nameof(DisableVariableNamesAndGetters))]
        public void DisableFeature_DontDisable_IfNotSet(string envVariableName, FlagGetter valueGetter)
        {
            // Arrange
            var testEnvironment = new TestEnvironment();
            var settingsProvider = new CliEnvironmentSettings(testEnvironment);

            // Act
            var value = valueGetter(settingsProvider);

            // Assert
            Assert.False(value);
        }
        #pragma warning restore

        [Theory]
        [MemberData(nameof(DisableVariableNamesAndGetters))]
        public void DisableFeature_Disable_IfSetToTrue(string envVariableName, FlagGetter valueGetter)
        {
            // Arrange
            var testEnvironment = new TestEnvironment();
            testEnvironment.Variables[envVariableName] = "true";
            var settingsProvider = new CliEnvironmentSettings(testEnvironment);

            // Act
            var value = valueGetter(settingsProvider);

            // Assert
            Assert.True(value);
        }
    }
}