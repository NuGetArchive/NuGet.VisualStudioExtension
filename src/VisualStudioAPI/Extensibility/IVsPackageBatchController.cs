// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.Extensibility
{
    /// <summary>
    /// IVsPackageBatchController provides support for disabling automatic updates and package restores
    /// to project.json based projects. Packages.config based projects will not be supported by this API.
    /// 
    /// Operations that edit project.json should start a batch mode operation here before editting the file
    /// to avoid multiple restores while making changes to the file.
    /// </summary>
    public interface IVsPackageBatchController
    {
        /// <summary>
        /// Start a batch mode operation. This will disable automatic operations triggered
        /// by package operations.
        /// </summary>
        void StartBatchMode();

        /// <summary>
        /// End a batch mode operation. New operations will trigger automatic updates as normal.
        /// </summary>
        void EndBatchMode();

        /// <summary>
        /// True if batch mode is currently enabled.
        /// </summary>
        bool IsBatchInProgress { get; }
    }
}
