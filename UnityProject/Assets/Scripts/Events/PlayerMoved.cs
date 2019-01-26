namespace Events
{
    public class PlayerMoved
    {
        public int Value { get; set; }
        public int PlayerID { get; set; }

        public PlayerMoved(int playerID, int value)
        {
            this.PlayerID = playerID;
            this.Value = value;
        }
    }
}
