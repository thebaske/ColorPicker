using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float leftAncor;
    [SerializeField] float rightAncor;
    [Header("1 = normalSpeed")]
    [SerializeField] float speedOfRotation;
    Vector3 left, right;
    Vector3 startPosition;
    [Header("0 = left, 1 = right")]
    [Range(0f, 1f)]
    [SerializeField] float startOffset;
    [SerializeField] bool continuous;
    [SerializeField] bool toFixedPosition;
    float movementCounter;
    bool changeDirection;

    private void OnEnable()
    {
        startPosition = transform.localPosition;
        
        movementCounter = 0;
    }
   
    private void Update()
    {
        
        Rotate();
    }
    private void Rotate()
    {
        if (toFixedPosition)
        {
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, leftAncor), Quaternion.Euler(0f, 0f, rightAncor), movementCounter);
            if (movementCounter >= 1 )
            {
                return;
            }
        }
        if (continuous)
        {
            transform.Rotate(Vector3.up, (speedOfRotation * 10 ) * Time.deltaTime);
            return;
        }

        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, leftAncor), Quaternion.Euler(0f, 0f, rightAncor), movementCounter);
        if (movementCounter >= 1 && !changeDirection)
        {
            changeDirection = true;
        }
        if (movementCounter <= 0 && changeDirection)
        {
            changeDirection = false;
        }
        if (changeDirection)
        {
            movementCounter -= Time.deltaTime * speedOfRotation;
        }
        else
        {
            movementCounter += Time.deltaTime * speedOfRotation;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            startPosition = transform.position;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, Quaternion.Euler(0f, 0f, leftAncor) * Vector3.right);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Quaternion.Euler(0f, 0f, rightAncor) * Vector3.forward);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, Quaternion.Euler(rightAncor, 0f, 0f) * Vector3.forward);

    }
}
