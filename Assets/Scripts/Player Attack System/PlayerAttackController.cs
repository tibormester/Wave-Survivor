using UnityEngine;

public class AttackAbilityManager : MonoBehaviour
{
    public WaveAttack attackPrefab;
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
        attack.sourceObject = gameObject;

        attack.waveMaxSize *= Mathf.Pow(note.GetPercentageToHitTime(), 2f);
        attack.currentRadius = 1.5f;
        attack.minAngle = GetMouseAngle() - 45f;
        attack.maxAngle = GetMouseAngle() + 45f;
        

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
