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
        /// </summary>
        /// <remarks>By default this includes nuget.org unless the user has disabled that source.</remarks>
        bool IncludeEnabledSources { get; }

        /// <summary>
        /// Local and remote sources.
        /// </summary>
        /// <remarks>This field is optional and may be null or empty.</remarks>
        IReadOnlyList<string> Sources { get; }

        /// <summary>
        /// Registry keys pointing to sources.
        /// </summary>
        /// <remarks>This field is optional and may be null or empty.</remarks>
        IReadOnlyList<string> RegistrySources { get; }

        /// <summary>
        /// Extension Ids containing sources.
        /// </summary>
        /// <remarks>This field is optional and may be null or empty.</remarks>
        IReadOnlyList<string> ExtensionSources { get; }
    }
}
