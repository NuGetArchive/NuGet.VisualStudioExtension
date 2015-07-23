// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace NuGet.VisualStudio
{
    [Export(typeof(IVsPackageSourceContextFactory))]
    public class VsPackageSourceContextFactory : IVsPackageSourceContextFactory
    {
        public IVsPackageSourceContext CreateContext(
            bool includeEnabledSources, 
            IEnumerable<string> sources, 
            IEnumerable<string> registryKeySources,
            IEnumerable<string> extensionIdSources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (registryKeySources == null)
            {
                throw new ArgumentNullException(nameof(registryKeySources));
            }

            if (extensionIdSources == null)
            {
                throw new ArgumentNullException(nameof(extensionIdSources));
            }

            return new VsPackageSourceContext()
            {
                IncludeEnabledSources = includeEnabledSources,
                Sources = sources.ToList(),
                RegistrySources = registryKeySources.ToList(),
                ExtensionSources = extensionIdSources.ToList()
            };
        }
    }
}
