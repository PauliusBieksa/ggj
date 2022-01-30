using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static float CurrentRunTime;
    public static List<float> BestRunTimes;

    private PlayerInputActions playerInputActions;

    // Start is called before the first frame update
    void OnEnable()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Utility.StartGame.Enable();
        playerInputActions.Utility.StartGame.performed += LoadGame;
        playerInputActions.Utility.Quit.Enable();
        playerInputActions.Utility.Quit.performed += Quit;
    }

    // Update is called once per frame
    private void LoadGame(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name != "GameScene")
            SceneManager.LoadScene("GameScene");
    }


    public void Quit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    public static void ScoreScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }
}