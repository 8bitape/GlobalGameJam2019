using UniRxEventAggregator.Events;

public class PlayerEvent
{
    public Player Player { get; set; }

    public PlayerEvent(Player player)
    {
        this.Player = player;
    }
}
