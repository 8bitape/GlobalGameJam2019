using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform healthBarVisual;

    void Awake()
    {
        // TODO: Subscribe to health events
    }

    void Update()
    {
        // DEBUG: Randomly generate health drops
        if (Random.Range(0f, 1f) > 0.5f)
        {
            UpdateHealthBarPercentage01(Random.Range(0f, 1f));
        }
    }

    private void UpdateHealthBarPercentage01(float percentage)
    {
        Vector3 newScale = healthBarVisual.localScale;
        newScale.x = percentage;
        healthBarVisual.localScale = newScale;
    }
}
