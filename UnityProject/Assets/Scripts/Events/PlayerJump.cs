using UnityEngine;

namespace Events
{
    public class PlayerJump
    {
        public int JoystickID { get; set; }
        public Vector3 MoveVector { get; set; }

        public PlayerJump(int joystickID, Vector3 moveVector)
        {
            this.JoystickID = joystickID;
            this.MoveVector = moveVector;
        }
    }
}