﻿/*
      █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀  ▀  ▀      ▀▀
      █
      █      ▄███████▄                                                                 ▄▄▄▄███▄▄▄▄
      █     ███    ███                                                               ▄██▀▀▀███▀▀▀██▄
      █     ███    ███   ▄█████   ▄██████    █  █▄     ▄█████     ▄████▄     ▄█████  ███   ███   ███   ▄█████  ██▄▄▄▄    ▄█████     ▄████▄     ▄█████    █████
      █     ███    ███   ██   ██ ██    ██   ██ ▄██▀    ██   ██   ██    ▀    ██   █   ███   ███   ███   ██   ██ ██▀▀▀█▄   ██   ██   ██    ▀    ██   █    ██  ██
      █   ▀█████████▀    ██   ██ ██    ▀    ██▐█▀      ██   ██  ▄██        ▄██▄▄     ███   ███   ███   ██   ██ ██   ██   ██   ██  ▄██        ▄██▄▄     ▄██▄▄█▀
      █     ███        ▀████████ ██    ▄  ▀▀████     ▀████████ ▀▀██ ███▄  ▀▀██▀▀     ███   ███   ███ ▀████████ ██   ██ ▀████████ ▀▀██ ███▄  ▀▀██▀▀    ▀███████
      █     ███          ██   ██ ██    ██   ██ ▀██▄    ██   ██   ██    ██   ██   █   ███   ███   ███   ██   ██ ██   ██   ██   ██   ██    ██   ██   █    ██  ██
      █    ▄████▀        ██   █▀ ██████▀    ▀█   ▀█▀   ██   █▀   ██████▀    ███████   ▀█   ███   █▀    ██   █▀  █   █    ██   █▀   ██████▀    ███████   ██  ██
      █
 ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄  ▄▄ ▄▄   ▄▄▄▄ ▄▄     ▄▄     ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄ ▄
 █████████████████████████████████████████████████████████████ ███████████████ ██  ██ ██   ████ ██     ██     ████████████████ █ █
      ▄
      █  Handles the installation and file management of the Packages used to extend the functionality of the application.
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using NLog.xLogger;
using OpenIIoT.Core.Common;
using OpenIIoT.SDK;
using OpenIIoT.SDK.Common;
using OpenIIoT.SDK.Package;
using OpenIIoT.SDK.Packaging.Operations;
using OpenIIoT.SDK.Platform;
using Utility.OperationResult;
using System.IO;
using OpenIIoT.SDK.Packaging.Manifest;

namespace OpenIIoT.Core.Package
{
    /// <summary>
    ///     Handles the installation and file management of the Packages used to extend the functionality of the application.
    /// </summary>
    public sealed class PackageManager : Manager, IPackageManager
    {
        #region Private Fields

        /// <summary>
        ///     The Singleton instance of PackageManager.
        /// </summary>
        private static IPackageManager instance;

        /// <summary>
        ///     The Logger for this class.
        /// </summary>
        private static new xLogger logger = xLogManager.GetCurrentClassxLogger();

        #endregion Private Fields

        #region Private Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PackageManager"/> class.
        /// </summary>
        /// <remarks>
        ///     This constructor is marked private and is intended to be called from the
        ///     <see cref="Instantiate(IApplicationManager, IPlatformManager)"/> method exclusively in order to implement the
        ///     Singleton design pattern.
        /// </remarks>
        /// <param name="manager">The <see cref="IApplicationManager"/> instance for the application.</param>
        /// <param name="platformManager">The <see cref="IPlatformManager"/> instance for the application.</param>
        private PackageManager(IApplicationManager manager, IPlatformManager platformManager)
        {
            base.logger = logger;
            logger.EnterMethod();

            ManagerName = "Package Manager";

            // register dependencies
            RegisterDependency<IApplicationManager>(manager);
            RegisterDependency<IPlatformManager>(platformManager);

            ChangeState(State.Initialized);

            Utility = new PackageUtility(platformManager.Platform);

            logger.ExitMethod();
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        ///     Gets the list of Packages available for installation.
        /// </summary>
        public IReadOnlyList<IPackage> Packages => ((List<IPackage>)PackageList).AsReadOnly();

        #endregion Public Properties

        #region Private Properties

        /// <summary>
        ///     Gets the Platform instance with which file operations are carried out.
        /// </summary>
        private IPlatform Platform => Dependency<IPlatformManager>().Platform;

        /// <summary>
        ///     Gets or sets the list of Packages available for installation.
        /// </summary>
        private IList<IPackage> PackageList { get; set; }

        /// <summary>
        ///     Gets or sets the PackageUtility used for packaging operations.
        /// </summary>
        private PackageUtility Utility { get; set; }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        ///     Instantiates and/or returns the <see cref="IPackageManager"/> instance.
        /// </summary>
        /// <remarks>
        ///     Invoked via reflection from the <see cref="IApplicationManager"/> . The parameters are used to build an array of
        ///     <see cref="IManager"/> parameters which are then passed to this method. To specify additional dependencies simply
        ///     insert them into the parameter list for the method and they will be injected when the method is invoked.
        /// </remarks>
        /// <param name="manager">The <see cref="IApplicationManager"/> instance for the application.</param>
        /// <param name="platformManager">The <see cref="IPlatformManager"/> instance for the application.</param>
        /// <returns>The Singleton instance of the <see cref="IManager"/>.</returns>
        public static IPackageManager Instantiate(IApplicationManager manager, IPlatformManager platformManager)
        {
            if (instance == null)
            {
                instance = new PackageManager(manager, platformManager);
            }

            return instance;
        }

        /// <summary>
        ///     Terminates Singleton instance of the <see cref="IManager"/>.
        /// </summary>
        public static void Terminate()
        {
            instance = null;
        }

        /// <summary>
        ///     Creates a <see cref="IPackage"/> file with the specified data.
        /// </summary>
        /// <remarks>
        ///     The resulting Package file is saved to the Packages directory with a filename composed of the Fully Qualified Name
        ///     and Version of the Package.
        /// </remarks>
        /// <param name="data">The data to save.</param>
        /// <returns>A Result containing the result of the operation and the created IPackage instance.</returns>
        public IResult<IPackage> CreatePackage(byte[] data)
        {
            logger.EnterMethod();
            logger.Info($"Creating new Package...");

            IResult<IPackage> retVal = new Result<IPackage>();

            string tempFile = Path.Combine(Platform.Directories.Temp, Guid.NewGuid().ToString());

            logger.Debug($"Saving new Package to '{tempFile}'...");

            retVal.Incorporate(Platform.WriteFileBytes(tempFile, data));

            if (retVal.ResultCode != ResultCode.Failure)
            {
                IResult<IPackage> readResult = Read(tempFile);

                retVal.Incorporate(readResult);

                if (retVal.ResultCode != ResultCode.Failure)
                {
                    string destinationFilename = GetPackageFilename(readResult.ReturnValue);

                    retVal.Incorporate(Platform.CopyFile(tempFile, destinationFilename, true));

                    if (retVal.ResultCode != ResultCode.Failure)
                    {
                        retVal.ReturnValue = readResult.ReturnValue;
                        retVal.ReturnValue.Filename = destinationFilename;
                    }
                }
            }

            if (retVal.ResultCode == ResultCode.Failure)
            {
                retVal.AddError("Unable to create Package from supplied data.");
            }

            retVal.LogResult(logger);
            logger.ExitMethod();
            return retVal;
        }

        /// <summary>
        ///     Asynchronously creates a <see cref="IPackage"/> file with the specified data.
        /// </summary>
        /// <remarks>
        ///     The resulting Package file is saved to the Packages directory with a filename composed of the Fully Qualified Name
        ///     and Version of the Package.
        /// </remarks>
        /// <param name="data">The data to save.</param>
        /// <returns>A Result containing the result of the operation and the created IPackage instance.</returns>
        public async Task<IResult<IPackage>> CreatePackageAsync(byte[] data)
        {
            return await Task.Run(() => CreatePackage(data));
        }

        /// <summary>
        ///     Deletes the <see cref="IPackage"/> matching the specified Fully Qualified Name from disk.
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the <see cref="IPackage"/> to delete.</param>
        /// <returns>A Result containing the result of the operation.</returns>
        public IResult DeletePackage(string fqn)
        {
            logger.EnterMethod(xLogger.Params(fqn));
            logger.Info($"Deleting Package {fqn}...");

            IResult retVal = new Result();
            IPackage findResult = FindPackage(fqn);

            if (findResult != default(IPackage))
            {
                string fileName = findResult.Filename;

                IResult deleteResult = Dependency<IPlatformManager>().Platform.DeleteFile(fileName);
                retVal.Incorporate(deleteResult);
            }

            retVal.LogResult(logger);
            logger.ExitMethod();
            return retVal;
        }

        /// <summary>
        ///     Asynchronously deletes the <see cref="IPackage"/> matching the specified Fully Qualified Name from disk.
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the <see cref="IPackage"/> to delete.</param>
        /// <returns>A Result containing the result of the operation.</returns>
        public async Task<IResult> DeletePackageAsync(string fqn)
        {
            return await Task.Run(() => DeletePackage(fqn));
        }

        /// <summary>
        ///     <para>
        ///         Scans the <see cref="Packages"/> list for a Package matching the specified Fully Qualified Name and, if found,
        ///         returns the found Package.
        ///     </para>
        ///     <para>
        ///         If a matching Package is not found, the <see cref="ScanPackages()"/> method is invoked to refresh the
        ///         <see cref="Packages"/> list from disk.
        ///     </para>
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the Package to find.</param>
        /// <returns>The result of the operation and the found Package, if applicable.</returns>
        public IPackage FindPackage(string fqn)
        {
            return FindPackage(fqn, false);
        }

        /// <summary>
        ///     <para>
        ///         Asynchronously scans the <see cref="Packages"/> list for a Package matching the specified Fully Qualified Name
        ///         and, if found, returns the found Package.
        ///     </para>
        ///     <para>
        ///         If a matching Package is not found, the <see cref="ScanPackages()"/> method is invoked to refresh the
        ///         <see cref="Packages"/> list from disk.
        ///     </para>
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the Package to find.</param>
        /// <returns>The result of the operation and the found Package, if applicable.</returns>
        public async Task<IPackage> FindPackageAsync(string fqn)
        {
            return await Task.Run(() => FindPackage(fqn));
        }

        /// <summary>
        ///     Installs the specified <see cref="IPackage"/> (extracts it to disk).
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the Package to install.</param>
        /// <returns>A Result containing the result of the operation.</returns>
        public IResult InstallPackage(string fqn)
        {
            return InstallPackage(fqn, default(PackageInstallationOptions));
        }

        /// <summary>
        ///     Installs the specified <see cref="IPackage"/> (extracts it to disk) using the specified options.
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the Package to install.</param>
        /// <param name="options">The installation options for the operation.</param>
        /// <returns>A Result containing the result of the operation.</returns>
        public IResult InstallPackage(string fqn, PackageInstallationOptions options)
        {
            logger.EnterMethod(xLogger.Params(fqn, options));
            logger.Info($"Installing Package '{fqn}'...");

            IResult retVal = new Result();
            IPackage findResult = FindPackage(fqn);

            if (findResult != default(IPackage))
            {
                retVal.Incorporate(Utility.Install(findResult, options));
            }
            else
            {
                retVal.AddError($"Failed to install Package '{fqn}'; the package could not be found.");
            }

            retVal.LogResult(logger);
            logger.ExitMethod();
            return retVal;
        }

        /// <summary>
        ///     Asynchronously installs the specified <see cref="IPackage"/> (extracts it to disk).
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the Package to install.</param>
        /// <returns>A Result containing the result of the operation.</returns>
        public async Task<IResult> InstallPackageAsync(string fqn)
        {
            return await Task.Run(() => InstallPackage(fqn));
        }

        /// <summary>
        ///     Asynchronously installs the specified <see cref="IPackage"/> (extracts it to disk) using the specified options.
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the Package to install.</param>
        /// <param name="options">The installation options for the operation.</param>
        /// <returns>A Result containing the result of the operation.</returns>
        public async Task<IResult> InstallPackageAsync(string fqn, PackageInstallationOptions options)
        {
            return await Task.Run(() => InstallPackage(fqn, default(PackageInstallationOptions)));
        }

        /// <summary>
        ///     Reads the <see cref="IPackage"/> file matching the specified Fully Qualified Name and returns the binary data.
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the <see cref="IPackage"/> to read.</param>
        /// <returns>A Result containing the result of the operation and the read binary data.</returns>
        public IResult<byte[]> ReadPackage(string fqn)
        {
            logger.EnterMethod(xLogger.Params(fqn));
            logger.Info($"Retrieving Package '{fqn}'...");

            IResult<byte[]> retVal = new Result<byte[]>();
            IPackage findResult = FindPackage(fqn);

            if (findResult != default(IPackage))
            {
                IPlatform platform = Dependency<IPlatformManager>().Platform;

                if (platform.FileExists(findResult.Filename))
                {
                    IResult<byte[]> readResult = platform.ReadFileBytes(findResult.Filename);
                    retVal.Incorporate(readResult);
                    retVal.ReturnValue = readResult.ReturnValue;
                }
            }

            retVal.LogResult(logger);
            logger.ExitMethod();
            return retVal;
        }

        /// <summary>
        ///     Asynchronously reads the <see cref="IPackage"/> file matching the specified Fully Qualified Name and returns the
        ///     binary data.
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the <see cref="IPackage"/> to read.</param>
        /// <returns>A Result containing the result of the operation and the read binary data.</returns>
        public async Task<IResult<byte[]>> ReadPackageAsync(string fqn)
        {
            return await Task.Run(() => ReadPackage(fqn));
        }

        /// <summary>
        ///     Scans for and returns a list of all Package files in the configured Packages directory.
        /// </summary>
        /// <returns>A Result containing the result of the operation and the list of found Packages.</returns>
        public IResult<IList<IPackage>> ScanPackages()
        {
            Guid guid = logger.EnterMethod(true);
            logger.Info("Scanning for Packages...");

            IResult<IList<IPackage>> retVal = new Result<IList<IPackage>>();
            retVal.ReturnValue = new List<IPackage>();

            string directory = Platform.Directories.Packages;

            logger.Debug($"Scanning directory '{directory}'...");

            ManifestExtractor extractor = new ManifestExtractor();
            extractor.Updated += (sender, e) => logger.Debug(e.Message);

            IResult<IList<string>> fileListResult = Platform.ListFiles(directory);
            retVal.Incorporate(fileListResult);

            if (retVal.ResultCode != ResultCode.Failure)
            {
                foreach (string file in fileListResult.ReturnValue)
                {
                    IResult<IPackage> readResult = Read(file);

                    if (readResult.ResultCode != ResultCode.Failure)
                    {
                        retVal.ReturnValue.Add(readResult.ReturnValue);
                    }
                    else
                    {
                        retVal.AddWarning(readResult.GetLastError());
                    }
                }
            }

            retVal.LogResult(logger);
            logger.ExitMethod(guid);

            return retVal;
        }

        /// <summary>
        ///     Asynchronously scans for and returns a list of all Package files in the configured Package directory.
        /// </summary>
        /// <returns>A Result containing the result of the operation and the list of found Packages.</returns>
        public async Task<IResult<IList<IPackage>>> ScanPackagesAsync()
        {
            return await Task.Run(() => ScanPackages());
        }

        /// <summary>
        ///     Verifies the specified <see cref="IPackage"/> using the optionally specified PGP Public Key.
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the Package to verify.</param>
        /// <param name="publicKey">The optional PGP Public Key with which to verify the package.</param>
        /// <returns>A Result containing the result of the operation and a value indicating whether the Package is valid.</returns>
        public IResult<bool> VerifyPackage(string fqn, string publicKey = "")
        {
            Guid guid = logger.EnterMethod(xLogger.Params(fqn, publicKey), true);
            logger.Info($"Verifying Package '{fqn}'...");

            IResult<bool> retVal = new Result<bool>();
            IPackage findResult = FindPackage(fqn);

            if (retVal != default(IPackage))
            {
                PackageVerifier verifier = new PackageVerifier();
                verifier.Updated += (sender, e) => logger.Debug(e.Message);

                try
                {
                    retVal.ReturnValue = verifier.VerifyPackage(findResult.Filename);
                }
                catch (Exception ex)
                {
                    retVal.AddError($"Error validating Package '{fqn}': {ex.Message}");
                }
            }

            retVal.LogResult(logger);
            logger.ExitMethod(guid);
            return retVal;
        }

        /// <summary>
        ///     Asynchronously verifies the specified <see cref="IPackage"/> using the optionally specified PGP Public Key.
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the Package to verify.</param>
        /// <param name="publicKey">The optional PGP Public Key with which to verify the package.</param>
        /// <returns>A Result containing the result of the operation and a value indicating whether the Package is valid.</returns>
        public async Task<IResult<bool>> VerifyPackageAsync(string fqn, string publicKey = "")
        {
            return await Task.Run(() => VerifyPackage(fqn, publicKey));
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        ///     <para>Executed upon shutdown of the Manager.</para>
        ///     <para>
        ///         If the specified <see cref="StopType"/> is not <see cref="StopType.Exception"/>, saves the configuration to disk.
        ///     </para>
        /// </summary>
        /// <param name="stopType">The nature of the stoppage.</param>
        /// <returns>A Result containing the result of the operation.</returns>
        protected override IResult Shutdown(StopType stopType = StopType.Stop)
        {
            Guid guid = logger.EnterMethod(true);
            logger.Debug("Performing Shutdown for '" + GetType().Name + "'...");

            IResult retVal = new Result();

            if (!stopType.HasFlag(StopType.Exception))
            {
                PackageList = default(IList<IPackage>);
            }

            retVal.LogResult(logger.Debug);
            logger.ExitMethod(retVal, guid);
            return retVal;
        }

        /// <summary>
        ///     <para>Executed upon startup of the Manager.</para>
        ///     <para>
        ///         Verifies the existence of the configuration file and if missing, builds it using all default options. Loads the
        ///         configuration, validates it, and, if valid, attaches it to the <see cref="Configuration"/> property.
        ///     </para>
        /// </summary>
        /// <returns>A Result containing the result of the operation.</returns>
        protected override IResult Startup()
        {
            Guid guid = logger.EnterMethod(true);
            logger.Debug("Performing Startup for '" + GetType().Name + "'...");

            IResult retVal = new Result();

            retVal.Incorporate(ScanPackages());

            retVal.LogResult(logger.Debug);
            logger.ExitMethod(retVal, guid);
            return retVal;
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        ///     <para>
        ///         Scans the <see cref="Packages"/> list for a Package matching the specified Fully Qualified Name and, if found,
        ///         returns the found Package.
        ///     </para>
        ///     <para>
        ///         If a matching Package is not found, the <see cref="ScanPackages()"/> method is invoked to refresh the
        ///         <see cref="Packages"/> list from disk.
        ///     </para>
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the Package to find.</param>
        /// <param name="rescanOnNotFound">
        ///     A value indicating whether the <see cref="ScanPackages()"/> method is to be invoked on a failure to find the
        ///     specified Package.
        /// </param>
        /// <returns>The result of the operation and the found Package, if applicable.</returns>
        private IPackage FindPackage(string fqn, bool rescanOnNotFound)
        {
            logger.EnterMethod(xLogger.Params(fqn, rescanOnNotFound));
            IPackage retVal;

            retVal = PackageList.Where(p => p.FQN == fqn).FirstOrDefault();

            if (retVal == default(IPackage))
            {
                if (rescanOnNotFound)
                {
                    ScanPackages();
                    return FindPackage(fqn, false);
                }
            }

            logger.ExitMethod();
            return retVal;
        }

        /// <summary>
        ///     Creates a <see cref="Package"/> instance with file metadata from the given file and the given
        ///     <see cref="PackageManifest"/> .
        /// </summary>
        /// <param name="fileName">The filename from which to retrieve the Package metadata.</param>
        /// <param name="manifest">The Manifest with which to initialize the <see cref="Package"/> instance.</param>
        /// <returns>The created Package.</returns>
        private IPackage GetPackage(string fileName, PackageManifest manifest)
        {
            FileInfo info = new FileInfo(fileName);

            return new Package(fileName, info.LastWriteTime, manifest);
        }

        /// <summary>
        ///     Creates and returns a valid filename for the specified <see cref="IPackage"/>.
        /// </summary>
        /// <param name="package">The Package for which the filename is to be created.</param>
        /// <returns>The created filename.</returns>
        private string GetPackageFilename(IPackage package)
        {
            string filename = package.FQN + "." + package.Version + PackageConstants.PackageFilenameExtension;

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, PackageConstants.PackageFilenameInvalidCharacterSubstitution);
            }

            return Path.Combine(Platform.Directories.Packages, filename);
        }

        /// <summary>
        ///     Reads the specified file and, if it is a valid <see cref="Package"/>, returns an <see cref="IPackage"/> instance
        ///     from the contents.
        /// </summary>
        /// <param name="fileName">The filename of the file to read.</param>
        /// <returns>A Result containing the result of the operation and the created IPackage instance.</returns>
        private IResult<IPackage> Read(string fileName)
        {
            logger.EnterMethod(true);
            logger.Debug($"Reading Package '{fileName}'...");

            IResult<IPackage> retVal = new Result<IPackage>();
            ManifestExtractor extractor = new ManifestExtractor();

            extractor.Updated += (sender, e) => logger.Debug(e.Message);

            PackageManifest manifest;

            try
            {
                manifest = extractor.ExtractManifest(fileName);

                retVal.ReturnValue = GetPackage(fileName, manifest);
            }
            catch (Exception ex)
            {
                retVal.AddError(ex.Message);
                retVal.AddError($"Unable to read Package '{Path.GetFileName(fileName)}'.");
            }

            retVal.LogResult(logger.Debug);
            logger.ExitMethod();

            return retVal;
        }

        #endregion Private Methods
    }
}