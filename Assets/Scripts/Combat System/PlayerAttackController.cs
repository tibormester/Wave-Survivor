using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public Attack attackPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            var attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
            attack.Initialize(gameObject);
        }
    }
}
