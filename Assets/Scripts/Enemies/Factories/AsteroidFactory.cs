using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFactory : MonoBehaviour
{
    public GameObject enemyPrefab;      // Reference to the enemy prefab
    public GameObject asteroidPrefab; // Reference to the asteroid prefab
    public float initialSpawnRate = 5f;      // Time interval between spawns
    public float spawnDistance = 10f; // Distance from screen bounds to spawn asteroids
    public float asteroidSpeed = 5f;  // Speed of the asteroid
    public float enemySpeed = 3f;

    public int minHealth = 1;
    public int maxHealth = 5;
    public int minSize = 1;
    public int maxSize = 5;

    private float screenWidth;
    private float screenHeight;

    public float currentSpawnRate;
    private float timeElapsed = 0f;
    void Start()
    {
        // Calculate screen bounds in world units
        screenHeight = Camera.main.orthographicSize;
        screenWidth = screenHeight * Camera.main.aspect;

        currentSpawnRate = initialSpawnRate;
        // Start spawning asteroids
        StartCoroutine(SpawnAsteroids());
    }

    IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            IncreaseDifficulty();
            // Randomly decide to spawn an asteroid or enemy
            int spawnType = Random.Range(0, 6); // 0 = asteroid, 1 = enemy

            if (spawnType <4)
            {
                SpawnAsteroid();
            }
            else
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(currentSpawnRate);

            
        }
    }

    void SpawnAsteroid()
    {
        // Determine which side to spawn on (0 = top, 1 = right, 2 = bottom, 3 = left)
        int spawnSide = Random.Range(0, 4);
        Vector2 spawnPosition = Vector2.zero;
        Vector2 targetPosition = Vector2.zero;

        // Set spawn position and target position based on spawnSide
        switch (spawnSide)
        {
            case 0: // Spawn on top, target on bottom
                spawnPosition = new Vector2(Random.Range(-screenWidth, screenWidth), screenHeight + spawnDistance);
                targetPosition = new Vector2(Random.Range(-screenWidth, screenWidth), -screenHeight - spawnDistance);
                break;
            case 1: // Spawn on right, target on left
                spawnPosition = new Vector2(screenWidth + spawnDistance, Random.Range(-screenHeight, screenHeight));
                targetPosition = new Vector2(-screenWidth - spawnDistance, Random.Range(-screenHeight, screenHeight));
                break;
            case 2: // Spawn on bottom, target on top
                spawnPosition = new Vector2(Random.Range(-screenWidth, screenWidth), -screenHeight - spawnDistance);
                targetPosition = new Vector2(Random.Range(-screenWidth, screenWidth), screenHeight + spawnDistance);
                break;
            case 3: // Spawn on left, target on right
                spawnPosition = new Vector2(-screenWidth - spawnDistance, Random.Range(-screenHeight, screenHeight));
                targetPosition = new Vector2(screenWidth + spawnDistance, Random.Range(-screenHeight, screenHeight));
                break;
        }

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Set random health, size, and speed
        int health = Random.Range(minHealth, maxHealth);
        int size = Random.Range(minSize, maxSize);

        // Apply these attributes to the asteroid, including the target position
        Asteroid asteroidComponent = asteroid.GetComponent<Asteroid>();
        if (asteroidComponent != null)
        {
            asteroidComponent.SetAttributes(health, size, targetPosition, asteroidSpeed, true);
        }
    }

    void SpawnEnemy()
    {
        // Determine which side to spawn on (0 = top, 1 = right, 2 = bottom, 3 = left)
        int spawnSide = Random.Range(0, 4);
        Vector2 spawnPosition = Vector2.zero;

        // Set spawn position and target position based on spawnSide
        switch (spawnSide)
        {
            case 0: // Spawn on top, target somewhere below
                spawnPosition = new Vector2(Random.Range(-screenWidth, screenWidth), screenHeight + spawnDistance);
                break;
            case 1: // Spawn on right, target somewhere to the left
                spawnPosition = new Vector2(screenWidth + spawnDistance, Random.Range(-screenHeight, screenHeight));
                break;
            case 2: // Spawn on bottom, target somewhere above
                spawnPosition = new Vector2(Random.Range(-screenWidth, screenWidth), -screenHeight - spawnDistance);
                break;
            case 3: // Spawn on left, target somewhere to the right
                spawnPosition = new Vector2(-screenWidth - spawnDistance, Random.Range(-screenHeight, screenHeight));
                break;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        
    }

    void IncreaseDifficulty()
    {
        Debug.Log("timing"+timeElapsed);
        // Increase the time elapsed
        timeElapsed += 1;

        // Decrease spawn rate (objects will spawn more frequently)
        if (timeElapsed > 2f) 
        {
            Debug.Log("dif inc 1");
            currentSpawnRate = Mathf.Max(1f, currentSpawnRate - 0.1f); // Set min spawn rate
            
        }

        // Gradually increase speed of asteroids and enemies
        if (timeElapsed > 5f) 
        {
            Debug.Log("dif inc 2");
            asteroidSpeed += 0.2f; // Increase asteroid speed
            enemySpeed += 0.1f;    // Increase enemy speed
        }

        // Gradually increase the health and size of asteroids
        if (timeElapsed > 7f) // After 90 seconds, increase health and size
        {
            Debug.Log("dif inc 3");
            maxHealth = Mathf.Min(10, maxHealth + 1); // Increase max health but cap it
            maxSize = Mathf.Min(7, maxSize + 1);     // Increase max size but cap it
            timeElapsed = 0f; // reset time elapsed for further progression
        }
    }
}
