namespace Events
{
    public class PlayerJumpStart
    {
        public int PlayerID { get; set; }

        public PlayerJumpStart(int playerID)
        {
            this.PlayerID = playerID;
        }
    }
}