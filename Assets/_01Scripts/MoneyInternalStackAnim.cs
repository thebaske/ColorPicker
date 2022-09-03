using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoneyInternalStackAnim : MonoBehaviour
{
    private Vector3 localStartPosition;
    private Quaternion localStartRotation;
    [SerializeField] private Transform moneyMOdel;
    [SerializeField] private float animDuration;
    private float animDurationCoutner;
    private Vector3 refVelocity;
    private bool repeat;
    private void OnEnable()
    {
        localStartPosition = new Vector3(Random.Range(-2f, 2f), Random.Range(1f, 3f), 0f);
        localStartRotation = new Quaternion(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1),
            Random.Range(-1, 1));
        moneyMOdel.position = localStartPosition;
        moneyMOdel.rotation = localStartRotation;
        animDurationCoutner = 0f;
        ServiceTicker.st.AddSubscriber(AnimWorker);
    }

    public void TriggerAnimWorker()
    {
        moneyMOdel.position = localStartPosition;
        moneyMOdel.rotation = localStartRotation;
        animDurationCoutner = 0f;
        repeat = true;
        ServiceTicker.st.AddSubscriber(AnimWorker);
    }

    void StopPreviousAnim()
    {
        repeat = false;
    }
    
    void AnimWorker()
    {

        animDurationCoutner += Time.deltaTime;
        if (animDurationCoutner >= animDuration)
        {
            if (!repeat)
            {
                ServiceTicker.st.RemoveSubscriber(AnimWorker);
            }
            animDurationCoutner = animDuration;
        }

        if (moneyMOdel != null )
        {
            moneyMOdel.transform.localPosition =
                Vector3.Lerp(localStartPosition, Vector3.zero, Utility.RemapValues(0f, animDuration, 0f, 1f, animDurationCoutner));
            moneyMOdel.transform.localRotation =
                Quaternion.Lerp(localStartRotation, Quaternion.Euler(-90f, 0f, 0f), Utility.RemapValues(0f, animDuration, 0f, 1f, animDurationCoutner));
        }
        else
        {
            ServiceTicker.st.RemoveSubscriber(AnimWorker);
        }
        
    }

    private void OnDestroy()
    {
        ServiceTicker.st.RemoveSubscriber(AnimWorker);
    }
}
