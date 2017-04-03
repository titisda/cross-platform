﻿using Newtonsoft.Json;

namespace OpenIIoT.SDK.Package.Manifest
{
    public class PackageManifestFile : IPackageManifestFile
    {
        #region Private Properties

        [JsonProperty(Order = 2)]
        public string Hash { get; set; }

        [JsonProperty(Order = 1)]
        public string Source { get; set; }

        #endregion Private Properties
    }
}