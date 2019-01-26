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

    [SerializeField]
    private MovementDirection startingMovementDirection;

    private MovementDirection currentMovementDirection = MovementDirection.None;

    private Rigidbody Body;

    [SerializeField]
    private GameObject OpponentObj;

    private void Awake()
    {
        this.Body = this.GetComponent<Rigidbody>();

        this.currentMovementDirection = this.startingMovementDirection;

        if(this.Body != null)
        {
            PubSub.GetEvent<PlayerMove>().Where(e => e.JoystickID == this.playerID).Subscribe(this.Move);
            PubSub.GetEvent<PlayerJump>().Where(e => e.JoystickID == this.playerID).Subscribe(this.Jump);
            PubSub.GetEvent<PlayerAttack>().Where(e => e.JoystickID == this.playerID).Subscribe(this.Attack);

            PubSub.GetEvent<PlayerSpawned>().Where(e => e.PlayerID != this.playerID).Subscribe(this.RegisterOpponent);
        }
    }

    private void Start()
    {
        PubSub.Publish<PlayerSpawned>(new PlayerSpawned(this.playerID, this.gameObject));
    }

    private void Update()
    {
        if (this.OpponentObj != null)
        {
            var playerXPos = this.transform.position.x;
            var opponentXPos = this.OpponentObj.transform.position.x;

            if (playerXPos > opponentXPos && this.currentMovementDirection != MovementDirection.Left)
            {
                this.Flip(this.GetFacingDirection());
            }
            else if (playerXPos < opponentXPos && this.currentMovementDirection != MovementDirection.Right)
            {
                this.Flip(this.GetFacingDirection());
            }
        }
    }

    private void RegisterOpponent(PlayerSpawned playerSpawned)
    {
        this.OpponentObj = playerSpawned.Obj;
    }

    private void Move(PlayerMove playerMove)
    {
        if(playerMove.MoveVector.x > 0.0f)
        {
            if (this.currentMovementDirection != MovementDirection.Right && !this.isJumping)
            {
                this.Body.velocity = Vector3.zero;
            }

            this.currentMovementDirection = this.GetFacingDirection();

            this.Body.velocity = new Vector3(playerMove.MoveVector.x * this.moveSpeed * Time.deltaTime, this.Body.velocity.y, 0.0f);
        }
        else if(playerMove.MoveVector.x < 0.0f)
        {
            if (this.currentMovementDirection != MovementDirection.Left && !this.isJumping)
            {
                this.Body.velocity = Vector3.zero;
            }

            this.currentMovementDirection = this.GetFacingDirection();

            this.Body.velocity = new Vector3(playerMove.MoveVector.x * this.moveSpeed * Time.deltaTime, this.Body.velocity.y, 0.0f);
        }
        else if(playerMove.MoveVector == Vector3.zero && !this.isJumping)
        {
            this.Body.velocity = Vector3.zero;
        }
    }

    private MovementDirection GetFacingDirection()
    {
        var playerXPos = this.transform.position.x;
        var opponentXPos = this.OpponentObj.transform.position.x;

        if (playerXPos > opponentXPos)
        {
            return MovementDirection.Left;
        }
        else if (playerXPos < opponentXPos)
        {
            return MovementDirection.Right;
        }
        else
        {
            return MovementDirection.None;
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

            PubSub.Publish<PlayerJumpStart>(new PlayerJumpStart(this.playerID));
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

    private void Flip(MovementDirection direction)
    {
        this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        this.currentMovementDirection = direction;
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

                    PubSub.Publish<PlayerJumpEnd>(new PlayerJumpEnd(this.playerID));
                }
                break;
        }
    }
}
