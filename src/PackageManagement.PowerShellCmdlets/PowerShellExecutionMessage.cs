// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    public class Message
    {
    }

    public class ExecutionCompleteMessage : Message
    {
    }

    public class LogMessage : Message
    {
        public LogMessage(MessageLevel level, string content)
        {
            Level = level;
            Content = content;
        }

        public MessageLevel Level { get; private set; }

        public string Content { get; private set; }
    }

    public class ScriptMessage : Message
    {
        public ScriptMessage(string scriptPath)
        {
            ScriptPath = scriptPath;
        }

        public string ScriptPath { get; private set; }
    }

    public class ProgressMessage: Message
    {
        public ProgressMessage(PackageProgressEventArgs args)
        {
            ProgressArgs = args;
        }

        public PackageProgressEventArgs ProgressArgs { get; private set; }
    }
}
