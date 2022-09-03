using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnTriggerReported : UnityEvent<Collider> { }
public class TriggerReporter : MonoBehaviour
{
    public OnTriggerReported OnEnterReported;
    public OnTriggerReported OnExitReported;
    private void OnTriggerEnter(Collider other)
    {
        OnEnterReported?.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        OnExitReported?.Invoke(other);
    }
}
