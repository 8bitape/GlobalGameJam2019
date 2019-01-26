using UnityEngine;
using UniRxEventAggregator.Events;
using UniRx;
using Events;

public class PlayerMovement : PubSubMonoBehaviour
{
    private int PlayerId { get; set; }

    private float MoveSpeed { get; set; }

    private float JumpForce { get; set; }

    private bool CanJump { get; set; }

    private bool IsJumping { get; set; }

    private bool IsBlocking { get; set; }

    private MovementDirection StartingMovementDirection { get; set; }

    private MovementDirection CurrentMovementDirection { get; set; }

    private Rigidbody Rigidbody { get; set; }

    private GameObject OpponentObj { get; set; }

    public void Init(Player player)
    {
        this.PlayerId = player.Id;
        this.MoveSpeed = player.MoveSpeed;
        this.JumpForce = player.JumpForce;
        this.StartingMovementDirection = player.StartingMovementDirection;
        this.CurrentMovementDirection = this.StartingMovementDirection;
    }

    private void Awake()
    {
        this.CanJump = true;

        this.Rigidbody = this.GetComponent<Rigidbody>();        

        if(this.Rigidbody != null)
        {
            PubSub.GetEvent<PlayerMove>().Where(e => e.JoystickID == this.PlayerId).Subscribe(this.Move);
            PubSub.GetEvent<PlayerJump>().Where(e => e.JoystickID == this.PlayerId).Subscribe(this.Jump);
            PubSub.GetEvent<PlayerAttack>().Where(e => e.JoystickID == this.PlayerId).Subscribe(this.Attack);
            PubSub.GetEvent<PlayerSpawned>().Where(e => e.PlayerID != this.PlayerId).Subscribe(this.RegisterOpponent);
        }
    }

    private void Update()
    {
        if (this.OpponentObj != null)
        {
            var playerXPos = this.transform.position.x;
            var opponentXPos = this.OpponentObj.transform.position.x;

            if (playerXPos > opponentXPos && this.CurrentMovementDirection != MovementDirection.Left)
            {
                this.Flip(this.GetFacingDirection());
            }
            else if (playerXPos < opponentXPos && this.CurrentMovementDirection != MovementDirection.Right)
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
            if (this.CurrentMovementDirection != MovementDirection.Right && !this.IsJumping)
            {
                this.Rigidbody.velocity = Vector3.zero;
            }

            var newFacingDirection = this.GetFacingDirection();

            if (this.CurrentMovementDirection != newFacingDirection)
            {
                this.Flip(newFacingDirection);
            }

            if (this.CurrentMovementDirection == MovementDirection.Right)
            {
                this.IsBlocking = false;

                PubSub.Publish<PlayerMoved>(new PlayerMoved(this.PlayerId, 1));

                this.Rigidbody.velocity = new Vector3(playerMove.MoveVector.x * this.MoveSpeed * Time.deltaTime, this.Rigidbody.velocity.y, 0.0f);
            }
            else if(this.CurrentMovementDirection == MovementDirection.Left)
            {
                this.IsBlocking = true;

                PubSub.Publish<PlayerMoved>(new PlayerMoved(this.PlayerId, -1));

                this.Rigidbody.velocity = new Vector3(playerMove.MoveVector.x * (this.MoveSpeed /2) * Time.deltaTime, this.Rigidbody.velocity.y, 0.0f);
            }
        }
        else if(playerMove.MoveVector.x < 0.0f)
        { 
            if (this.CurrentMovementDirection != MovementDirection.Left && !this.IsJumping)
            {
                this.Rigidbody.velocity = Vector3.zero;
            }

            var newFacingDirection = this.GetFacingDirection();

            if(this.CurrentMovementDirection != newFacingDirection)
            {
                this.Flip(newFacingDirection);
            }

            // Publish an event to indicate the direction the player has moved (forwards/backwards)
            if (this.CurrentMovementDirection == MovementDirection.Right)
            {
                this.IsBlocking = true;

                PubSub.Publish<PlayerMoved>(new PlayerMoved(this.PlayerId, -1));

                this.Rigidbody.velocity = new Vector3(playerMove.MoveVector.x * (this.MoveSpeed/2) * Time.deltaTime, this.Rigidbody.velocity.y, 0.0f);
            }
            else if (this.CurrentMovementDirection == MovementDirection.Left)
            {
                this.IsBlocking = false;

                PubSub.Publish<PlayerMoved>(new PlayerMoved(this.PlayerId, 1));

                this.Rigidbody.velocity = new Vector3(playerMove.MoveVector.x * this.MoveSpeed * Time.deltaTime, this.Rigidbody.velocity.y, 0.0f);
            }
        }
        else if(playerMove.MoveVector == Vector3.zero && !this.IsJumping)
        {
            this.IsBlocking = false;

            this.Rigidbody.velocity = Vector3.zero;
            PubSub.Publish<PlayerMoved>(new PlayerMoved(this.PlayerId, 0));
        }
    }

    private MovementDirection GetFacingDirection()
    {
        if (this.OpponentObj != null)
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
        }

        return MovementDirection.None;
    }

    private void Jump(PlayerJump playerJump)
    {
        if(this.CanJump)
        {
            this.Rigidbody.velocity = Vector3.zero;
            this.Rigidbody.AddForce(new Vector3(0.0f, this.JumpForce, 0.0f), ForceMode.Impulse);
            this.CanJump = false;
            this.IsJumping = true;

            PubSub.Publish<PlayerJumpStart>(new PlayerJumpStart(this.PlayerId));
        }
    }

    private void Attack(PlayerAttack playerAttack)
    {
        var opponentID = this.OpponentObj.GetComponent<PlayerMovement>().PlayerId;

        switch(playerAttack.attackType)
        {
            case AttackType.LightPunch:
                PubSub.Publish<PlayerHit>(new PlayerHit(opponentID, AttackType.LightPunch, false));
                break;

            case AttackType.HeavyPunch:
                PubSub.Publish<PlayerHit>(new PlayerHit(opponentID, AttackType.HeavyPunch, false));
                break;

            case AttackType.LightKick:
                PubSub.Publish<PlayerHit>(new PlayerHit(opponentID, AttackType.LightKick, false));
                break;

            case AttackType.HeavyKick:
                PubSub.Publish<PlayerHit>(new PlayerHit(opponentID, AttackType.HeavyKick, false));
                break;
        }
    }

    private void Flip(MovementDirection direction)
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, -this.transform.localScale.z);
        this.CurrentMovementDirection = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.collider.tag)
        {
            case "Floor":
                if(!this.CanJump && this.IsJumping)
                {
                    this.CanJump = true;
                    this.IsJumping = false;

                    PubSub.Publish<PlayerJumpEnd>(new PlayerJumpEnd(this.PlayerId));
                }
                break;
        }
    }
}
