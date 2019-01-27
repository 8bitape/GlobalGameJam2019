using UniRxEventAggregator.Events;
using UniRx;
using Events;

public class PlayerHealth : PubSubMonoBehaviour
{
    private Player Player { get; set; }
    
    public BehaviorSubject<CurrentPlayerHealth> CurrentHealth { get; private set; }

    private bool IsActive { get; set; }

    public void Init(Player player)
    {
        this.Player = player;

        this.CurrentHealth = new BehaviorSubject<CurrentPlayerHealth>(new CurrentPlayerHealth(this.Player.Id, this.Player.MaxHealth));

        this.Register(this.CurrentHealth);

        PubSub.GetEvent<HealthChange>().Where(e => e.playerID == this.Player.Id).Subscribe(e => this.SetHealth(e));

        PubSub.GetEvent<PlayerHit>().Where(e => e.PlayerID == this.Player.Id).Subscribe(e => this.TakeDamage(e));

        this.Subscribe<RoundStarted>(e => this.IsActive = true);
        this.Subscribe<PlayerKnockedOut>(e => this.IsActive = false);
    }

    public void Reset()
    {
        this.CurrentHealth.OnNext(new CurrentPlayerHealth(this.Player.Id, this.Player.MaxHealth));
    }

    private void SetHealth(HealthChange healthChange)
    {
        if (!this.IsActive)
        {
            return;
        }

        this.CurrentHealth.OnNext(new CurrentPlayerHealth(this.Player.Id, this.CurrentHealth.Value.Health + healthChange.Amount));
        
        if(this.CurrentHealth.Value.Health <= 0)
        {
            PubSub.Publish<PlayerKnockedOut>(new PlayerKnockedOut(this.Player.Id));
        }
        
        this.CurrentHealth.OnNext(new CurrentPlayerHealth(this.Player.Id, this.CurrentHealth.Value.Health + healthChange.Amount));
    }

    private void TakeDamage(PlayerHit playerHit)
    {
        switch (playerHit.AttackType)
        {
            case AttackType.LightPunch:
                PubSub.Publish<HealthChange>(new HealthChange(this.Player.Id, (playerHit.Blocked ? -5 : -10)));
                break;

            case AttackType.HeavyPunch:
                PubSub.Publish<HealthChange>(new HealthChange(this.Player.Id, (playerHit.Blocked ? -10 : -20)));
                break;

            case AttackType.LightKick:
                PubSub.Publish<HealthChange>(new HealthChange(this.Player.Id, (playerHit.Blocked ? -5 : -10)));
                break;

            case AttackType.HeavyKick:
                PubSub.Publish<HealthChange>(new HealthChange(this.Player.Id, (playerHit.Blocked ? -10 : -20)));
                break;
        }
    }
}
