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
        this.Subscribe<EndCharacterSelect>(this.EndCharacterSelect);
   }

   private void Start()
   {
        PubSub.Publish(new GameStart());
        PubSub.Publish(new StartCharacterSelect());
   }

   private void EndCharacterSelect(EndCharacterSelect endCharacterSelect)
   {
        var playerOneSpawnPos = new Vector3(-2.0f, 0.0f, 0.0f);
        var playerTwoSpawnPos = new Vector3(2.0f, 0.0f, 0.0f);
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
