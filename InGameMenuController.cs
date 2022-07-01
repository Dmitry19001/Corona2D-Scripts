using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class InGameMenuController : MonoBehaviour
{
    public Canvas CanvasInGame;
    public GameObject MenuScreen;

    // Start is called before the first frame update
    void Start()
    {
        //CanvasInGame.enabled = false;
        MenuScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInGameMenu();
        }
    }

    public void ToggleInGameMenu()
    {
        bool active = MenuScreen.activeSelf;
        //CanvasInGame.enabled = true;
        MenuScreen.SetActive(!active);

        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }   
    public void OpenInGameMenu()
    {
        //CanvasInGame.enabled = true;
        MenuScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseInGameMenu()
    {
        //CanvasInGame.enabled = false;
        MenuScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        // Quit the game. 
        Application.Quit();
    }
    public void ExitToMain()
    {
        // Exit to Main. 
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
