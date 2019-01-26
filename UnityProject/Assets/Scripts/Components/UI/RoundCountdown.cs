using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundCountdown : MonoBehaviour
{
    // We may not need these, or they may be stored on a game controller
    // object
    public bool paused = false;
    public bool active = false;

    // Visual text object for the fight round timer
    private TextMeshProUGUI label;
    private float roundTimeRemaining;

    void Awake()
    {
        label = this.GetComponent<TextMeshProUGUI>();
        resetRoundTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (!active || paused) return;
        if (roundTimeRemaining > 0f)
        {
            roundTimeRemaining -= Time.deltaTime;
        }
        label.SetText(roundTimeRemaining.ToString("0"));
    }

    public void resetRoundTime()
    {
        roundTimeRemaining = 60f;
    }
}
