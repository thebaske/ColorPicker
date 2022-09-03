using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "State", menuName ="States/State")]
public class State_Scriptable : ScriptableObject
{
    //public UnityAction OnStateDrive;
    //public UnityAction OnStateCompleted;
    //public UnityAction OnStateStarted;
    public StateCommand stateCommand;
    [Button("Initialize")]
    public void InitializeState()
    {
        stateCommand = new StateCommand();
        stateCommand.name = name;
    }

}
