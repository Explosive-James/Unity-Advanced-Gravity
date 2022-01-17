using UnityEngine;
using AdvancedGravity.Internal;

namespace AdvancedGravity
{
    public sealed class CapsuleGravity : GravityField
    {
        #region Data
        [SerializeField] private float _length = 5;
        #endregion

        #region Properties
        public float Length => 5;
        #endregion

        #region Unity Functions
        private void Start()
        {
            // Adding collider to invoke trigger events.
            CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();

            // Setting up capsule collider.
            collider.isTrigger = true;
            collider.direction = 2;

            // Adjusting collider size.
            collider.radius = _maximumRange;
            collider.height = _length * .5f;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Adjusting gizmos matrix.
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            // Adjusting gizmos colour.
            Color gizmosColour = GetGameObjectColour();
            Gizmos.color = gizmosColour;

            // Calculating the max axis to force circular shape.
            float maxAxis = Mathf.Max(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.y));
            // Finding the global length of the capsule.
            float length = Mathf.Max((_length * transform.lossyScale.z * 2) - (_maximumRange * 2 * maxAxis), 0);
            // Precalculating rotation value.
            Quaternion rotation = Quaternion.Euler(90, 0, 0);

            // Drawing range where gravity is at max.
            GravityGizmos.DrawWireCapsule(Vector3.zero, rotation, (_maximumRange - _maximumFade) * maxAxis, length);
            GravityGizmos.DrawWireCapsule(Vector3.zero, rotation, (_minimumRange + _minimumFade) * maxAxis, length);

            // Adjusting gizmos colour.
            gizmosColour.a = gizmosAlpha;
            Gizmos.color = gizmosColour;

            // Drawing range where gravity is at zero.
            GravityGizmos.DrawWireCapsule(Vector3.zero, rotation, _maximumRange * maxAxis, length);
            GravityGizmos.DrawWireCapsule(Vector3.zero, rotation, _minimumFade * maxAxis, length);
        }
#endif
        #endregion

        #region Public Functions
        public override Vector3 GetFieldGravity(Vector3 rigidbodyPosition)
        {
            // Converting position from global to local space, ignoring scale.
            Vector3 localPosition = Quaternion.Inverse(transform.rotation) * (rigidbodyPosition - transform.position);

            // Finding the maximum axis to force circular shape.
            float maxAxis = Mathf.Max(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.y));
            // Finding the world space length value.
            float length = Mathf.Max((_length * transform.lossyScale.z * 2) - (_maximumRange * 2 * maxAxis), 0);

            // Subtracting the length from the local position to get the capsule shape.
            localPosition.z -= Mathf.Min(Mathf.Abs(localPosition.z), length * .5f) * Mathf.Sign(localPosition.z);

            // Calculating the distance relative to the scale.
            float distance = localPosition.magnitude / maxAxis;

            return transform.rotation * localPosition.normalized * CalculateFadeMultiplier(distance) * -_strength;
        }
        #endregion
    }
}
