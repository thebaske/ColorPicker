using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkConnection : MonoBehaviour
{
    [SerializeField] Transform connection;
    
    public bool special;
    public Transform Connection => connection;

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     if (connection != null)
    //     {
    //         Gizmos.DrawCube(connection.position, new Vector3(0.1f, 10.5f, 1.5f)); 
    //     }
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawCube(transform.position, new Vector3(0.1f, 15f, 0.1f));
    // }
}
