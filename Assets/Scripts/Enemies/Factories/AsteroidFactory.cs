using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFactory : MonoBehaviour
{
    public GameObject asteroidPrefab; // Reference to the asteroid prefab
    public float spawnRate = 2f;      // Time interval between spawns
    public float spawnDistance = 10f; // Distance from screen bounds to spawn asteroids
    public float asteroidSpeed = 5f;  // Speed of the asteroid

    public int minHealth = 1;
    public int maxHealth = 5;
    public int minSize = 1;
    public int maxSize = 5;

    private float screenWidth;
    private float screenHeight;

    void Start()
    {
        // Calculate screen bounds in world units
        screenHeight = Camera.main.orthographicSize;
        screenWidth = screenHeight * Camera.main.aspect;

        // Start spawning asteroids
        StartCoroutine(SpawnAsteroids());
    }

    IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            SpawnAsteroid();
            yield return new WaitForSeconds(spawnRate);
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
            asteroidComponent.SetAttributes(health, size, targetPosition, asteroidSpeed);
        }
    }
}
