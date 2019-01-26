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
