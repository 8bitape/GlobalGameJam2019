namespace Events
{
    public class PlayerJumpEnd
    {
        public int PlayerID { get; set; }

        public PlayerJumpEnd(int playerID)
        {
            this.PlayerID = playerID;
        }
    }
}
