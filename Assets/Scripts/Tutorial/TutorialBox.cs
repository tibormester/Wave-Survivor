using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialBox : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData){
        Destroy(gameObject);
    }
}
