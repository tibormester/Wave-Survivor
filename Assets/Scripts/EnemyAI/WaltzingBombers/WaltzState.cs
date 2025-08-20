using UnityEngine;

public class WaltzState : MonoBehaviour, IEnemyState{


    [SerializeField] private float speed = 3f;
    [SerializeField] private float angleOffset = 30f;
    private Vector3 _targetPosition;
    private float _sign = 1f;
    
    private MovementController _movementController;

    void Start(){
        _movementController = GetComponent<MovementController>();
        _sign = Random.Range(0, 2) == 0 ? 1f : -1f;
    }
    public void Enter(){
        _targetPosition = GetComponent<WaitNoteState>()._targetPosition;
        _sign = -_sign;
    
    }

    public void Tick(){
        Vector3 direction = _targetPosition - transform.position;
        direction = Quaternion.Euler(0f, 0f, angleOffset * _sign) * direction;
        _movementController.movementInput = direction.normalized * speed * Time.deltaTime;
    }

    public void Exit(){
    
    }

    public bool IsValid(){
        return true;
    }

    public int GetPriority(){
        return 0;
    }
}