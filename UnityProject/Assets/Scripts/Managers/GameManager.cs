using UnityEngine;
using UniRxEventAggregator.Events;
using Events;

public class GameManager : PubSubMonoBehaviour
{
   private void Awake()
   {
        this.Subscribe<TimeRanOut>(this.TimeRanOut);
        this.Subscribe<PlayerKnockedOut>(this.PlayerKnockedOut);
        this.Subscribe<FightOver>(this.GameOver);
        this.Subscribe<EndCharacterSelect>(this.EndCharacterSelect);
   }

   private void Start()
   {
        PubSub.Publish(new StartCharacterSelect());
   }

   private void EndCharacterSelect(EndCharacterSelect endCharacterSelect)
   {
        PubSub.Publish(new FightStart(endCharacterSelect));
   }

   private void TimeRanOut(TimeRanOut timeRanOut)
   {

   }

   private void PlayerKnockedOut(PlayerKnockedOut playerKnockedOut)
   {
        PubSub.Publish(new FightOver());
    }

   private void GameOver(FightOver gameOver)
   {
        PubSub.Publish(new StartCharacterSelect());
   }
}
