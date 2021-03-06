﻿// <copyright file="GraphConstants.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Common
{
    /// <summary>
    /// Microsoft Graph Constants.
    /// </summary>
    public static class GraphConstants
    {
        /// <summary>
        /// Max page size.
        /// </summary>
        public const int MaxPageSize = 999;

        /// <summary>
        /// Max retry for Graph API calls.
        /// Note: Max value allowed is 10.
        /// </summary>
        public const int MaxRetry = 5;
    }
}
