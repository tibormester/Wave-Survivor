using UnityEngine;
using System.Collections;
public class Boostable : MonoBehaviour, IMovementAffector
{
    public float boostMultiplier = 1.5f;
    public float boostDuration = 0.8f;
    public float boostCooldown = 0.1f;

    private bool isBoosting = false;
    private bool isOnCooldown = false;

    public float ModifyBaseSpeed(float baseSpeed)
    {
        return isBoosting ? baseSpeed * boostMultiplier : baseSpeed;
    }

    public Vector3 ModifyMovement(Vector3 movement)
    {
        return movement;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isBoosting && !isOnCooldown)
        {
            StartCoroutine(BoostRoutine());
        }
    }

    private IEnumerator BoostRoutine()
    {
        isBoosting = true;
        yield return new WaitForSeconds(boostDuration);
        isBoosting = false;
        isOnCooldown = true;
        yield return new WaitForSeconds(boostCooldown);
        isOnCooldown = false;
    }
}