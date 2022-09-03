using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformController : MonoBehaviour
{
    [SerializeField] Transform modelHolder;
    bool ready;
    public float maxCircularOffset = 15f;
    public float touchSensitivity = 10f;
    [Header("InVector3")]
    public float screenDistance = 250f;
    public float deltaTouchX;
    public float deltaTouchY;
    float forwardCounter;
    public float forwardSpeed;
    public float movementLerpSpeed = 4f;
    Vector2 startingVector;
    Vector2 direction;
   
    public void OnTouchDown(bool value, Vector2 pos)
    {
        if (value)
        {
            startingVector = pos; 
        }
    }
    public void OnTouchMove(Vector2 position)
    {
        direction = position - startingVector;
        direction = Vector3.ClampMagnitude(direction, maxCircularOffset);
        deltaTouchX += position.x * Time.deltaTime * touchSensitivity;
        deltaTouchY += position.y * Time.deltaTime * touchSensitivity;
        if (deltaTouchX > maxCircularOffset)
        {
            deltaTouchX = maxCircularOffset;
        }
        if (deltaTouchX < -maxCircularOffset)
        {
            deltaTouchX = -maxCircularOffset;
        }
        if (deltaTouchY > maxCircularOffset)
        {             
            deltaTouchY = maxCircularOffset;
        }             
        if (deltaTouchY < -maxCircularOffset)
        {             
            deltaTouchY = -maxCircularOffset;
        }
    }
    private void FixedUpdate()
    {
        modelHolder.localPosition = Vector3.Lerp(modelHolder.localPosition, new Vector3(deltaTouchX, deltaTouchY, 0f), Time.deltaTime * movementLerpSpeed);
        transform.Translate(new Vector3(0f, 0f, forwardSpeed * Time.deltaTime));
        
    }
}
