using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Killable : MonoBehaviour, IDamageListener{
    public float destroyDelay = 0.15f;

    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _hitSound;
    private AudioSource _audioSource;
    private bool _isDead = false;

    void Start(){
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnDamage(float damageAmount, float newHealth, float maxHealth, GameObject source){
        if (newHealth <= 0){
            StartCoroutine(DelayedKill());
        }else{
            _audioSource.PlayOneShot(_hitSound);
        }
    }

    private IEnumerator DelayedKill(){
        _audioSource.PlayOneShot(_deathSound);
        var enemyManager = GetComponent<EnemyStateManager>();
        if (enemyManager != null){
            enemyManager.enabled = false;
        }
        yield return new WaitForSeconds(destroyDelay);

        if (this == null || gameObject == null){
            yield break;
        }
        if (gameObject.tag == "Player"){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else {
            Destroy(gameObject);
        }
        
    }
}