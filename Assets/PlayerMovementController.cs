using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    public float moveSpeed = 5f;
    private Vector3 movementInput = new Vector3(0, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkInputs();
        transform.position += movementInput.normalized * moveSpeed * Time.deltaTime;
    }

    private void checkInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movementInput = new Vector3(moveX, moveY, 0);
    }
}
