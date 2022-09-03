using Sirenix.OdinInspector;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    
    [SerializeField] private Vector3 bounds;
    [SerializeField] private Vector3 center;
    [SerializeField] private Quaternion rotation;
    private Vector3 tmpPosition;
    public Vector3 GetPoint()
    {
        return center + new Vector3(Random.Range(-bounds.x, bounds.x)/2, Random.Range(-bounds.y, bounds.y)/2,
            Random.Range(-bounds.z, bounds.z)/2);
    }

    [Button("Test random pos")]
    void DrawRandomPos()
    {
        tmpPosition = GetPoint();
    }
    private void OnDrawGizmos()
    {
        Quaternion rot = rotation;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(center, rot, bounds);
        Gizmos.matrix = rotationMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, bounds);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(tmpPosition, 0.2f);
    }
}
