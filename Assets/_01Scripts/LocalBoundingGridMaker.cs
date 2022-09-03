using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LocalBoundingGridMaker : MonoBehaviour
{
    [SerializeField] private float gridSpacing;
    [SerializeField] private float gridPadding;
    [SerializeField] Vector3[] localGrid;
    Vector3 localXPosLeftFront ;
    Vector3 localxPosRightFront;
    Vector3 localXPosLeftBack ;
    Vector3 localxPosRightBack ;
    public Vector3[] GetMyGrid()
    {
        
        return localGrid;
    }
    [Button("Generate local bounds grid")]
    void GenerateGrid()
    {
        Bounds bnds = Utility.GetMaxBoundsOmniKnowing(gameObject);
        localXPosLeftFront = bnds.center + new Vector3(-bnds.extents.x, transform.position.y, bnds.extents.z);
        localxPosRightFront = bnds.center + new Vector3(bnds.extents.x, transform.position.y, bnds.extents.z);
        localXPosLeftBack = bnds.center + new Vector3(-bnds.extents.x,  transform.position.y, -bnds.extents.z);
        localxPosRightBack = bnds.center + new Vector3(bnds.extents.x,  transform.position.y, -bnds.extents.z);
        int gridSizeX = (int)((bnds.extents.x * 2f) / gridSpacing);
        int gridSizeZ = (int) ((bnds.extents.z * 2f) / gridSpacing);
        localGrid = new Vector3[gridSizeX * gridSizeZ];
        
        int gridCounter = 0;
        
        for (int i = 0; i < gridSizeZ; i++)
        {
            for (int j = 0; j < gridSizeX; j++)
            {
                float x = Vector3.Lerp(localXPosLeftFront, localxPosRightFront, Utility.RemapValues(0, gridSizeX - 1, 0f + gridPadding, 1 - gridPadding, j)).x;
                float y = transform.position.y;
                float z = Vector3.Lerp(localXPosLeftFront, localXPosLeftBack, Utility.RemapValues(0, gridSizeZ - 1, 0f + gridPadding, 1 - gridPadding, i)).z;
                localGrid[gridCounter] = transform.InverseTransformPoint(new Vector3(x, y, z));
                gridCounter++;
                
            }
        }
        
    }
    // private void OnDrawGizmos()
    // {
    //     Gizmos.matrix = transform.localToWorldMatrix;
    //     if (localGrid != null)
    //     {
    //         Gizmos.color = Color.red;
    //         for (int i = 0; i < localGrid.Length; i++)
    //         {
    //             Gizmos.DrawSphere(localGrid[i], 0.2f);
    //             
    //         }
    //     }
    //     Gizmos.color =Color.blue;
    //     Gizmos.DrawCube(localXPosLeftBack, new Vector3(0.2f, 10f, 0.2f));
    //     Gizmos.DrawCube(localxPosRightBack, new Vector3(0.2f, 10f, 0.2f));
    //     Gizmos.DrawCube(localxPosRightFront, new Vector3(0.2f, 10f, 0.2f));
    //     Gizmos.DrawCube(localXPosLeftFront, new Vector3(0.2f, 10f, 0.2f));
    //     
    // }
}
