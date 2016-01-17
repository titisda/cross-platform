﻿using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using Symbiote.Core.Platform;
using Symbiote.Core.Configuration.Plugin;
using System.Linq;

namespace Symbiote.Core.Plugin
{
    /// <summary>
    /// The PluginManager class controls the plugin subsystem.
    /// <remarks>
    /// This class is implemented using the Singleton and (a variant of) Factory design patterns.
    /// </remarks>
    /// </summary>
    public class PluginManager
    {
        private ProgramManager manager;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static PluginManager instance;
        private bool pluginsLoaded = false;

        /// <summary>
        /// A list of currently loaded plugin assemblies.
        /// </summary>
        public List<IPluginAssembly> PluginAssemblies { get; private set; }

        /// <summary>
        /// A list of all plugin instances.
        /// </summary>
        public List<IPluginInstance> PluginInstances { get; private set; }

        /// <summary>
        /// Private constructor, only called by Instance()
        /// </summary>
        /// <param name="manager">The ProgramManager instance for the application.</param>
        private PluginManager(ProgramManager manager) {
            this.manager = manager;
            PluginAssemblies = new List<IPluginAssembly>();
            PluginInstances = new List<IPluginInstance>();
        }

        /// <summary>
        /// Instantiates and/or returns the PluginManager instance.
        /// </summary>
        /// <param name="manager">The ProgramManager instance for the application.</param>
        /// <returns>The Singleton instance of PluginManager.</returns>
        internal static PluginManager Instance(ProgramManager manager)
        {
            if (instance == null)
                instance = new PluginManager(manager);

            return instance;
        }

        /// <summary>
        /// Given a list of files, validate and load each assembly found in the list.
        /// </summary>
        /// <remarks>
        /// The checks performed on the files is as such:
        ///      Compute the MD5 checksum of the plugin and check it against the configuration
        ///         If it is in the authorized plugin list, proceed to the next step
        ///         If it is in the unauthorized plugin list, continue to the next file (no further checks)
        ///         If it is in neither list, add it to the unauthorized list and continue to the next file
        ///      Using GetAssemblyName, ensure the file is a valid assembly
        ///      Pass the retrieved assembly name to GetPluginValidationMessage to ensure the name meets the application requirements
        ///      If the name is correct, Load the assembly
        /// </remarks>
        /// <param name="folder">The folder containing the plugin files to load.</param>
        /// <param name="platform">The IPlatform representing the current platform for the application.</param>
        public void LoadPlugins(string folder, IPlatform platform)
        {
            // prevent assemblies from being loaded twice
            if (pluginsLoaded)
                throw new Exception("Error: plugins already loaded.  Restart the application to re-load.");

            // fetch a list of files from the specified directory using the platform-independent GetFileList method
            List<string> files = platform.GetFileList(folder, "*.dll");

            // iterate through the found files
            foreach (string plugin in files)
            {
                AssemblyName assemblyName;
                Assembly assembly;

                logger.Trace("Found plugin: " + plugin + "'; authorizing...");

                PluginAuthorization auth = GetPluginAuthorization(plugin);

                // if the plugin authorization is unknown it does not yet exist in the list of assemblies in the config file.
                // add it to the list and, depending on configuration, either authorize it or leave it unauthorized.
                // try to add it but we don't care if it fails.
                if (auth == PluginAuthorization.Unknown)
                {
                    try
                    {
                        AddNewPlugin(plugin);
                    }
                    catch (Exception ex)
                    {
                        logger.Trace("Failed to add new plugin '" + plugin + "' to the internal list of assemblies.");
                    }
                }
                    
                // the plugin hasn't been authorized yet, log a warning and move on to the next plugin.
                if (auth != PluginAuthorization.Authorized) 
                {
                    logger.Warn("Plugin '" + plugin + "' has not been added to the list of authorized plugins and will not be loaded.");
                    continue;
                }

                // ensure the file is a valid assembly and that the name meets the application requirements
                logger.Trace("Attempting to determine assembly name...");
                try
                {
                    // ensure the file is a valid assembly
                    assemblyName = AssemblyName.GetAssemblyName(plugin);

                    // check that the name meets the application requirements
                    string validationMessage = GetPluginValidationMessage(assemblyName);
                    if (validationMessage != null)
                        throw new Exception(validationMessage);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Plugin file '" + plugin + "' is not a valid plugin assembly.");                    
                    continue;
                }




                // attempt to load the assembly and add it to the internal list of plugins
                logger.Trace("Validated assembly '" + assemblyName.ToString() + "'; attempting to load...");
                try
                {
                    assembly = Assembly.Load(assemblyName);
                    PluginAssemblies.Add(
                        new PluginAssembly(
                            assembly.GetName().Name, 
                            assembly.FullName, 
                            assembly.GetName().Version, 
                            GetPluginType(assembly.GetName().Name), 
                            assembly.GetTypes()[0], 
                            assembly
                        )
                    );
                    logger.Info("Loaded plugin '" + plugin + "' as type " + assembly.GetTypes()[0].ToString());
                    logger.Trace("Plugin attributes:");
                    logger.Trace("\tName: " + assembly.GetName().Name);
                    logger.Trace("\tFull Name: " + assembly.FullName);
                    logger.Trace("\tVersion: " + assembly.GetName().Version);
                    logger.Trace("\tPluginType: " + GetPluginType(assembly.GetName().Name).ToString());
                    logger.Trace("\tType: " + assembly.GetTypes()[0].ToString());
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Failed to load assembly from plugin file '" + plugin + "'.  Ignoring.");
                }
            }
            pluginsLoaded = true;
        }

        private void AddNewPlugin(string fileName)
        {
            logger.Trace("Encountered new plugin '" + fileName + "'; adding it to the list of assemblies.");

            AssemblyName assemblyName = AssemblyName.GetAssemblyName(fileName);

            // check that the name meets the application requirements
            // just forget it if we get any errors
            string validationMessage = GetPluginValidationMessage(assemblyName);
            if (validationMessage != null)
                return;

            ConfigurationPluginItem newPlugin = new ConfigurationPluginItem()
            {
                Name = assemblyName.Name,
                FullName = assemblyName.FullName,
                Version = assemblyName.Version.ToString(),
                PluginType = GetPluginType(assemblyName.Name).ToString(),
                FileName = System.IO.Path.GetFileName(fileName),
                Checksum = GetPluginChecksum(fileName)
            };

            logger.Trace("Adding plugin assembly to configuration file with attributes:");
            logger.Trace("\tName: " + newPlugin.Name);
            logger.Trace("\tFull Name: " + newPlugin.FullName);
            logger.Trace("\tVersion: " + newPlugin.Version);
            logger.Trace("\tPluginType: " + newPlugin.PluginType.ToString());
            logger.Trace("\tFileName: " + newPlugin.FileName);
            logger.Trace("\tChecksum: " + newPlugin.Checksum);

            // TODO: check to see if the plugin exists by name only, and if so update the checksum if the config enables it
            if (manager.Configuration.Plugins.AuthorizeNewPlugins)
            {
                newPlugin.Authorization = PluginAuthorization.Authorized;
            }
            else
            {
                newPlugin.Authorization = PluginAuthorization.Unauthorized;
            }

            logger.Trace("\tAuthorization: " + newPlugin.Authorization.ToString());

            manager.Configuration.Plugins.Assemblies.Add(newPlugin);
            manager.ConfigurationManager.SaveConfiguration();
        }

        public string GetPluginChecksum(string fileName)
        {
            logger.Trace("Computing checksum for plugin file '" + fileName + "'...");
            string retVal = manager.Platform.ComputeFileChecksum(fileName);
            logger.Trace("MD5 checksum for file '" + fileName + "' computed as '" + retVal + ".");
            return retVal;
        }
        public PluginAuthorization GetPluginAuthorization(string fileName)
        {
            string checksum = GetPluginChecksum(fileName);

            logger.Trace("Determining authorization for plugin file '" + fileName + "' with checksum '" + checksum + "'...");

            ConfigurationPluginItem retObj = manager.Configuration.Plugins.Assemblies
                        .Where(p => p.FileName == System.IO.Path.GetFileName(fileName))
                        .Where(p => p.Checksum == checksum)
                        .FirstOrDefault();

            if (retObj != null)
            {
                logger.Trace("Retrieved authoriation for plugin '" + fileName + "': " + retObj.Authorization.ToString());
                return retObj.Authorization;
            }

            logger.Trace("Unable to find a matching entry in the assembly configuration for this plugin.  Returning Unknown authorization.");
            return PluginAuthorization.Unknown;
        }
        /// <summary>
        /// Given a string containing the FQN of a loaded plugin assembly, return the matching IPluginAssembly object.
        /// </summary>
        /// <param name="fqn"></param>
        /// <returns>The instance of IPluginAssembly matching the requested FQN</returns>
        public IPluginAssembly FindPluginAssembly(string fqn)
        {
            return PluginAssemblies.Where(p => p.Name == fqn).FirstOrDefault();
        }

        /// <summary>
        /// Creates and returns an instance of the specified plugin type with the specified name
        /// </summary>
        /// <remarks>
        /// The instanceName is propagated through the plugin instance and any internal reference (such as a ConnectorItem).  This name
        /// should match references to the plugin, either through fully qualified addressing or configuration.
        /// </remarks>
        /// <param name="instanceName">The desired internal name of the instance</param>
        /// <param name="type">The type to be instantiated</param>
        /// <returns></returns>
        public T CreatePluginInstance<T>(string instanceName, Type type)
        {
            // check to see if the instance name has already been used
            if (FindPluginInstance(instanceName) == default(IPluginInstance))
            {
                logger.Trace("Creating instance of plugin type '" + type.ToString() + "' with instance name '" + instanceName + "'");
                T newPluginInstance = (T)Activator.CreateInstance(type, instanceName);
                PluginInstances.Add((IPluginInstance)newPluginInstance);
                return newPluginInstance;
            }
            else
            {
                logger.Warn("A plugin with InstanceName '" + instanceName + "' has already been intantiated.");
                return default(T);
            }
        }

        /// <summary>
        /// Given an instance name string, return the matching instance of IPluginInstance.
        /// </summary>
        /// <param name="instanceName"></param>
        /// <returns>The instance of IPluginInstance matching the requested InstanceName.</returns>
        public IPluginInstance FindPluginInstance(string instanceName)
        {
            return PluginInstances.Where(p => p.InstanceName == instanceName).FirstOrDefault();
        }

        // static methods

        /// <summary>
        /// Evaluates the supplied assembly name for correctness and returns an error message if it is incorrect.
        /// </summary>
        /// <remarks>
        /// The expected format of an assembly name is:  "Symbiote.Plugin.[Connector|Service].&gt;PluginName&lt;"
        /// If/when the plugin system expands this code will need to be made to be more dynamic.
        /// </remarks>
        /// <param name="assemblyName">The AssemblyName to be validated.</param>
        private static string GetPluginValidationMessage(AssemblyName assemblyName)
        {
            string[] name = assemblyName.Name.Split('.');
            if (name.Length != 4)
                return "Invalid assembly name (required: 4 tuples, supplied: " + name.Length + ")";
            if (name[0] != "Symbiote")
                return "Invalid application identifier (required: Symbiote, supplied: " + name[0] + ")";
            if (name[1] != "Plugin")
                return "Invalid namespace identifier (required: Plugin, supplied: " + name[1] + ")";
            if (GetPluginType(assemblyName.Name) == default(PluginType))
                return "Invalid plugin type identifier (supplied: " + name[2] + ")";
            return default(string);
        }

        /// <summary>
        /// Returns an enumeration instance representing the type of the plugin, derived from the third tuple of the plugin name.
        /// </summary>
        /// <param name="name">The fully qualified assembly name from which to parse the plugin type.</param>
        /// <returns>An instance of PluginType corresponding to the parsed type.</returns>
        private static PluginType GetPluginType(string name)
        {
            PluginType retVal;
            if (Enum.TryParse<PluginType>(name.Split('.')[2], out retVal))
                return retVal;
            else return default(PluginType);
        }
    }
}