using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    private PlayerInputActions playerInputActions;
    // Start is called before the first frame update
    void OnEnable()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Utility.StartGame.Enable();
        playerInputActions.Utility.StartGame.performed += LoadGame;
    }

    // Update is called once per frame
    private  void LoadGame(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadPauseScreen()
    {

    }

    public void UnloadPauseScreen()
    {

    }
}
