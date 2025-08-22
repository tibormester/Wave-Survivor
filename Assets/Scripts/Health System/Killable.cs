using UnityEngine;
using System.Collections;

public class Killable : MonoBehaviour, IDamageListener{
    public float destroyDelay = 0.15f;

    public void OnDamage(float damageAmount, float newHealth, float maxHealth, GameObject source){
        if (newHealth <= 0){
            StartCoroutine(DelayedKill());
        }
    }

    private IEnumerator DelayedKill(){
        var enemyManager = GetComponent<EnemyStateManager>();
        if (enemyManager != null){
            enemyManager.enabled = false;
        }
        yield return new WaitForSeconds(destroyDelay);

        if (this == null || gameObject == null){
            yield break;
        }

        Destroy(gameObject);
    }
}