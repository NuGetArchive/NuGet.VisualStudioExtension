// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;

namespace NuGet.VisualStudio
{
    [Export(typeof(IVsPackageBatching))]
    public class VsPackageBatching : IVsPackageBatching
    {
        private static bool _isBatchInProgress;
        private static object _lockObject = new object();

        public bool IsBatchInProgress
        {
            get
            {
                lock (_lockObject)
                {
                    return _isBatchInProgress;
                }
            }
        }

        public void EndBatchMode()
        {
            lock (_lockObject)
            {
                _isBatchInProgress = false;
            }
        }

        public void StartBatchMode()
        {
            lock (_lockObject)
            {
                _isBatchInProgress = true;
            }
        }
    }
}
