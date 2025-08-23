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
        var movementController = GetComponent<MovementController>();
        var attackController = GetComponent<PlayerAttackController>();
        if (enemyManager != null){
            enemyManager.enabled = false;
        }
        if (movementController != null){
            movementController.enabled = false;
        }
        if (attackController != null){
            attackController.enabled = false;
        }
        yield return new WaitForSeconds(destroyDelay);

        if (this == null || gameObject == null){
            yield break;
        }
        if (gameObject.tag == "Player"){
            var gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            gameManager.DisplayStats();
            yield return new WaitForSeconds(2.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else {
            Destroy(gameObject);
        }
        
    }
}