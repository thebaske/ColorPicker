using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed;
    public float _movespeed;
    public float maxVertSpeed;

    public Joystick leftJoystick;
    public Joystick rightJoystick;

    public Vector3 rightL;
    public Vector3 forwardL;
    

    public Vector3 rightR;
    public Vector3 forwardR;
    


    public Rigidbody rb;
    public float rotationSpeedL = 1f;
    public float rotationSpeedR = 1f;

    public Animator playerAnim;
    //public float baseAnimSpeed;

    //public GameObject shoot;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {

        


        Vector3 xzVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 yVel = new Vector3(0, rb.velocity.y, 0);

        xzVel = Vector3.ClampMagnitude(xzVel, _movespeed);
        yVel = Vector3.ClampMagnitude(yVel, maxVertSpeed);

        rb.velocity = xzVel + yVel;


        rightL = Vector3.right * leftJoystick.Horizontal;
        forwardL = Vector3.forward * leftJoystick.Vertical;
        

        rightR = Vector3.right * rightJoystick.Horizontal;
        forwardR = Vector3.forward * rightJoystick.Vertical;
        

        Vector3 moveVectorL = (rightL + forwardL);
        Vector3 moveVectorR = (rightR + forwardR);

        //playerAnim.speed = baseAnimSpeed + rb.velocity.magnitude /10;


        

        if (moveVectorL != Vector3.zero)
        {
            
            rb.AddForce(moveVectorL * speed * Time.deltaTime, ForceMode.VelocityChange);
            
        }

        if (moveVectorL != Vector3.zero && moveVectorR == Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(rightL + forwardL);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeedL * Time.deltaTime);
            playerAnim.SetBool("run", true);
            playerAnim.SetBool("shoot", false);
            //shoot.SetActive(false);


        }

        if (moveVectorR != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(rightR + forwardR);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeedR * Time.deltaTime);
            playerAnim.SetBool("shoot", true);
            playerAnim.SetBool("run", false);
            //shoot.SetActive(true);



            //playerAnim.Play("shoot");



        }

        if (moveVectorL == Vector3.zero && moveVectorR == Vector3.zero)
        {
            //rb.velocity = Vector3.zero;
            //rb.velocity.magnitude = Vector3.zero;
            playerAnim.SetBool("run", false);
            playerAnim.SetBool("shoot", false);
            //shoot.SetActive(false);

            //playerAnim.Play("idle");
        }

        if (moveVectorL == Vector3.zero && moveVectorR != Vector3.zero)
        {
            //playerAnim.Play("run");
            playerAnim.SetBool("run", false);
            playerAnim.SetBool("shoot", true);
            //shoot.SetActive(true);


        }

    }
}
