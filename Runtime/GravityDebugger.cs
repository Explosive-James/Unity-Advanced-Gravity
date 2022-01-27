using AdvancedGravity.Internal;
using UnityEngine;

public class GravityDebugger : MonoBehaviour
{
#if UNITY_EDITOR
    #region Data
    public Vector3 scale = Vector3.one;

    public float displaySize = .5f;
    public int resolution = 5;

    private GravityField gravityField;
    #endregion

    #region Unity Functions
    private void OnDrawGizmos()
    {
        if (gravityField == null) {

            // Grabbing the gravity field component.
            gravityField = GetComponent<GravityField>();
        }
        else {

            Vector3 initialPosition = -gravityField.MaximumRange * scale;
            Vector3 offsetPosition = scale / (resolution - 1) * gravityField.MaximumRange * 2;

            for (int x = 0; x < resolution; x++)
                for (int y = 0; y < resolution; y++)
                    for (int z = 0; z < resolution; z++) {

                        Vector3 position = transform.TransformPoint(initialPosition + Vector3.Scale(offsetPosition, new Vector3(x, y, z)));
                        Vector3 velocity = gravityField.GetFieldGravity(position);

                        float length = velocity.magnitude / gravityField.Strength;

                        Gizmos.color = Color.Lerp(Color.red, Color.green, length);
                        Gizmos.DrawLine(position, position + (velocity.normalized * length * displaySize));
                    }
        }
    }
    #endregion
#endif
}
