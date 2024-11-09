using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Creeper : Enemy
{


    public float ignitionDistance = 5;
    public static float explosionRadius = 5;

    public Transform center;
    public GameObject aoeIndicator;
    public GameObject explosionEffect;

    private Boolean hasExploded = false;

    public float rotationSpeed = 5f;
    private void Start()
    {
        AcquireTarget();
        maxHP = 3;
        health = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        /**
        if (!hasExploded && DistanceFromTarget() < ignitionDistance)
        {
            //explode
            hasExploded = true;
            //anim.SetTrigger("trigger_explode");
            GameObject indicator = Instantiate(aoeIndicator, center.position, center.rotation, transform);
            indicator.transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);

            GameObject effect = Instantiate(explosionEffect, center.position, center.rotation, transform);
            effect.GetComponent<Explosion>().explosionRadius *= 5;
            effect.GetComponent<Explosion>().growth *= 5;
            effect.GetComponent<Explosion>().damage = attackDamage;


            GameObject finalTarget = Instantiate(new GameObject("explosionPoint"), target.transform.position, target.transform.rotation);
            //make an empty gameObject to set as the new target
            target = finalTarget;
            moveSpeed *= 1.3f;

        }
        **/
        RotateTowardsTarget();
        MoveTowardsTarget();
    }

    private void RotateTowardsTarget()
    {

        // Calculate the direction to the target
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // Find the angle of the direction in degrees
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Add 45 degrees to rotate 45 degrees to the right (clockwise)
        targetAngle -= 90f;
       
        // Get the current rotation of the object (in degrees, around the Z-axis)
        float currentAngle = transform.eulerAngles.z;
        //Debug.Log("current: " + currentAngle + "  target: " + targetAngle);
        // Gradually rotate towards the target angle using Quaternion.RotateTowards
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        // Apply the new rotation as a Quaternion
        transform.rotation = Quaternion.Euler(0, 0, newAngle);

    }


    public void MoveTowardsTarget()
    {
        //Sets movement of this enemy towards the player
        // Calculate the direction 90 degrees to the left of the current forward direction
        Vector3 leftDirection = Quaternion.Euler(0, 0, 90) * transform.up;

        // Move the enemy in the calculated left direction
        transform.Translate(leftDirection * moveSpeed * Time.deltaTime);

    }


}
