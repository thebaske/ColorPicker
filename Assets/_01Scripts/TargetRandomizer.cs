using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRandomizer : MonoBehaviour
{
    public Transform targetElement;

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        targetElement.transform.localPosition = new Vector3(0f, Random.Range(0f, -5f), 0f);
    }
}
