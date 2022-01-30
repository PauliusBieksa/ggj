using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarFill : MonoBehaviour
{
    public GameObject player;

    private Image bar;
    private float targetDistance;
    private float playerDistance;

    private void Awake()
    {
        bar = GetComponent<Image>();
        targetDistance = player.GetComponent<PlayerController>().TargetDistance;
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = player.transform.position.z / targetDistance;
    }
}
