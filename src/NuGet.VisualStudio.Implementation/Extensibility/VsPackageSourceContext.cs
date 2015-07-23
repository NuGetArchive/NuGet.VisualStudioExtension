// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.VisualStudio
{
    public class VsPackageSourceContext : IVsPackageSourceContext
    {
        public IReadOnlyList<string> ExtensionSources { get; internal set; }

        public bool IncludeEnabledSources { get; internal set; }

        public IReadOnlyList<string> RegistrySources { get; internal set; }

        public IReadOnlyList<string> Sources { get; internal set; }

        public IReadOnlyList<string> UnzippedRegistrySources { get; internal set; }
    }
}
