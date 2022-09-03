using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatableLocal
{
    StatesLocal RespondingState { get; }
    void Drive();
    void OnStateStarted();
    void OnStateCompleted();
}
