using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    private float rotationSpeed = 180;
    public float maxSpeed;

    [Header("Shooting")]
    public float shootCooldown;
    public float bulletSpeed;
    public KeyCode shootKey = KeyCode.Space;
    public float reloadTime;
    public float maxAmmo;

    [Header("Bullets")]
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;

    [Header("Player")]
    public float invulnTime;
    public int health = 3;

    float horizontalInput;
    float verticalInput;
    Rigidbody2D rb;

    bool readyToShoot = true;
    bool cooldownStart = false;

    bool invulnerable = false;
    float invulnTimer = 0;
    int blinkTimer = 0;
    float reloadTimer = 0;

    //Player attributes
    float ammo = 10;
    float varMaxAmmo;
    float varReloadTime;
    int lvl = 1;
    int score = 0;

    bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        varMaxAmmo = maxAmmo;
        varReloadTime = reloadTime;

    }

    // Update is called once per frame
    void Update()
    {

        //Handle player input
        PlayerInput();

        //Handle invulnerability frames
        if(invulnTimer > 0)
        {
            invulnTimer -= Time.deltaTime;

            if (blinkTimer % 2 == 0)
            {
                BlinkOn();
                blinkTimer++;
            }

            else
            {
                BlinkOff();
                blinkTimer++;
            }
        }
        else
        {
            BlinkOn();
        }
    }

    private void BlinkOn() {
        GetComponent<Renderer>().enabled = true;
    }
    private void BlinkOff()
    {
        GetComponent<Renderer>().enabled = false;
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
        if (Input.GetKey(shootKey) && ammo > 0)
        {
            reloadTimer = varReloadTime;
            if (!cooldownStart && !readyToShoot)
            {
                cooldownStart = true;
                Invoke(nameof(ResetShoot), shootCooldown);
            }

            else if (readyToShoot && ammo > 0)
            {
                Shoot();
            }
        }

        else if ((!Input.GetKey(shootKey) || ammo == 0) && ammo < varMaxAmmo)
        {
            if(reloadTimer > 0)
                reloadTimer -= Time.deltaTime;

            else
                Reload();
        }
    }

    private void Shoot()
    {

        readyToShoot = false;

        if(lvl == 1)
        {
            //Instantiate bullet
            GameObject bullet = Instantiate(bullet1, transform.position, transform.rotation);

            //Get rigidbody
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.isKinematic = false;

            //Disable collision with player
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            //Add force 
            Vector2 forceToAdd = transform.TransformDirection(Vector2.up) * bulletSpeed;

            bulletRb.AddForce(forceToAdd, ForceMode2D.Impulse);
            ammo--;
        }

        if (lvl == 2)
        {
            //Instantiate bullet
            GameObject bul1 = Instantiate(bullet2, transform.position + transform.TransformDirection(Vector2.right) * 0.4f, transform.rotation);
            GameObject bul2 = Instantiate(bullet2, transform.position + transform.TransformDirection(Vector2.left) * 0.4f, transform.rotation);

            //Get rigidbody
            Rigidbody2D rb1 = bul1.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = bul2.GetComponent<Rigidbody2D>();
            rb1.isKinematic = false;
            rb2.isKinematic = false;

            //Disable collision with player
            Physics2D.IgnoreCollision(bul1.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul2.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            //Add force 
            Vector2 forceToAdd = transform.TransformDirection(Vector2.up) * bulletSpeed;

            rb1.AddForce(forceToAdd, ForceMode2D.Impulse);
            rb2.AddForce(forceToAdd, ForceMode2D.Impulse);
            ammo--;
        }

        if (lvl == 3)
        {
            //Instantiate bullet
            GameObject bul1 = Instantiate(bullet3, transform.position, transform.rotation);
            GameObject bul2 = Instantiate(bullet3, transform.position, transform.rotation * Quaternion.Euler(0, 0, 16f));
            GameObject bul3 = Instantiate(bullet3, transform.position, transform.rotation * Quaternion.Euler(0, 0, -16f));

            //Get rigidbody
            Rigidbody2D rb1 = bul1.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = bul2.GetComponent<Rigidbody2D>();
            Rigidbody2D rb3 = bul3.GetComponent<Rigidbody2D>();
            rb1.isKinematic = false;
            rb2.isKinematic = false;
            rb3.isKinematic = false;

            //Disable collision with player
            Physics2D.IgnoreCollision(bul1.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul2.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul3.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul1.GetComponent<Collider2D>(), bul2.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul1.GetComponent<Collider2D>(), bul3.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul2.GetComponent<Collider2D>(), bul3.GetComponent<Collider2D>());

            //Add force 
            Vector2 forceToAdd1 = transform.TransformDirection(Vector2.up) * bulletSpeed;
            Vector2 forceToAdd2 = rotate(transform.TransformDirection(Vector2.up), 0.2f) * bulletSpeed;
            Vector2 forceToAdd3 = rotate(transform.TransformDirection(Vector2.up), -0.2f) * bulletSpeed;

            rb1.AddForce(forceToAdd1, ForceMode2D.Impulse);
            rb2.AddForce(forceToAdd2, ForceMode2D.Impulse);
            rb3.AddForce(forceToAdd3, ForceMode2D.Impulse);
            ammo--;
        }
        if (lvl == 4)
        {
            //Instantiate bullet
            GameObject bul1 = Instantiate(bullet4, transform.position, transform.rotation * Quaternion.Euler(0, 0, 15f));
            GameObject bul2 = Instantiate(bullet4, transform.position, transform.rotation * Quaternion.Euler(0, 0, -15f));
            GameObject bul3 = Instantiate(bullet4, transform.position, transform.rotation * Quaternion.Euler(0, 0, 30f));
            GameObject bul4 = Instantiate(bullet4, transform.position, transform.rotation * Quaternion.Euler(0, 0, -30f));

            //Get rigidbody
            Rigidbody2D rb1 = bul1.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = bul2.GetComponent<Rigidbody2D>();
            Rigidbody2D rb3 = bul3.GetComponent<Rigidbody2D>();
            Rigidbody2D rb4 = bul4.GetComponent<Rigidbody2D>();
            rb1.isKinematic = false;
            rb2.isKinematic = false;
            rb3.isKinematic = false;
            rb4.isKinematic = false;

            //Disable collision with player
            Physics2D.IgnoreCollision(bul1.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul2.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul3.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul4.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul1.GetComponent<Collider2D>(), bul2.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul1.GetComponent<Collider2D>(), bul3.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul1.GetComponent<Collider2D>(), bul4.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul2.GetComponent<Collider2D>(), bul3.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul2.GetComponent<Collider2D>(), bul4.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(bul3.GetComponent<Collider2D>(), bul4.GetComponent<Collider2D>());

            //Add force 
            Vector2 forceToAdd1 = rotate(transform.TransformDirection(Vector2.up), 0.12f) * bulletSpeed;
            Vector2 forceToAdd2 = rotate(transform.TransformDirection(Vector2.up), -0.12f) * bulletSpeed;
            Vector2 forceToAdd3 = rotate(transform.TransformDirection(Vector2.up), 0.3f) * bulletSpeed;
            Vector2 forceToAdd4 = rotate(transform.TransformDirection(Vector2.up), -0.3f) * bulletSpeed;

            rb1.AddForce(forceToAdd1, ForceMode2D.Impulse);
            rb2.AddForce(forceToAdd2, ForceMode2D.Impulse);
            rb3.AddForce(forceToAdd3, ForceMode2D.Impulse);
            rb4.AddForce(forceToAdd4, ForceMode2D.Impulse);
            ammo--;
        }
    }
    private Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    private void ResetShoot()
    {
        readyToShoot = true;
        cooldownStart = false;
    }

    private void Reload()
    {
        ammo++;
        reloadTimer = varReloadTime;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Border")
        {
            if (collider.transform.position.x == 0)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y * -1);
            }
            
            else if (collider.transform.position.y == 0)
            {
                transform.position = new Vector2(transform.position.x * -1, transform.position.y);
            }
            
        }

        if(collider.tag == "Level Up")
        {
            if ( lvl < 4 )
            {
                lvl++;
                varReloadTime -= 0.07f;
                shootCooldown -= 0.05f;
            }
            Destroy(collider.gameObject);
            score += 300;
        }

        if(collider.tag == "Ammo Up")
        {
            varMaxAmmo += 2;
            ammo = varMaxAmmo;
            reloadTimer = varReloadTime;
            Destroy(collider.gameObject);
            score += 300;
        }

        if (collider.tag == "Health Up")
        {
            Destroy(collider.gameObject);
            health++;
            score += 300;
        }

        if (collider.tag == "Reload Up")
        {
            Destroy(collider.gameObject);
            varReloadTime -= 0.05f;
            score += 300;
        }
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 20;
        GUI.Label(new Rect(Screen.width - 150, Screen.height - 40, 150, 40), "Ammo: " + ammo.ToString() + "/" + varMaxAmmo.ToString());
        GUI.Label(new Rect(10, 10, 150, 40), "Score: " + score.ToString());
        GUI.Label(new Rect(10, 30, 150, 40), "Player Level: " + lvl.ToString());

        GUI.skin.label.fontSize = 40;

        for(int i = 0; i < health; i++)
        {
            GUI.Label(new Rect(20 + (40*i), Screen.height - 70, 150, 80), "❤️");
        }

        if(gameOver)
        {
            GUI.skin.label.fontSize = 50;
            GUI.Label(new Rect(Screen.width/2-170, Screen.height/2-50, 500, 200), "GAME OVER");
            Time.timeScale = 0f;
        }
    }

    public void Hurt()
    {
        if (invulnTimer > 0)
            return;

        invulnTimer = invulnTime;
        health--;
        varReloadTime = reloadTime;
        shootCooldown += 0.05f * (lvl - 1);
        lvl = 1;
        varMaxAmmo = maxAmmo;

        //Game end
        if(health == 0)
        {
            gameOver = true;
            //_GameOverPanel.Setactive(true); // <- Show GameOver Panel
        }
    }

    public void IncreaseScore(int inc)
    {
        score += inc;
    }


}
