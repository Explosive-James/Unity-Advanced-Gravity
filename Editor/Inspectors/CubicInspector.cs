using UnityEditor;
using UnityEngine;

namespace AdvancedGravity.UnityEditor.Inspectors
{
    [CustomEditor(typeof(CubicGravity))]
    public class CubicInspector : Editor
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

            // Finding _aspectRatio property inside of the gravity field.
            SerializedProperty aspectProperty = serializedGravity.FindProperty("_aspectRatio");
            // Initialising the _aspectRatio inspector content.
            GUIContent aspectContent = new GUIContent("Aspect Ratio", "The ratio of the cube realtive to the other axes");

            // Drawing aspect inspector gui.
            Vector3 aspectValue = EditorGUILayout.Vector3Field(aspectContent, aspectProperty.vector3Value);

            // Checking if _aspectRatio needs updating.
            if (EditorGUI.EndChangeCheck()) {

                aspectProperty.vector3Value = aspectValue;
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
