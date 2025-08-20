using UnityEngine;

public class StraightStepState : MonoBehaviour, IEnemyState{


    [SerializeField] private float speed = 3f;
    private Vector3 _targetPosition;
    
    private MovementController _movementController;

    void Start(){
        _movementController = GetComponent<MovementController>();
    }
    public void Enter(){
        _targetPosition = GetComponent<WaitBeatState>()._targetPosition;
    }

    public void Tick(){
        Vector3 direction = _targetPosition - transform.position;
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