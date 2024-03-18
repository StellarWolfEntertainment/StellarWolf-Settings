namespace StellarWolf
{

    /// <summary>
    /// Enumerates the different types of settings usage in StellarWolf.
    /// </summary>
    public enum SettingsUsage
    {

        /// <summary>
        /// Represents settings used during runtime.
        /// </summary>
        Runtime,
#if UNITY_EDITOR
        /// <summary>
        /// Represents project-specific settings.
        /// </summary>
        Project,

        /// <summary>
        /// Represents user-specific settings.
        /// </summary>
        User
#endif
    }
}