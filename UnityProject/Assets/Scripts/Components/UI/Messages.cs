using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRxEventAggregator.Events;
using Events;

public class Messages : PubSubMonoBehaviour
{
    private Animator anim;
    void Awake()
    {
        this.anim = GetComponent<Animator>();
        this.Subscribe<FightStart>(this.ShowFightMessage);
    }

    private void ShowFightMessage(FightStart fightStart)
    {
        anim.SetTrigger("Fight");
    }
}
