using UnityEditor;
using UnityEngine;
using AdvancedGravity;

namespace AdvancedGravityEditor.Inspectors
{
    [CustomEditor(typeof(PlanarGravity))]
    public class PlanarInspector : Editor
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
            GUIContent aspectContent = new GUIContent("Aspect Ratio", "The ratio of the cube realtive to the other axis");

            // Drawing aspect inspector gui.
            Vector2 aspectValue = EditorGUILayout.Vector2Field(aspectContent, aspectProperty.vector2Value);

            // Checking if _aspectRatio needs updating.
            if (EditorGUI.EndChangeCheck()) {

                aspectProperty.vector2Value = aspectValue;
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
