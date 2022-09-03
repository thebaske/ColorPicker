using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSetter : MonoBehaviour
{
    [HideInInspector]public StateScheduler sttScheduler;
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        sttScheduler = FindObjectOfType<StateScheduler>();
    }
    public void SetRunning()
    {
        sttScheduler.OnRunningState();
    }
    public void SetFlying()
    {
        sttScheduler.OnFlyingState();
    }
    public void SetLanding()
    {
        sttScheduler.OnLandingState();
    }
}
