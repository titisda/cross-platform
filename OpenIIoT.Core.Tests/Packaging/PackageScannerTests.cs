﻿/*
      █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀  ▀  ▀      ▀▀
      █
      █      ▄███████▄                                                                 ▄████████
      █     ███    ███                                                                ███    ███
      █     ███    ███   ▄█████   ▄██████    █  █▄     ▄█████     ▄████▄     ▄█████   ███    █▀   ▄██████   ▄█████  ██▄▄▄▄  ██▄▄▄▄     ▄█████    █████
      █     ███    ███   ██   ██ ██    ██   ██ ▄██▀    ██   ██   ██    ▀    ██   █    ███        ██    ██   ██   ██ ██▀▀▀█▄ ██▀▀▀█▄   ██   █    ██  ██
      █   ▀█████████▀    ██   ██ ██    ▀    ██▐█▀      ██   ██  ▄██        ▄██▄▄    ▀███████████ ██    ▀    ██   ██ ██   ██ ██   ██  ▄██▄▄     ▄██▄▄█▀
      █     ███        ▀████████ ██    ▄  ▀▀████     ▀████████ ▀▀██ ███▄  ▀▀██▀▀             ███ ██    ▄  ▀████████ ██   ██ ██   ██ ▀▀██▀▀    ▀███████
      █     ███          ██   ██ ██    ██   ██ ▀██▄    ██   ██   ██    ██   ██   █     ▄█    ███ ██    ██   ██   ██ ██   ██ ██   ██   ██   █    ██  ██
      █    ▄████▀        ██   █▀ ██████▀    ▀█   ▀█▀   ██   █▀   ██████▀    ███████  ▄████████▀  ██████▀    ██   █▀  █   █   █   █    ███████   ██  ██
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
      █  Unit tests for the PackageScanner class.
      █
      █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀ ▀ ▀▀▀     ▀▀               ▀
      █  The GNU Affero General Public License (GNU AGPL)
      █
      █  Copyright (C) 2016-2017 JP Dillingham (jp@dillingham.ws)
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

namespace OpenIIoT.Core.Tests.Packaging
{
    using System;
    using Moq;
    using OpenIIoT.Core.Packaging;
    using OpenIIoT.SDK.Platform;
    using Xunit;
    using OpenIIoT.SDK.Common.OperationResult;
    using System.Collections.Generic;
    using OpenIIoT.SDK.Packaging;
    using OpenIIoT.Core.Platform;

    /// <summary>
    ///     Unit tests for the <see cref="PackageScanner"/> class.
    /// </summary>
    public class PackageScannerTests
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PackageScannerTests"/> class.
        /// </summary>
        public PackageScannerTests()
        {
            PlatformMock = new Mock<IPlatform>();
            PlatformManagerMock = new Mock<IPlatformManager>();
            PackageFactoryMock = new Mock<IPackageFactory>();
            PackageArchiveMock = new Mock<IPackageArchive>();

            SetupMocks();
        }

        #endregion Public Constructors

        #region Private Properties

        /// <summary>
        ///     Gets or sets the IPackageArchive mockup.
        /// </summary>
        private Mock<IPackageArchive> PackageArchiveMock { get; set; }

        /// <summary>
        ///     Gets or sets the IPlatformFactory mockup.
        /// </summary>
        private Mock<IPackageFactory> PackageFactoryMock { get; set; }

        /// <summary>
        ///     Gets or sets the IPlatformManager mockup.
        /// </summary>
        private Mock<IPlatformManager> PlatformManagerMock { get; set; }

        /// <summary>
        ///     Gets or sets the IPlatform mockup.
        /// </summary>
        private Mock<IPlatform> PlatformMock { get; set; }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        ///     Tests the constructor.
        /// </summary>
        [Fact]
        public void Constructor()
        {
            PackageScanner test;
            Exception ex = Record.Exception(() => test = new PackageScanner(PlatformManagerMock.Object, PackageFactoryMock.Object));

            Assert.Null(ex);
        }

        /// <summary>
        ///     Tests the <see cref="PackageScanner.ScanPackageArchives"/> method.
        /// </summary>
        [Fact]
        public void ScanPackageArchives()
        {
            PackageScanner test = new PackageScanner(PlatformManagerMock.Object, PackageFactoryMock.Object);

            IResult<IList<IPackageArchive>> result = test.ScanPackageArchives();

            Assert.Equal(ResultCode.Success, result.ResultCode);
            Assert.Equal(1, result.ReturnValue.Count);
            Assert.Equal(PackageArchiveMock.Object, result.ReturnValue[0]);
        }

        [Fact]
        public void ScanPackageArchivesInvalidArchive()
        {
            PackageFactoryMock.Setup(p => p.GetPackageArchive(It.IsAny<string>())).Returns(new Result<IPackageArchive>(ResultCode.Failure));

            PackageScanner test = new PackageScanner(PlatformManagerMock.Object, PackageFactoryMock.Object);

            IResult<IList<IPackageArchive>> result = test.ScanPackageArchives();

            Assert.Equal(ResultCode.Warning, result.ResultCode);
            Assert.Equal(0, result.ReturnValue.Count);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Configures the mockups for the unit tests.
        /// </summary>
        private void SetupMocks()
        {
            IResult<IList<string>> listResult = new Result<IList<string>>()
                .SetReturnValue(new List<string>(new[] { string.Empty }));

            PlatformMock.Setup(p => p.ListFiles(It.IsAny<string>()))
                .Returns(listResult);

            Mock<IDirectories> directories = new Mock<IDirectories>();
            directories.Setup(d => d.PackageArchives).Returns(string.Empty);

            PlatformManagerMock.Setup(p => p.Platform).Returns(PlatformMock.Object);
            PlatformManagerMock.Setup(p => p.Directories).Returns(directories.Object);

            IResult<IPackageArchive> factoryResult = new Result<IPackageArchive>()
                .SetReturnValue(PackageArchiveMock.Object);

            PackageFactoryMock.Setup(p => p.GetPackageArchive(It.IsAny<string>()))
                .Returns(factoryResult);
        }

        #endregion Private Methods
    }
}