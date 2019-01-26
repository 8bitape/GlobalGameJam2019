using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRxEventAggregator.Events;

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
            case PlayerAttack.AttackType.LightPunch:
                this.Text.text = "LIGHT PUNCH";
                break;

            case PlayerAttack.AttackType.HeavyPunch:
                this.Text.text = "HEAVY PUNCH";
                break;

            case PlayerAttack.AttackType.LightKick:
                this.Text.text = "LIGHT KICK";
                break;

            case PlayerAttack.AttackType.HeavyKick:
                this.Text.text = "HEAVY KICK";
                break;
        }
    }
}
