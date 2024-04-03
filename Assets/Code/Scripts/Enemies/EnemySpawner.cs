using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    //Header and Serialized Field are for the inspector menu in Unity. These values can modified from the Unity Editor without changing the code.
    [Header("References")]
    //List of all available enemies that can be spawned from
    [SerializeField] private GameObject[] enemyPrefabs;
    
    [Header("Attritubes")]
    [SerializeField] private int baseEnemies = 8;               //number of enemies that start
    [SerializeField] private float enemiesPerSecond = 0.5f;     //how many enemies spawn/second
    [SerializeField] private float timeBetweenWaves = 5f;       //how much time (in seconds) is between every wave
    [SerializeField] private float scalingFacor = 0.75f;        //difficulty scaling factor. Used as a multiplyer for enemies/wave
    [SerializeField] private float enemiesPerSecondCap = 15f;   //maximum enemies/second

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;        //current enemy wave   
    private float timeSinceLastSpawn;   //time since the last enemy spawned
    private int enemiesAlive;           //current number of enemies alive
    private int enemiesLeftToSpawn;     //remaining enemies to spawn
    private float eps;                  //Enemies per second
    private bool isSpawning = false;    //enemies currently spawning

    //Called when the script instance is loaded
    private void Awake()
    {
        //used to allow the Invoke() method in EnemyMovement for destroying enemies
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    //Called by Unity at the start of the Scene
    private void Start()
    {
        //Starts the waves
        StartCoroutine(StartWave());
    }

    //Update is called every frame
    private void Update()
    {
        //if not spawning, do nothing
        if (!isSpawning) return;

        //add time to "timeSinceLastSpawn"
        timeSinceLastSpawn += Time.deltaTime;

        //if timeSinceLastSpawn is greater than enemies/second and there are still enemies to spawn, then spawn an enemy
        if (timeSinceLastSpawn >= (1f/ eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();               //Spawn a new enemy
            enemiesLeftToSpawn--;       //reduce the number of remaining enemies to spawn
            enemiesAlive++;             //increase enemiesAlive
            timeSinceLastSpawn = 0f;    //reset timeSinceLastSpawn
        }

        //if there are no more enemies to spawn, and all enemies are dead, End the wave
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    //Reduce enemiesAlive when an Enemy is destroyed
    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }
    
    //Start the wave
    private IEnumerator StartWave()
    {
        //wait until timeBetweenWaves seconds have passed
        yield return new WaitForSeconds(timeBetweenWaves);
        
        //set isSpawning, enemiesLeftToSpawn, and EnemiesPerSecond
        isSpawning = true; ;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    //End the wave by setting isSpawning to false, reset timeSinceLastSpawn, increase the wave number, then start a new wave.
    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        LevelManager.main.IncreaseRallyCurrency(1); // Increment Rally currency by a fixed amount (e.g., 10)
        StartCoroutine(StartWave());
    }

    //Spawn an enemy
    private void SpawnEnemy()
    {
        //randomly select an enemy from the list to spawn
        int index = Random.Range(0, enemyPrefabs.Length);
        //set the prefab to be spawned
        GameObject prefabToSpawn = enemyPrefabs[index];
        //Spawn the enemy at the path starting point, with default rotation
        Instantiate(prefabToSpawn, LevelManager.main.startpoint.position, Quaternion.identity);
    }

    //Calculate new Enemies/Wave
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, scalingFacor));
    }

    //Calculate new Enemies/Second
    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, scalingFacor), 0f, enemiesPerSecondCap);
    }

}
