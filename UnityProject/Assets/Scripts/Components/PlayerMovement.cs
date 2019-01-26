using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRxEventAggregator.Events;
using UniRx;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : PubSubMonoBehaviour
{
    private enum MovementDirection { Left, Right, Up, Down, None }

    [SerializeField]
    private int playerID = 0;

    [SerializeField]
    private float moveSpeed = 0.0f;

    [SerializeField]
    private float jumpForce = 0.0f;

    [SerializeField]
    private bool canJump = true;

    [SerializeField]
    private bool isJumping = false;

    private MovementDirection currentMovementDirection = MovementDirection.None;

    private Rigidbody Body;

    private void Awake()
    {
        this.Body = this.GetComponent<Rigidbody>();

        if(this.Body != null)
        {
            PubSub.GetEvent<PlayerMove>().Where(e => e.JoystickID == this.playerID).Subscribe(this.Move);
            PubSub.GetEvent<PlayerJump>().Where(e => e.JoystickID == this.playerID).Subscribe(this.Jump);
            PubSub.GetEvent<PlayerAttack>().Where(e => e.JoystickID == this.playerID).Subscribe(this.Attack);
        }
    }

    private void Move(PlayerMove playerMove)
    {
        if(playerMove.MoveVector.x > 0.0f)
        {
            if (this.currentMovementDirection != MovementDirection.Right && !this.isJumping)
            {
                this.Body.velocity = Vector3.zero;
            }

            this.currentMovementDirection = MovementDirection.Right;

            this.Body.velocity = new Vector3(playerMove.MoveVector.x * this.moveSpeed * Time.deltaTime, this.Body.velocity.y, 0.0f);
        }
        else if(playerMove.MoveVector.x < 0.0f)
        {
            if (this.currentMovementDirection != MovementDirection.Left && !this.isJumping)
            {
                this.Body.velocity = Vector3.zero;
            }

            this.currentMovementDirection = MovementDirection.Left;

            this.Body.velocity = new Vector3(playerMove.MoveVector.x * this.moveSpeed * Time.deltaTime, this.Body.velocity.y, 0.0f);
        }
        else if(playerMove.MoveVector == Vector3.zero && !this.isJumping)
        {
            this.Body.velocity = Vector3.zero;
            this.currentMovementDirection = MovementDirection.None;
        }
    }

    private void Jump(PlayerJump playerJump)
    {
        if(this.canJump)
        {
            this.Body.velocity = Vector3.zero;
            this.Body.AddForce(new Vector3(0.0f, this.jumpForce, 0.0f), ForceMode.Impulse);
            this.canJump = false;
            this.isJumping = true;
        }
    }

    private void Attack(PlayerAttack playerAttack)
    {
        switch(playerAttack.attackType)
        {
            case PlayerAttack.AttackType.LightPunch:
                break;

            case PlayerAttack.AttackType.HeavyPunch:
                break;

            case PlayerAttack.AttackType.LightKick:
                break;

            case PlayerAttack.AttackType.HeavyKick:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.collider.tag)
        {
            case "Floor":
                if(!this.canJump && this.isJumping)
                {
                    this.canJump = true;
                    this.isJumping = false;
                }
                break;
        }
    }
}
