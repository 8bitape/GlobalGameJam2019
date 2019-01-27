using Events;
using UniRx;
using UniRxEventAggregator.Events;

public class PlayerOpponent : PubSubMonoBehaviour
{
    private Player Player { get; set; }

    public BehaviorSubject<CurrentPlayerOpponent> CurrentPlayerOpponent { get; private set; }

    public void Init(Player player)
    {
        this.Player = player;

        this.CurrentPlayerOpponent = new BehaviorSubject<CurrentPlayerOpponent>(new CurrentPlayerOpponent(this.Player, null));

        this.Register(this.CurrentPlayerOpponent);

        PubSub.GetEvent<PlayerSpawned>().Where(e => e.Player != this.Player).Subscribe(this.RegisterOpponent);
    }

    private void RegisterOpponent(PlayerSpawned playerSpawned)
    {
        this.CurrentPlayerOpponent.OnNext(new CurrentPlayerOpponent(this.Player, playerSpawned.Player));
    }
}
