using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Skript, welches Gegner und Münzen spawnt, für Spawner.
    public bool canSpawn;
    public static Spawner instance;
    [SerializeField] GameObject Enemy, Coin;
    [SerializeField] float maxSpawnTime, minSpawnTime, minimumDividerOfStartTime, minSpeed, maxSpeed, minSpawnTimeCoin, maxSpawnTimeCoin, reducePower;
    [SerializeField] int maxEntities, maxCoins;
    float minimumMaxSpawnTime, minimumMinSpawnTime;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        minimumMinSpawnTime = minSpawnTime / minimumDividerOfStartTime;
        minimumMaxSpawnTime = maxSpawnTime / minimumDividerOfStartTime;
        StartCoroutine(spawnEnemies());
        StartCoroutine(spawnCoins());
    }

    IEnumerator spawnEnemies()
    {
        //Spawnt Gegner zwischen min und maxSpawnTime, falls MaxEntities noch nicht erreicht worden ist.
        if (canSpawn & GameObject.FindGameObjectsWithTag("Enemy").Length < maxEntities)
        {
            GameManager GM = GameManager.instance;
            Vector3 spawnPosition = Vector3.down;
            bool resetSpawnPosition = false;
            do
            {
                resetSpawnPosition = false;
                spawnPosition = new Vector3(UnityEngine.Random.Range(GM.minX + 10, GM.maxX - 10), UnityEngine.Random.Range(GM.minY + 10, GM.maxY - 10), 0f);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, Enemy.GetComponent<SpriteRenderer>().bounds.size.x);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].tag == "pole" || colliders[i].tag == "Enemy" || colliders[i].tag == "SpawnProtection")
                    {
                        resetSpawnPosition = true;
                    }
                }
            }
            while (PlayerAbilitites.instance.DistanceCalc2D(PlayerAbilitites.instance.gameObject.transform.position, spawnPosition) < 3.5f || resetSpawnPosition);
            GameObject spawnedEnemy = Instantiate(Enemy, spawnPosition, Quaternion.identity);
            EnemyMovement enemyMovementScript = spawnedEnemy.GetComponent<EnemyMovement>();
            enemyMovementScript.targetPositions.Add(spawnPosition);
            enemyMovementScript.radius = UnityEngine.Random.Range(9f, 14f);
            Vector2 secondPosition = new Vector2(Mathf.Clamp(UnityEngine.Random.Range(spawnPosition.x - enemyMovementScript.radius, spawnPosition.x + enemyMovementScript.radius), GameManager.instance.minX, GameManager.instance.maxX), Mathf.Clamp(UnityEngine.Random.Range(spawnPosition.y - enemyMovementScript.radius, spawnPosition.y + enemyMovementScript.radius), GameManager.instance.minY, GameManager.instance.maxY));
            enemyMovementScript.targetPositions.Add(secondPosition);
            spawnedEnemy.GetComponent<EnemyMovement>().speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
            minSpawnTime = Mathf.Max(minimumMinSpawnTime, minSpawnTime / reducePower);
            maxSpawnTime = Mathf.Max(minimumMaxSpawnTime, maxSpawnTime / reducePower);
        }
        yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnTime, maxSpawnTime));
        StartCoroutine(spawnEnemies());
    }
    IEnumerator spawnCoins()
    {
        //Spawnt Turnaround Münzen, falls Max Coins nicht erreicht worden ist.
        if (canSpawn & GameObject.FindGameObjectsWithTag("coin").Length < maxCoins)
        {
            GameManager GM = GameManager.instance;
            Vector3 spawnPosition = Vector3.down;
            do
            {
                spawnPosition = new Vector3(UnityEngine.Random.Range(GM.minX + 20, GM.maxX - 20), UnityEngine.Random.Range(GM.minY + 20, GM.maxY - 20), 0f);
            }
            while (PlayerAbilitites.instance.DistanceCalc2D(PlayerAbilitites.instance.gameObject.transform.position, spawnPosition) < 3.5f);
            Instantiate(Coin, spawnPosition, Quaternion.identity);
        }
        yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnTimeCoin, maxSpawnTimeCoin));
        StartCoroutine(spawnCoins());
    }
}
