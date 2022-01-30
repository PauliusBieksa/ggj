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
        DontDestroyOnLoad(gameObject);
        playerInputActions = new PlayerInputActions();
        playerInputActions.Utility.StartGame.Enable();
        playerInputActions.Utility.StartGame.performed += LoadGame;
        playerInputActions.Utility.StartGame.performed += Restart;
        playerInputActions.Utility.Quit.Enable();
        playerInputActions.Utility.Quit.performed += Quit;
    }


    private void LoadGame(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == "StartScreen")
        {
            SceneManager.LoadScene("GameScene");
        }
    }


    public void Quit(InputAction.CallbackContext context)
    {
        if(Pause.IsDown)
            Application.Quit();
    }

    public static void ScoreScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }

    public static void Restart(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == "EndScreen")
        {
            SceneManager.LoadScene("StartScreen");
        }
    }

    public IEnumerator DisableUtilityInput(float seconds)
    {
        playerInputActions.Utility.Quit.Disable();
        yield return new WaitForSeconds(seconds);
        playerInputActions.Utility.Quit.Enable();
    }
}