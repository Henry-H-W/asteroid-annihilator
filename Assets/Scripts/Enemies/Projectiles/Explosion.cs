using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.TextCore.Text;

public class Explosion : MonoBehaviour
{
    public float damage;

    public float growth = 0.01f;

    private Boolean isExploding = false;

    public float explosionRadius = 5;

    // Start is called before the first frame update


    // Update is called once per frame
    void FixedUpdate()
    {

        if (this.gameObject.transform.localScale.y > explosionRadius / 5)
        {
            Debug.Log("boom boom");
            isExploding = true;
            //Destroy(gameObject, 0.25f);
            Destroy(transform.parent.gameObject, 0.25f);
        }
        else
        {
            this.gameObject.transform.localScale += new Vector3(growth, growth, growth);
        }
    }




    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("uh oh");
        if (this.isExploding)
        {
            if (collision.CompareTag("Player"))
            {
                //hits the player 
                //collision.GetComponent<Character>().TakeDamage(damage);

            }
            else if (collision.CompareTag("Enemy"))
            {
                //hits the enemy for a reduced amount of damage
                //collision.GetComponent<Character>().TakeDamage(damage * 0.3f);
            }




            Debug.Log("byebye");
            //Destroy(gameObject);
            Destroy(transform.parent.gameObject);

        }
    }

}

