using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSensor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CallHaptic._aInstance.HapticLight();
        Destroy(other.transform.gameObject);
    }
}
