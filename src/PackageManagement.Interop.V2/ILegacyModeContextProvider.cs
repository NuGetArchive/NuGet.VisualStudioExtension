namespace NuGet.PackageManagement.Interop.V2
{
    public interface ILegacyModeContextProvider
    {
        /// <summary>
        /// Retrieve the global legacy context
        /// </summary>
        LegacyModeContext GetContext();
    }
}