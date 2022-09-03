using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterActable
{
    int InterActableID { get; set; }
    GameObject GetGameObject();
    void StartedLife();
    void EndedLife();
}
