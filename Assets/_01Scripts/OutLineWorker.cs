using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineWorker : MonoBehaviour
{
    [SerializeField] private float outLineDuration = 1f;
    private float outLinecounter = 0f;
    [SerializeField] private Outline ol;
    private bool deminishOutline = false;
    
    public void OutlineMe()
    {
        outLinecounter = 0f;
        ServiceTicker.st.AddSubscriber(Worker);
    }

    void Worker()
    {
        if (!deminishOutline)
        {
            if (outLinecounter <= outLineDuration)
            {
                ol.OutlineWidth = Mathf.Lerp(0f, 10f, Utility.RemapValues(0f, outLineDuration, 0f, 1f, outLinecounter));
                outLinecounter += Time.deltaTime;
            }
            else
            {
                deminishOutline = true;
            }
        }
        else
        {
            if (outLinecounter >= 0f)
            {
                ol.OutlineWidth = Mathf.Lerp(10f, 0, Utility.RemapValues(outLineDuration, 0f, 0f, 1f, outLinecounter));
                outLinecounter -= Time.deltaTime;
            }
            else
            {
                deminishOutline = false;
                ServiceTicker.st.RemoveSubscriber(Worker);
                ol.OutlineWidth = 0f;
            }
        }
        
    }
}
