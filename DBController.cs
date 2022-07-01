using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts;

public class DBController : MonoBehaviour
{
    const string urlBackendHighScoresTime = "http://172.30.139.140/highscorestime.php";
    const string urlBackendHighScoresFit = "http://172.30.139.140/highscoresfit.php";

    bool updateHighScoreTextAreaTime = true;
    bool updateHighScoreTextAreaFit = true;

    // highScore table
    HighScores.HighScores hsTime;
    HighScores.HighScores hsFit;

    // logging info
    string log = "";
    int fetchCounter = 0;

    // UI elements
    public UnityEngine.UI.Text highScoresTextAreaTime;
    public UnityEngine.UI.Text highScoresTextAreaFit;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequestForHighScoresTime(urlBackendHighScoresTime));
        StartCoroutine(GetRequestForHighScoresFit(urlBackendHighScoresFit));
    }

    // Update is called once per frame
    void Update()
    {
        if (updateHighScoreTextAreaTime)
        {
            highScoresTextAreaTime.text = CreateHighScoreListTime();
            if (hsTime != null)
            {
                updateHighScoreTextAreaTime = false;
            }
        }
        if (updateHighScoreTextAreaFit)
        {
            highScoresTextAreaFit.text = CreateHighScoreListFit();
            if (hsFit != null)
            {
                updateHighScoreTextAreaFit = false;
            }
        }
    }

    string CreateHighScoreListTime()
    {
        string hsListTime = "";

        if (hsTime != null)
        {
            int len = (hsTime.scores.Length < 3) ? hsTime.scores.Length : 3;
            for (int i = 0; i < len; i++)
            {
                hsTime.scores[i].playtime = hsTime.scores[i].playtime.Substring(0, 10);
                hsListTime += string.Format("{0} {1,15}  {2,15}s   {3,5}\n",
                    (i + 1), hsTime.scores[i].playername, hsTime.scores[i].score, hsTime.scores[i].playtime);
            }
        }
        return hsListTime;
    }

    string CreateHighScoreListFit()
    {
        string hsListFit = "";

        if (hsFit != null)
        {
            int len = (hsFit.scores.Length < 3) ? hsFit.scores.Length : 3;
            for (int i = 0; i < len; i++)
            {
                hsFit.scores[i].playtime = hsFit.scores[i].playtime.Substring(0, 10);
                hsListFit += string.Format("{0} {1,15}  {2,15}   {3,5}\n",
                    (i + 1), hsFit.scores[i].playername, hsFit.scores[i].score, hsFit.scores[i].playtime);
            }
        }
        return hsListFit;
    }

    IEnumerator GetRequestForHighScoresTime(string uri)
    { 
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            InsertToLog("Request sent to " + uri);
            // set downloadHandler for json
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            // Request and wait for reply
            yield return webRequest.SendWebRequest();

            // get raw data and convert it to string
            string resultStrTime = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
            if (webRequest.isNetworkError)
            {

                InsertToLog("Error encountered: " + webRequest.error);
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // create HighScore item from json string
                hsTime = JsonUtility.FromJson<HighScores.HighScores>(resultStrTime);

                InsertToLog("Response received succesfully ");
                Debug.Log("Received(UTF8): " + resultStrTime);
                //Debug.Log("Received(HS): " + JsonUtility.ToJson(hs));
                //Debug.Log("Received(HS) name: " + hs.scores[0].playername);
            }
        }
        
    }

    IEnumerator GetRequestForHighScoresFit(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            InsertToLog("Request sent to " + uri);
            // set downloadHandler for json
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            // Request and wait for reply
            yield return webRequest.SendWebRequest();

            // get raw data and convert it to string
            string resultStrFit = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);

            if (webRequest.isNetworkError)
            {
                InsertToLog("Error encountered: " + webRequest.error);
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // create HighScore item from json string
                hsFit = JsonUtility.FromJson<HighScores.HighScores>(resultStrFit);

                InsertToLog("Response received succesfully ");
                Debug.Log("Received(UTF8): " + resultStrFit);
                //Debug.Log("Received(HS): " + JsonUtility.ToJson(hs));
                //Debug.Log("Received(HS) name: " + hs.scores[0].playername);
            }
        }

    }
    
    string InsertToLog(string s)
    {
        return log = "[" + fetchCounter + "] " + s + "\n" + log;
    }


}
