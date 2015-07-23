// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NuGet.VisualStudio
{
    [ComImport]
    [Guid("74D03834-F5C5-4206-B830-F5773EC45E01")]
    public interface IVsPackageSourceContext
    {
        /// <summary>
        /// If true all enabled sources from nuget.config will be included as fallback sources.
        /// To use only the sources specified in this context set this flag to false.
        /// </summary>
        /// <remarks>NuGet 2.x and 3.x sources are supported.</remarks>
        /// <remarks>By default this includes nuget.org unless the user has disabled that source.</remarks>
        bool IncludeEnabledSources { get; }

        /// <summary>
        /// Local and remote sources. Paths to local folders, UNC shares, and URLs to
        /// online repositories may be added here.
        /// </summary>
        /// <remarks>This field is optional and may contain an empty list.</remarks>
        IReadOnlyList<string> Sources { get; }

        /// <summary>
        /// Registry keys pointing to sources.
        /// </summary>
        /// <remarks>This field is optional and may contain an empty list.</remarks>
        IReadOnlyList<string> RegistrySources { get; }

        /// <summary>
        /// Extension Ids containing sources.
        /// </summary>
        /// <remarks>This field is optional and may contain an empty list.</remarks>
        IReadOnlyList<string> ExtensionSources { get; }
    }
}
