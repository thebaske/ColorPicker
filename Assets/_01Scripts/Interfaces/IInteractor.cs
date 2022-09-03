using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractor
{
    bool Used { get;}
    int myID { get;}

    void Interact(IInterActable interActable);
}
