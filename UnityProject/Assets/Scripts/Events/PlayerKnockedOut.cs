namespace Events
{
    public class PlayerKnockedOut
    {
        public int PlayerID { get; set; }

        public PlayerKnockedOut(int playerID)
        {
            this.PlayerID = playerID;
        }
    }
}
