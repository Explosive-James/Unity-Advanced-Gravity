using AdvancedGravity.Internal;
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
        public static Vector3 GetGlobalGravity(Rigidbody rigidbody) => GravityField.GetGlobalGravity(rigidbody);

        /// <summary>
        /// Gets the gravity at a given position.
        /// </summary>
        /// <param name="position">Position to check the gravity at.</param>
        public static Vector3 GetGlobalGravity(Vector3 position) => GravityField.GetGlobalGravity(position);
        #endregion
    }
}
