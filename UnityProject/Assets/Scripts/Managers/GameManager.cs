using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRxEventAggregator.Events;
using Events;

public class GameManager : PubSubMonoBehaviour
{
   public GameObject PlayerPrefab;

   private void Awake()
   {
        this.PlayerPrefab = (GameObject)Resources.Load("Prefabs/Player");

        this.Subscribe<TimeRanOut>(this.TimeRanOut);
        this.Subscribe<PlayerKnockedOut>(this.PlayerKnockedOut);
        this.Subscribe<GameOver>(this.GameOver);
        this.Subscribe<EndCharacterSelect>(this.EndCharacterSelect);
   }

   private void Start()
   {
        PubSub.Publish(new GameStart());
        PubSub.Publish<StartCharacterSelect>(new StartCharacterSelect());
   }

   private void EndCharacterSelect(EndCharacterSelect endCharacterSelect)
   {
        var playerOneSpawnPos = new Vector3(-5.0f, 0.0f, 0.0f);
        var playerTwoSpawnPos = new Vector3(5.0f, 0.0f, 0.0f);

        var playerOne = (GameObject)GameObject.Instantiate(this.PlayerPrefab);
        playerOne.GetComponent<Player>().Id = 1;

        var playerTwo = (GameObject)GameObject.Instantiate(this.PlayerPrefab);
        playerOne.GetComponent<Player>().Id = 2;

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
