﻿using Newtonsoft.Json;
using NLog;
using OpenIIoT.Core.Platform;
using OpenIIoT.Core.Plugin;
using OpenIIoT.Core.Plugin.Archive;
using OpenIIoT.SDK;
using OpenIIoT.SDK.Common;
using OpenIIoT.SDK.Plugin;
using OpenIIoT.SDK.Plugin.Archive;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Utility.OperationResult;

namespace OpenIIoT.Core.Service.Web.API
{
    /// <summary>
    ///     Handles the API methods for AppArchives.
    /// </summary>
    public class PluginController : ApiController, IApiController
    {
        #region Variables

        /// <summary>
        ///     The Logger for this class.
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     The ApplicationManager for the application.
        /// </summary>
        private IApplicationManager manager = ApplicationManager.GetInstance();

        /// <summary>
        ///     The default serialization properties for an AppArchive.
        /// </summary>
        private static List<string> pluginArchiveSerializationProperties = new List<string>(new string[] { });

        #endregion Variables

        #region Instance Methods

        /// <summary>
        ///     Returns the list of available AppArchives.
        /// </summary>
        /// <returns>The list of available AppArchives.</returns>
        [Route("api/plugin/archive")]
        [HttpGet]
        public HttpResponseMessage ListPluginArchives()
        {
            ApiResult<IList<IPluginArchive>> retVal = new ApiResult<IList<IPluginArchive>>(Request);
            retVal.LogRequest(logger.Info);

            retVal.ReturnValue = manager.GetManager<PluginManager>().PluginArchives;

            retVal.LogResult(logger);
            return retVal.CreateResponse(JsonFormatter(pluginArchiveSerializationProperties, ContractResolverType.OptOut));
        }

        /// <summary>
        ///     Reloads the list of PluginArchives from disk and returns the list.
        /// </summary>
        /// <returns>The reloaded list of available PluginArchives.</returns>
        [Route("api/plugin/archive/reload")]
        [HttpGet]
        public HttpResponseMessage ReloadPluginArchives()
        {
            ApiResult<IPluginArchiveLoadResult> retVal = new ApiResult<IPluginArchiveLoadResult>(Request);
            retVal.LogRequest(logger.Info);

            retVal.ReturnValue = manager.GetManager<PluginManager>().ReloadPluginArchives();

            if (retVal.ReturnValue.ResultCode == ResultCode.Failure)
                retVal.StatusCode = HttpStatusCode.InternalServerError;

            retVal.LogResult(logger);
            return retVal.CreateResponse(JsonFormatter(pluginArchiveSerializationProperties, ContractResolverType.OptOut));
        }

        /// <summary>
        ///     Returns the PluginArchive from the list of available PluginArchives that matches the supplied filename.
        /// </summary>
        /// <param name="fileName">The Fully Qualified Name of the PluginArchive to return.</param>
        /// <returns>The matching PluginArchive.</returns>
        [Route("api/plugin/archive/{fileName}")]
        [HttpGet]
        public HttpResponseMessage GetPluginArchive(string fileName)
        {
            ApiResult<IPluginArchive> retVal = new ApiResult<IPluginArchive>(Request);
            retVal.LogRequest(logger.Info);

            retVal.ReturnValue = manager.GetManager<PluginManager>().PluginArchives.Where(p => p.FileName == fileName).FirstOrDefault();

            if (retVal.ReturnValue == default(PluginArchive))
                retVal.StatusCode = HttpStatusCode.NotFound;

            retVal.LogResult(logger);
            return retVal.CreateResponse(JsonFormatter(new List<string>(new string[] { }), ContractResolverType.OptOut));
        }

        /// <summary>
        ///     Installs the supplied PluginArchive.
        /// </summary>
        /// <param name="fileName">The filename of the Plugin Archive to install.</param>
        /// <returns>The App instance resulting from the installation.</returns>
        [Route("api/plugin/archive/{fileName}/install")]
        [HttpGet]
        public async Task<HttpResponseMessage> InstallPlugin(string fileName)
        {
            ApiResult<Result<IPlugin>> retVal = new ApiResult<Result<IPlugin>>(Request);
            retVal.LogRequest(logger.Info);

            retVal.ReturnValue = await manager.GetManager<PluginManager>().InstallPluginAsync(manager.GetManager<PluginManager>().PluginArchives.Where(p => p.FileName == fileName).FirstOrDefault());

            if ((retVal.ReturnValue == default(Result<Plugin.Plugin>)) || (retVal.ReturnValue.ResultCode == ResultCode.Failure))
                retVal.StatusCode = HttpStatusCode.InternalServerError;

            retVal.LogResult(logger);
            return retVal.CreateResponse(JsonFormatter(new List<string>(new string[] { }), ContractResolverType.OptOut));
        }

        [Route("api/plugin")]
        [HttpGet]
        public HttpResponseMessage ListPlugins()
        {
            ApiResult<List<IPlugin>> retVal = new ApiResult<List<IPlugin>>(Request);
            retVal.LogRequest(logger.Info);

            retVal.ReturnValue = manager.GetManager<PluginManager>().Configuration.InstalledPlugins.ToList<IPlugin>();

            retVal.LogResult(logger);
            return retVal.CreateResponse(JsonFormatter(pluginArchiveSerializationProperties, ContractResolverType.OptOut));
        }

        /// <summary>
        ///     Uninstalls the supplied Plugin.
        /// </summary>
        /// <param name="fqn">The Fully Qualified Name of the Plugin to uninstall.</param>
        /// <returns>An ApiResult containing the result of the operation.</returns>
        [Route("api/plugin/{fqn}/uninstall")]
        [HttpGet]
        public async Task<HttpResponseMessage> UninstallPlugin(string fqn)
        {
            ApiResult<Result> retVal = new ApiResult<Result>(Request);
            retVal.LogRequest(logger.Info);

            retVal.ReturnValue = await manager.GetManager<PluginManager>().UninstallPluginAsync(manager.GetManager<PluginManager>().FindPlugin(fqn));

            retVal.LogResult(logger);
            return retVal.CreateResponse(JsonFormatter(new List<string>(new string[] { }), ContractResolverType.OptOut));
        }

        [Route("api/plugin/archive/{fileName}/download")]
        [HttpGet]
        public HttpResponseMessage DownloadPluginArchive(string fileName)
        {
            ApiResult<bool> retVal = new ApiResult<bool>(Request);
            retVal.LogRequest(logger.Info);

            string pluginArchive = System.IO.Path.Combine(manager.GetManager<PlatformManager>().Platform.Directories.Archives, manager.GetManager<PluginManager>().PluginArchives.Where(p => p.FileName == fileName).FirstOrDefault().FileName);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StreamContent(new System.IO.FileStream(pluginArchive, System.IO.FileMode.Open));
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = System.IO.Path.GetFileName(pluginArchive) };

            retVal.LogResult(logger);
            return result;
        }

        /// <summary>
        ///     Returns the JsonMediaTypeFormatter to use with this controller.
        /// </summary>
        /// <param name="serializationProperties">
        ///     A list of properties to exclude or include, depending on the ContractResolverType, in the serialized result.
        /// </param>
        /// <param name="contractResolverType">
        ///     A ContractResolverType representing the desired behavior of serializationProperties, OptIn or OptOut.
        /// </param>
        /// <param name="includeSecondaryTypes">
        ///     True if secondary types, such as those loaded from Plugins, should be included in the serialization.
        /// </param>
        /// <returns>A configured instance of JsonMediaTypeFormatter</returns>
        public JsonMediaTypeFormatter JsonFormatter(List<string> serializationProperties, ContractResolverType contractResolverType)
        {
            JsonMediaTypeFormatter retVal = new JsonMediaTypeFormatter();

            retVal.SerializerSettings = new JsonSerializerSettings();

            retVal.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            retVal.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            retVal.SerializerSettings.Formatting = Formatting.Indented;
            retVal.SerializerSettings.ContractResolver = new ContractResolver(serializationProperties, contractResolverType);
            retVal.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            return retVal;
        }

        #endregion Instance Methods
    }
}