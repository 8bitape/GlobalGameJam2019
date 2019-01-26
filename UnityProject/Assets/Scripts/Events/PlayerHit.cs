namespace Events
{
    public class PlayerHit
    {
        public int PlayerID { get; set; }
        public AttackType AttackType { get; set; }
        public bool Blocked { get; set; }

        public PlayerHit(int playerID, AttackType attackType, bool blocked)
        {
            this.PlayerID = playerID;
            this.AttackType = attackType;
            this.Blocked = blocked;
        }
    }
}