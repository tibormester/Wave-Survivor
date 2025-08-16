using UnityEngine;

public class PlayerMovementInputCollector : MonoBehaviour
{
    // Update is called once per frame
    private MovementController _movementController;

    void Start()
    {
        _movementController = GetComponent<MovementController>();
    }

    public void Update()
    {   
        _movementController.movementInput = checkInputs();
    }

    private Vector3 checkInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        return new Vector3(moveX, moveY, 0);
    }
}
