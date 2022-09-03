using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatable
{   
    void Drive();
    void OnStateStarted();
    void OnStateCompleted();
}
