using System;
using System.Collections;
using DataSystem;
using UnityEngine;

public class ProgressionTracker : MonoBehaviour
{
    // [SerializeField] private TrackHolder tHolder;
    
    [SerializeField] private FloatValue Progression;
    [SerializeField] private IntValue cargoMoved;
    
    private int goalResult;
    private bool readyToTrack = false;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        goalResult = 50;
        
        readyToTrack = true;
        cargoMoved.MyValueChanged += RefreshProgression;
        RefreshProgression(cargoMoved.MyValue);
        // Injector<LevelInfo, ProgressionTracker> tmp = new Injector<LevelInfo, ProgressionTracker>();
        // tmp.Injectme(LevelInfo.li, this, this);
    }

    void RefreshProgression(int value)
    {
        Progression.MyValue = Utility.RemapValues(0, goalResult, 0f, 1f, cargoMoved.MyValue);
    }
    // private void Update()
    // {
    //     TrackProgression();
    // }

    private void OnDisable()
    {
        cargoMoved.MyValue = 0;
    }

    void TrackProgression()
    {
        if (readyToTrack)
        {
            Progression.MyValue = Utility.RemapValues(0, goalResult, 0f, 1f, cargoMoved.MyValue);
        }
    }
    // public void Inject(LevelInfo reference)
    // {
    //     li = reference;
    // }
}
