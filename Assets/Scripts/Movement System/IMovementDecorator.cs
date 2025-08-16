using UnityEngine;

public interface IMovementDecorator
{
    public float ModifyBaseSpeed(float baseSpeed);
    public Vector3 ModifyMovement(Vector3 movement);
}
