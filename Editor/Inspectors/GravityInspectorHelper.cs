using UnityEngine;
using UnityEditor;
using AdvancedGravityEditor.Settings;
using AdvancedGravity;

namespace AdvancedGravityEditor.Inspectors
{
    public static class GravityInspectorHelper
    {
        #region Public Functions
        public static void DrawPriorityInspector(SerializedObject gravityField)
        {
            EditorGUI.BeginChangeCheck();

            // Finding the _priority property inside the gravity field.
            SerializedProperty priorityProperty = gravityField.FindProperty("_priority");
            // Initilaising the _priority inspector content,
            GUIContent priorityContent = new GUIContent("Priority", "Gravity fields with a higher priority will have exclusive influence over a rigidbody.");

            // Drawing the _priority inspector gui.
            int priorityValue = EditorGUILayout.IntField(priorityContent, priorityProperty.intValue);

            // Checking if the priority needs updating.
            if (EditorGUI.EndChangeCheck()) {

                priorityProperty.intValue = priorityValue;
                gravityField.ApplyModifiedProperties();
            }
        }
        public static void DrawGravityInspector(SerializedObject gravityField)
        {
            EditorGUI.BeginChangeCheck();

            // Finding the gravity names inside the gravity settings scriptable object.
            string[] gravityNames = GetGravityStrengthNames();

            // Finding the target strength property inside the gravity field.
            SerializedProperty targetProperty = gravityField.FindProperty("targetStrength");
            // initialising the target strength inspector content.
            GUIContent targetContent = new GUIContent("Gravity Mode", "The strength of the gravity field's gravitational force.");

            // Drawing the target strength popup.
            int targetValue = EditorGUILayout.Popup(targetContent, targetProperty.intValue + 1, gravityNames) - 1;

            // Checking if the target strength property needs updating.
            if (EditorGUI.EndChangeCheck()) {

                targetProperty.intValue = targetValue;
                gravityField.ApplyModifiedProperties();
            }

            // Checking if custom gravity is being used.
            if(targetValue == -1) {

                EditorGUI.BeginChangeCheck();

                // Finding the _strength property inside the gravity field.
                SerializedProperty strengthProperty = gravityField.FindProperty("_strength");
                // Initialising the _strength inspector content.
                GUIContent strengthContent = new GUIContent("Strength", "The custom gravitation strength of the gravity field.");

                // Drawing the _strength inspector gui.
                float strengthValue = EditorGUILayout.FloatField(strengthContent, strengthProperty.floatValue);

                // Checking if the _strength property needs updating.
                if (EditorGUI.EndChangeCheck()) {

                    strengthProperty.floatValue = strengthValue;
                    gravityField.ApplyModifiedProperties();
                }
            }
            else {

                // Disabling float field to make it readonly.
                EditorGUI.BeginDisabledGroup(true);

                // Finding the gravity strength of the gravity field.
                float strength = GetGravityStrength(targetValue);
                EditorGUILayout.FloatField(new GUIContent("Strength", "The current gravitation strength of the gravity field."), strength);

                EditorGUI.EndDisabledGroup();
            }
        }
        public static void DrawMaximumRangeInspector(SerializedObject gravityField)
        {
            EditorGUI.BeginChangeCheck();

            // Finding the maximum range property inside the gravity field.
            SerializedProperty maximumRangeProperty = gravityField.FindProperty("_maximumRange");
            // Initialising the maximum range inspector content.
            GUIContent maximumRangeContent = new GUIContent("Maximum Range", "The maximum range of the gravity field.");

            // Drawing maximum range inspector gui.
            float maximumRangeValue = EditorGUILayout.FloatField(maximumRangeContent, maximumRangeProperty.floatValue);

            // Finding the maximum fade property inside gravity field.
            SerializedProperty maximumFadeProperty = gravityField.FindProperty("_maximumFade");
            // Initialising the maximum fade inspector content.
            GUIContent maximumFadeContent = new GUIContent("Range", "The transition from maximum gravity and zero from the maximum range.");

            // Drawing maximum fade inspector gui.
            float minimumRangeValue = gravityField.FindProperty("_minimumRange").floatValue;
            float maximumFadeValue = EditorGUILayout.Slider(maximumFadeContent, maximumFadeProperty.floatValue, 0, maximumRangeValue - minimumRangeValue);

            // Checking if the values have been updated.
            if (EditorGUI.EndChangeCheck()) {

                maximumRangeProperty.floatValue = maximumRangeValue;
                maximumFadeProperty.floatValue = maximumFadeValue;

                gravityField.ApplyModifiedProperties();
            }
        }
        public static void DrawMinimumRangeInspector(SerializedObject gravityField)
        {
            EditorGUI.BeginChangeCheck();

            // Finding the minimum range property inside the gravity field.
            SerializedProperty minimumRangeProperty = gravityField.FindProperty("_minimumRange");
            // Initialising the minimum range inspector content.
            GUIContent minimumRangeContent = new GUIContent("Minimum Range", "The minimum range of the gravity field.");

            // Drawing minimum range inspector gui.
            float minimumRangeValue = EditorGUILayout.FloatField(minimumRangeContent, minimumRangeProperty.floatValue);

            // Finding the minimum fade property inside gravity field.
            SerializedProperty minimumFadeProperty = gravityField.FindProperty("_minimumFade");
            // Initialising the minimum fade inspector content.
            GUIContent minimumFadeContent = new GUIContent("Range", "The transition from maximum gravity and zero from the minimum range.");

            // Drawing minimum fade inspector gui.
            float maximumRangeValue = gravityField.FindProperty("_maximumRange").floatValue;
            float minimumFadeValue = EditorGUILayout.Slider(minimumFadeContent, minimumFadeProperty.floatValue, 0, maximumRangeValue - minimumRangeValue);

            // Checking if the values have been updated.
            if (EditorGUI.EndChangeCheck()) {

                minimumRangeProperty.floatValue = minimumRangeValue;
                minimumFadeProperty.floatValue = minimumFadeValue;

                gravityField.ApplyModifiedProperties();
            }
        }
        #endregion

        #region Private Functions
        private static string[] GetGravityStrengthNames()
        {
            GravitySettings settings = GravitySettings.Instance;
            string[] results = new string[settings.strengths.Length + 1];

            for (int i = 0; i < settings.strengths.Length; i++)
                results[i + 1] = settings.strengths[i].name;
            results[0] = "Custom";

            return results;
        }
        private static float GetGravityStrength(int index)
        {
            GravitySettings settings = GravitySettings.Instance;
            return settings.strengths[index].strength;
        }
        #endregion
    }
}
