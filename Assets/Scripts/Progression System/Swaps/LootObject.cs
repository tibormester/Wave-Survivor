using UnityEngine;

[CreateAssetMenu(fileName = "LootObject", menuName = "Loot Object")]
public class LootObject : ScriptableObject{
    public string name = "Loot Object";
    public Sprite icon;
    public string description = "Loot Object Description";
    
    public bool isMovementAbility = false;
    public MovementAbilitySwapper.MovementAbility movementAbility;
    public Rhythm rhythm;
}