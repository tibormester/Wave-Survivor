using UnityEngine;
using System;

//Detects if collides with player and emits an event

[RequireComponent(typeof(Collider2D))]
public class Pickupable : MonoBehaviour
{
    public event Action<GameObject> OnPickup;
    [SerializeField] private AudioClip _pickupSound;
    private AudioSource _audioSource;

    void Start(){
        _audioSource = GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            PickedUp(collision.gameObject);
        }
    }

    private void PickedUp(GameObject player){
        _audioSource.PlayOneShot(_pickupSound);
        OnPickup?.Invoke(player);
        Destroy(gameObject);
    }

}
