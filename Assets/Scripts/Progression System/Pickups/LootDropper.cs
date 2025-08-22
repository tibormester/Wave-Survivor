using UnityEngine;

[RequireComponent(typeof(HealthController))]
public class LootDropper : MonoBehaviour {

    [SerializeField] private GameObject[] lootTable;
    [SerializeField] private float lootDropChance = 1f;
    
    [SerializeField] private AudioClip _lootDropSound;
    private AudioSource _audioSource;

    public void Start(){
        GetComponent<HealthController>().healthZeroed += DropLoot;
        _audioSource = GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>();
    }

    public void DropLoot(){
        if (Random.Range(0f, 1f) < lootDropChance){
            var lootIndex = Random.Range(0, lootTable.Length);
            Instantiate(lootTable[lootIndex], transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_lootDropSound);
        }
    }
}