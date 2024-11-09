using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class Asteroid : MonoBehaviour
{
    public int maxHP;
    public float health;

    public Vector2 target; // Target position for the asteroid
    public float size;
    public float speed;

    public float rotationSpeed = 30f;
    public GameObject asteroidPrefab; // Reference to the asteroid prefab

    // Start is called before the first frame update
    private float screenWidth;
    private float screenHeight;
    public float spawnDistance = 10f;

    private bool deleteAtTarget = true;


    void Start()
    {
        // Calculate screen bounds in world units
        screenHeight = Camera.main.orthographicSize;
        screenWidth = screenHeight * Camera.main.aspect;
        // Adjust the scale based on the size
        transform.localScale = Vector3.one * size;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
        RotateAsteroid();
    }

    public void Split()
    {
        // Check if the asteroid is large enough to split
        if (size > 0.5f) // Adjust this threshold as needed
        {
            int numberOfFragments = Random.Range(2, 4); // Generate 2 or 3 smaller asteroids

            for (int i = 0; i < numberOfFragments; i++)
            {
                // Get a unique direction for each fragment by dividing 360 degrees by the number of fragments
                float angle = i * (360f / numberOfFragments);
                Vector2 offsetDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                // Offset spawn position in this direction
                Vector2 offsetPosition = (Vector2)transform.position + offsetDirection * size * 0.5f;

                // Instantiate a new asteroid at the offset position
                GameObject newAsteroid = Instantiate(asteroidPrefab, offsetPosition, Quaternion.identity);

                // Get the Asteroid component of the new asteroid
                Asteroid asteroidComponent = newAsteroid.GetComponent<Asteroid>();

                if (asteroidComponent != null)
                {
                    // Set smaller size and lower health for the fragment
                    float newSize = size * 0.5f; // Each fragment is half the original size
                    int newHealth = (int)Mathf.Max(1, health / 2); // Reduce health for smaller asteroids

                    // Set the direction of travel to the offset direction with slight speed variation
                    float fragmentSpeed = speed * Random.Range(0.8f, 1.2f); // Randomize speed slightly
                    Vector2 fragmentTarget = (Vector2)transform.position + offsetDirection * 5f;

                    // Set attributes for the new asteroid
                    asteroidComponent.SetAttributes(newHealth, newSize, fragmentTarget, fragmentSpeed, false);
                }
            }
        }

        // Destroy the original asteroid after splitting
        Destroy(gameObject);
    }

    public Vector2 GetRandomDirection()
    {
        Vector2 targetPosition = Vector2.zero;
        int targetSide = Random.Range(0, 4);
        // Set spawn position and target position based on spawnSide
        switch (targetSide)
        {
            case 0: // Spawn on top, target on bottom
                targetPosition = new Vector2(Random.Range(-screenWidth, screenWidth), screenHeight + spawnDistance);
             
                break;
            case 1: // Spawn on right, target on left
                targetPosition = new Vector2(screenWidth + spawnDistance, Random.Range(-screenHeight, screenHeight));
                
                break;
            case 2: // Spawn on bottom, target on top
                targetPosition = new Vector2(Random.Range(-screenWidth, screenWidth), -screenHeight - spawnDistance);
                
                break;
            case 3: // Spawn on left, target on right
                targetPosition = new Vector2(-screenWidth - spawnDistance, Random.Range(-screenHeight, screenHeight));
                break;
        }
        return targetPosition;
    }


    public void SetAttributes(float newHealth, float newSize, Vector2 newTargetPosition, float newSpeed, bool newDeleteAtTarget)
    {
        health = newHealth;
        size = newSize;
        target = newTargetPosition;
        speed = newSpeed;
        transform.localScale = Vector3.one * size;
        deleteAtTarget = newDeleteAtTarget;
    }

    void MoveTowardsTarget()
    {
        // Calculate the direction and move towards the target position
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

       if((Vector2)transform.position == target && deleteAtTarget)
        {
            Destroy(gameObject);
        }
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
            if (0.5 * size > otherAsteroid.size)
            {
                // If this asteroid is much larger, it will absorb the smaller asteroid
                SetAttributes(health, size+otherAsteroid.size/2, target, speed, true);
            }
            else if (size >0.5)
            {
                // If comparable in size or smaller, split
                Split();
            }
            else
            {
                // If much smaller, just die
                Destroy(gameObject);
            }


        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            //enemies should take damage from small asteroids and be destroyed by large ones
            //if bigger than 4 nothing happens (to asteroid)
            if (size < 4)
            {
                // If this asteroid is decent size, split and kill enemy
                Split();
            }
        }
        /**else if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            //player bullets will do damage and split the roid if it dies

        }
        //enemy bullets will not do anything (put into bullet logic)
        **/
    }


    void RotateAsteroid()
    {
        // Rotate asteroid by rotationSpeed degrees per second
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
