using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(FuelManager))]
public class DashAbility : MonoBehaviour, IMovementDecorator, IMovementAbility
{
    public float dashDistance = 5f;
    public float dashDuration = 0.7f;
    public float dashFuelCost = 75f;

    private Vector3 _queuedDash = Vector3.zero;
    private float _dashTimer = 0f;
    private float _noteAccuracy = 0f;
    private FuelManager _fuelManager;
    private NoteManager _noteManager;
    private MovementController _movementController;
    void Start(){
        _fuelManager = GetComponent<FuelManager>();
        _noteManager = GetComponent<NoteManager>();
        _movementController = GetComponent<MovementController>();
    }

    public float ModifyBaseSpeed(float baseSpeed)
    {
        return baseSpeed;
    }

//TODO: This logic could be improved, its just copy and pasted from the knockbackable component
    public Vector3 ModifyMovement(Vector3 movement)
    {
        float dashMagnitude = _queuedDash.magnitude;
        if (dashMagnitude > 0f){
            float maxDashThisFrame = dashDistance * Time.deltaTime * (dashMagnitude / dashDuration);
            Vector3 dashThisFrame;
            if (dashMagnitude > maxDashThisFrame){
                dashThisFrame = _queuedDash.normalized * maxDashThisFrame;
                _queuedDash -= dashThisFrame;
            } else {
                dashThisFrame = _queuedDash;
                _queuedDash = Vector3.zero;
            }
            movement += dashThisFrame;
            _dashTimer += Time.deltaTime;
            if (_dashTimer >= dashDuration){
                _queuedDash = Vector3.zero;
            }
        }
        return movement;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.LeftShift) && _movementController.movementInput != Vector3.zero && _fuelManager.TryUseFuel(dashFuelCost)){
            _queuedDash += _movementController.movementInput * dashDistance;
            _dashTimer = 0f;
        }
    }

}