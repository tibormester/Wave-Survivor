using UnityEngine;

public class HealthDebugger : MonoBehaviour
{
    private GameObject _debugSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _debugSource = new GameObject("DebugSource");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)){
            GetComponent<HealthController>().AdjustHealth(1, getMousePosition());
        }
        if (Input.GetKeyDown(KeyCode.J)){
            GetComponent<HealthController>().AdjustHealth(-1, getMousePosition());
        }
        if (Input.GetKeyDown(KeyCode.K)){
            GetComponent<HealthController>().SetHealth(100);
        }
    }

    private GameObject getMousePosition(){
        var mousePosition = Input.mousePosition;
        // Distance from camera to z=0 plane in world space (orthographic)
        mousePosition.z = -Camera.main.transform.position.z;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPos.z = 0f; // keep on 2D plane
        _debugSource.transform.position = worldPos;
        return _debugSource;
    }
}
