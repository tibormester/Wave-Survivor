using UnityEngine;

public class HealthDebugger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)){
            GetComponent<HealthController>().AdjustHealth(1);
        }
        if (Input.GetKeyDown(KeyCode.J)){
            GetComponent<HealthController>().AdjustHealth(-1);
        }
        if (Input.GetKeyDown(KeyCode.K)){
            GetComponent<HealthController>().SetHealth(100);
        }
    }
}
