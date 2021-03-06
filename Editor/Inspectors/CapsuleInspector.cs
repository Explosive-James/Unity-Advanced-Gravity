using UnityEngine;
using UnityEditor;
using AdvancedGravity;

namespace AdvancedGravityEditor.Inspectors
{
    [CustomEditor(typeof(CapsuleGravity))]
    public class CapsuleInspector : Editor
    {
        #region Public Functions
        public override void OnInspectorGUI()
        {
            SerializedObject serializedGravity = new SerializedObject(target);

            // Drawing priority inspector gui.
            EditorGUILayout.Separator();
            GravityInspectorHelper.DrawPriorityInspector(serializedGravity);

            // Drawing gravity inspector gui.
            EditorGUILayout.Separator();
            GravityInspectorHelper.DrawGravityInspector(serializedGravity);

            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();

            // Finding _length property inside of the gravity field.
            SerializedProperty lengthProperty = serializedGravity.FindProperty("_length");
            // Initialising the _length inspector content.
            GUIContent lengthContent = new GUIContent("Length", "The length along the local z-axis the capsule should be.");

            // Drawing length inspector gui.
            float lengthValue = EditorGUILayout.FloatField(lengthContent, lengthProperty.floatValue);

            // Checking if _length needs updating.
            if (EditorGUI.EndChangeCheck()) {

                lengthProperty.floatValue = lengthValue;
                serializedGravity.ApplyModifiedProperties();
            }

            // Drawing maximum range inspector gui.
            EditorGUILayout.Separator();
            GravityInspectorHelper.DrawMaximumRangeInspector(serializedGravity);

            // Drawing minimum range inspector gui.
            EditorGUILayout.Separator();
            GravityInspectorHelper.DrawMinimumRangeInspector(serializedGravity);
        }
        #endregion
    }
}
