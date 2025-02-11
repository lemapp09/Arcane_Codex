using System.Collections.Generic;
using UnityEngine;

public class MissilePooler : MonoBehaviour
{
    [SerializeField]
    private GameObject missilePrefab; // Reference to the missile prefab
    [SerializeField]
    private GameObject enemyMissilePrefab; // Reference to the enemy missile prefab
    [SerializeField]
    private GameObject[] coinPrefabs; // Reference to the coin prefabs
    [SerializeField]
    private int poolSize = 100; // The initial size of the missile pool
    [SerializeField]
    private int enemyPoolSize = 25; // The initial size of the enemy missile pool
    private Queue<GameObject> missilePool = new Queue<GameObject>();
    private Queue<GameObject> enemyMissilePool = new Queue<GameObject>();
    private Queue<GameObject> coinPool = new Queue<GameObject>();

    void Start()
    {
        // Preload the pool with missile objects
        for (int i = 0; i < poolSize; i++)
        {
            // Preload the pool with player missile objects
            GameObject missile = Instantiate(missilePrefab, transform);
            missile.transform.name = "Player Missile " + i.ToString().PadLeft(3, '0');
            missile.SetActive(false); // Deactivate it initially
            missile.GetComponent<Laser>().SetMissilePooler(this); // Set the missile pooler
            missilePool.Enqueue(missile);
        }

        // Preload the pool with enemy missile objects
        for (int i = 0; i < enemyPoolSize; i++)
        {
            // Preload the pool with enemy missile objects
            GameObject enemyMissile = Instantiate(enemyMissilePrefab, transform);
            enemyMissile.transform.name = "Enemy Missile " + i.ToString().PadLeft(3, '0');
            enemyMissile.SetActive(false); // Deactivate it initially
            enemyMissile.GetComponent<EnemyLaser>().SetMissilePooler(this); // Set the missile pooler
            enemyMissilePool.Enqueue(enemyMissile);
        }
        
        // Preload the pool with coin objects
        for (int i = 0; i < coinPrefabs.Length; i++)
        {
            // Preload the pool with coin objects
            GameObject coin = Instantiate(coinPrefabs[i], transform);
            coin.transform.name = "Coin " + i.ToString().PadLeft(3, '0');
            coin.SetActive(false); // Deactivate it initially
            coin.GetComponent<CoinSpin>().SetMissilePooler(this); // Set the missile pooler
            coinPool.Enqueue(coin);
        }
    }

    // Get a player missile from the pool
    public GameObject GetMissile()
    {
        if (missilePool.Count > 0)
        {
            GameObject missile = missilePool.Dequeue();
            return missile;
        }
        else
        {
            // If the pool is empty, you can either expand the pool or return null
            // Here we're expanding the pool by adding a new missile
            GameObject missile = Instantiate(missilePrefab, transform);
            missile.GetComponent<Laser>().SetMissilePooler(this); // Set the missile pooler
            return missile;
        }
    }

    // Get an enemy missile from the pool
    public GameObject GetEnemyMissile()
    {
        if (enemyMissilePool.Count > 0)
        {
            GameObject enemyMissile = enemyMissilePool.Dequeue();
            return enemyMissile;
        }
        else
        {
            // If the pool is empty, you can either expand the pool or return null
            // Here we're expanding the pool by adding a new missile
            GameObject enemyMissile = Instantiate(enemyMissilePrefab, transform);
            enemyMissile.GetComponent<EnemyLaser>().SetMissilePooler(this); // Set the missile pooler
            return enemyMissile;
        }
    }

    // Get an enemy missile from the pool
    public GameObject GetCoin()
    {
        if (coinPool.Count > 0)
        {
            GameObject coin = coinPool.Dequeue();
            return coin;
        }
        else
        {
            // If the pool is empty, you can either expand the pool or return null
            // Here we're expanding the pool by adding a new coin
            int i = Random.Range(0, coinPrefabs.Length);
            GameObject coin = Instantiate(coinPrefabs[i], transform);
            coin.GetComponent<CoinSpin>().SetMissilePooler(this); // Set the missile pooler
            return coin;
        }
    }

    // Return a missile to the pool
    public void ReturnMissileToPool(GameObject missile)
    {
        missile.SetActive(false); // Deactivate the missile when it returns to the pool
        missile.transform.rotation = Quaternion.Euler(0,0,0); // Reset the position of the missile
        missilePool.Enqueue(missile);
    }

    // Return an Enemy missile to the pool
    public void ReturnEnemyMissileToPool(GameObject enemyMissile)
    {
        enemyMissile.SetActive(false); // Deactivate the enemy missile when it returns to the pool
        enemyMissilePool.Enqueue(enemyMissile);
    }

    // Return an Enemy missile to the pool
    public void ReturnCoinToPool(GameObject coin)
    {
        coin.SetActive(false); // Deactivate the coin when it returns to the pool
        enemyMissilePool.Enqueue(coin);
    }
}