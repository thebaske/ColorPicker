using UnityEngine;
using UnityEngine.Events;


public class StateCommand
{
    public string name;
    public UnityAction OnStateDrive;
    public UnityAction OnStateCompleted;
    public UnityAction OnStateStarted;
}
