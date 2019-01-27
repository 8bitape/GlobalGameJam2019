using UnityEngine;

public class CurrentPlayerOpponent : PlayerEvent
{
    public Player Opponent { get; set; }

    public CurrentPlayerOpponent(Player player, Player opponent) : base(player)
    {
        this.Opponent = opponent;
    }
}
