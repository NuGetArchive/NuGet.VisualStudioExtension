// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Represents a basic entry in a package spec that contains an id.
    /// </summary>
    [ComImport]
    [Guid("7B8682EC-C512-420E-B098-59588CF93D64")]
    public interface IVsPackageSpecEntry
    {
        /// <summary>
        /// Entry Id
        /// </summary>
        string Id { get; }
    }
}
