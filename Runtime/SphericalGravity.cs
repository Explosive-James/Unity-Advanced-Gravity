using AdvancedGravity.Internal;
using UnityEngine;

namespace AdvancedGravity
{
    public sealed class SphericalGravity : GravityField
    {
        #region Unity Functions
        private void Start()
        {
            // Adding a sphere collider to invoke trigger events.
            SphereCollider collider = gameObject.AddComponent<SphereCollider>();

            // Adjusting collider settings.
            collider.isTrigger = true;
            collider.radius = _maximumRange;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Adjusting gizmos matrix.
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            // Adjusting gizmos colour.
            Color gizmosColour = GetGameObjectColour();
            Gizmos.color = gizmosColour;

            float maxAxis = Mathf.Max(Mathf.Abs(transform.lossyScale.x),
                Mathf.Abs(transform.lossyScale.y), Mathf.Abs(transform.lossyScale.z));

            // Drawing range where gravity is at max.
            Gizmos.DrawWireSphere(Vector3.zero, (_maximumRange - _maximumFade) * maxAxis);
            Gizmos.DrawWireSphere(Vector3.zero, (_minimumRange + _minimumFade) * maxAxis);

            // Adjusting gizmos colour.
            gizmosColour.a = gizmosAlpha;
            Gizmos.color = gizmosColour;

            // Drawing range where gravity is at zero.
            Gizmos.DrawWireSphere(Vector3.zero, _maximumRange * maxAxis);
            Gizmos.DrawWireSphere(Vector3.zero, _minimumRange * maxAxis);
        }
#endif
        #endregion

        #region Public Functions
        public override Vector3 GetFieldGravity(Vector3 rigidbodyPosition)
        {
            // Calculating the local position to the gravity field.
            Vector3 localPosition = rigidbodyPosition - transform.position;
            // Caching lossy scale to prevent any overhead.
            Vector3 lossyScale = transform.lossyScale;

            // The maximum scale of the object to find the local distance to the gravity field.
            float maxAxis = Mathf.Max(Mathf.Abs(lossyScale.x), 
                Mathf.Abs(lossyScale.y), Mathf.Abs(lossyScale.z));
            // How far the position is from the center.
            float localDistance = localPosition.magnitude / maxAxis;

            return -_strength * CalculateFadeMultiplier(localDistance) * localPosition.normalized;
        }
        #endregion
    }
}
