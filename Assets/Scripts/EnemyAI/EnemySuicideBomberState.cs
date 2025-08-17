using UnityEngine;

public class EnemySuicideBomberState : MonoBehaviour, IEnemyState{

    public float suicideBomberTime = 1f;
    public float suicideDistance = 3f;
    public GameObject explosionPrefab;

    private GameObject _player;
    private MovementController _movementController;
    private HealthController _healthController;
    private float _timer = 0f;


    void Start(){
        _player = GameObject.FindGameObjectWithTag("Player");
        _movementController = GetComponent<MovementController>();
        _healthController = GetComponent<HealthController>();
    }
    public void Enter(){
        
    }

    public void Tick(){
        if (_player == null || _movementController == null){
            return;
        }
    
        _movementController.movementInput = Vector2.zero;
        _timer += Time.deltaTime;
        if (_timer >= suicideBomberTime){
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            _healthController.SetHealth(0);
            _timer = 0f;

        }
    }

    public void Exit(){
    
    }

    public bool IsValid(){
        return _player != null && (_player.transform.position - transform.position).magnitude < suicideDistance;
    }

    public int GetPriority(){
        return 10;
    }
}