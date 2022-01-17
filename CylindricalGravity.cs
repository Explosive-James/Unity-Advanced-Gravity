using AdvancedGravity.Internal;
using UnityEngine;

namespace AdvancedGravity
{
    public sealed class CylindricalGravity : GravityField
    {
        #region Data
        [SerializeField] private float _length = 5;
        #endregion

        #region Properties
        public float Length => _length;
        #endregion

        #region Unity Functions
        private void Start()
        {
            // Adding a box collider to invoke trigger events.
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();

            // Updating collider settings.
            collider.isTrigger = true;
            collider.size = new Vector3(_maximumRange, _maximumRange, _length) * 2;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Updating gizmos matrix to match the transforms.
            Gizmos.matrix = transform.localToWorldMatrix;

            // Adjusting gizmos colour.
            Color gizmosColour = GetGameObjectColour();
            Gizmos.color = gizmosColour;

            // Precalculating rotation value.
            Quaternion rotation = Quaternion.Euler(90, 0, 0);

            // Drawing range where gravity is at max.
            GravityGizmos.DrawWireCylinder(Vector3.zero, rotation, _maximumRange - _maximumFade, _length * 2);
            GravityGizmos.DrawWireCylinder(Vector3.zero, rotation, _minimumRange + _minimumRange, _length * 2);

            // Adjusting gizmos colour.
            gizmosColour.a = gizmosAlpha;
            Gizmos.color = gizmosColour;

            // Drawing range where gravity is at zero.
            GravityGizmos.DrawWireCylinder(Vector3.zero, rotation, _maximumRange, _length * 2);
            GravityGizmos.DrawWireCylinder(Vector3.zero, rotation, _maximumRange, _length * 2);
        }
#endif
        #endregion

        #region Public Functions
        public override Vector3 GetFieldGravity(Vector3 rigidbodyPosition)
        {
            // Converting position from global to local space.
            Vector3 localPosition = transform.InverseTransformPoint(rigidbodyPosition);
            // Calculating the distance, ignoring the z-axis.
            float distance = new Vector2(localPosition.x, localPosition.y).magnitude;

            // Checking if the position is outside the cyliner along the length.
            if (Mathf.Abs(localPosition.z) > _length) {
                return Vector3.zero;
            }

            return transform.rotation * new Vector3(localPosition.x, localPosition.y, 0).normalized *
                CalculateFadeMultiplier(distance) * -_strength;
        }
        #endregion
    }
}
