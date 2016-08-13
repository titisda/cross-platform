﻿/*
      █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀  ▀  ▀      ▀▀ 
      █   
      █      ▄███████▄                                                             
      █     ███    ███                                                             
      █     ███    ███    █████  ██████     ▄████▄     █████   ▄█████     ▄▄██▄▄▄  
      █     ███    ███   ██  ██ ██    ██   ██    ▀    ██  ██   ██   ██  ▄█▀▀██▀▀█▄ 
      █   ▀█████████▀   ▄██▄▄█▀ ██    ██  ▄██        ▄██▄▄█▀   ██   ██  ██  ██  ██ 
      █     ███        ▀███████ ██    ██ ▀▀██ ███▄  ▀███████ ▀████████  ██  ██  ██ 
      █     ███          ██  ██ ██    ██   ██    ██   ██  ██   ██   ██  ██  ██  ██ 
      █    ▄████▀        ██  ██  ██████    ██████▀    ██  ██   ██   █▀   █  ██  █  
      █
 ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄  ▄▄ ▄▄   ▄▄▄▄ ▄▄     ▄▄     ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄ ▄ 
 █████████████████████████████████████████████████████████████ ███████████████ ██  ██ ██   ████ ██     ██     ████████████████ █ █ 
      ▄  
      █  The main application class.
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
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using NLog;
using Symbiote.Core.Configuration;
using Symbiote.Core.Event;
using Symbiote.Core.Model;
using Symbiote.Core.Platform;
using Symbiote.Core.Plugin;
using Symbiote.Core.Service;
using Symbiote.Core.Plugin.Connector;
using NLog.xLogger;

namespace Symbiote.Core
{
    /// <summary>
    /// The main application class.
    /// </summary>
    public class Program
    {
        #region Fields

        /// <summary>
        /// The main logger for the application.
        /// </summary>
        private static xLogger logger = (xLogger)LogManager.GetCurrentClassLogger(typeof(xLogger));

        /// <summary>
        /// The ApplicationManager for the application.
        /// </summary>
        private static IApplicationManager applicationManager;

        /// <summary>
        /// The list of Managers for the application.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Each Manager must be listed in the order in which they are to be instantiated and started.  The order
        ///     will be reversed when the application stops.
        /// </para>
        /// <para>
        ///     Inter-Manager dependencies must be taken into consideration when determining the order.
        /// </para>
        /// </remarks>
        private static Type[] managers = new Type[]
        {
            typeof(PlatformManager),
            typeof(ConfigurationManager),
            typeof(EventManager),
            typeof(PluginManager),
            typeof(ModelManager),
            typeof(ServiceManager)
        };

        #endregion

        #region Methods

        #region Internal Methods

        #region Internal Static Methods

        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Responsible for instantiating the platform and determining whether to start the application as a 
        ///     Windows service or console/interactive application.
        /// </para>
        /// <para>
        ///     The "-logLevel:*" argument is used to determine the logging level of the application.  Acceptable values are:
        ///     <list type="bullet">
        ///         <item>
        ///             <term>trace</term>
        ///             <description>The lowest logging level.  The output for this level is extremely verbose and only outputs to the log file.</description>
        ///         </item>
        ///         <item>
        ///             <term>debug</term>
        ///             <description>Basic debugging information.  These messages will appear in the console if this level is enabled.</description>
        ///         </item>
        ///         <item>
        ///             <term>info</term>    
        ///             <description>The default logging level; contains basic status information.</description>
        ///         </item>
        ///         <item>
        ///             <term>warn</term>
        ///             <description>Contains warning messages.</description>
        ///         </item>
        ///         <item>
        ///             <term>error</term>
        ///             <description>Contains error messages.  Typically errors produced on this level will not stop the application.</description>
        ///         </item>
        ///         <item>
        ///             <term>fatal</term>
        ///             <description>Fatal error messages; these errors stop the application.</description>
        ///         </item>
        ///     </list>
        ///     Note that the levels are additive; each level contains the messages associated with that level specifically as well as all "higher" (more severe) levels.
        /// </para>
        /// <para>
        ///     The "-(un)install-service" argument is used to install or uninstall the Windows service.  If either of these arguments is used, the application
        ///     performs the requested command and stops.  Re-run the application omitting the argument to start normally.
        /// </para>
        /// <para>
        ///     Any additional arguments will be discarded; the application does not check for, nor does it react to, arguments other than those listed above.
        /// </para>
        /// </remarks>
        /// <param name="args">Command line arguments.</param>
        /// <exception cref="ApplicationArgumentException">Thrown when an error is encountered while parsing and applying command line arguments.</exception>
        /// <exception cref="ApplicationInitializationException">Thrown when an error is encountered while initializing the program.</exception>
        internal static void Main(string[] args)
        {
            logger.EnterMethod(xLogger.Params((object)args));
            logger.Heading(LogLevel.Info, Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            logger.Info("Initializing application...");

            try
            {
                //----------------------- - --------- -  - 
                // process the command line arguments used to start the application
                logger.Debug($"Program started with {(args.Length > 0 ? "arguments: " + string.Join(", ", args) : "no arguments.")}");

                if (args.Length > 0)
                {
                    try
                    {
                        // check to see if logger arguments were supplied
                        string logarg = args.Where(a => Regex.IsMatch(a, "^((?i)-logLevel:)((?i)trace|debug|info|warn|error|fatal)$")).FirstOrDefault();
                        if (logarg != default(string))
                        {
                            // reconfigure the logger based on the command line arguments.
                            // valid values are "fatal" "error" "warn" "info" "debug" and "trace"
                            // supplying any value will disable logging for any level beneath that level, from left to right as positioned above
                            logger.Info($"Reconfiguring logger to log level '{logarg.Split(':')[1]}'...");
                            Utility.SetLoggingLevel(logarg.Split(':')[1]);
                            logger.Info("Successfully reconfigured logger.");
                        }

                        // check to see if service install/uninstall arguments were supplied
                        string servicearg = args.Where(a => Regex.IsMatch(a, "^(?i)(-(un)?install-service)$")).FirstOrDefault();
                        if (servicearg != default(string))
                        {
                            string action = servicearg.Split('-')[1];
                            logger.Info($"Attempting to {action} Windows Service...");

                            if (Utility.ModifyService(action))
                            {
                                logger.Info($"Successfully {action}ed Windows Service.");
                            }
                            else
                            {
                                logger.Error($"Failed to {action} Windows Service.");
                            }

                            // if we do anything with the service, do it then quit.  don't start the application if either argument was used.
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadLine();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Exception(ex);
                        throw new ApplicationArgumentException("Error parsing command line arguments.  See the inner exception for details.", ex);
                    }
                }

                //---------- - - -    ------------------------  -          -
                // instantiate the Program Manager.
                // the Program Manager acts as a Service Locator and Dependency Injector for the application; all of the various IManager instances specified in
                // the list of managers are instantiated (but not started) within the constructor of ApplicationManager.
                logger.Heading(LogLevel.Debug, "Initialization");
                logger.Debug("Instantiating the Application Manager...");

                try
                {
                    applicationManager = ApplicationManager.Instantiate(managers);
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    throw new ApplicationInitializationException("Error instantiating the Program Manager.  See the inner exception for details.", ex);
                }

                logger.Debug("The Program Manager was instantiated successfully.");

                //-------------- -   ---------------------------------------
                // determine whether the application is being run as a Windows service or as a console application and start accordingly.
                // it is possible to run Windows services on unix using mono-service, however this functionality is currently TBD.
                if ((PlatformManager.GetPlatformType() == PlatformType.Windows) && (!Environment.UserInteractive))
                {
                    logger.Info("Starting the application in service mode...");
                    ServiceBase.Run(new WindowsService());
                }
                else
                {
                    logger.Info("Starting the application in interactive mode...");
                    Start(args);
                    Stop();
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("The application encountered a fatal error.");
                logger.Exception(LogLevel.Fatal, ex);
            }
            finally
            {
                logger.ExitMethod();
            }
        }

        /// <summary>
        /// Entry point for the application logic.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <exception cref="ApplicationStartException">Thrown when an exception is encountered while starting the application.</exception>
        internal static void Start(string[] args)
        {
            logger.EnterMethod(xLogger.Params((object)args));
            logger.Heading(LogLevel.Debug, "Startup");

            // this is the main try/catch for the application logic.  If an unhandled exception is thrown
            // anywhere in the application it will be caught here and treated as a fatal error, stopping the application.
            try
            {
                // start the program manager.
                logger.SubHeading(LogLevel.Debug, applicationManager.ManagerName);
                logger.Info($"Starting the {applicationManager.ManagerName}...");

                Result managerStartResult = applicationManager.Start();

                if (managerStartResult.ResultCode == ResultCode.Failure)
                {
                    throw new ApplicationStartException($"The {applicationManager.ManagerName} failed to start: {managerStartResult.GetLastError()}");
                }

                logger.Info($"{applicationManager.ManagerName} started.");
                logger.Info("Performing startup tasks...");

                Startup();
                
                logger.Info($"{applicationManager.ProductName} is running.");

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                throw new ApplicationStartException("Error starting the application.  See the inner exception for details.", ex);
            }
            finally
            {
                logger.ExitMethod();
            }
        }

        /// <summary>
        /// Exit point for the application logic.
        /// </summary>
        /// <exception cref="ApplicationStopException">Thrown when an exception is encountered while stopping the application.</exception>
        internal static void Stop()
        {
            logger.EnterMethod();

            try
            {
                // stop the program manager.
                logger.Heading(LogLevel.Debug, "Shutdown");
                logger.Info($"{applicationManager.ProductName} is stopping...");

                Result managerStopResult = applicationManager.Stop(StopType.Shutdown);

                if (managerStopResult.ResultCode == ResultCode.Failure)
                {
                    throw new ApplicationStopException($"The Program Manager failed to stop: {managerStopResult.GetLastError()}");
                }

                logger.Info($"{applicationManager.ManagerName} stopped.");
                logger.Info("Performing shutdown tasks...");

                Shutdown();

                logger.Info($"{applicationManager.ProductName} stopped.");
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                throw new ApplicationStopException("Error stopping the application.  See the inner exception for details.", ex);
            }
            finally
            {
                logger.ExitMethod();
            }
        }

        #endregion

        #endregion

        #region Private Methods

        #region Private Static Methods

        /// <summary>
        /// Miscellaneous startup tasks.
        /// </summary>
        /// <exception cref="ApplicationStartupRoutineException">Thrown when an exception is encountered while performing the program startup routine.</exception>
        private static void Startup()
        {
            Guid guid = logger.EnterMethod(true);

            try
            {
                // attach the Platform connector items to the model
                // detatch anything in "Symbiote.System.Platform" that was loaded from the config file
                logger.Info("Detatching potentially stale Platform items...");
                applicationManager.GetManager<IModelManager>().RemoveItem(applicationManager.GetManager<ModelManager>().FindItem(applicationManager.InstanceName + ".System.Platform"));

                logger.Info("Attaching new Platform items...");

                // find or create the parent for the Platform items
                Item systemItem = applicationManager.GetManager<IModelManager>().FindItem(applicationManager.InstanceName + ".System");
                if (systemItem == default(Item))
                {
                    systemItem = applicationManager.GetManager<IModelManager>().AddItem(new Item(applicationManager.InstanceName + ".System")).ReturnValue;
                }

                // attach the Platform items to Symbiote.System
                applicationManager.GetManager<IModelManager>().AttachItem(applicationManager.GetManager<IPlatformManager>().Platform.Connector.Browse(), systemItem);
                logger.Info("Attached Platform items to '" + systemItem.FQN + "'.");

                // find the root item, or create it if it doesnt exist for some reason
                Item symItem = applicationManager.GetManager<IModelManager>().FindItem(applicationManager.InstanceName);
                if (symItem == default(Item))
                {
                    symItem = applicationManager.GetManager<IModelManager>().AddItem(new Item(applicationManager.InstanceName)).ReturnValue;
                }

                // attach all of the simulation items to the model
                applicationManager.GetManager<IModelManager>().AttachItem(((IConnector)applicationManager.GetManager<IPluginManager>().FindPluginInstance("Simulation")).Browse(), symItem);

                // subscribe to the datetime item
                Result subscribe = applicationManager.GetManager<IModelManager>().FindItem("Symbiote.Simulation.DateTime.Time").SubscribeToSource();
                subscribe.LogResult(logger.Info);

                applicationManager.GetManager<IPluginManager>().FindPluginInstance("Simulation").Start();

                // show 'em what they've won!
                Utility.PrintLogo(logger);
                Utility.PrintModel(logger, applicationManager.GetManager<IModelManager>().Model, 0, null, true);
                Utility.PrintLogoFooter(logger);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                throw new ApplicationStartupRoutineException("Error encountered during the startup routine.  See the inner exception for details.", ex);
            }
            finally
            {
                logger.ExitMethod(guid);
            }
        }

        /// <summary>
        /// Miscellaneous shutdown tasks.
        /// </summary>
        /// <exception cref="ApplicationShutdownRoutineException">Thrown when an exception is encountered while performing the program shutdown routine.</exception>
        private static void Shutdown()
        {
            Guid guid = logger.EnterMethod(true);

            try
            {
                // maybe later!?
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                throw new ApplicationShutdownRoutineException("Error encountered during the shutdown routine.  See the inner exception for details.", ex);
            }
            finally
            {
                logger.ExitMethod(guid);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
