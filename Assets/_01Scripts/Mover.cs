using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float leftAncor;
    [SerializeField] float rightAncor;
    [Header("1 = normalSpeed")]
    [SerializeField] float speedOfMovement;
    Vector3 left, right;
    Vector3 startPosition;
    [Header("0 = left, 1 = right")]
    [Range(0f, 1f)]
    [SerializeField] float startOffset;
    float movementCounter;
    bool changeDirection;
    float delayMovement = 1f;
    private void OnEnable()
    {
        startPosition = transform.localPosition;
        ConvertAncorTOVector();
        movementCounter = Utility.RemapValues(left.x, right.x, 0f, 1f, startPosition.x);
    }
    private void ConvertAncorTOVector()
    {
        left =  new Vector3(leftAncor, startPosition.y, 0f);
        right =  new Vector3(rightAncor, startPosition.y, 0f);
    }
    private void Update()
    {
        if (delayMovement > 0)
        {
            delayMovement -= Time.deltaTime;
            return;
        }
        Move();
    }
    private void Move()
    {
        transform.localPosition = Vector3.Lerp(left, right, movementCounter);
        if (movementCounter >= 1 && !changeDirection)
        {
            changeDirection = true;
        }
        if (movementCounter<=0 && changeDirection)
        {
            changeDirection = false;
        }
        if (changeDirection)
        {
            movementCounter -= Time.deltaTime * speedOfMovement;
        }
        else
        {
            movementCounter += Time.deltaTime * speedOfMovement;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            startPosition = transform.position; 
        }
        Gizmos.DrawSphere(startPosition + new Vector3(leftAncor, 0f, 0f), 0.1f);
        Gizmos.DrawSphere(startPosition + new Vector3(rightAncor, 0f, 0f), 0.1f);

    }
}
