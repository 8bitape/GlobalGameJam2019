﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRxEventAggregator.Events;
using Events;

public class RoundCountdown : PubSubMonoBehaviour
{
    // We may not need these, or they may be stored on a game controller
    // object
    public bool paused = false;
    public bool active = false;

    // Visual text object for the fight round timer
    private TextMeshProUGUI label;
    private float roundTimeRemaining;

    private readonly int ROUND_DURATION = 60;
    private readonly int ROUND_DELAY = 5;

    void Awake()
    {
        label = this.GetComponent<TextMeshProUGUI>();

        this.Subscribe<FightStart>(e => this.resetRoundTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (!active || paused) return;
        if (roundTimeRemaining > 0f)
        {
            roundTimeRemaining -= Time.deltaTime;
        }

        if (roundTimeRemaining <= this.ROUND_DURATION)
        {
            label.SetText(roundTimeRemaining.ToString("0"));
        }        
    }

    public void resetRoundTime()
    {
        roundTimeRemaining = this.ROUND_DURATION + this.ROUND_DELAY;
        label.SetText(this.ROUND_DURATION.ToString("0"));
    }
}
