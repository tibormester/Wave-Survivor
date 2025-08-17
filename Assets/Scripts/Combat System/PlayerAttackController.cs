using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public Attack attackPrefab;
    private BeatManager _beatManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _beatManager = GetComponent<BeatManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")){
            Beat beat = _beatManager.GetNextBeat();

            if (beat == null){
                return;
            }
            Debug.Log("Beat: " + beat.hitTime);
            if (beat.hitTime > (double)AudioSettings.dspTime){
                HitBeat(beat);
            }
        }
    }

    void HitBeat(Beat beat){
        beat.Expire();
        var attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        attack.Initialize(gameObject);

        var wave = attack as WaveAttack;
        if (wave != null){
            wave.waveMaxSize *= Mathf.Pow(beat.GetPercentageToHitTime(), 2f);
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
}
