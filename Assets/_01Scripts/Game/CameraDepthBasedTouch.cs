using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DataSystem;

public class CameraDepthBasedTouch : MonoBehaviour
{
    Camera cam_main;
    [SerializeField] Vector3Value touchPosition;
    Ray camRay;
    PointerEventData ped;
    public LayerMask objectDetectionLayer;
    int selectedID;
    bool selected;
    public string hitObject;
    void Start()
    {
        cam_main = Camera.main;
    }

    void OnFingerMove(Vector2 position)
    {
        SetTouchPosition(position);
    }
    void OnFingerTouch(bool touched, Vector2 position)
    {
        if (touched)
        {
            SetTouchPosition(position); 
        }
    }
    void SetTouchPosition(Vector2 position)
    {
        camRay = cam_main.ScreenPointToRay(position);
        Debug.DrawRay(camRay.origin, camRay.direction * 50f, Color.yellow, 5f);
        touchPosition.MyValue = camRay.origin + (camRay.direction * Mathf.Abs(cam_main.transform.position.z));
        if (Physics.Raycast(camRay.origin, camRay.direction, out RaycastHit hitInfo, 1000f, objectDetectionLayer))
        {
            hitObject = hitInfo.transform.name;
            ITappable tappable = hitInfo.transform.GetComponent<ITappable>();
            
            if (tappable != null)
            {
                if (!selected)
                {
                    selected = true;
                    selectedID = tappable.MyID;
                }
                if (tappable.MyID == selectedID)
                {
                    tappable.Tap();
                }
            }
        }
    }

   
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(touchPosition.MyValue, 1f);
    }
}
