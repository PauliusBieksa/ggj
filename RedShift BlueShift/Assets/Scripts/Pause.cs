using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public static int ScreenHeight = 1080;

    private RectTransform rt;
    private Vector3 downPos;
    private Vector3 upPos;
    public static bool IsDown;
    private PlayerInputActions playerInputActions;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Utility.Pause.Enable();
        playerInputActions.Utility.Pause.performed += Toggle;

        //get the recttransform of the sliding panel
        rt = GetComponent<RectTransform>();
        downPos = rt.localPosition;
        upPos = downPos + new Vector3(0, Pause.ScreenHeight * 2, 0);
        SetUp();
    }

    public void SetDown()
    {
        rt.localPosition = downPos;
        Pause.IsDown = true;
    }

    public void SetUp()
    {
        rt.localPosition = upPos;
        Pause.IsDown = false;
    }

    public void Toggle(InputAction.CallbackContext context)
    {
        if (Pause.IsDown)
        {
            SetUp();
            Time.timeScale = 1;
        }
        else
        {
            SetDown();
            Time.timeScale = 0;
        }
    }
}