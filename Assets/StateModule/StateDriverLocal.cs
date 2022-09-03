using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum StatesLocal { Running, Flying, Landing}
public class StateDriverLocal : MonoBehaviour
{
    public StateLocal Fly;
    public StateLocal Run;
    public StateLocal Land;
    public UnityAction<StateCommand> OnStateChanged;
    StateLocal activeState;

    private void OnEnable()
    {
        Fly = new StateLocal();
        Fly.Initialize("Flying");
        Run = new StateLocal();
        Run.Initialize("Running");
        Land = new StateLocal();
        Land.Initialize("Landing");
        SubscribeStatables();
    }
    void SubscribeStatables()
    {
        IStatableLocal[] allStatables = GetComponents<IStatableLocal>();
        for (int i = 0; i < allStatables.Length; i++)
        {
            StateLocal state = GetMyState(allStatables[i].RespondingState);
            state.stateCommand.OnStateStarted += allStatables[i].OnStateStarted;
            state.stateCommand.OnStateCompleted += allStatables[i].OnStateCompleted;
            state.stateCommand.OnStateDrive += allStatables[i].Drive;
        }
    }
    public StateLocal GetActiveState()
    {
        return activeState;
    }
    public void OnGameStarted() 
    {
        activeState = Run;
        activeState.stateCommand.OnStateStarted?.Invoke();
    }
    private void Update()
    {
        if (activeState != null)
        {
            activeState.stateCommand.OnStateDrive?.Invoke(); 
        }
    }
    public StateLocal GetMyState(StatesLocal requestedState)
    {
        StateLocal result = new StateLocal();
        switch (requestedState)
        {
            case StatesLocal.Running:
                result = Run;
                break;
            case StatesLocal.Flying:
                result = Fly;
                break;
            case StatesLocal.Landing:
                result = Land;
                break;
            default:
                result = Run;
                break;
        }
        return result;
    }
    public void SetState(StatesLocal state)
    {
        activeState.stateCommand.OnStateCompleted?.Invoke();
        activeState = null;
        switch (state)
        {
            case StatesLocal.Running:
                activeState = Run;
                OnStateChanged?.Invoke(activeState.stateCommand);
                activeState.stateCommand.OnStateStarted?.Invoke();
                break;
            case StatesLocal.Flying:
                activeState = Fly;
                OnStateChanged?.Invoke(activeState.stateCommand);
                activeState.stateCommand.OnStateStarted?.Invoke();
                break;
            case StatesLocal.Landing:
                activeState = Land;
                OnStateChanged?.Invoke(activeState.stateCommand);
                activeState.stateCommand.OnStateStarted?.Invoke();
                break;
            default:
                break;
        }
    }
}
public class StateLocal
{
    public StateCommand stateCommand;
    public void Initialize(string stateName)
    {
        stateCommand = new StateCommand();
        stateCommand.name = stateName;
    }
}
