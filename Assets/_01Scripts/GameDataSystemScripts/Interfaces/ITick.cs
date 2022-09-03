using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ITick 
{
    public UnityAction OnUpdateEvent { get; set; }
    public UnityAction OnLateUpdateEvent { get; set; }
    public UnityAction OnFixedUpdateEvent { get; set; }
}
