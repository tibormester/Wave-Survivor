using UnityEngine;

public class AttackStepState : MonoBehaviour, IEnemyState{
    [SerializeField] private int damage = 1;
    [SerializeField] private float detectPlayerDistance = 3f;
    [SerializeField] private float maxAttackDistance = 1f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float attackCooldown = 1f;
    private int _lastAttackNoteIndex = -1;
    private GameObject _player;
    private NoteManager _noteManager;
    private MovementController _movementController;

    void Start(){
        _player = GameObject.FindGameObjectWithTag("Player");
        _movementController = GetComponent<MovementController>();
        _noteManager = GetComponent<NoteManager>();
    }
    public void Enter(){
    }

    public void Tick(){
        if (Vector3.Distance(transform.position, _player.transform.position) < maxAttackDistance){
            AttackPlayer();
        }
        else{
            MoveTowardsPlayer();
        }
    }

    public void MoveTowardsPlayer(){
        Vector3 direction = _player.transform.position - transform.position;
        _movementController.movementInput = direction.normalized * speed * Time.deltaTime;
    }
    public void AttackPlayer(){
        _lastAttackNoteIndex = _noteManager.currentNoteIndex;
        HealthController playerHealthController = _player.GetComponent<HealthController>();
        playerHealthController.AdjustHealth(-damage, gameObject);  
    }

    public void Exit(){
    
    }

    public bool IsValid(){
        if (_lastAttackNoteIndex == _noteManager.currentNoteIndex){
            return false;
        }
        if (Vector3.Distance(transform.position, _player.transform.position) > detectPlayerDistance){
            return false;
        }
        return true;
    }

    public int GetPriority(){
        return 1;
    }
}