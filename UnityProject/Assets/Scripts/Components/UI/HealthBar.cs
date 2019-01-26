using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRxEventAggregator.Events;
using UniRx;

public class HealthBar : PubSubMonoBehaviour
{
    // Assign this to the health bar game object that will be scaled
    // to the health of the player with id `playerID`
    public Transform healthBarVisual;
    public int playerID;

    // DEBUG cooldown between simulated damage events (for testing)
    private float debugCooldown = 1f;

    // Convenience utility to choose a random value from an array
    private T RandomChoice<T>(T[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }

    void Awake()
    {
        PubSub.GetEvent<CurrentPlayerHealth>().Where(e=>e.PlayerID == playerID).Subscribe<CurrentPlayerHealth>(this.UpdateHealthBar);
    }

    void Update()
    {
        // DEBUG: Randomly generate health drops for testing
        debugCooldown -= Time.deltaTime;
        if (debugCooldown <= 0f)
        {
            debugCooldown = 1f;
            if (Random.Range(0f, 1f) > 0.5f)
            {
                int playerToDamage = this.RandomChoice(new int[] { 1, 2 });
                int damageAmount = Random.Range(0, 10);
                PubSub.Publish<HealthChange>(new HealthChange(playerToDamage, -damageAmount));
            }
        }
        ////
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
