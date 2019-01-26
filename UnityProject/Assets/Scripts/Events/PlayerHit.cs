namespace Events
{
    public class PlayerHit
    {
        public int PlayerID { get; set; }
        public AttackType AttackType { get; set; }

        public PlayerHit(int playerID, AttackType attackType)
        {
            this.PlayerID = playerID;
            this.AttackType = AttackType;
        }
    }
}