using System;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace StellarWolf
{

    /// <summary>
    /// Represents a base class for settings objects.
    /// </summary>
    /// <typeparam name="T">The type of the settings object.</typeparam>
    public abstract class Settings<T> : ScriptableObject where T : Settings<T>
    {

        #region Fields

        private static T s_Instance;
        private static SettingsAttribute s_Attribute;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the instance of the settings object.
        /// </summary>
        protected static T instance { get { return Initialize (); } }

        /// <summary>
        /// Gets the settings attribute associated with the settings object.
        /// </summary>
        public static SettingsAttribute attribute { get { return s_Attribute ??= typeof ( T ).GetCustomAttribute<SettingsAttribute> ( true ); } }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the settings object.
        /// </summary>
        /// <returns>The instance of the settings object.</returns>
        private static T Initialize ()
        {
            if (s_Instance != null)
            {
                return s_Instance;
            }

            if (attribute == null)
            {
                throw new InvalidOperationException ( $"[Settings] attribute missing for type: {typeof ( T ).Name}" );
            }

            string filename = attribute.filename ?? typeof ( T ).Name;
            string path = GetSettingsPath () + filename + ".asset";

            if (attribute.usage == SettingsUsage.Runtime)
            {
                s_Instance = Resources.Load<T> ( filename );
            }
#if UNITY_EDITOR
            else
            {
                s_Instance = AssetDatabase.LoadAssetAtPath<T> ( path );
            }

            if (s_Instance != null)
            {
                return s_Instance;
            }

            T [] instances = Resources.FindObjectsOfTypeAll<T> ();

            if (instances.Length > 0)
            {
                string oldPath = AssetDatabase.GetAssetPath ( instances [ 0 ] );
                string result = AssetDatabase.MoveAsset ( oldPath, path );

                if (string.IsNullOrEmpty ( result ))
                {
                    return s_Instance = instances [ 0 ];
                }
                else
                {
                    Debug.LogWarning ( $"Failed to move previous settings asset '{oldPath}' to '{path}'. A new settings asset will be created.", s_Instance );
                }
            }
#endif

            if (s_Instance != null)
            {
                return s_Instance;
            }

            s_Instance = CreateInstance<T> ();

#if UNITY_EDITOR
            MonoScript script = MonoScript.FromScriptableObject ( s_Instance );

            if (script == null || script.name != typeof ( T ).Name)
            {
                DestroyImmediate ( s_Instance );
                s_Instance = null;
                throw new InvalidOperationException ( "Settings-derived class and filename must match: {typeof(T).Name}" );
            }

            Directory.CreateDirectory ( Path.Combine ( Directory.GetCurrentDirectory (), Path.GetDirectoryName ( path ) ) );
            AssetDatabase.CreateAsset ( s_Instance, path );
#endif

            return s_Instance;
        }

        /// <summary>
        /// Gets the path where the settings object is stored.
        /// </summary>
        /// <returns>The path where the settings object is stored.</returns>
        private static string GetSettingsPath ()
        {
            return "Assets/Settings/" + attribute.usage switch
            {
                SettingsUsage.Runtime => "Resources/",
#if UNITY_EDITOR
                SettingsUsage.Project => "Editor/",
                SettingsUsage.User => "Editor/User/" + GetProjectFolderName () + '/',
#endif
                _ => throw new InvalidOperationException ()
            };
        }

        /// <summary>
        /// Validates the settings object.
        /// </summary>
        protected virtual void OnValidate () { }

        /// <summary>
        /// Sets a setting value and triggers validation.
        /// </summary>
        /// <typeparam name="S">The type of the setting.</typeparam>
        /// <param name="setting">The reference to the setting.</param>
        /// <param name="value">The value to set.</param>
        protected void Set<S> ( ref S setting, S value )
        {
            if (EqualityComparer<S>.Default.Equals ( setting, value ))
            {
                return;
            }

            setting = value;
            OnValidate ();

#if UNITY_EDITOR
            SetDirty ();
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// Sets the settings object as dirty.
        /// </summary>
        protected new void SetDirty ()
        {
            EditorUtility.SetDirty ( this );
        }

        /// <summary>
        /// Gets the name of the project folder.
        /// </summary>
        /// <returns>The name of the project folder.</returns>
        private static string GetProjectFolderName ()
        {
            string [] path = Application.dataPath.Split ( '/' );
            return path [ ^2 ];
        }
#endif

        /// <summary>
        /// Represents a base class for sub-settings objects.
        /// </summary>
        [Serializable]
        public abstract class SubSettings
        {

            /// <summary>
            /// Validates the sub-settings object.
            /// </summary>
            protected virtual void OnValidate () { }

            /// <summary>
            /// Sets a sub-setting value and triggers validation.
            /// </summary>
            /// <typeparam name="S">The type of the sub-setting.</typeparam>
            /// <param name="setting">The reference to the sub-setting.</param>
            /// <param name="value">The value to set.</param>
            protected void Set<S> ( ref S setting, S value )
            {
                if (EqualityComparer<S>.Default.Equals ( setting, value ))
                {
                    return;
                }

                setting = value;
                OnValidate ();

#if UNITY_EDITOR
                instance.SetDirty ();
#endif
            }
        }

        #endregion
    }
}
