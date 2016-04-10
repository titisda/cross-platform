﻿using System.Collections.Generic;
using Symbiote.Core.Plugin.Connector;

namespace Symbiote.Core.Platform
{
    /// <summary>
    /// Defines the interface for Platform objects.
    /// </summary>
    public interface IPlatform
    {
        #region Properties

        /// <summary>
        /// The Platform Type.
        /// </summary>
        PlatformType PlatformType { get; }

        /// <summary>
        /// The Version of the Platform OS.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// The accompanying Connector Plugin for the Platform.
        /// </summary>
        IConnector Connector { get; }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Instantiates the accompanying Connector Plugin with the supplied root path.
        /// </summary>
        /// <param name="instanceName"></param>
        /// <returns>The instantiated Connector Plugin.</returns>
        IConnector InstantiateConnector(string instanceName);

        #region Directory Methods 

        /// <summary>
        /// Returns true if the specified directory exists, false otherwise.
        /// </summary>
        /// <param name="directory">The directory to check.</param>
        /// <returns>True if the specified directory exists, false otherwise.</returns>
        bool DirectoryExists(string directory);

        /// <summary>
        /// Returns a list of subdirectories within the supplied path.
        /// </summary>
        /// <param name="parentDirectory">The parent directory to search.</param>
        /// <returns>An OperationResult containing the result of the operation and list containing the fully qualified path of each directory found.</returns>
        OperationResult<List<string>> ListDirectories(string parentDirectory);

        /// <summary>
        /// Deletes the supplied directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <returns>An OperationResult containing the result of the operation.</returns>
        OperationResult DeleteDirectory(string directory);

        /// <summary>
        /// Deletes all files and subdirectories within the supplied directory.
        /// </summary>
        /// <param name="directory">The directory to clear.</param>
        /// <returns>An OperationResult containing the result of the operation.</returns>
        OperationResult ClearDirectory(string directory);

        /// <summary>
        /// Creates the supplied directory.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        /// <returns>An OperationResult containing the result of the operation and the fully qualified path to the directory.</returns>
        OperationResult<string> CreateDirectory(string directory);

        #endregion

        #region File Methods

        /// <summary>
        /// Returns true if the specified file exists, false otherwise.
        /// </summary>
        /// <param name="file">The file to check.</param>
        /// <returns>True if the specified file exists, false otherwise.</returns>
        bool FileExists(string file);

        /// <summary>
        /// Returns a list of files within the supplied directory matching the supplied searchPattern.
        /// </summary>
        /// <param name="parentDirectory">The directory to search.</param>
        /// <param name="searchPattern">The search pattern to match files against.</param>
        /// <returns>An OperationResult containing the result of the operation and a list containing the fully qualified filename of each file found.</returns>
        OperationResult<List<string>> ListFiles(string parentDirectory, string searchPattern);

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>An OperationResult containing the result of the operation.</returns>
        OperationResult DeleteFile(string file);

        /// <summary>
        /// Reads the contents of the specified file into a single string.
        /// </summary>
        /// <param name="file">The file to read.</param>
        /// <returns>An OperationResult containing the result of the operation and a string containing the entire contents of the file.</returns>
        OperationResult<string> ReadFile(string file);

        /// <summary>
        /// Reads the contents of the specified file into a string array.
        /// </summary>
        /// <param name="file">The file to read.</param>
        /// <returns>An OperationResult containing the result of the operation and a string array containing all of the lines from the file.</returns>
        OperationResult<string[]> ReadFileLines(string file);

        /// <summary>
        /// Writes the contents of the supplied string into the specified file.  If the destination file already exists it is overwritten.
        /// </summary>
        /// <param name="file">The file to write.</param>
        /// <param name="contents">The text to write to the file.</param>
        /// <returns>The fully qualified name of the written file.</returns>
        OperationResult<string> WriteFile(string file, string contents);

        #endregion

        #region Zip File Methods

        /// <summary>
        /// Returns a list of files contained within the specified zip file matching the supplied searchPattern.
        /// </summary>
        /// <param name="zipFile">The zip file to search.</param>
        /// <param name="searchPattern">The search pattern to match files against.</param>
        /// <returns>An OperationResult containing the result of the operation and a list containing the fully qualified filename of each file found.</returns>
        OperationResult<List<string>> ListZipFiles(string zipFile, string searchPattern);

        /// <summary>
        /// Extracts the contents of the supplied zip file to the specified destination, 
        /// clearing the destination first if clearDestination is true.
        /// </summary>
        /// <param name="zipFile">The zip file to extract.</param>
        /// <param name="destination">The destination directory.</param>
        /// <param name="clearDestination">True if the destination directory should be cleared prior to extraction, false otherwise.</param>
        /// <returns>An OperationResult containing the result of the operation and the fully qualified path to the extracted files.</returns>
        OperationResult<string> ExtractZip(string zipFile, string destination, bool clearDestination = true);

        /// <summary>
        /// Extracts the supplied file from the supplied zip file to the supplied destination, overwriting the file if overwrite is true.
        /// </summary>
        /// <param name="zipFile">The zip file from which to extract the file.</param>
        /// <param name="file">The file to extract from the zip file.</param>
        /// <param name="destination">The destination directory.</param>
        /// <param name="overwrite">True if an existing file should be overwritten, false otherwise.</param>
        /// <returns>An OperationResult containing the result of the operation and the fully qualified filename of the extracted file.</returns>
        OperationResult<string> ExtractZipFile(string zipFile, string file, string destination, bool overwrite = true);

        #endregion

        /// <summary>
        /// Computes the checksum of the specified file using the SHA256 hashing algorithm.
        /// </summary>
        /// <remarks>To ensure cross-platform, cross-installation compatibility, only an unsalted SHA256 algorithm is to be used.</remarks>
        /// <param name="file">The file for which the checksum is to be computed.</param>
        /// <returns>An OperationResult containing the result of the operation and the computed checksum.</returns>
        OperationResult<string> ComputeFileChecksum(string file);

        #endregion
    }
}
