using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class StateScheduler : MonoBehaviour
{
    public State_Scriptable Flying;
    public State_Scriptable Running;
    public State_Scriptable Landing;
    public UnityAction<StateCommand> OnStateChanged;
    StateDriver stateDriver;
    private void Awake()
    {
        Flying.InitializeState();
        Running.InitializeState();
        Landing.InitializeState();
    }
    private void OnEnable()
    {
        
        stateDriver = FindObjectOfType<StateDriver>();
    }
    public void OnRunningState()
    {
        OnStateChanged?.Invoke(Running.stateCommand);
        stateDriver.SetActiveState(new State_Scriptable[] { Running });
    }
    public void OnFlyingState()
    {
        OnStateChanged?.Invoke(Flying.stateCommand);
        stateDriver.SetActiveState(new State_Scriptable[] { Flying });
    }
    public void OnLandingState()
    {
        OnStateChanged?.Invoke(Landing.stateCommand);
        stateDriver.SetActiveState(new State_Scriptable[] { Landing });

    }
}
