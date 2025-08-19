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

        var wave = attack as WaveAttack;
        if (wave != null){
            wave.waveMaxSize *= Mathf.Pow(note.GetPercentageToHitTime(), 2f);
            wave.currentRadius = 1.5f;
        }

        return true;
    }
}
