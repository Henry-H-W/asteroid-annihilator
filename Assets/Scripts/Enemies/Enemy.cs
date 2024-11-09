using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.SocialPlatforms;
using UnityEngine.TextCore.Text;
using System.Drawing;

public abstract class Enemy : MonoBehaviour
{

    public float moveSpeed = 1f;
    public int maxHP = 3;
    public float health;


    public GameObject target;


    public float DistanceFromTarget()
    {
        return Vector2.Distance(transform.position, target.transform.position);
    }

    
    public void AcquireTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Player"))
        {

            //Debug.Log("touch player");
            //player take damage;
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            Asteroid otherAsteroid = collision.gameObject.GetComponent<Asteroid>();
            //smaller asteroids should just die
            //larger asteroids should survive
            // Compare the size of this asteroid with the size of the other asteroid
            if (otherAsteroid.size > 4)
            {
                // If this asteroid is much larger, kill
                Destroy(gameObject);
            }
            else
            {
                // If comparable in size or smaller, damage
                TakeDamage();
            }
        }
        /**else if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            //player bullets will do damage and split the roid if it dies

        }**/
    }

    //enemy bullets will not do anything (put into bullet logic)
    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
           Destroy(gameObject);
        }
    }

}

