using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSetterLocal : MonoBehaviour
{
    
    StateDriverLocal stateDriver;
    
    void Start()
    {
        stateDriver = GetComponent<StateDriverLocal>();
    }
   
    public void SetState(StatesLocal state)
    {
        stateDriver.SetState(state);
    }
}
