using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    //Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        LoadNextLevel();
    //    }
    //}

    public void LoadNextLevel()
    {
        //Get playername and game mode
        GameObject playernameText = GameObject.Find("TextEnterName");
        GlobalVars.playerName = playernameText.GetComponent<Text>().text;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        Debug.Log("Load Next Level. playername: " + GlobalVars.playerName+ ", gamemode: " + GlobalVars.levelOption);
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
