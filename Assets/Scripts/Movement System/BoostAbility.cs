using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(FuelManager))]
public class BoostAbility : MonoBehaviour, IMovementDecorator, IMovementAbility
{
    public float boostMultiplier = 1.85f;
    public float boostFuelCostSecond = 25f;

    private bool isBoosting = false;
    private float noteAccuracy = 0f;
    private FuelManager _fuelManager;
    private NoteManager _noteManager;
    void Start(){
        _fuelManager = GetComponent<FuelManager>();
        _noteManager = GetComponent<NoteManager>();
    }

    public float ModifyBaseSpeed(float baseSpeed)
    {
        if (isBoosting && _fuelManager.TryUseFuel(boostFuelCostSecond * Time.deltaTime)){
            return baseSpeed * (boostMultiplier + (Mathf.Max(noteAccuracy -0.8f, 0f) * 2f));
        }
        else{
            return baseSpeed;
        }
    }

    public Vector3 ModifyMovement(Vector3 movement)
    {
        return movement;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            isBoosting = true;
            noteAccuracy = _noteManager.GetNextNote().GetPercentageToHitTime();
        }
        if (!Input.GetKey(KeyCode.LeftShift)){
            isBoosting = false;
        }
    }


}