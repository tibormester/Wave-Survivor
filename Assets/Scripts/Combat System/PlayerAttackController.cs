using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public Attack attackPrefab;
    private NoteManager _noteManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _noteManager = GetComponent<NoteManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")){
            Note note = _noteManager.GetNextNote();

            if (note == null){
                return;
            }
            Debug.Log("Note: " + note.hitTime);
            if (note.hitTime > (double)AudioSettings.dspTime){
                HitNote(note);
            }
        }
    }

    void HitNote(Note note){
        note.Expire();
        var attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        attack.Initialize(gameObject);

        var wave = attack as WaveAttack;
        if (wave != null){
            wave.waveMaxSize *= Mathf.Pow(note.GetPercentageToHitTime(), 2f);
            wave.currentRadius = 1.5f;
        }
        /**
        var arc = attack as ArcWaveAttack;
        if (arc != null){
            arc.waveMaxSize *= Mathf.Pow(ring.hitPercentage, 2f);
            arc.currentRadius = 1.5f;
        }
        */
    }

    void onRingExpire(RhythmDisplayer.RingInstance ring){
        var attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        attack.Initialize(gameObject);

        var wave = attack as WaveAttack;
        if (wave != null){
            wave.waveMaxSize *= Mathf.Pow(ring.hitPercentage, 2f);
            wave.currentRadius = 1.5f;
        }
        
    }
}
