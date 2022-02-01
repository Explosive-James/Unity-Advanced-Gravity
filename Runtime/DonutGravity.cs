using AdvancedGravity.Internal;
using UnityEngine;

namespace AdvancedGravity
{
    public sealed class DonutGravity : GravityField
    {
        #region Data
        [SerializeField] private float _radius = 5;
        #endregion

        #region Properties
        public float Radius => _radius;
        #endregion

        #region Unity Functions
        private void Start()
        {
            // Adding a box collider to invoke trigger events.
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;

            // Adjusting box collider size.
            float size = (_radius + _maximumRange) * 2;
            collider.size = new Vector3(size, _maximumRange, size);
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Adjusting gizmos matrix.
            Gizmos.matrix = transform.localToWorldMatrix;

            // Updating gizmos colour.
            Color gizmosColour = GetGameObjectColour();
            Gizmos.color = gizmosColour;

            // Drawing range where gravity is at max.
            GravityGizmos.DrawWireDonut(Vector3.zero, Quaternion.identity, _radius, _maximumRange - _maximumFade);
            GravityGizmos.DrawWireDonut(Vector3.zero, Quaternion.identity, _radius, _minimumRange + _minimumFade);

            // Adjusting gizmos colour.
            gizmosColour.a = gizmosAlpha;
            Gizmos.color = gizmosColour;

            // Drawing range where gravity is at zero.
            GravityGizmos.DrawWireDonut(Vector3.zero, Quaternion.identity, _radius, _maximumRange);
            GravityGizmos.DrawWireDonut(Vector3.zero, Quaternion.identity, _radius, _minimumRange);
        }
#endif
        #endregion

        #region Public Functions
        public override Vector3 GetFieldGravity(Vector3 rigidbodyPosition)
        {
            Vector3 localPosition = transform.InverseTransformPoint(rigidbodyPosition);
            Vector3 targetPosition = new Vector3(localPosition.x, 0, localPosition.z).normalized * _radius;

            Vector3 direction = localPosition - targetPosition;
            float distance = direction.magnitude;

            return _strength * CalculateFadeMultiplier(distance) * -direction;
        }
        #endregion
    }
}
