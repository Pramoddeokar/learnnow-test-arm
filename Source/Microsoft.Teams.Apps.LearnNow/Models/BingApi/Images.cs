// <copyright file="Images.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Used to deserialize Bing search result.
    /// </summary>
    public class Images
    {
        /// <summary>
        /// Gets or sets source URL for the image.
        /// </summary>
        [JsonProperty("contentUrl")]
        public string ContentUrl { get; set; }
    }
}
