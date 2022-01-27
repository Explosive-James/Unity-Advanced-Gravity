using UnityEditor;
using AdvancedGravity;

namespace AdvancedGravityEditor.Inspectors
{
    [CustomEditor(typeof(SphericalGravity))]
    public class SphericalInspector : Editor
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
