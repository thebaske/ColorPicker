using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;
    public Joystick joystick;

    public Vector3 right;
    public Vector3 forward;
    public Vector3 up;
    //public Rigidbody rb;
    public float rotationSpeed = 1f;

    

    void FixedUpdate()
    {

        right = Vector3.right * joystick.Horizontal;
        forward = Vector3.forward * joystick.Vertical;
        up = Vector3.zero;



        Vector3 moveVector = (right + forward + up);
        //rb = GetComponent<Rigidbody>();


        //playerAnim.speed = rb.velocity.magnitude * animationSpeedModifier;

        if (moveVector != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(right + forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            //rb.AddForce(moveVector * speed * Time.deltaTime, ForceMode.VelocityChange);
            //rb.velocity = moveVector * (player.IsGrounded ? speed : fallSpeed) * Time.deltaTime;
        }

        if (moveVector == Vector3.zero)
        {
            //rb.velocity = Vector3.zero;
            //rb.velocity.magnitude = Vector3.zero;
            //playerAnim.Play("walkRun", 0, 0);
        }

    }
}