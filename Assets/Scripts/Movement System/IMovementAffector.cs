using UnityEngine;

public interface IMovementAffector
{
    public float ModifyBaseSpeed(float baseSpeed);
    public Vector3 ModifyMovement(Vector3 movement);
}
