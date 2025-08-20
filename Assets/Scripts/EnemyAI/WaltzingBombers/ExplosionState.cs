using UnityEngine;

public class ExplosionState : MonoBehaviour, IEnemyState{

    public int suicideBomberNotes = 3;
    public float suicideDistance = 3f;
    public GameObject explosionPrefab;

    private int _startNoteIndex;
  

    private GameObject _player;
    private MovementController _movementController;
    private NoteManager _noteManager;

    void Start(){
        _player = GameObject.FindGameObjectWithTag("Player");
        _movementController = GetComponent<MovementController>();
        _noteManager = GetComponent<NoteManager>();
    }
    public void Enter(){
        _startNoteIndex = _noteManager.currentNoteIndex;
        _movementController.movementInput = Vector3.zero;
    }

    public void Tick(){
        if (_noteManager.currentNoteIndex - _startNoteIndex >= suicideBomberNotes){
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            GetComponent<HealthController>().SetHealth(0);
        }
    }

    public void Exit(){
    
    }

    public bool IsValid(){
        return _player != null && (_player.transform.position - transform.position).magnitude < suicideDistance;
    }

    public int GetPriority(){
        return 15;
    }
}