using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public string name;
    public int score;

    public void SendScore()
    {
        HighScores.UploadScore(name, score);
    }
}
