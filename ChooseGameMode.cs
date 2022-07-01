using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class ChooseGameMode : MonoBehaviour
{
    private void Start()
    {
        GlobalVars.levelOption = 0;
    }
    public void ChooseMode()
    {
        if(gameObject.name == "Option1")
        {
            Debug.Log("Game Mode 1");
            GlobalVars.levelOption = 0;
        }
        else
        {
            Debug.Log("Game Mode 2");
            GlobalVars.levelOption = 1;
        }
    }    
}
