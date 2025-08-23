using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    
    const float SPAWN_RADIUS = 25f;
    public int _waveNumber = 0;
    public int _enemiesKilled = 0;
    private int _enemyCount = 0;

    private LootSelectionManager _lootSelectionManager;
    const int LOOT_WAVE_INTERVAL = 1;

    private AudioSource _audioSource;

    [SerializeField] private GameObject _statsPopup;

    [SerializeField] private AudioClip _levelUpSound;
    void Start(){
        _lootSelectionManager = GetComponent<LootSelectionManager>();
        _audioSource = GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>();
        NextWave();
        
    }

    private void SpawnEnemy(Vector3 spawnPoint){
        var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint, Quaternion.identity);
        enemy.GetComponent<HealthController>().healthZeroed += OnEnemyDeath;
        _enemyCount++;
    }

    private void OnEnemyDeath(){
        _enemyCount--;
        _enemiesKilled++;
        if (_enemyCount == 0){
            NextWave();
        }
    }
    public void DisplayStats(){
        var ui = GameObject.FindGameObjectWithTag("ui");
        var statsPopup = Instantiate(_statsPopup, ui.transform);
        statsPopup.GetComponent<StatsPopup>().DisplayStats(_waveNumber, _enemiesKilled);
    }
    private async Task NextWave(){
        _waveNumber++;
        _audioSource.PlayOneShot(_levelUpSound);
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
