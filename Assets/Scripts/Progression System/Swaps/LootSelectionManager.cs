using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class LootSelectionManager : MonoBehaviour{
    [SerializeField] private Canvas lootSelectionCanvas;
    [SerializeField] private List<LootObject> lootObjects;
    [SerializeField] private LootCard lootCardPrefab;

    public TaskCompletionSource<bool> lootSelected;

    private GameObject _player;
    private MovementAbilitySwapper _movementAbilitySwapper;
    private RhythmSwapper _rhythmSwapper;
    private List<LootCard> _lootCards;

    void Start(){
        _player = GameObject.FindGameObjectWithTag("Player");
        _movementAbilitySwapper = GetComponent<MovementAbilitySwapper>();
        _rhythmSwapper = GetComponent<RhythmSwapper>();
    }

    public Task<bool> WaitForLootSelection(){
        lootSelected = new TaskCompletionSource<bool>();
        
        _lootCards = new List<LootCard>();
        for (int i = -1; i <= 1; i++) {
            _lootCards.Add(CreateRandomLootCard(i * _lootCardOffset));
        }
        
        return lootSelected.Task;
    }

    public LootCard CreateRandomLootCard(Vector2 anchoredPosition){
        var lootCard = Instantiate(lootCardPrefab, lootSelectionCanvas.transform);
        lootCard.Initialize(lootObjects[Random.Range(0, lootObjects.Count)]);
        lootCard.LootSelected += OnLootSelected;
        return lootCard;
    }

    public void OnLootSelected(LootObject lootObject){
        foreach (var lootCard in _lootCards){
            Destroy(lootCard.gameObject);
        }

        if (lootObject.isMovementAbility){
            _movementAbilitySwapper.SwapMovementAbility(lootObject.movementAbility);
        } else{
            _rhythmSwapper.SwapRhythm(lootObject.rhythm);
        }

        lootSelected?.TrySetResult(true);
    }


}
