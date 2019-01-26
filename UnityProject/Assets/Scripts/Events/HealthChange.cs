public class HealthChange
{
    public int playerID { get; set; }
    public int Amount { get; set; }

    public HealthChange(int playerID, int amount)
    {
        this.playerID = playerID;
        this.Amount = amount;
    }
}
