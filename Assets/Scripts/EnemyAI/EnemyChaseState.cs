using UnityEngine;

public class EnemyChaseState : MonoBehaviour, IEnemyState{

    private GameObject _player;
    private MovementController _movementController;

    void Start(){
        _player = GameObject.FindGameObjectWithTag("Player");
        _movementController = GetComponent<MovementController>();
    }
    public void Enter(){
        
    }

    public void Tick(){
        if (_player == null || _movementController == null){
            return;
        }

        _movementController.movementInput = (_player.transform.position - transform.position).normalized;
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