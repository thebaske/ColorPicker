using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;

public class DragMove : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTapStarted;
    internal float xDelta;
    internal float zDelta;
    [SerializeField] internal float sensitivity = 2f;
    [SerializeField] internal float deltaLerpSpeed = 6f;
    internal bool fingerDown;
    [SerializeField] internal bool clampSettings;
    [SerializeField] internal float maxDragDistance = 2f;

    

    void OnDisable()
    {   
        LeanTouch.OnFingerDown   -= OnFingerDown;
        LeanTouch.OnFingerUpdate -= OnFingerMove;
        LeanTouch.OnFingerUp     -= OnFingerUp;
    }
    

    public void Initialize()
    {
        
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUpdate += OnFingerMove;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    public void DeInitialize()
    {
        
        LeanTouch.OnFingerDown   -= OnFingerDown;
        LeanTouch.OnFingerUpdate -= OnFingerMove;
        LeanTouch.OnFingerUp     -= OnFingerUp;
    }

    void OnFingerDown(LeanFinger finger)
    {
        xDelta = 0f;
        zDelta = 0f;
        fingerDown = true;
        OnTapStarted?.Invoke();
    }
    public virtual void OnFingerMove(LeanFinger finger)
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
    public virtual void OnFingerUp(LeanFinger finger)
    {
        fingerDown = false;
        transform.localPosition = new Vector3(0f, 0f, 5f);
    }
}
