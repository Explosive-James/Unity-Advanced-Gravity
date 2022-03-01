using AdvancedGravity.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedGravity
{
    public static class Gravity
    {
        #region Public Functions
        /// <summary>
        /// Gets the current gravity being applied to a rigidbody.
        /// </summary>
        /// <param name="rigidbody">Rigidbody to find the gravity of.</param>
        public static Vector3 GetGlobalGravity(Rigidbody rigidbody)
        {
            // If the rigidbody has no priority it isn't in a gravity field,
            // and if it doesn't use gravity it won't have any gravity applied.
            if (!GravityField.globalPriorities.ContainsKey(rigidbody) || !rigidbody.useGravity) {
                return Vector3.zero;
            }

            // Caching the priority of the rigidbody to save repeated calls.
            int targetPriority = GravityField.globalPriorities[rigidbody];
            // The combined local gravity values.
            Vector3 globalGravity = Vector3.zero;

            // Searching for gravity fields with the correct priority.
            for (int i = 0; i < GravityField.globalFields.Count; i++)
                if (GravityField.globalFields[i].Priority == targetPriority &&
                    GravityField.globalFields[i].enabled) {

                    // Adding the field gravity to the total gravity value.
                    globalGravity += GravityField.globalFields[i].GetFieldGravity(rigidbody.position);
                }

            return globalGravity;
        }

        /// <summary>
        /// Gets the gravity at a given position.
        /// </summary>
        /// <param name="position">Position to check the gravity at.</param>
        public static Vector3 GetGlobalGravity(Vector3 position)
        {
            // Used to track the highest prioty field at the position.
            int currentPriority = int.MinValue;
            // The combined local gravity values.
            Vector3 globalGravity = Vector3.zero;

            for (int i = 0; i < GravityField.globalFields.Count; i++)
                if (GravityField.globalFields[i].Priority >= currentPriority &&
                    GravityField.globalFields[i].enabled) {

                    // Adding gravity to the current calculation.
                    Vector3 fieldGravity = GravityField.globalFields[i].GetFieldGravity(position);
                    globalGravity += fieldGravity;

                    // If the field has a higher priority, all previous values should be ignored.
                    if (fieldGravity != Vector3.zero &&
                        GravityField.globalFields[i].Priority > currentPriority) {

                        currentPriority = GravityField.globalFields[i].Priority;
                        globalGravity = fieldGravity;
                    }
                }

            return globalGravity;
        }

        /// <summary>
        /// Returns an array of gravity fields currently influencing the rigidbody.
        /// </summary>
        /// <param name="rigidbody">Rigidbody to find the influence of.</param>
        /// <returns>Array of gravity fields.</returns>
        public static GravityField[] GetActiveFields(Rigidbody rigidbody)
        {
            // If the rigidbody has no priority it isn't in a gravity field,
            // and if it doesn't use gravity it won't have any gravity applied.
            if (!GravityField.globalPriorities.ContainsKey(rigidbody) || !rigidbody.useGravity) {
                return new GravityField[0];
            }

            // Creating the return values, with the capacity to store every field if needed to save re-allocation.
            List<GravityField> fields = new List<GravityField>(GravityField.globalFields.Count);
            // Caching the priority of the rigidbody to save repeated calls.
            int targetPriority = GravityField.globalPriorities[rigidbody];

            for (int i = 0; i < GravityField.globalFields.Count; i++)
                if (GravityField.globalFields[i].Priority == targetPriority &&
                    GravityField.globalFields[i].enabled) {

                    // Checking if the field is actually applying a force.
                    Vector3 fieldGravity = GravityField.globalFields[i].GetFieldGravity(rigidbody.position);

                    // If a force isn't being applied then it can be ignored.
                    if (fieldGravity != Vector3.zero) {
                        fields.Add(GravityField.globalFields[i]);
                    }
                }

            return fields.ToArray();
        }
        public static GravityField[] GetActiveFields(Vector3 position)
        {
            // Creating the return values, with the capacity to store every field if needed to save re-allocation.
            List<GravityField> fields = new List<GravityField>(GravityField.globalFields.Count);
            // Used to track the highest priotity fields at the position.
            int currentPriority = int.MaxValue;

            for (int i = 0; i < GravityField.globalFields.Count; i++)
                if (GravityField.globalFields[i].Priority >= currentPriority &&
                    GravityField.globalFields[i].enabled) {

                    // Checking if the field is actually applying a force.
                    Vector3 fieldGravity = GravityField.globalFields[i].GetFieldGravity(position);
                    // Adding gravity field to the current return list.
                    fields.Add(GravityField.globalFields[i]);

                    // If the field has a higher priority, all previous fields should be ignored.
                    if (fieldGravity != Vector3.zero &&
                        GravityField.globalFields[i].Priority > currentPriority) {

                        currentPriority = GravityField.globalFields[i].Priority;

                        fields.Clear();
                        fields.Add(GravityField.globalFields[i]);
                    }
                }

            return fields.ToArray();
        }
        #endregion
    }
}
