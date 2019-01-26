public class CurrentPlayerHealth
{
    public int PlayerID { get; set; }
    public int Health { get; set; }

    public CurrentPlayerHealth(int playerID, int health)
    {
        this.PlayerID = playerID;
        this.Health = health;
    }
}