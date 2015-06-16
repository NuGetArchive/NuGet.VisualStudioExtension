// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(IDeleteOnRestartManager))]
    public class VsDeleteOnRestartManager : IDeleteOnRestartManager
    {
        private ISolutionManager _solutionManager;
        private ISettings _settings;
        private IDeleteOnRestartManager _deleteOnRestartManager;

        public VsDeleteOnRestartManager() : this(
            ServiceLocator.GetInstance<ISettings>(),
            ServiceLocator.GetInstance<ISolutionManager>())
        {
        }

        public VsDeleteOnRestartManager(ISettings settings, ISolutionManager solutionManager)
        {
            _settings = settings;
            _solutionManager = solutionManager;
            _solutionManager.SolutionOpened += OnSolutionOpened;
        }

        public IDeleteOnRestartManager DeleteOnRestartManager
        {
            get
            {
                if (_deleteOnRestartManager == null)
                {
                    _deleteOnRestartManager = new DeleteOnRestartManager(_settings, _solutionManager);
                }
                return _deleteOnRestartManager;
            }

            set
            {
                _deleteOnRestartManager = value;
            }
        }

        /// <summary>
        /// Marks package directory for future removal if it was not fully deleted during the normal uninstall process
        /// if the directory does not contain any added or modified files.
        /// </summary>
        public void MarkPackageDirectoryForDeletion(PackageIdentity package, string packageRoot, INuGetProjectContext projectContext)
        {
            DeleteOnRestartManager.MarkPackageDirectoryForDeletion(package, packageRoot, projectContext);
        }

        /// <summary>
        /// Attempts to remove marked package directories that were unable to be fully deleted
        /// during the original uninstall.
        /// </summary>
        /// <param name="projectContext"></param>
        /// <returns></returns>
        public void DeleteMarkedPackageDirectories(INuGetProjectContext projectContext)
        {
           DeleteOnRestartManager.DeleteMarkedPackageDirectories(projectContext);
        }

        /// <summary>
        /// Gets the list of package directories that are still need to be deleted in the
        /// local package repository.
        /// </summary>
        /// <returns>List of package directories that need to be deleted.</returns>
        public IReadOnlyList<string> GetPackageDirectoriesMarkedForDeletion()
        {
            return DeleteOnRestartManager.GetPackageDirectoriesMarkedForDeletion();
        }

        private void OnSolutionOpened(object sender, EventArgs e)
        {
            DeleteOnRestartManager = new DeleteOnRestartManager(_settings, _solutionManager);
            DeleteOnRestartManager.DeleteMarkedPackageDirectories(_solutionManager.NuGetProjectContext);
        }      
    }
}
