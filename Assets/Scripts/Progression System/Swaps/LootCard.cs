using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class LootCard : MonoBehaviour{
    public LootObject lootObject;

    private TextMeshProUGUI _nameText;
    private Image _iconImage;
    private TextMeshProUGUI _descriptionText;

    public event Action<LootObject> LootSelected;

    void Start(){
        _nameText = GetComponentInChildren<TextMeshProUGUI>();
        _iconImage = GetComponentInChildren<Image>();
        _descriptionText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initialize(LootObject lootObject){
        _nameText.text = lootObject.name;
        _iconImage.sprite = lootObject.icon;
        _descriptionText.text = lootObject.description;
    }

    
    public void OnSelect(){
        LootSelected?.Invoke(lootObject);
    }
}