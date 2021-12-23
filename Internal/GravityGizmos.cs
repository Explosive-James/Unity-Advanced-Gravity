using UnityEngine;

namespace AdvancedGravity.Internal
{
    public static class GravityGizmos
    {
        #region Constants
        public const int RESOLUTION = 64;
        #endregion

        #region Public Functions
        public static void DrawWireArc(Vector3 position, Quaternion rotation, float radius, float degrees)
        {
#if UNITY_EDITOR
            // Calculating initial position.
            Vector3 previousPosition = position + rotation * new Vector3(0, 0, radius);

            // Calculating resolution for the arc.
            int resolution = (int)(RESOLUTION * (degrees / 360));
            float stepSize = degrees / resolution;

            for (int i = 0; i < resolution; i++) {

                Vector3 currentPosition = position + (rotation *
                    Quaternion.Euler(0, stepSize * (i + 1), 0) * new Vector3(0, 0, radius));

                Gizmos.DrawLine(previousPosition, currentPosition);
                previousPosition = currentPosition;
            }
#endif
        }

        public static void DrawWireCylinder(Vector3 position, Quaternion rotation, float radius, float length)
        {
#if UNITY_EDITOR
            float halfLength = length * .5f;
            Vector3 offset = rotation * new Vector3(0, halfLength, 0);

            // Drawing cylinder caps.
            DrawWireArc(position + offset, rotation, radius, 360);
            DrawWireArc(position - offset, rotation, radius, 360);

            // Drawing clinder lines.
            Gizmos.DrawLine(position + (rotation * new Vector3(radius, halfLength, 0)), position + (rotation * new Vector3(radius, -halfLength, 0)));
            Gizmos.DrawLine(position + (rotation * new Vector3(-radius, halfLength, 0)), position + (rotation * new Vector3(-radius, -halfLength, 0)));

            Gizmos.DrawLine(position + (rotation * new Vector3(0, halfLength, radius)), position + (rotation * new Vector3(0, -halfLength, radius)));
            Gizmos.DrawLine(position + (rotation * new Vector3(0, halfLength, -radius)), position + (rotation * new Vector3(0, -halfLength, -radius)));
#endif
        }
        public static void DrawWireCapsule(Vector3 position, Quaternion rotation, float radius, float length)
        {
#if UNITY_EDITOR
            float halfLength = length * .5f;
            Vector3 offset = rotation * new Vector3(0, halfLength, 0);

            // Drawing capsule half spheres.
            DrawWireArc(position + offset, rotation, radius, 360);
            DrawWireArc(position + offset, rotation * Quaternion.Euler(0, 90, 90), radius, 180);
            DrawWireArc(position + offset, rotation * Quaternion.Euler(0, 0, 90), radius, 180);

            DrawWireArc(position - offset, rotation, radius, 360);
            DrawWireArc(position - offset, rotation * Quaternion.Euler(0, 90, -90), radius, 180);
            DrawWireArc(position - offset, rotation * Quaternion.Euler(0, 0, -90), radius, 180);

            // Drawing capsule lines.
            Gizmos.DrawLine(position + (rotation * new Vector3(radius, halfLength, 0)), position + (rotation * new Vector3(radius, -halfLength, 0)));
            Gizmos.DrawLine(position + (rotation * new Vector3(-radius, halfLength, 0)), position + (rotation * new Vector3(-radius, -halfLength, 0)));

            Gizmos.DrawLine(position + (rotation * new Vector3(0, halfLength, radius)), position + (rotation * new Vector3(0, -halfLength, radius)));
            Gizmos.DrawLine(position + (rotation * new Vector3(0, halfLength, -radius)), position + (rotation * new Vector3(0, -halfLength, -radius)));
#endif
        }
        #endregion
    }
}
