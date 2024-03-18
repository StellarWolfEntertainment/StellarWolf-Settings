using UnityEditor;
using UnityEngine;

namespace StellarWolf.Editor
{

    /// <summary>
    /// Provides extension methods for settings objects.
    /// </summary>
    public static class SettingsExtensions
    {

        /// <summary>
        /// Gets the settings provider for the specified settings object.
        /// </summary>
        /// <typeparam name="T">The type of the settings object.</typeparam>
        /// <param name="settings">The settings object.</param>
        /// <returns>The settings provider.</returns>
        public static SettingsProvider GetSettingsProvider<T> ( this Settings<T> settings ) where T : Settings<T>
        {
            Debug.Assert ( Settings<T>.attribute.displayName != null );
            return new ScriptableObjectSettingsProvider ( settings, Settings<T>.attribute.usage == SettingsUsage.User ? SettingsScope.User : SettingsScope.Project, Settings<T>.attribute.displayName );
        }
    }
}