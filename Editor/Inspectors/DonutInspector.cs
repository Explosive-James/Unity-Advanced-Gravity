using AdvancedGravity;
using UnityEditor;
using UnityEngine;

namespace AdvancedGravityEditor.Inspectors
{
    [CustomEditor(typeof(DonutGravity))]
    public class DonutInspector : Editor
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

            // Finding _radius property inside of the gravity field.
            SerializedProperty radiusProperty = serializedGravity.FindProperty("_radius");
            // Initialising the _radius inspector content.
            GUIContent radiusContent = new GUIContent("Radius", "The radius of the donut that gravity is centered around.");

            // Drawing radius inspector gui.
            float radiusValue = EditorGUILayout.FloatField(radiusContent, radiusProperty.floatValue);

            // Checking if _radius needs updating.
            if (EditorGUI.EndChangeCheck()) {

                radiusProperty.floatValue = radiusValue;
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
