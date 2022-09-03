using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DataSystem;
public class GlobalEventHolder : MonoBehaviour
{

    public static GlobalEventHolder geh { get; set; }

    public GameEvent OnGameSuccess;
    public GameEvent OnGameFailed;
    public GameEvent OnGameCompleted;
    public GameEvent OnLevelSpawned;
    public GameEvent OnWrongStation;
    public BoolValue GameOver;
    public UnityAction OnDisableCamPlayerFollow;
    public UnityAction StopCamFollow;
    

    
    private void Awake()
    {   
        geh = this;
    }

  
}
