using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField] private int levelTime = 10;
    private float timeCount = 0;
    [SerializeField] private GameObject UI;



    //[SerializeField] private int gameStyle= 1;
    [SerializeField] private GameObject citizenList;

    [SerializeField] private GameObject MenuScreen;

    private GameObject ScoreText;
    private TextMeshProUGUI TimeText;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if (GlobalVars.levelOption == 1){levelTime = 60;}
        timeCount = 0;
        TimeText = UI.transform.Find("UI_KelloPanel").Find("UI_Kello").GetComponent<TextMeshProUGUI>();

        var MenuPanel = GameObject.Find("CanvasInGame").transform.Find("MenuScreen").transform.Find("MenuPanel");

        var CloseButton = MenuPanel.Find("CloseButton").gameObject;
        CloseButton.SetActive(true);

        ScoreText = MenuPanel.Find("ScoreText").gameObject;

        ScoreText.SetActive(false);

        //GlobalVars.levelOption = gameStyle;

        Debug.Log("GameMode: " + GlobalVars.levelOption.ToString());

        startTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        timeCount += Time.fixedDeltaTime;
        //Debug.Log("time:" + timeCount.ToString());
        //QUICK RUN
        if (GlobalVars.levelOption == 1)
        {
            TimeText.text = Mathf.RoundToInt(levelTime - timeCount).ToString();
            if (timeCount >= levelTime)
            {
                //Debug.Log("LevelOption 1: QUICK RUN. End Game");
                GlobalVars.playerScore = Mathf.RoundToInt(timeCount) * citizenList.transform.GetChild(0).childCount;
                UI.SetActive(false);
                //TimeText.text += " :) PELI LOPPUI, PISTEET: " + GlobalVars.playerScore;

                EndGame();
            }
        }
        //HEAL ALL
        else if (timeCount <= 10) { TimeText.text = Mathf.RoundToInt(timeCount).ToString(); }
        else
        {
            TimeText.text = Mathf.RoundToInt(timeCount).ToString();

            //Debug.Log("LevelOption 2: SAVE ALL. End Game");
            if (citizenList.transform.GetChild(1).childCount == 0)
            {
                GlobalVars.playerScore = Mathf.RoundToInt(timeCount);
                UI.SetActive(false);
                //TimeText.text += " :) PELI LOPPUI. AIKAA KULUI: " + GlobalVars.playerScore;

                EndGame();
            }
        }
    }

    void startTimer()
    {
        StartCoroutine("UpdateLevel");
    }
    IEnumerator UpdateLevel()
    {
        for (; ;){

            

            yield return new WaitForSeconds(1);
        }
    }
    private void EndGame()
    {
        Time.timeScale = 0;
        MenuScreen.SetActive(true);
        GameObject titleText = GameObject.Find("TitleText");
        titleText.GetComponent<TextMeshProUGUI>().text="WINNER!";


        ScoreText.SetActive(true);
        ScoreText.GetComponent<TextMeshProUGUI>().text = GlobalVars.playerName+"\nSCORE: " + GlobalVars.playerScore;

        GameObject.Find("CloseButton").SetActive(false);

    }
}
