using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float rotationSpeed;
    public float maxSpeed;

    [Header("Shooting")]
    public float shootCooldown;
    public KeyCode shootKey = KeyCode.Space;

    [Header("Bullets")]
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;

    float horizontalInput;
    float verticalInput;
    Rigidbody2D rb;

    bool readyToShoot = true;
    bool cooldownStart = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        //Get WASD input
        verticalInput = Input.GetAxisRaw("Vertical");
        if (verticalInput < 0)
        {
            verticalInput = 0;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 forwardMove = new Vector2(0, 1);

        //Handle movement
        rb.AddForce(transform.TransformDirection(Vector2.up * speed * verticalInput * Time.deltaTime), ForceMode2D.Force);
        rb.rotation -= horizontalInput * rotationSpeed * Time.deltaTime;

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

        //Shoot gun
        if (Input.GetKey(shootKey))
        {
            if (!cooldownStart && !readyToShoot)
            {
                Debug.Log("Cool");
                cooldownStart = true;
                Invoke(nameof(ResetShoot), shootCooldown);
            }
            
            else if(readyToShoot)
            {
                readyToShoot = false;
                Debug.Log("Shoot");

                //Instantiate bullet
                //GameObject proj = Object.Instantiate(projectile, attPoint.position, cam.rotation);
            }
        }
    }

    private void ResetShoot()
    {
        readyToShoot = true;
        cooldownStart = false;
    }

}
