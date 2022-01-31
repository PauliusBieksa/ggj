using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using Newtonsoft.Json.Serialization;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text currentScoreText;
    public TMP_Text topScoreText;

    private ScoreListObject scoreObject;

    private void Awake()
    {

        TimeSpan runTime = TimeSpan.FromSeconds(GameManager.CurrentRunTime);
        string scoreText = runTime.ToString("mm':'ss':'fff");
        currentScoreText.text = $"Your Score\n{scoreText}";
        topScoreText.text = "";

        LoadTimes();

        scoreObject.allTimes.Add(GameManager.CurrentRunTime);
        scoreObject.allTimes.Sort();


        for (int i = 0; i < 10; i++)
        {
            if(i < scoreObject.allTimes.Count)
            {
                float t = scoreObject.allTimes[i];
                TimeSpan tempTime = TimeSpan.FromSeconds(t);
                string tempScore = tempTime.ToString("mm':'ss':'fff");

                if (tempTime == runTime)
                    topScoreText.text += $"*{tempScore}*\n";
                else
                    topScoreText.text += $"{tempScore}\n";
            }
        }
        SaveTimes();
    }

    private void LoadTimes()
    {
        if (File.Exists(Application.dataPath + "/scores.json"))
        {
            string jsonString = File.ReadAllText(Application.dataPath + "/scores.json");
            Debug.Log("Load: " + jsonString);
            scoreObject = JsonUtility.FromJson<ScoreListObject>(jsonString);
        }
        else
        {
            scoreObject = new ScoreListObject();
        }
    }

    private void SaveTimes()
    {
        string jsonString = JsonUtility.ToJson(scoreObject);
        Debug.Log("Save: " + jsonString);
        File.WriteAllText(Application.dataPath + "/scores.json", jsonString);
    }
}

[Serializable]
public class ScoreListObject
{
    public List<float> allTimes;

    public ScoreListObject()
    {
        allTimes = new List<float>();
    }
}
