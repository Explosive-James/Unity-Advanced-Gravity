using System.Collections.Generic;
using UnityEngine;

namespace AdvancedGravity.Internal
{
    [DisallowMultipleComponent]
    public abstract class GravityField : MonoBehaviour
    {
        #region Static Data
        /// <summary>
        /// Global list of gravity fields used to calculate global gravity.
        /// </summary>
        internal static List<GravityField> globalFields = new List<GravityField>();
        /// <summary>
        /// Global list that dictates if a gravity field can influence a rigidbody.
        /// </summary>
        internal static Dictionary<Rigidbody, int> globalPriorities = new Dictionary<Rigidbody, int>();
        #endregion

        #region Data
        [SerializeField] protected int _priority;
        [SerializeField] protected float _strength = 9.81f;

        protected const float gizmosAlpha = 0.5f;
#if UNITY_EDITOR
#pragma warning disable 0414
        [SerializeField, HideInInspector] private int targetStrength = -1;
#pragma warning restore 0414
#endif

        [SerializeField] protected float _maximumRange = 5, _maximumFade = 1;
        [SerializeField] protected float _minimumRange = 1, _minimumFade = 1;
        #endregion

        #region Properties
        /// <summary>
        /// If a rigidbody enters a gravity field it will be given the priority of that field,
        /// any gravity field that has less than the rigidbody's priority will not be allowed to
        /// influence the rigidbody's velocity.
        /// </summary>
        public int Priority => _priority;
        /// <summary>
        /// The strength of the gravity, acceleration in meters per second.
        /// </summary>
        public float Strength => _strength;

        /// <summary>
        /// Controls the size of the gravity field's influence.
        /// </summary>
        public float MaximumRange => _maximumRange;
        /// <summary>
        /// The transition distance from maximum gravity to zero gravity.
        /// </summary>
        public float MaximumFade => _maximumFade;

        /// <summary>
        /// Controls the size of the gravity field's influence.
        /// </summary>
        public float MinimumRange => _minimumRange;
        /// <summary>
        /// The transition distance from maximum gravity to zero gravity.
        /// </summary>
        public float MinimumFade => _minimumFade;
        #endregion

        #region Unity Functions
        /// <summary>
        /// Registering the gravity field into a global list to avoid FindObjectsOfType calls.
        /// </summary>
        private void OnEnable() => globalFields.Add(this);
        /// <summary>
        /// Removing gravity field from the global list as it is unactive and shouldn't be included in global gravity calculations.
        /// </summary>
        private void OnDisable() => globalFields.Remove(this);

        private void OnTriggerStay(Collider other)
        {
            // Caching rigidbody reference.
            Rigidbody targetRb = other.attachedRigidbody;

            // Adding a rigidbody to the priorities.
            if (targetRb != null && !globalPriorities.ContainsKey(targetRb)) {
                globalPriorities.Add(targetRb, _priority);
            }
            // If the rigidbody is null or not using gravity we cannot do anything.
            if (targetRb == null || !targetRb.useGravity) {
                return;
            }

            // Caching rigidbody priority.
            int targetPriority = globalPriorities[targetRb];

            // Checking if gravity should be calculated.
            if (_priority >= targetPriority) {

                // Calculating and applying gravity to the rigidbody.
                Vector3 localGravity = GetFieldGravity(targetRb.position);
                targetRb.velocity += localGravity * Time.fixedDeltaTime;

                // Checking if the priority needs to be increased
                if (_priority > targetPriority && localGravity != Vector3.zero) {
                    globalPriorities[targetRb] = _priority;
                }
                // Checking if the priority needs to be recalculated.
                else if (_priority == targetPriority && localGravity == Vector3.zero) {
                    globalPriorities[targetRb] = GetRigidbodyPriority(targetRb);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            // Caching rigidbody reference.
            Rigidbody targetRb = other.attachedRigidbody;

            // Checking if priority should be recalculated.
            if (targetRb != null && globalPriorities[targetRb] == _priority) {

                // Recalculating rigidbody priority.
                int targetPriority = GetRigidbodyPriority(targetRb);
                globalPriorities[targetRb] = targetPriority;

                // If the priority is int.MinValue then there probably isn't gravity being applied.
                if (targetPriority == int.MinValue) globalPriorities.Remove(targetRb);
            }
        }
        #endregion

        #region Abstract Functions
        /// <summary>
        /// What is the gravity at a specified position from this gravity field.
        /// </summary>
        /// <param name="rigidbodyPosition">Position to calculate the gravity at.</param>
        /// <returns>Gravitational Force.</returns>
        public abstract Vector3 GetFieldGravity(Vector3 rigidbodyPosition);
        #endregion

        #region Protected Functions
        /// <summary>
        /// Calculates a multiplier that transitions from maximum gravity to zero gravity
        /// based on the maximum and minimum fade values.
        /// </summary>
        /// <param name="distance">Distance from the 'origin' of the gravity.</param>
        /// <returns>Gravity force multiplier. (0 to 1)</returns>
        protected float CalculateFadeMultiplier(float distance)
        {
            float maximumFactor = Mathf.Clamp01(-(distance - _maximumRange) / _maximumFade);
            float minimumFactor = Mathf.Clamp01((distance - _minimumRange) / _minimumFade);

            if (float.IsNaN(maximumFactor)) return minimumFactor;
            if (float.IsNaN(minimumFactor)) return maximumFactor;

            return maximumFactor * minimumFactor;
        }
        protected Color GetGameObjectColour()
        {
#if UNITY_EDITOR
            System.Random rng = new System.Random(gameObject.GetInstanceID());
            return Color.HSVToRGB((float)rng.NextDouble(), .7f, 1);
#else
            return Color.white;
#endif
        }
        #endregion

        #region Public Functions
        [System.Obsolete("please use Gravity.GetGlobalGravity instead.")]
        /// <summary>
        /// Gets the current gravity being applied to a rigidbody. This considers the rigidbody's 'useGravity' property.
        /// </summary>
        /// <param name="rigidbody">Rigidbody to find the gravity of.</param>
        public static Vector3 GetGlobalGravity(Rigidbody rigidbody) => Gravity.GetGlobalGravity(rigidbody);
        [System.Obsolete("please use Gravity.GetGlobalGravity instead.")]
        /// <summary>
        /// Gets the gravity at a given position.
        /// </summary>
        /// <param name="position">Position to check the gravity at.</param>
        public static Vector3 GetGlobalGravity(Vector3 position) => Gravity.GetGlobalGravity(position);
        #endregion

        #region Private Functions
        /// <summary>
        /// Finds the highest priority at the rigidbody's position.
        /// </summary>
        /// <param name="rigidbody">Rigidbody to find the priority of.</param>
        private static int GetRigidbodyPriority(Rigidbody rigidbody)
        {
            // Keeping track of the current priority.
            int currentPriority = int.MinValue;

            // Searching through gravity fields.
            for (int i = 0; i < globalFields.Count; i++)
                // Checking if they are a higher priority and applying a force.
                if (globalFields[i]._priority > currentPriority &&
                    globalFields[i].GetFieldGravity(rigidbody.position) != Vector3.zero) {

                    // Updating current priority.
                    currentPriority = globalFields[i]._priority;
                }

            return currentPriority;
        }
        #endregion
    }
}
