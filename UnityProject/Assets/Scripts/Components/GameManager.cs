using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRxEventAggregator.Events;
using Events;

public class GameManager : PubSubMonoBehaviour
{
    private void Awake()
    {
        this.Subscribe<TimeRanOut>(this.TimeRanOut);
        this.Subscribe<PlayerKnockedOut>(this.PlayerKnockedOut);
        this.Subscribe<GameOver>(this.GameOver);
    }

    private void Start()
    {
        PubSub.Publish<StartCharacterSelect>(new StartCharacterSelect());
    }

    private void TimeRanOut(TimeRanOut timeRanOut)
    {

    }

    private void PlayerKnockedOut(PlayerKnockedOut playerKnockedOut)
    {

    }

    private void GameOver(GameOver gameOver)
    { 
    
    }
}
