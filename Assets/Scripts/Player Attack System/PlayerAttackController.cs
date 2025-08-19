using UnityEngine;

public class AttackAbilityManager : MonoBehaviour
{
    public Attack attackPrefab;
    private NoteManager _noteManager;

    void Start()
    {
        _noteManager = GetComponent<NoteManager>();
    }

    public void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            TryTriggerAttack(_noteManager.GetNextNote());
        }
    }
    public bool TryTriggerAttack(Note note){
        if (note == null){
            return false;
        }

        note.Expire();
        var attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        attack.Initialize(gameObject);

        var wave = attack as UpdatedWaveAttack;
        if (wave != null){
            wave.waveMaxSize *= Mathf.Pow(note.GetPercentageToHitTime(), 2f);
            wave.currentRadius = 1.5f;
            wave.minAngle = GetMouseAngle() - 45f;
            wave.maxAngle = GetMouseAngle() + 45f;
        }

        return true;
    }

    private float GetMouseAngle(){
        var mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPos.z = 0f;
        return Mathf.Atan2(worldPos.y - transform.position.y, worldPos.x - transform.position.x) * Mathf.Rad2Deg;
    }
}
