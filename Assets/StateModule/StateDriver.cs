using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDriver : MonoBehaviour
{
    [SerializeField]List<State_Scriptable> activeStates = new List<State_Scriptable> ();
    bool stateChange = false;
    float statechangeLimitter;
    public void SetActiveState(State_Scriptable[] state)
    {
        
        stateChange = true;
        CompleteStates();
        activeStates.Clear();
        for (int i = 0; i < state.Length; i++)
        {
            activeStates.Add(state[i]);
        }
        StartStates();
        stateChange = false;
    }
    private void FixedUpdate()
    {
        if (!stateChange)
        {
            DriveActiveStates(); 
        }
    }
    public void ClearStates()
    {
        activeStates.Clear();
    }
    void CompleteStates()
    {
        if (activeStates != null && activeStates.Count > 0)
        {
            while (activeStates.Count > 0)
            {
                activeStates[0].stateCommand.OnStateCompleted?.Invoke();
                activeStates.RemoveAt(0);
            }
        }
    }
    void StartStates()
    {
        if (activeStates != null && activeStates.Count > 0)
        {
            for (int i = 0; i < activeStates.Count; i++)
            {
                activeStates[i].stateCommand.OnStateStarted?.Invoke();
            }
        }
    }
    void DriveActiveStates()
    {
        if (activeStates != null && activeStates.Count > 0)
        {
            for (int i = 0; i < activeStates.Count; i++)
            {
                activeStates[i].stateCommand.OnStateDrive?.Invoke();
            }
        }
    }
}
