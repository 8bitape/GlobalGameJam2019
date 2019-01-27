using Events;
using UniRxEventAggregator.Events;
using UnityEngine;

public class FightMode : PubSubMonoBehaviour
{
    private CanvasGroup CanvasGroup { get; set; }

    private void Awake()
    {
        this.CanvasGroup = this.GetComponent<CanvasGroup>();

        if (this.CanvasGroup != null)
        {
            this.Subscribe<FightStart>(e => this.EnableCanvas(true));
            this.Subscribe<FightOver>(e => this.EnableCanvas(false));
        }
    }

    private void EnableCanvas(bool isEnabled)
    {
        if (isEnabled)
        {
            this.CanvasGroup.alpha = 1;
        }
        else
        {
            this.CanvasGroup.alpha = 0;
        }
    }
}
