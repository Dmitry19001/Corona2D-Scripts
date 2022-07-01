using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class MainController : MonoBehaviour
{
    public Canvas CanvasMainScreen;
    public Canvas CanvasStartGame;
    public GameObject SettingsScreen;
    public GameObject AboutScreen;
    public GameObject CreditsScreen;

    // Start is called before the first frame update
    void Start()
    {
        CanvasStartGame.enabled = false;
        SettingsScreen.SetActive(false);
        AboutScreen.SetActive(false);
        CreditsScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGame()
    {
        CanvasMainScreen.enabled = false;
        CanvasStartGame.enabled = true;
    }

    public void StartGame(string name)
    {
        // transfer name and options code here
        SceneManager.LoadScene(name);
    }
    public void OpenSettings()
    {
        SettingsScreen.SetActive(true);
    }

    public void CloseSettings()
    {
        SettingsScreen.SetActive(false);
    }

    public void OpenAbout()
    {
        AboutScreen.SetActive(true);
    }

    public void CloseAbout()
    {
        AboutScreen.SetActive(false);
    }

    public void OpenCredits()
    {
        CreditsScreen.SetActive(true);
    }
    public void CloseCredits()
    {
        CreditsScreen.SetActive(false);
    }

    public void CloseStartCanvas()
    {
        CanvasStartGame.enabled = false;
        CanvasMainScreen.enabled = true;
    }
    public void ExitGame()
    {
        // Quit the game. 
        Application.Quit();
    }
}
