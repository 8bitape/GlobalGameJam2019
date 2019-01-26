using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRxEventAggregator.Events;
using UniRx;

public class PlayerHealth : PubSubMonoBehaviour
{
    [SerializeField]
    private int playerID;

    public BehaviorSubject<CurrentPlayerHealth> CurrentHealth { get; private set; }

    private void Awake()
    {
        this.CurrentHealth = new BehaviorSubject<CurrentPlayerHealth>(new CurrentPlayerHealth(this.playerID, 100));

        this.Register(this.CurrentHealth);

        PubSub.GetEvent<HealthChange>().Where(e => e.playerID == this.playerID).Subscribe(e => this.SetHealth(e));
    }

    private void SetHealth(HealthChange healthChange)
    {
        this.CurrentHealth.OnNext(new CurrentPlayerHealth(this.playerID, this.CurrentHealth.Value.Health + healthChange.Amount));
    }
}
