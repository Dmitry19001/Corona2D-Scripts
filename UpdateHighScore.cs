using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts;

public class UpdateHighScore : MonoBehaviour
{
    const string urlBackendHighScores = "http://172.30.139.140/highscores.php";

    // highScore table
    HighScores.HighScores hs;

    // logging info
    string log = "";
    int fetchCounter = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PostGameResults()
    {
        HighScores.HighScore hsItem = new HighScores.HighScore();
        hsItem.playername = GlobalVars.playerName;
        hsItem.score = GlobalVars.playerScore;

        if (GlobalVars.levelOption == 0)
        {
            hsItem.gameoption = "time";
        }
        if (GlobalVars.levelOption == 1)
        {
            hsItem.gameoption = "fit";
        }

        StartCoroutine(PostRequestForHighScores(urlBackendHighScores, hsItem));
    }

    IEnumerator PostRequestForHighScores(string uri, HighScores.HighScore hsItem)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, JsonUtility.ToJson(hsItem)))
        {
            InsertToLog("Post request sent to " + uri);

            // set downloadHandler for json
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            // Request and wait for reply
            yield return webRequest.SendWebRequest();

            // get raw data and convert it to string
            string resultStr = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);

            if (webRequest.isNetworkError)
            {
                InsertToLog("Error encountered in POST request: " + webRequest.error);
                Debug.Log("Error in post request: " + webRequest.error);
            }
            else
            {
                InsertToLog("Post received succesfully ");
                Debug.Log("Received(UTF8): " + resultStr);
            }
        }

    }

    string InsertToLog(string s)
    {
        return log = "[" + fetchCounter + "] " + s + "\n" + log;
    }
}
