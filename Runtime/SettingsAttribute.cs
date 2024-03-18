using System;

namespace StellarWolf
{
    /// <summary>
    /// Represents an attribute used to mark classes as settings.
    /// </summary>
    [AttributeUsage ( AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
    public class SettingsAttribute : Attribute
    {

        #region Fields

        private readonly SettingsUsage m_Usage;
        private readonly string m_DisplayName;
        private readonly string m_Filename;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the usage of the settings.
        /// </summary>
        public SettingsUsage usage { get { return m_Usage; } }

        /// <summary>
        /// Gets the display name of the settings.
        /// </summary>
        public string displayName { get { return m_DisplayName; } }

        /// <summary>
        /// Gets the filename associated with the settings.
        /// </summary>
        public string filename { get { return m_Filename; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SettingsAttribute class with the specified usage, display name, and filename.
        /// </summary>
        /// <param name="usage">The usage of the settings.</param>
        /// <param name="displayName">The display name of the settings.</param>
        /// <param name="filename">The filename associated with the settings.</param>
        public SettingsAttribute ( SettingsUsage usage, string displayName = null, string filename = null )
        {
            m_Usage = usage;
            m_Filename = filename;
#if UNITY_EDITOR
            m_DisplayName = string.IsNullOrEmpty ( displayName ) ? null : ((usage == SettingsUsage.User ? "Preferences/" : "Project/") + displayName);
#else
            m_DisplayName = string.IsNullOrEmpty ( displayName ) ? null : "Project/" + displayName);
#endif
        }

#endregion
    }
}