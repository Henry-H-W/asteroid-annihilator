using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Target
{
    public GameObject target;
    public float moveSpeed;
    public SpriteRenderer spRend;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RotateTowardsTarget()
    {
        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();

        //find angle of direction in degrees --- use to determine the direction the enemy sprites will face
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Debug.Log(angle);

        if ((angle > 90 && angle < 270) || (angle < -90 && angle > -270))//angle towards left
        {
            spRend.flipX = true;

        }
        else//angle towards right
        {
            spRend.flipX = false;

        }


    }

    public void MoveTowardsTarget()
    {
        //Sets movement of this enemy towards the player
        transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }
}
