using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns = 2f;//thời gian giữa mỗi lần spawn
    [SerializeField] private int spawnCountIncreaseInterval = 30;//time tăng spawn count
    [SerializeField] private int maxSpawnCount = 10;// Số lượng enemy tối đa có thể spawn cùng lúc

    private int currentSpawnCount = 1;
    private float elapsedTime = 0f;// bộ đếm time
    private void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }
    
    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);

            elapsedTime += timeBetweenSpawns;

            currentSpawnCount = Mathf.Min(
                1 + (int)(elapsedTime / spawnCountIncreaseInterval),
                maxSpawnCount
            );

            for (int i = 0; i < currentSpawnCount; i++)
            {
                // Chọn ngẫu nhiên loại enemy
                GameObject enemy = enemies[Random.Range(0, enemies.Length)];
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(enemy, spawnPoint.position, Quaternion.identity);
            }
        }
    }
}
