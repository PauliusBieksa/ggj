using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public enum SpeedClass
{
    Slow = 5,
    Fast = 10,
    Penalty = 2
};

public class PlayerController : MonoBehaviour
{
    public SpeedClass PlayerSpeed = SpeedClass.Slow;
    public float StrafeSpeed = 5;
    public float CollisionCooldownTime;

    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    private Transform trans;
    private float collisionTime;


    void OnEnable()
    {
        trans = gameObject.transform;
        playerInputActions = new PlayerInputActions();
        PlayerSpeed = SpeedClass.Slow;

        playerInputActions.PlayerControls.SpeedToggle.Enable();
        playerInputActions.PlayerControls.Movement.Enable();
        playerInputActions.PlayerControls.SpeedToggle.performed += ToggleSpeed;
    }

    void FixedUpdate()
    {
        Translate();
    }

    private void Translate()
    {
        Strafe();
        Propulsion();
    }

    private void Strafe()
    {
        Vector2 strafeVector = playerInputActions.PlayerControls.Movement.ReadValue<Vector2>();
        Vector3 diff = new Vector3(strafeVector.x, strafeVector.y, 0) * StrafeSpeed * Time.fixedDeltaTime;
        Debug.Log(strafeVector + " " + diff);
        transform.Translate(diff);
    }

    private void Propulsion()
    {
        Debug.Log(PlayerSpeed + " " + (int) PlayerSpeed);

        //set forward velocity
        Vector3 pos = trans.position + trans.forward * (int) PlayerSpeed * Time.fixedDeltaTime;
        trans.position = pos;
    }

    private void ToggleSpeed(InputAction.CallbackContext context)
    {
        if (!context.started && context.action.name != "SpeedToggle") return;

        //check if penalised
        if (Time.time - collisionTime >= 2 && PlayerSpeed == SpeedClass.Penalty)
        {
            PlayerSpeed = SpeedClass.Slow;
            Debug.Log("Recovered");
        }

        //check for speed toggle
        switch (PlayerSpeed)
        {
            case SpeedClass.Slow:
                PlayerSpeed = SpeedClass.Fast;
                break;
            case SpeedClass.Fast:
                PlayerSpeed = SpeedClass.Slow;
                break;
            case SpeedClass.Penalty:
                Debug.Log("can't toggle speed during penalty time");
                break;
            default:
                Debug.LogError("Unexpected SpeedClass");
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("colliding");
        CollisionEffects(collision);
    }

    private void CollisionEffects(Collision collision)
    {
        collisionTime = Time.time;
    }
}