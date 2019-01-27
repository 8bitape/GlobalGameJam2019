public class CurrentPlayerIsBlocking : PlayerEvent
{
    public bool IsBlocking { get; set; }

    public CurrentPlayerIsBlocking(Player player, bool isBlocking) : base(player)
    {
        this.IsBlocking = isBlocking;
    }
}