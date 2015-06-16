// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// An <see cref="DeleteOnRestartManager"/> manger which is used for surfacing errors and UI in VS.
    /// </summary>
    [Export(typeof(IDeleteOnRestartManager))]
    public class VsDeleteOnRestartManager : DeleteOnRestartManager
    {
        /// <summary>
        /// Creates a new instance of <see cref="VsDeleteOnRestartManager"/>.
        /// </summary>
        public VsDeleteOnRestartManager() : this(
            ServiceLocator.GetInstance<ISettings>(),
            ServiceLocator.GetInstance<ISolutionManager>())
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="VsDeleteOnRestartManager"/>.
        /// </summary>
        /// <param name="settings">An <see cref="ISettings"/> for the current solution.</param>
        /// <param name="solutionManager">An <see cref="ISolutionManager"/> for the current solution.</param>
        public VsDeleteOnRestartManager(ISettings settings, ISolutionManager solutionManager) :
            base(settings, solutionManager)
        {
            SolutionManager.SolutionOpened += OnSolutionOpenedOrClosed;
            SolutionManager.SolutionClosed += OnSolutionOpenedOrClosed;
        }

        private void OnSolutionOpenedOrClosed(object sender, EventArgs e)
        {
            // This is a solution event. Should be on the UI thread
            ThreadHelper.ThrowIfNotOnUIThread();

            // We need to do the check even on Solution Closed because, let's say if the yellow Update bar
            // is showing and the user closes the solution; in that case, we want to hide the Update bar.
            DeleteMarkedPackageDirectories(SolutionManager.NuGetProjectContext);
        }
    }
}
