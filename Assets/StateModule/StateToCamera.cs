using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class StateToCamera : StateBase
{
    CinemachineVirtualCamera cmv;
    [SerializeField] int activePriority;
    private void OnEnable()
    {
        cmv = GetComponent<CinemachineVirtualCamera>();
        SubscribeToStates();
    }
    private void OnDisable()
    {
        UnSubscribeFromStates();
    }
    public override void Drive()
    {
        base.Drive();
    }
    public override void OnStateStarted()
    {
        base.OnStateStarted();
        cmv.Priority = activePriority;
    }
    public override void OnStateCompleted()
    {
        base.OnStateCompleted();
        cmv.Priority = 0;
    }

}
