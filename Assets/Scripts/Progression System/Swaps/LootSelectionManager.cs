using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class LootSelectionManager : MonoBehaviour{
    [SerializeField] private List<LootObject> lootObjects;
    [SerializeField] private LootCard lootCardPrefab;
    public TaskCompletionSource<bool> lootSelected;
    [SerializeField] private Vector3 _lootCardOffset = new Vector3(10f, 0, 0);
    private GameObject _player;
    private List<LootCard> _lootCards;

    void Start(){
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public Task<bool> WaitForLootSelection(){
        lootSelected = new TaskCompletionSource<bool>();
        
        _lootCards = new List<LootCard>();
        for (int i = -1; i <= 1; i++) {
            _lootCards.Add(CreateRandomLootCard(transform.position + i * _lootCardOffset));
        }
        
        return lootSelected.Task;
    }

    public LootCard CreateRandomLootCard(Vector3 position){
        var lootCard = Instantiate(lootCardPrefab, position, Quaternion.identity);
        lootCard.Initialize(lootObjects[Random.Range(0, lootObjects.Count)]);
        lootCard.LootSelected += OnLootSelected;
        return lootCard;
    }

    public void OnLootSelected(LootObject lootObject){
        foreach (var lootCard in _lootCards){
            Destroy(lootCard.gameObject);
        }

        if (lootObject.isMovementAbility){
            _player.GetComponent<MovementAbilitySwapper>().SwapMovementAbility(lootObject.movementAbility);
        } else{
            _player.GetComponent<RhythmSwapper>().SwapRhythm(lootObject.rhythm);
        }

        lootSelected?.TrySetResult(true);
    }


}
