using AdvancedGravity.Internal;
using UnityEngine;

namespace AdvancedGravity
{
    public sealed class PlanarGravity : GravityField
    {
        #region Data
        [SerializeField] private Vector2 _aspectRatio = Vector2.one;
        #endregion

        #region Properties
        public Vector2 AspectRatio => _aspectRatio;
        #endregion

        #region Unity Functions
        private void Awake()
        {
            // Adding a box collider to invoke trigger events.
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;

            // Calculating the y size of the box collider.
            float size = _maximumRange - _minimumRange;

            // Adjsting box collider settings.
            collider.center = new Vector3(0, (size / 2) + _minimumRange);
            collider.size = new Vector3(_aspectRatio.x * 2, size, _aspectRatio.y * 2);
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Updating Gizmos matrix to match the transforms.
            Gizmos.matrix = transform.localToWorldMatrix;

            // Adjusting gizmos colour.
            Color gizmosColour = GetGameObjectColour();
            Gizmos.color = gizmosColour;

            // Calculating the y size of the box.
            float maxRange = (_maximumRange - _maximumFade) - (_minimumRange + _minimumFade);
            // Drawing the range where gravity is at max.
            Gizmos.DrawWireCube(new Vector3(0, (maxRange / 2) + _minimumRange + _minimumFade, 0),
                new Vector3(_aspectRatio.x * 2, maxRange, _aspectRatio.y * 2));

            // Adjsting gizmos colour.
            gizmosColour.a = gizmosAlpha;
            Gizmos.color = gizmosColour;

            // Calculating the y size of the box.
            float fadeRange = _maximumRange - _minimumRange;
            Gizmos.DrawWireCube(new Vector3(0, (fadeRange / 2) + _minimumRange, 0),
                new Vector3(_aspectRatio.x * 2, fadeRange, _aspectRatio.y * 2));
        }
#endif
        #endregion

        #region Public Functions
        public override Vector3 GetFieldGravity(Vector3 rigidbodyPosition)
        {
            // Converting position from world to local space.
            Vector3 localPosition = transform.InverseTransformPoint(rigidbodyPosition);

            // Checking if the position is outside the counds of the plane.
            if (Mathf.Abs(localPosition.x) > _aspectRatio.x ||
                Mathf.Abs(localPosition.z) > _aspectRatio.y) {

                return Vector2.zero;
            }

            return transform.rotation * new Vector3(0, 0 - _strength * CalculateFadeMultiplier(localPosition.y), 0);
        }
        #endregion
    }
}
