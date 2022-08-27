using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private CharacterController controller;
    public float speed = 12f;
    public float accel = 0.1f;
    public float deccel = 0.3f;
    public float dragMultiplier = 0.5f;
    private float drag;
    private float dt;
    private float x = 0;
    private float z = 0;

    public Vector3 velocity;
    public float gravity = -9.81f;
    private float ogGravity;
    public float jumpHeight = 3f;
    public bool isGrounded;
    public float jumpSlow = 0.1f;
    public float hangTime = 0.5f;
    private float hangTimeCounter;
    public Vector3 move = new Vector3(0f, 0f, 0f);


    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float jumpBufferLength = 0.1f;
    private float jumpBufferCounter;
    public float floatVelStart = 0.7f;
    public LayerMask groundMask;

    void Start()
    {
        ogGravity = gravity;
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //IS  GROUNDED
        if (isGrounded)
        {
            drag = 1f;
            hangTimeCounter = hangTime;
            velocity.y = -2f;
        }
        //IS NOT GROUNDED
        else if (!isGrounded)
        {
            drag = dragMultiplier;
            hangTimeCounter -= dt;
            velocity.y += gravity * dt;
        }

        // x accel
        float garH = Input.GetAxisRaw("Horizontal");
        float garV = Input.GetAxisRaw("Vertical");
        Vector3 gar = new Vector3(garH, 0f, garV);
        gar.Normalize();
        if (gar.x > 0.1f || gar.x < -0.1f)
        {
            x += accel * gar.x * dt;
        }
        else if (gar.x < 0.1f && gar.x > -0.1f && (x > 0.0001f || x < -0.0001f))
        {
            float tempx = Mathf.Sign(x);

            x -= deccel * dt * tempx * drag;

            if (tempx > 0.1f)
            {
                if (x <= 0f) { x = 0f; }

            }
            else if (tempx < -0.1f)
            {
                if (x >= 0f) { x = 0f; }

            }

        }
        x = Mathf.Clamp(x, -1f, 1f);

        // z accel

        if (gar.z > 0.1f || gar.z < -0.1f)
        {
            z += accel * gar.z * dt;
        }
        else if (gar.z < 0.1f && gar.z > -0.1f && (z > 0.0001f || z < -0.0001f))
        {
            float tempz = Mathf.Sign(z);
            z -= deccel * dt * tempz * drag;
            if (tempz > 0.1f)
            {
                if (z <= 0f) { z = 0f; }

            }
            else if (tempz < -0.1f)
            {
                if (z >= 0f) { z = 0f; }

            }

        }
        z = Mathf.Clamp(z, -1f, 1f);


        //HORIZONTAL MOVEMENT
        move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1f || move.magnitude < -1)
        {
            move.Normalize();
        }
        controller.Move(move * speed * dt);
        if (Input.GetButtonDown("Jump")) { jumpBufferCounter = jumpBufferLength; }


        if (jumpBufferCounter > 0f && hangTimeCounter > 0f)
        {
            hangTimeCounter = 0f;
            jumpBufferCounter = 0f;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else if (!Input.GetButton("Jump") && !isGrounded && velocity.y >= 0f)
        {

            velocity.y -= jumpSlow;

        }
        //HALVED GRAVITY JUMP PEAK
        else if (Input.GetButton("Jump") && (velocity.y < floatVelStart && velocity.y > -floatVelStart))
        {
            gravity = ogGravity / 2;
        }
        else
        {
            gravity = ogGravity;
        }

        jumpBufferCounter -= dt;


        controller.Move(velocity * dt);
    }
}
