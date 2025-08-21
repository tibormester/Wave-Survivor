using UnityEngine;
using System;
public class LootCard : MonoBehaviour{
    public LootObject lootObject;

    public event Action<LootObject> LootSelected;

    public void Initialize(LootObject lootObject){

    }
    public void OnSelect(){
        LootSelected?.Invoke(lootObject);
    }
}