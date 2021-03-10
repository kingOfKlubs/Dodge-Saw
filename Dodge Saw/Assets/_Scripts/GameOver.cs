using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // This is called GameOVer because it should happen only when the player hits a gameover 

    public void OnEnable()
    {
        CloudOnceServices.instance.SubmitScoreToLeaderBoard(PlayerPrefs.GetInt("HighScore",0));
    }
}
