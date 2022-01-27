using AdvancedGravity.Internal;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdvancedGravityEditor.Settings
{
    internal class GravityBuilder : IProcessSceneWithReport
    {
        #region Properties
        public int callbackOrder => 0;
        #endregion

        #region Public Functions
        public void OnProcessScene(Scene scene, BuildReport report)
        {
            // Finding gravity settings.
            GravitySettings settings = GravitySettings.Instance;

            // Searching for gravity fields.
            foreach (GameObject obj in scene.GetRootGameObjects())
                foreach (GravityField field in obj.GetComponentsInChildren<GravityField>()) {

                    // Serialising to find correct properties.
                    SerializedObject serializedObject = new SerializedObject(field);
                    SerializedProperty targetStrength = serializedObject.FindProperty("targetStrength");

                    if (targetStrength.intValue >= 0) {

                        // Finding gravity property.
                        SerializedProperty gravityForce = serializedObject.FindProperty("_strength");

                        // Adjust gravity strength based on the target index.
                        gravityForce.floatValue = settings.strengths[targetStrength.intValue].strength;
                        serializedObject.ApplyModifiedProperties();
                    }
                }
        }
        #endregion
    }
}
