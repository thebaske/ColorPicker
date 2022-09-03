using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlipManager_Chained : MonoBehaviour
{
    [SerializeField]MeshRenderer meshRenderer;
    SkinnedMeshRenderer skinnedMeshRenderer;
    public Material[] blipToMat;
    int blipMatCounter;
   
    [Header("Duration")]
    [Header("========================")]
    public float matTransitionDuration;
    [Header("Last mat delay timer")]
    public float lastMatDelay;
    bool useMeshRenderer = false;
    bool useSkinnedMeshRenderer = false;
    bool blipOn;
    private void Start()
    {
        if (meshRenderer != null)
        {

        }
        else
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
       
    }
    public void ManualBlipOn()
    {
        if (!blipOn)
        {
            blipOn = true;
            StopAllCoroutines();
            StartCoroutine(ColorBlipOnRunner());
        }
    }
    public void ManualBlipOff()
    {
        if (blipOn)
        {
            blipOn = false;
            StopAllCoroutines();
            StartCoroutine(ColorBlipOFFRunner());
        }
    }
    [ContextMenu("Blip Run")]
    public void ColorBlipRunner()
    {
        StartCoroutine(ColorBlipOnceRunner());
    }
   
    IEnumerator ColorBlipOnceRunner()
    {
        Material startMat;
        float counter = 0;
        startMat = new Material(meshRenderer.material);
        while(blipMatCounter < blipToMat.Length )
        {
            while (counter <= matTransitionDuration)
            {
                counter += Time.deltaTime;
                if (counter >= 0)
                {
                    meshRenderer.material.Lerp(startMat, blipToMat[blipMatCounter], Utility.RemapValues(0f, matTransitionDuration, 0f, 1f, counter)); 
                }

                yield return null;
            }
            blipMatCounter++;
            counter = 0f;
            if (blipMatCounter == blipToMat.Length - 1)
            {
                counter -= lastMatDelay;
            }
            yield return null;
        }
    }
    IEnumerator ColorBlipOnRunner()
    {
        Material startMat;
        float counter = 0;
        blipMatCounter = 1;
        startMat = new Material(meshRenderer.material);
            while (counter <= matTransitionDuration)
            {
                counter += Time.deltaTime;
                if (counter >= 0)
                {
                    meshRenderer.material.Lerp(startMat, blipToMat[blipMatCounter], Utility.RemapValues(0f, matTransitionDuration, 0f, 1f, counter));
                }

                yield return null;
            }
        //while (blipMatCounter < blipToMat.Length)
        //{
        //    yield return null;
        //}
    }
    IEnumerator ColorBlipOFFRunner()
    {
        Material startMat;
        float counter = 0;
        blipMatCounter = 0;
        startMat = new Material(meshRenderer.material);
            while (counter <= matTransitionDuration)
            {
                counter += Time.deltaTime;
                if (counter >= 0)
                {
                    meshRenderer.material.Lerp(meshRenderer.material, blipToMat[blipMatCounter], Utility.RemapValues(0f, matTransitionDuration, 0f, 1f, counter));
                }

                yield return null;
            }
        //while (blipMatCounter < blipToMat.Length)
        //{
        //    yield return null;
        //}
    }
}
