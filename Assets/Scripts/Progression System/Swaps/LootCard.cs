using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LootCard : MonoBehaviour, IPointerClickHandler{
    public LootObject lootObject;

    private TextMeshProUGUI _nameText;
    private Image _iconImage;
    private TextMeshProUGUI _descriptionText;

    public event Action<LootObject> LootSelected;

    private void Awake() {
        _nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        _iconImage = transform.Find("Icon").GetComponent<Image>();
        _descriptionText = transform.Find("Description").GetComponent<TextMeshProUGUI>();
    }

    public void Initialize(LootObject lootObject){
        _nameText.text = lootObject.name;
        _iconImage.sprite = lootObject.icon;
        _descriptionText.text = lootObject.description;
        this.lootObject = lootObject;
    }

    public void OnPointerClick(PointerEventData eventData){
        Debug.Log("OnPointerClick:\n" + eventData);
        OnSelect();
    }

    public void OnSelect(){
        LootSelected?.Invoke(lootObject);
    }
}