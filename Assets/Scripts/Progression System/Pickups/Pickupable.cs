using UnityEngine;
using System;

//Detects if collides with player and emits an event

[RequireComponent(typeof(Collider2D))]
public class Pickupable : MonoBehaviour
{
    public event Action<GameObject> OnPickup;
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            PickedUp(collision.gameObject);
        }
    }

    private void PickedUp(GameObject player){
        OnPickup?.Invoke(player);
        Destroy(gameObject);
    }

}
