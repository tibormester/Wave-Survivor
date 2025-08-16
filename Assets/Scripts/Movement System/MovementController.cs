using UnityEngine;

public class MovementController : MonoBehaviour
{

    public float moveSpeed = 5f;


    public Vector3 movementInput = new Vector3(0, 0, 0);
  
    // Update is called once per frame
    public void Update()
    {
        float finalSpeed = moveSpeed;
        var affectors = GetComponents<IMovementDecorator>();
        //Can i use a collections function like map with an accumulator?
        foreach (var affector in affectors)
        {
            finalSpeed = affector.ModifyBaseSpeed(finalSpeed);
        }
        var finalMovement = movementInput.normalized * finalSpeed * Time.deltaTime;
        foreach (var affector in affectors)
        {
            finalMovement = affector.ModifyMovement(finalMovement);
        }
        transform.position += finalMovement;
    }

}
