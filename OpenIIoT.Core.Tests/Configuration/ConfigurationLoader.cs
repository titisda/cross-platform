﻿/*
      █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀  ▀  ▀      ▀▀
      █
      █   ▄████████                                                                                                       ▄█
      █   ███    ███                                                                                                     ███
      █   ███    █▀   ██████  ██▄▄▄▄     ▄█████  █     ▄████▄  ██   █     █████   ▄█████      ██     █   ██████  ██▄▄▄▄  ███        ██████    ▄█████  ██████▄     ▄█████    █████
      █   ███        ██    ██ ██▀▀▀█▄   ██   ▀█ ██    ██    ▀  ██   ██   ██  ██   ██   ██ ▀███████▄ ██  ██    ██ ██▀▀▀█▄ ███       ██    ██   ██   ██ ██   ▀██   ██   █    ██  ██
      █   ███        ██    ██ ██   ██  ▄██▄▄    ██▌  ▄██       ██   ██  ▄██▄▄█▀   ██   ██     ██  ▀ ██▌ ██    ██ ██   ██ ███       ██    ██   ██   ██ ██    ██  ▄██▄▄     ▄██▄▄█▀
      █   ███    █▄  ██    ██ ██   ██ ▀▀██▀▀    ██  ▀▀██ ███▄  ██   ██ ▀███████ ▀████████     ██    ██  ██    ██ ██   ██ ███       ██    ██ ▀████████ ██    ██ ▀▀██▀▀    ▀███████
      █   ███    ███ ██    ██ ██   ██   ██      ██    ██    ██ ██   ██   ██  ██   ██   ██     ██    ██  ██    ██ ██   ██ ███▌    ▄ ██    ██   ██   ██ ██   ▄██   ██   █    ██  ██
      █   ████████▀   ██████   █   █    ██      █     ██████▀  ██████    ██  ██   ██   █▀    ▄██▀   █    ██████   █   █  █████▄▄██  ██████    ██   █▀ ██████▀    ███████   ██  ██
      █
      █       ███
      █   ▀█████████▄
      █      ▀███▀▀██    ▄█████   ▄█████     ██      ▄█████
      █       ███   ▀   ██   █    ██  ▀  ▀███████▄   ██  ▀
      █       ███      ▄██▄▄      ██         ██  ▀   ██
      █       ███     ▀▀██▀▀    ▀███████     ██    ▀███████
      █       ███       ██   █     ▄  ██     ██       ▄  ██
      █      ▄████▀     ███████  ▄████▀     ▄██▀    ▄████▀
      █
 ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄  ▄▄ ▄▄   ▄▄▄▄ ▄▄     ▄▄     ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄ ▄
 █████████████████████████████████████████████████████████████ ███████████████ ██  ██ ██   ████ ██     ██     ████████████████ █ █
      ▄
      █  Unit tests for the ConfigurationLoader class.
      █
      █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀ ▀ ▀▀▀     ▀▀               ▀
      █  The GNU Affero General Public License (GNU AGPL)
      █
      █  Copyright (C) 2016 JP Dillingham (jp@dillingham.ws)
      █
      █  This program is free software: you can redistribute it and/or modify
      █  it under the terms of the GNU Affero General Public License as published by
      █  the Free Software Foundation, either version 3 of the License, or
      █  (at your option) any later version.
      █
      █  This program is distributed in the hope that it will be useful,
      █  but WITHOUT ANY WARRANTY; without even the implied warranty of
      █  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
      █  GNU Affero General Public License for more details.
      █
      █  You should have received a copy of the GNU Affero General Public License
      █  along with this program.  If not, see <http://www.gnu.org/licenses/>.
      █
      ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀  ▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀██
                                                                                                   ██
                                                                                               ▀█▄ ██ ▄█▀
                                                                                                 ▀████▀
                                                                                                   ▀▀                            */

using System;
using System.Collections.Generic;
using Moq;
using OpenIIoT.SDK.Common.Exceptions;
using OpenIIoT.SDK.Platform;
using Utility.OperationResult;
using Xunit;

namespace OpenIIoT.Core.Tests.Configuration
{
    /// <summary>
    ///     Unit tests for the <see cref="Core.Configuration.ConfigurationLoader"/> class.
    /// </summary>
    [Collection("ConfigurationLoader")]
    public class ConfigurationLoader
    {
        /// <summary>
        ///     Tests the constructor.
        /// </summary>
        [Fact]
        public void Constructor()
        {
            Mock<IPlatform> platform = new Mock<IPlatform>();

            Core.Configuration.ConfigurationLoader loader = new Core.Configuration.ConfigurationLoader(platform.Object);

            Assert.IsType<Core.Configuration.ConfigurationLoader>(loader);
        }

        /// <summary>
        ///     Tests the <see cref="Core.Configuration.ConfigurationLoader.Build"/> method.
        /// </summary>
        [Fact]
        public void Build()
        {
            Mock<IPlatform> platform = new Mock<IPlatform>();

            Core.Configuration.ConfigurationLoader loader = new Core.Configuration.ConfigurationLoader(platform.Object);

            Result<Dictionary<string, Dictionary<string, object>>> result = loader.Build();

            Assert.Equal(ResultCode.Success, result.ResultCode);
            Assert.NotNull(result.ReturnValue);
            Assert.IsType<Dictionary<string, Dictionary<string, object>>>(result.ReturnValue);
        }

        /// <summary>
        ///     Tests the
        ///     <see cref="Core.Configuration.ConfigurationLoader.Save(Dictionary{string, Dictionary{string, object}}, string)"/> method.
        /// </summary>
        [Fact]
        public void Save()
        {
            Mock<IPlatform> platform = new Mock<IPlatform>();
            platform.Setup(p => p.WriteFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new Result<string>());

            Core.Configuration.ConfigurationLoader loader = new Core.Configuration.ConfigurationLoader(platform.Object);

            Dictionary<string, Dictionary<string, object>> configuration = new Dictionary<string, Dictionary<string, object>>();

            Result result = loader.Save(configuration, "file.ext");

            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        /// <summary>
        ///     Tests the <see cref="Core.Configuration.ConfigurationLoader.Load(string)"/> method.
        /// </summary>
        [Fact]
        public void Load()
        {
            Mock<IPlatform> platform = new Mock<IPlatform>();
            platform.Setup(p => p.WriteFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new Result<string>());
            platform.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);
            platform.Setup(p => p.ReadFile(It.IsAny<string>())).Returns(new Result<string>().SetReturnValue("{}"));

            Core.Configuration.ConfigurationLoader loader = new Core.Configuration.ConfigurationLoader(platform.Object);

            Result result = loader.Load("file.ext");

            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        /// <summary>
        ///     Tests the <see cref="Core.Configuration.ConfigurationLoader.Load(string)"/> method with a mockup which simulates a
        ///     failure to read the specified file.
        /// </summary>
        [Fact]
        public void LoadReadFailure()
        {
            Mock<IPlatform> platform = new Mock<IPlatform>();
            platform.Setup(p => p.WriteFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new Result<string>());
            platform.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);
            platform.Setup(p => p.ReadFile(It.IsAny<string>())).Returns(new Result<string>().SetResultCode(ResultCode.Failure));

            Core.Configuration.ConfigurationLoader loader = new Core.Configuration.ConfigurationLoader(platform.Object);

            Exception ex = Record.Exception(() => loader.Load("file.ext"));

            Assert.NotNull(ex);
            Assert.IsType<ConfigurationLoadException>(ex);
        }

        /// <summary>
        ///     Tests the <see cref="Core.Configuration.ConfigurationLoader.Load(string)"/> method with a mockup which indicates
        ///     the specified file could not be found.
        /// </summary>
        [Fact]
        public void LoadFileNotFound()
        {
            Mock<IPlatform> platform = new Mock<IPlatform>();
            platform.Setup(p => p.WriteFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new Result<string>());
            platform.Setup(p => p.FileExists(It.IsAny<string>())).Returns(false);

            Core.Configuration.ConfigurationLoader loader = new Core.Configuration.ConfigurationLoader(platform.Object);

            Result result = loader.Load("file.ext");

            Assert.Equal(ResultCode.Failure, result.ResultCode);
        }
    }
}