// <copyright file="BingImageService.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.LearnNow.Common.Interfaces;
    using Microsoft.Teams.Apps.LearnNow.Models;
    using Microsoft.Teams.Apps.LearnNow.Models.Configuration;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Service class for getting images from Bing image search API service.
    /// </summary>
    public class BingImageService : IImageProviderService
    {
        /// <summary>
        /// Bing Image height size
        /// </summary>
        public const int BingImageHeight = 200;

        /// <summary>
        /// Bing Image width size
        /// </summary>
        public const int BingImageWidth = 200;

        /// <summary>
        /// Bing cognitive service setting.
        /// </summary>
        private readonly IOptions<BingSearchServiceSettings> options;

        /// <summary>
        /// Provides a base class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BingImageService"/> class.
        /// </summary>
        /// <param name="options">Bing cognitive service settings</param>
        /// <param name="httpClient">Instance of HttpClient.</param>
        public BingImageService(IOptions<BingSearchServiceSettings> options, HttpClient httpClient)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Method to get image URL's from Bing Image search API for given search text.
        /// </summary>
        /// <param name="searchQueryTerm">Find image URL's based on search query term.</param>
        /// <returns>Returns a collection of image URL from Bing Image API service.</returns>
        public async Task<IEnumerable<string>> GetSearchResultAsync(string searchQueryTerm)
        {
            var contentUrlResult = new List<string>();

            // Make the search request to the Bing Image API, and get the results.
            this.httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.options.Value.Key);

            string requestUri = this.options.Value.Endpoint
                + "?q=" + HttpUtility.HtmlEncode(searchQueryTerm)
                + "&height=" + BingImageHeight
                + "&width=" + BingImageWidth
                + "&safeSearch=" + this.options.Value.SafeSearch;

            HttpResponseMessage response = await this.httpClient.GetAsync(new Uri(requestUri));
            string contentString = await response.Content.ReadAsStringAsync();
            JObject siteListDataResponse = JObject.Parse(contentString);

            if (siteListDataResponse["value"] != null)
            {
                var searchResult = siteListDataResponse["value"].ToString();
                var images = JsonConvert.DeserializeObject<List<Images>>(searchResult);
                var filteredUrlResult = images.Where(image => image.ContentUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    .Select(image => image.ContentUrl);
                contentUrlResult.AddRange(filteredUrlResult);
            }

            return contentUrlResult;
        }
    }
}