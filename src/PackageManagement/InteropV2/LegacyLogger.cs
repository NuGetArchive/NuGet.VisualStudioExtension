extern alias Legacy;
using Legacy.NuGet;
using NuGet.ProjectManagement;
using System;

namespace NuGet.PackageManagement.InteropV2
{
    public class LegacyLogger : ILogger
    {
        private readonly INuGetProjectContext _projectContext;

        public LegacyLogger(INuGetProjectContext projectContext)
        {
            _projectContext = projectContext;
        }

        public void Log(Legacy.NuGet.MessageLevel level, string message, params object[] args)
        {
            _projectContext.Log(GetLevel(level), message, args);
        }

        public FileConflictResolution ResolveFileConflict(string message)
        {
            return GetAction(_projectContext.ResolveFileConflict(message));
        }

        private FileConflictResolution GetAction(FileConflictAction conflict)
        {
            switch (conflict)
            {
                case FileConflictAction.Ignore:
                    return FileConflictResolution.Ignore;
                case FileConflictAction.IgnoreAll:
                    return FileConflictResolution.IgnoreAll;
                case FileConflictAction.Overwrite:
                    return FileConflictResolution.Overwrite;
                case FileConflictAction.OverwriteAll:
                    return FileConflictResolution.OverwriteAll;
            }

            return FileConflictResolution.Overwrite;
        }

        private static ProjectManagement.MessageLevel GetLevel(Legacy.NuGet.MessageLevel level)
        {
            switch (level)
            {
                case Legacy.NuGet.MessageLevel.Debug:
                    return ProjectManagement.MessageLevel.Debug;
                case Legacy.NuGet.MessageLevel.Error:
                    return ProjectManagement.MessageLevel.Error;
                case Legacy.NuGet.MessageLevel.Warning:
                    return ProjectManagement.MessageLevel.Warning;
            }

            return ProjectManagement.MessageLevel.Info;
        }
    }
}
