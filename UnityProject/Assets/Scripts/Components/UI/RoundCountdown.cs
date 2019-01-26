using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundCountdown : MonoBehaviour
{
    public TextMeshProUGUI label;
    public bool paused = false;
    public bool active = false;
    private float roundTimeRemaining;

    void Start()
    {
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
