using UnityEngine;

namespace Events
{
    public class PlayerMove
    {
        public int JoystickID { get; set; }
        public Vector3 MoveVector { get; set; }

        public PlayerMove(int joystickID, Vector3 moveVector)
        {
            this.JoystickID = joystickID;
            this.MoveVector = moveVector;
        }
    }
}