using UnityEngine;

public class SpinningArcState : MonoBehaviour, IEnemyState{

    [SerializeField] private WaveAttack _attackPrefab;
    [SerializeField] private float _rotationSpeed = 100f;

    private int _lastAttackNoteIndex = -1;

    private Vector3 _targetPosition;
    private Vector3 _startPosition;
    private bool _clockwise = true;
    
    private MovementController _movementController;
    private GameObject _player;

    void Awake(){
       _clockwise = Random.Range(0, 2) == 0 ? true : false;
       _movementController = GetComponent<MovementController>();
       _player = GameObject.FindGameObjectWithTag("Player");
       GetComponent<NoteManager>().OnNoteHit += ShootAttack;
    }

    public void Enter(){
        _targetPosition = _player.transform.position;
        _startPosition = transform.position;
    }

    public void Tick(){
        var distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance > 4f){
            MoveAlongArc();
        }
        else{
            Rotate();
            Rotate();
        }
        Rotate();


    }

    private void MoveAlongArc(){
        Vector2 projection;
        Vector2 tangent = CircleUtility.TangentDirectionAtProjection(transform.position, _startPosition, _targetPosition, _clockwise, out projection);
        Vector3 direction = new Vector3(tangent.x, tangent.y, 0) + 0.5f * (_player.transform.position - transform.position).normalized;
        _movementController.movementInput = direction.normalized;
    }
    private void Rotate(){
        transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
    }

    private void ShootAttack(){
        var attack =Instantiate(_attackPrefab, transform.position, transform.rotation);
        var waveAttack = attack.GetComponent<WaveAttack>();
        //waveAttack.
        //waveAttack.SetDamage(_damage);
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