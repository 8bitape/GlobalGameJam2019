
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRxEventAggregator.Events;
using UniRx;
using Events;

public class PlayerHealth : PubSubMonoBehaviour
{
    [SerializeField]
    private int playerID;

    [SerializeField]
    private int MaxHealth;

    public BehaviorSubject<CurrentPlayerHealth> CurrentHealth { get; private set; }

    private void Awake()
    {
        this.CurrentHealth = new BehaviorSubject<CurrentPlayerHealth>(new CurrentPlayerHealth(this.playerID, this.MaxHealth));

        this.Register(this.CurrentHealth);

        PubSub.GetEvent<HealthChange>().Where(e => e.playerID == this.playerID).Subscribe(e => this.SetHealth(e));

        PubSub.GetEvent<PlayerHit>().Where(e => e.PlayerID == this.playerID).Subscribe(e => this.TakeDamage(e));
    }

    private void SetHealth(HealthChange healthChange)
    {
        this.CurrentHealth.OnNext(new CurrentPlayerHealth(this.playerID, this.CurrentHealth.Value.Health + healthChange.Amount));
        
        if(this.CurrentHealth.Value.Health <= 0)
        {
            PubSub.Publish<PlayerKnockedOut>(new PlayerKnockedOut(this.playerID));
        }
        
        this.CurrentHealth.OnNext(new CurrentPlayerHealth(this.playerID, this.CurrentHealth.Value.Health + healthChange.Amount));
    }

    private void TakeDamage(PlayerHit playerHit)
    {
        switch(playerHit.AttackType)
        {
            case AttackType.LightPunch:
                PubSub.Publish<HealthChange>(new HealthChange(this.playerID, (playerHit.Blocked ? -5 : -10)));
                break;

            case AttackType.HeavyPunch:
                PubSub.Publish<HealthChange>(new HealthChange(this.playerID, (playerHit.Blocked ? -10 : -20)));
                break;

            case AttackType.LightKick:
                PubSub.Publish<HealthChange>(new HealthChange(this.playerID, (playerHit.Blocked ? -5 : -10)));
                break;

            case AttackType.HeavyKick:
                PubSub.Publish<HealthChange>(new HealthChange(this.playerID, (playerHit.Blocked ? -10 : -20)));
                break;
        }
    }
}
