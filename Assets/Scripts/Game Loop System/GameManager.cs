using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    
    private int _waveNumber = 0;

    void Start(){
        SpawnEnemy();
    }

    private void SpawnEnemy(){
        var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemy.GetComponent<HealthController>().healthZeroed += OnEnemyDeath;
    }

    private void OnEnemyDeath(){
        _waveNumber++;
        SpawnEnemy();
    }
}
