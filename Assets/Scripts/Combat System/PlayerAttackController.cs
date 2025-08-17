using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public Attack attackPrefab;
    private RhythmDisplayer _rhythmDisplayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rhythmDisplayer = GetComponent<RhythmDisplayer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            var ring = _rhythmDisplayer.GetBestRing();

            if (ring == null){
                return;
            }
            Debug.Log("Ring: " + ring.hitPercentage);
            if (ring.hitPercentage > 0.5f){
                ring.OnHit += onRingExpire;
            }
        }
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
