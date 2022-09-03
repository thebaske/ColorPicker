using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase : MonoBehaviour, IStatable
{
    public State_Scriptable[] MyStates;
    private void OnEnable()
    {
        SubscribeToStates();
    }
    private void OnDisable()
    {
        UnSubscribeFromStates();
    }
    public virtual void Drive()
    {

    }
    public virtual void OnStateStarted()
    {

    }
    public virtual void OnStateCompleted()
    {

    }

    public void SubscribeToStates()
    {
        for (int i = 0; i < MyStates.Length; i++)
        {   
            MyStates[i].stateCommand.OnStateStarted += OnStateStarted;
            MyStates[i].stateCommand.OnStateCompleted += OnStateCompleted;
            MyStates[i].stateCommand.OnStateDrive += Drive;
        }
    }
    public void UnSubscribeFromStates()
    {
        for (int i = 0; i < MyStates.Length; i++)
        {
            MyStates[i].stateCommand.OnStateStarted -= OnStateStarted;
            MyStates[i].stateCommand.OnStateCompleted -= OnStateCompleted;
            MyStates[i].stateCommand.OnStateDrive -= Drive;
        }
    }
}
