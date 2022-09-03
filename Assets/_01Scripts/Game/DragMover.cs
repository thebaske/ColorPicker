using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;


public class DragMover : MonoBehaviour
{
    private float xDelta;
    private float zDelta;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float deltaLerpSpeed = 6f;
    
    private bool fingerDown;
    [SerializeField] private bool clampSettings;
    [SerializeField] private float maxDragDistance = 2f;
    private void Start()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUpdate += OnFingerMove;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    void OnFingerDown(LeanFinger finger)
    {
        xDelta = 0f;
        zDelta = 0f;
        fingerDown = true;
    }
    void OnFingerMove(LeanFinger finger)
    {
        if (!fingerDown) return;
        
        xDelta = Mathf.Lerp(xDelta,finger.ScaledDelta.x * Time.deltaTime * sensitivity, Time.deltaTime * deltaLerpSpeed);
        zDelta = Mathf.Lerp(zDelta,finger.ScaledDelta.y * Time.deltaTime * sensitivity, Time.deltaTime * deltaLerpSpeed);
        transform.localPosition += new Vector3(xDelta, 0f, zDelta);
        if (clampSettings)
        {
            transform.localPosition =
                Vector3.ClampMagnitude(transform.position - transform.parent.position, maxDragDistance);
        }
    }
    void OnFingerUp(LeanFinger finger)
    {
        xDelta = 0f;
        zDelta = 0f;
        fingerDown = false;
    }
}