using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRxEventAggregator.Events;
using UniRx;
using Events;

public class HealthBar : PubSubMonoBehaviour
{
    // Assign this to the health bar game object that will be scaled
    // to the health of the player with id `playerID`
    public Transform healthBarVisual;
    public int playerID;

    void Awake()
    {
        PubSub.GetEvent<CurrentPlayerHealth>().Where(e=>e.PlayerID == playerID).Subscribe(this.UpdateHealthBar);
    }

    private void UpdateHealthBar(CurrentPlayerHealth health)
    {
        // TODO: Get max health of player
        float maxHealth = 100f;
        float healthPercentage01 = Mathf.Clamp01((float)health.Health / maxHealth);
        UpdateHealthBarPercentage01(healthPercentage01);
    }

    private void UpdateHealthBarPercentage01(float percentage01)
    {
        Vector3 newScale = healthBarVisual.localScale;
        newScale.x = percentage01;
        healthBarVisual.localScale = newScale;
    }
}
