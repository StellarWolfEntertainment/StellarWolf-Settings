using UnityEditor;

namespace StellarWolf.Editor
{

    /// <summary>
    /// Custom editor for settings objects.
    /// </summary>
    [CustomEditor ( typeof ( Settings<> ), true )]
    public class SettingsEditor : UnityEditor.Editor
    {

        private static readonly string [] s_ExcludedFields = { "m_Script" };

        /// <summary>
        /// Draws the default inspector GUI for the settings object.
        /// </summary>
        public override void OnInspectorGUI ()
        {
            DrawDefaultInspector ();
        }

        /// <summary>
        /// Draws the default inspector GUI for the settings object, excluding certain fields.
        /// </summary>
        /// <returns>True if changes were made; otherwise, false.</returns>
        protected new bool DrawDefaultInspector ()
        {
            if (serializedObject.targetObject == null)
            {
                return false;
            }

            EditorGUI.BeginChangeCheck ();
            serializedObject.UpdateIfRequiredOrScript ();
            DrawPropertiesExcluding ( serializedObject, s_ExcludedFields );
            serializedObject.ApplyModifiedProperties ();
            return EditorGUI.EndChangeCheck ();
        }
    }
}
