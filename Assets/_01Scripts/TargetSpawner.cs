using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] GameObject Target;
    public float targetDistance;
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Instantiate(Target, new Vector3(0f, 0f, i * (targetDistance + Random.Range(0f, 11f))), Quaternion.identity);
        }
    }

   
}
