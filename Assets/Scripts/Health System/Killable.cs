using UnityEngine;
using System.Collections;

public class Killable : MonoBehaviour, IDamageListener{
    public float destroyDelay = 0.15f;

    public void OnDamage(int damageAmount, int newHealth, int maxHealth, GameObject source){
        if (newHealth <= 0){
            StartCoroutine(DelayedKill());
        }
    }

    private IEnumerator DelayedKill(){
        yield return new WaitForSeconds(destroyDelay);

        if (this == null || gameObject == null){
            yield break;
        }

        Destroy(gameObject);
    }
}