using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdvancedGravityEditor.Settings
{
    internal static class GravityProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider("Project/Gravity Settings", SettingsScope.Project) {

                label = "Advanced Gravity",
                keywords = new HashSet<string>(new[] { "Advanced", "Gravity", "Strength" }),

                guiHandler = (searchContext) => {

                    SerializedObject settings = new SerializedObject(GravitySettings.Instance);
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.Separator();
                    EditorGUILayout.PropertyField(settings.FindProperty("strengths"), new GUIContent("Strengths"));

                    if (EditorGUI.EndChangeCheck()) {

                        settings.ApplyModifiedProperties();
                        GravitySettings.SaveOrCreate();

                        if (Application.isPlaying) {
                            GravityBuilder builder = new GravityBuilder();
                            builder.OnProcessScene(SceneManager.GetActiveScene(), null);
                        }
                    }
                },
            };
            return provider;
        }
    }
}
