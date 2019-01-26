using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRxEventAggregator.Events;
using Events;

public class InputManager : PubSubMonoBehaviour
{
    [System.Serializable]
    public class AxisSet
    {
        public int JoystickID = 0;
        public string HorizontalAxis;
        public string VerticalAxis;
    }

    [System.Serializable]
    public class InputSet
    {
        public int JoystickID = 0;
        public string LightPunch;
        public string HeavyPunch;
        public string LightKick;
        public string HeavyKick;
    }

    [SerializeField]
    private AxisSet[] AxisSets;

    [SerializeField]
    private InputSet[] InputSets;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PubSub.Publish<PlayerAttack>(new PlayerAttack(1, AttackType.HeavyPunch));
        }

        // AXIS INPUT
        if (this.AxisSets != null)
        {
            foreach(var axisSet in this.AxisSets)
            {
                var moveVector = new Vector3(Input.GetAxis(axisSet.HorizontalAxis), Input.GetAxis(axisSet.VerticalAxis));

                // MOVEMENT
                if(moveVector == Vector3.zero)
                {
                    PubSub.Publish<PlayerMove>(new PlayerMove(axisSet.JoystickID, Vector3.zero));
                }
                else if (moveVector.x != 0.0f)
                {
                    PubSub.Publish<PlayerMove>(new PlayerMove(axisSet.JoystickID, moveVector));
                }

                // JUMPING
                if (moveVector.y > 0.0f)
                {
                    PubSub.Publish<PlayerJump>(new PlayerJump(axisSet.JoystickID, moveVector));
                }
            }
        }

        // BUTTON INPUT
        if(this.InputSets != null)
        {
            foreach(var inputSet in this.InputSets)
            {
                if (Input.GetButtonDown(inputSet.LightPunch))
                {
                    PubSub.Publish<PlayerAttack>(new PlayerAttack(inputSet.JoystickID, AttackType.LightPunch));
                }

                if (Input.GetButtonDown(inputSet.HeavyPunch))
                {
                    PubSub.Publish<PlayerAttack>(new PlayerAttack(inputSet.JoystickID, AttackType.HeavyPunch));
                }

                if (Input.GetButtonDown(inputSet.LightKick))
                {
                    PubSub.Publish<PlayerAttack>(new PlayerAttack(inputSet.JoystickID, AttackType.LightKick));
                }

                if (Input.GetButtonDown(inputSet.HeavyKick))
                {
                    PubSub.Publish<PlayerAttack>(new PlayerAttack(inputSet.JoystickID, AttackType.HeavyKick));
                }
            }
        }
    }
}
