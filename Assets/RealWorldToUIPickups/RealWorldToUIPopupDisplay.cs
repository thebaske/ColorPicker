using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataSystem;
public class RealWorldToUIPopupDisplay : MonoBehaviour
{
    [SerializeField] RectTransform lerpPoint;
    [SerializeField] Vector3 lerpPointMaxPopScale;
    [SerializeField] float popDuration;
    [SerializeField] AnimationCurve popCurve;
    float popCoutner;
    [SerializeField] Transform popupHolder;
    int childCounter;
    int textPopupChildCounter;
    [SerializeField] Transform textParent;
    public static RealWorldToUIPopupDisplay rwPop { get; private set; }
    Camera cam_main;
    List<RectTransform> childrenRects = new List<RectTransform>();
    List<PopupLerper> lerpers;
    public float lerpDuration;
    [SerializeField] IntValue reachedMultiplier;
    [SerializeField] IntValue crystalCount;
    
    private void Awake()
    {
        rwPop = this;
        for (int i = 0; i < popupHolder.childCount; i++)
        {
            childrenRects.Add(popupHolder.GetChild(i).GetComponent<RectTransform>());
            popupHolder.GetChild(i).gameObject.SetActive(false);
        }
        lerpers = new List<PopupLerper>();

    }
    private void Start()
    {
        cam_main = Camera.main;
    }
    public void PopHere(Vector3 worldSpacePosition)
    {
        //crystalCount.MyValue++;
        childrenRects[childCounter].gameObject.SetActive(true);
        textParent.GetChild(textPopupChildCounter).gameObject.SetActive(true);
        Vector3 worldToCanvasPos = cam_main.WorldToScreenPoint(worldSpacePosition);
        childrenRects[childCounter].position = worldToCanvasPos;
        textParent.GetChild(textPopupChildCounter).position = worldToCanvasPos;
        
        textParent.GetChild(textPopupChildCounter).GetComponent<PopupValueDisplayHandler>().SetPopup(1); 
        PopupLerper pl = new PopupLerper();
        pl.rect = childrenRects[childCounter];
        pl.lerpCounter = lerpDuration;
        pl.startPos = childrenRects[childCounter].position;
        pl.lerperDelay = 0.9f;
        lerpers.Add(pl);

        ServiceTicker.st.AddSubscriber(LerpWorker);
        textPopupChildCounter++;
        if (textPopupChildCounter > textParent.childCount - 1)
        {
            textPopupChildCounter = 0;
        }
        childCounter++;
        if (childCounter > transform.childCount - 1)
        {
            childCounter = 0;
        }
    }
    void LerpWorker()
    {
        if (lerpers.Count > 0)
        {
            for (int i = 0; i < lerpers.Count; i++)
            {
                if (lerpers[i].lerperDelay <= 0f)
                {
                    Vector3 lerperPos = lerpPoint.position.WithAddX(-40).WithAddY(-80);
                    lerpers[i].rect.position = Vector3.Lerp(lerpers[i].startPos, lerperPos, Utility.RemapValues(lerpDuration, 0f, 0f, 1f, lerpers[i].lerpCounter));
                    lerpers[i].lerpCounter -= Time.deltaTime;
                    float squareMagnitude = (lerpers[i].rect.position - lerperPos).sqrMagnitude;
                    if (squareMagnitude < 0.2f)
                    {
                        lerpers[i].rect.gameObject.SetActive(false);
                        lerpers.RemoveAt(i);
                        ServiceTicker.st.AddSubscriber(LerperPopScale);
                    }
                }
                else
                {
                    lerpers[i].lerperDelay -= Time.deltaTime;
                }
            }
        }
        else
        {
            ServiceTicker.st.RemoveSubscriber(LerpWorker);
        }
    }
    void LerperPopScale()
    {
        popCoutner += Time.deltaTime;
        if (popCoutner >= popDuration)
        {
            popCoutner = popDuration;
            ServiceTicker.st.RemoveSubscriber(LerperPopScale);
        }
        lerpPoint.transform.localScale = Vector3.LerpUnclamped(Vector3.one, lerpPointMaxPopScale, popCurve.Evaluate(Utility.RemapValues(0f, popDuration, 0f, 1f, popCoutner)));
    }
}
[System.Serializable]
public class PopupLerper
{
    public float lerperDelay = 0.69f;
    public Vector3 startPos;
    public RectTransform rect;
    public float lerpCounter;
}
