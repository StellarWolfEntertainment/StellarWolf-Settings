using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UEditor = UnityEditor.Editor;

namespace StellarWolf.Editor
{
    /// <summary>
    /// Provides a settings provider for scriptable objects.
    /// </summary>
    public class ScriptableObjectSettingsProvider : SettingsProvider
    {

        #region Fields

        /// <summary>
        /// The scriptable object settings associated with the provider.
        /// </summary>
        public readonly ScriptableObject settings;

        private UEditor m_Editor;
        private bool m_KeywordsBuilt;
        private SerializedObject m_SerializedSettings;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the serialized settings object associated with the provider.
        /// </summary>
        public SerializedObject serializedSettings
        {
            get
            {
                return m_SerializedSettings ??= new SerializedObject ( settings );
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScriptableObjectSettingsProvider class with the specified settings, scope, and display path.
        /// </summary>
        /// <param name="settings">The scriptable object settings.</param>
        /// <param name="scope">The scope of the settings provider.</param>
        /// <param name="displayPath">The display path of the settings provider.</param>
        public ScriptableObjectSettingsProvider ( ScriptableObject settings, SettingsScope scope, string displayPath ) : base ( displayPath, scope )
        {
            this.settings = settings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when the settings provider is activated.
        /// </summary>
        /// <param name="searchContext">The search context.</param>
        /// <param name="rootElement">The root element of the visual tree.</param>
        public override void OnActivate ( string searchContext, VisualElement rootElement )
        {
            m_Editor = UEditor.CreateEditor ( settings );
            base.OnActivate ( searchContext, rootElement );
        }

        /// <summary>
        /// Called when the settings provider is deactivated.
        /// </summary>
        public override void OnDeactivate ()
        {
            UEditor.DestroyImmediate ( m_Editor );
            m_Editor = null;
            base.OnDeactivate ();
        }

        /// <summary>
        /// Called to draw the GUI for the settings provider.
        /// </summary>
        /// <param name="searchContext">The search context.</param>
        public override void OnGUI ( string searchContext )
        {
            if (settings == null || m_Editor == null)
            {
                return;
            }

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 250;
            GUILayout.BeginHorizontal ();
            GUILayout.Space ( 10 );
            GUILayout.BeginVertical ();
            GUILayout.Space ( 10 );

            m_Editor.OnInspectorGUI ();

            GUILayout.EndVertical ();
            GUILayout.EndHorizontal ();
            EditorGUIUtility.labelWidth = labelWidth;
        }

        /// <summary>
        /// Indicates whether the settings provider has search interest.
        /// </summary>
        /// <param name="searchContext">The search context.</param>
        /// <returns>True if the settings provider has search interest; otherwise, false.</returns>
        public override bool HasSearchInterest ( string searchContext )
        {
            if (!m_KeywordsBuilt)
            {
                keywords = GetSearchKeywordsFromSerializedObject ( serializedSettings );
                m_KeywordsBuilt = true;
            }

            return base.HasSearchInterest ( searchContext );
        }

        #endregion
    }
}
