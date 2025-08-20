using UnityEngine;

public class WaitNoteState : MonoBehaviour, IEnemyState{

    [SerializeField] private float waitTimeBeats = 0.25f;
    [SerializeField] private float noteDivisor = 1f;
    public Vector3 _targetPosition {get; private set;}

    private GameObject _player;
    private NoteManager _noteManager;
    private MovementController _movementController;

    void Start(){
        _player = GameObject.FindGameObjectWithTag("Player");
        _movementController = GetComponent<MovementController>();
        _noteManager = GetComponent<NoteManager>();
    }
    public void Enter(){
        _targetPosition = _player.transform.position;
    }

    public void Tick(){
        _movementController.movementInput = Vector2.zero;
    }

    public void Exit(){
    
    }

    public bool IsValid(){
        if (_noteManager.currentNoteIndex % noteDivisor != 0){
            return false;
        }
        double previousNoteHitTime = _noteManager.GetNoteHitTimeFromRhythm(_noteManager.currentNoteIndex - 1);
        return AudioSettings.dspTime <= previousNoteHitTime + (_noteManager._beatManager.GetSecondsPerBeat() * waitTimeBeats);
    }

    public int GetPriority(){
        return 10;
    }
}