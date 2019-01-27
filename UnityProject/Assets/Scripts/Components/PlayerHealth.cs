using UniRxEventAggregator.Events;
using UniRx;
using Events;

public class PlayerHealth : PubSubMonoBehaviour
{
    private int PlayerId { get; set; }

    private int MaxHealth { get; set; }

    public BehaviorSubject<CurrentPlayerHealth> CurrentHealth { get; private set; }

    public void Init(Player player)
    {
        this.PlayerId = player.Id;
        this.MaxHealth = player.MaxHealth;
    }

    private void Awake()
    {
        this.CurrentHealth = new BehaviorSubject<CurrentPlayerHealth>(new CurrentPlayerHealth(this.PlayerId, this.MaxHealth));

        this.Register(this.CurrentHealth);

        PubSub.GetEvent<HealthChange>().Where(e => e.playerID == this.PlayerId).Subscribe(e => this.SetHealth(e));

        PubSub.GetEvent<PlayerHit>().Where(e => e.PlayerID == this.PlayerId).Subscribe(e => this.TakeDamage(e));
    }

    private void SetHealth(HealthChange healthChange)
    {
        this.CurrentHealth.OnNext(new CurrentPlayerHealth(this.PlayerId, this.CurrentHealth.Value.Health + healthChange.Amount));
        
        if(this.CurrentHealth.Value.Health <= 0)
        {
            PubSub.Publish<PlayerKnockedOut>(new PlayerKnockedOut(this.PlayerId));
        }
        
        this.CurrentHealth.OnNext(new CurrentPlayerHealth(this.PlayerId, this.CurrentHealth.Value.Health + healthChange.Amount));
    }

    private void TakeDamage(PlayerHit playerHit)
    {
        switch(playerHit.AttackType)
        {
            case AttackType.LightPunch:
                PubSub.Publish<HealthChange>(new HealthChange(this.PlayerId, (playerHit.Blocked ? -5 : -10)));
                break;

            case AttackType.HeavyPunch:
                PubSub.Publish<HealthChange>(new HealthChange(this.PlayerId, (playerHit.Blocked ? -10 : -20)));
                break;

            case AttackType.LightKick:
                PubSub.Publish<HealthChange>(new HealthChange(this.PlayerId, (playerHit.Blocked ? -5 : -10)));
                break;

            case AttackType.HeavyKick:
                PubSub.Publish<HealthChange>(new HealthChange(this.PlayerId, (playerHit.Blocked ? -10 : -20)));
                break;
        }
    }
}
