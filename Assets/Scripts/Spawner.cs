using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject coinPrefabs;
    public GameObject MissilePrefabs;

    [Header("스폰 타이밍 생성")]
    public float minSpwanInterval = 0.5f;
    public float maxSpawnInterval = 2.0f;

    [Header("동전 스폰 확률 설정")]
    [Range(0, 100)]
    public int coinSpawnerChance = 50;

    public float timer = 0.0f;
    public float nextSpawTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= nextSpawTime)
        {
            SpawnObject();
            timer = 0.0f;
            SetNextSpawnTimer();
        }
        
    }

    void SpawnObject()
    {
        Transform SpawnTransform = transform;

        int randomValue = Random.Range(0, 100);
        if(randomValue < coinSpawnerChance)
        {
            Instantiate(coinPrefabs, SpawnTransform.position, SpawnTransform.rotation);
        }
        else
        {
            Instantiate(MissilePrefabs, SpawnTransform.position, SpawnTransform. rotation);
        }
            Instantiate(coinPrefabs, SpawnTransform.position, SpawnTransform.rotation);
    }

    void SetNextSpawnTimer()
    {
        nextSpawTime = Random.Range(minSpwanInterval, maxSpawnInterval);
    }
}
