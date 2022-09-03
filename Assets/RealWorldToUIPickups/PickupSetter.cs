using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PickupSetter : MonoBehaviour
{
    [SerializeField] Camera renderTexCamera;
    [SerializeField] GameObject pickupPrefab;
    public Transform instantiationPosition;
    private void Start()
    {
        GameObject tpm = Instantiate(pickupPrefab, instantiationPosition.position, Quaternion.identity, instantiationPosition);
        
    }
}
