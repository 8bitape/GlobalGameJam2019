using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRxEventAggregator.Events;
using Events;

[RequireComponent(typeof(Text))]
public class DebugText : PubSubMonoBehaviour
{
    private Text Text { get; set; }

    private void Awake()
    {
        this.Text = this.GetComponent<Text>();

        if(this.Text != null)
        {
            this.Subscribe<PlayerAttack>(this.UpdateText);
        }
    }

    private void UpdateText(PlayerAttack playerAttack)
    {
        switch(playerAttack.attackType)
        {
            case AttackType.LightPunch:
                this.Text.text = "LIGHT PUNCH";
                break;

            case AttackType.HeavyPunch:
                this.Text.text = "HEAVY PUNCH";
                break;

            case AttackType.LightKick:
                this.Text.text = "LIGHT KICK";
                break;

            case AttackType.HeavyKick:
                this.Text.text = "HEAVY KICK";
                break;
        }
    }
}
