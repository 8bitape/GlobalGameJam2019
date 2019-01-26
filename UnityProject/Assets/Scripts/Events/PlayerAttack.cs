public class PlayerAttack
{
    public enum AttackType { LightPunch, HeavyPunch, LightKick, HeavyKick }

    public int JoystickID { get; set; }
    public AttackType attackType { get; set; }

    public PlayerAttack(int joystickID, AttackType attackType)
    {
        this.JoystickID = joystickID;
        this.attackType = attackType;
    }
}