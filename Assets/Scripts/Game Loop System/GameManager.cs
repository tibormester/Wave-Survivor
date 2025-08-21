using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    
    const float SPAWN_RADIUS = 25f;
    private int _waveNumber = 0;
    private int _enemyCount = 0;

    private LootSelectionManager _lootSelectionManager;
    const int LOOT_WAVE_INTERVAL = 1;

    void Start(){
        _lootSelectionManager = GetComponent<LootSelectionManager>();
        NextWave();
    }

    private void SpawnEnemy(Vector3 spawnPoint){
        var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint, Quaternion.identity);
        enemy.GetComponent<HealthController>().healthZeroed += OnEnemyDeath;
        _enemyCount++;
    }

    private void OnEnemyDeath(){
        _enemyCount--;
        if (_enemyCount == 0){
            NextWave();
        }
    }

    private async Task NextWave(){
        _waveNumber++;
        if (_waveNumber % LOOT_WAVE_INTERVAL == 0){
            await _lootSelectionManager.WaitForLootSelection();
        }
        var spawnPoints = GetSpawnPoints(_waveNumber);
        foreach (var spawnPoint in spawnPoints){
            SpawnEnemy(spawnPoint);
        }
    }

    private Vector3[] GetSpawnPoints(int count = 1){
        var spawnPoints = new Vector3[count];
        var center = transform.position;
        var radius = SPAWN_RADIUS;
        for (int i = 0; i < count; i++){
            var angle = i * 2 * Mathf.PI / count;
            spawnPoints[i] = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        }
        return spawnPoints;
    }
}
