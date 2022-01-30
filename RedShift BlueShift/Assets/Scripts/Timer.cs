using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TMP_Text display;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
    }

    public void FixedUpdate()
    {
        TimeSpan runTime = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
        display.text = runTime.ToString("mm':'ss':'fff");
    }

}
