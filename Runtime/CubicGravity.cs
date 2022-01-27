using AdvancedGravity.Internal;
using UnityEngine;

namespace AdvancedGravity
{
    public sealed class CubicGravity : GravityField
    {
        #region Data
        [SerializeField] private Vector3 _aspectRatio = Vector3.one;
        #endregion

        #region Properties
        public Vector3 AspectRatio => _aspectRatio;
        #endregion

        #region Unity Functions
        private void Start()
        {
            // Adding collider to invoke trigger events.
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();

            // Adjusting collider settings.
            collider.isTrigger = true;
            collider.size = _aspectRatio * _maximumRange * 2;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Updating gizmos matrix to match the transforms.
            Gizmos.matrix = transform.localToWorldMatrix;

            // Adjusting gizmos colour.
            Color gizmosColour = GetGameObjectColour();
            Gizmos.color = gizmosColour;

            // Drawing range where gravity is at max.
            Gizmos.DrawWireCube(Vector3.zero, _aspectRatio * (_maximumRange - _maximumFade) * 2);
            Gizmos.DrawWireCube(Vector3.zero, _aspectRatio * (_minimumRange + _minimumFade) * 2);

            // Adjusting gizmos colour.
            gizmosColour.a = gizmosAlpha;
            Gizmos.color = gizmosColour;

            // Drawing range where gravity is at zero.
            Gizmos.DrawWireCube(Vector3.zero, _aspectRatio * _maximumRange * 2);
            Gizmos.DrawWireCube(Vector3.zero, _aspectRatio * _minimumRange * 2);

            // Calculating the minimum and maximum values.
            Vector3 minimum = Vector3.Scale(_aspectRatio, Vector3.one * _minimumRange);
            Vector3 maximum = Vector3.Scale(_aspectRatio, Vector3.one * _maximumRange);

            // Drawing upper cube lines.
            Gizmos.DrawLine(new Vector3(minimum.x, minimum.y, minimum.z), new Vector3(maximum.x, maximum.y, maximum.z));
            Gizmos.DrawLine(new Vector3(-minimum.x, minimum.y, minimum.z), new Vector3(-maximum.x, maximum.y, maximum.z));
            Gizmos.DrawLine(new Vector3(minimum.x, minimum.y, -minimum.z), new Vector3(maximum.x, maximum.y, -maximum.z));
            Gizmos.DrawLine(new Vector3(-minimum.x, minimum.y, -minimum.z), new Vector3(-maximum.x, maximum.y, -maximum.z));

            // Drawing lower cube lines.
            Gizmos.DrawLine(new Vector3(minimum.x, -minimum.y, minimum.z), new Vector3(maximum.x, -maximum.y, maximum.z));
            Gizmos.DrawLine(new Vector3(-minimum.x, -minimum.y, minimum.z), new Vector3(-maximum.x, -maximum.y, maximum.z));
            Gizmos.DrawLine(new Vector3(minimum.x, -minimum.y, -minimum.z), new Vector3(maximum.x, -maximum.y, -maximum.z));
            Gizmos.DrawLine(new Vector3(-minimum.x, -minimum.y, -minimum.z), new Vector3(-maximum.x, -maximum.y, -maximum.z));
        }
#endif
        #endregion

        #region Public Functions
        public override Vector3 GetFieldGravity(Vector3 rigidbodyPosition)
        {
            // Converting position from global to local space.
            Vector3 localPosition = transform.InverseTransformPoint(rigidbodyPosition);

            // Finding the axis with the furthest distance from the center.
            float distance = Mathf.Max(Mathf.Abs(localPosition.x),
                Mathf.Abs(localPosition.y), Mathf.Abs(localPosition.z));

            // Calculating the firection based on the furthest distance.
            Vector3 direction = new Vector3((int)(localPosition.x / distance),
                (int)(localPosition.y / distance), (int)(localPosition.z / distance));

            return transform.rotation * direction * CalculateFadeMultiplier(distance) * -_strength;
        }
        #endregion
    }
}
