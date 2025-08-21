using UnityEngine;

[RequireComponent(typeof(HealthController))]
public class LootDropper : MonoBehaviour {

    [SerializeField] private GameObject[] lootTable;
    [SerializeField] private float lootDropChance = 1f;

    public void Start(){
        GetComponent<HealthController>().healthZeroed += DropLoot;
    }

    public void DropLoot(){
        if (Random.Range(0f, 1f) < lootDropChance){
            var lootIndex = Random.Range(0, lootTable.Length);
            Instantiate(lootTable[lootIndex], transform.position, Quaternion.identity);
        }
    }
}