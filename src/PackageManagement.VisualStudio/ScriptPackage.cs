// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Packaging;

namespace NuGet.PackageManagement.VisualStudio
{
    // this scriptPackage is a package inferface for script executor
    // it provides an IPackage like interface to make sure all install.ps scripts which depend on IPackage keep working
    public class ScriptPackage : IScriptPackage
    {
        private string _id;
        private string _version;
        private string _installPath;
        private IList<IPackageAssemblyReference> _assemblyReferences;
        private const string ResourceAssemblyExtension = ".resources.dll";
        private const string PackageEmptyFileName = "_._";

        public ScriptPackage(string id, string version, string installPath)
        {
            _id = id;
            _version = version;
            _installPath = installPath;
        }

        public string Id
        {
            get { return _id; }
        }

        public string Version
        {
            get { return _version; }
        }

        public IEnumerable<IPackageAssemblyReference> AssemblyReferences
        {
            get
            {
                if (_assemblyReferences == null)
                {
                    _assemblyReferences = GetAssemblyReferencesCore().ToList();
                }

                return _assemblyReferences;
            }
        }

        private IEnumerable<IPackageAssemblyReference> GetAssemblyReferencesCore()
        {
            var reader = new PackageFolderReader(_installPath);
           
            return (from file in reader.GetFiles()
                    where IsAssemblyReference(file)
                    select (IPackageAssemblyReference)new PackageAssemblyReference(file)).ToList();
        }

        private bool IsAssemblyReference(string filePath)
        {
            // assembly reference must be under lib/
            if (!filePath.StartsWith("lib" + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var fileName = Path.GetFileName(filePath);

            // if it's an empty folder, yes
            if (fileName == PackageEmptyFileName)
            {
                return true;
            }

            // Assembly reference must have a .dll|.exe|.winmd extension and is not a resource assembly;
            return !filePath.EndsWith(ResourceAssemblyExtension, StringComparison.OrdinalIgnoreCase) &&
                Constants.AssemblyReferencesExtensions.Contains(Path.GetExtension(filePath), StringComparer.OrdinalIgnoreCase);
        }
    }
}
