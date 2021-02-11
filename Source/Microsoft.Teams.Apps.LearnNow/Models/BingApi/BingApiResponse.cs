// <copyright file="BingApiResponse.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Models.BingApiRequestModel
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Used to deserialize Bing search result.
    /// </summary>
    public class BingApiResponse
    {
        /// <summary>
        /// Gets or sets Images.
        /// </summary>
        [JsonProperty("value")]
        public IEnumerable<Images> Images { get; set; }
    }
}
