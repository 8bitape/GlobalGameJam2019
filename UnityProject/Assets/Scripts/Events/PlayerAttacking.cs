public class PlayerAttacking : PlayerEvent
{
    public Player Opponent;
    public AttackType AttackType { get; set; }

    public PlayerAttacking(Player player, Player opponent, AttackType attackType) : base(player)
    {
        this.Opponent = opponent;
        this.AttackType = attackType;
    }    
}
