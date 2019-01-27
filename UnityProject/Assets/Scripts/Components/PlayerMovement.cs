using UnityEngine;
using UniRxEventAggregator.Events;
using UniRx;
using Events;

public class PlayerMovement : PubSubMonoBehaviour
{
    private Player Player { get; set; }

    private Player CurrentOpponent { get; set; }

    private bool CanJump { get; set; }

    private bool IsJumping { get; set; }

    private BehaviorSubject<CurrentPlayerIsBlocking> IsBlocking { get; set; }

    private MovementDirection CurrentMovementDirection { get; set; }

    private Rigidbody Rigidbody { get; set; }

    private bool IsCameraBoundsActive { get; set; }
    
    private readonly float CAMERA_BOUNDS_PADDING = 0.75f;

    private bool canReceiveInput = false;

    public void Init(Player player)
    {
        this.Player = player;
        this.CurrentMovementDirection = this.Player.StartingMovementDirection;

        this.CanJump = true;

        this.IsBlocking = new BehaviorSubject<CurrentPlayerIsBlocking>(new CurrentPlayerIsBlocking(this.Player, false));

        this.Register(this.IsBlocking);

        this.Rigidbody = this.GetComponent<Rigidbody>();

        if (this.Rigidbody != null)
        {
            PubSub.GetEvent<PlayerMove>().Where(e => e.JoystickID == this.Player.Id).Subscribe(this.Move);
            PubSub.GetEvent<PlayerJump>().Where(e => e.JoystickID == this.Player.Id).Subscribe(this.Jump);
        }

        PubSub.GetEvent<CurrentPlayerOpponent>().Where(e => e.Player == this.Player).Subscribe(e => this.CurrentOpponent = e.Opponent);

        this.Subscribe<FightOver>(e => this.EnableCameraBounds(false));
        this.Subscribe<PlayersSpawned>(e => this.EnableCameraBounds(true));

        this.Subscribe<FightOver>(e => this.EnableInput(false));
        this.Subscribe<RoundStarted>(e => this.EnableInput(true));
    }

    private void Update()
    {
        this.ClampPositionWithinCameraBounds();

        if (this.CurrentOpponent != null)
        {
            var playerXPos = this.transform.position.x;
            var opponentXPos = this.CurrentOpponent.gameObject.transform.position.x;

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

    private void EnableInput(bool isEnabled)
    {
        this.canReceiveInput = isEnabled;
    }

    private void EnableCameraBounds(bool isEnabled)
    {
        this.IsCameraBoundsActive = isEnabled;
    }

    private void ClampPositionWithinCameraBounds()
    {
        if (!this.IsCameraBoundsActive)
        {
            return;
        }

        var dist = (this.transform.position - Camera.main.transform.position).z;

        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        this.transform.position = new Vector3(
        Mathf.Clamp(this.transform.position.x, leftBorder + this.CAMERA_BOUNDS_PADDING / 2, rightBorder - this.CAMERA_BOUNDS_PADDING / 2),
        Mathf.Clamp(this.transform.position.y, topBorder + this.CAMERA_BOUNDS_PADDING / 2, bottomBorder - this.CAMERA_BOUNDS_PADDING / 2),
        this.transform.position.z);
    }

    private void Move(PlayerMove playerMove)
    {
        if(!this.canReceiveInput)
        {
            return;
        }

        if (playerMove.MoveVector.x > 0.0f)
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
                this.IsBlocking.OnNext(new CurrentPlayerIsBlocking(this.Player, false));

                PubSub.Publish<PlayerMoved>(new PlayerMoved(this.Player.Id, 1));

                this.Rigidbody.velocity = new Vector3(playerMove.MoveVector.x * this.Player.MoveSpeed * Time.deltaTime, this.Rigidbody.velocity.y, 0.0f);
            }
            else if(this.CurrentMovementDirection == MovementDirection.Left)
            {
                this.IsBlocking.OnNext(new CurrentPlayerIsBlocking(this.Player, true));

                PubSub.Publish<PlayerMoved>(new PlayerMoved(this.Player.Id, -1));

                this.Rigidbody.velocity = new Vector3(playerMove.MoveVector.x * (this.Player.MoveSpeed /2) * Time.deltaTime, this.Rigidbody.velocity.y, 0.0f);
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
                this.IsBlocking.OnNext(new CurrentPlayerIsBlocking(this.Player, true));

                PubSub.Publish<PlayerMoved>(new PlayerMoved(this.Player.Id, -1));

                this.Rigidbody.velocity = new Vector3(playerMove.MoveVector.x * (this.Player.MoveSpeed/2) * Time.deltaTime, this.Rigidbody.velocity.y, 0.0f);
            }
            else if (this.CurrentMovementDirection == MovementDirection.Left)
            {
                this.IsBlocking.OnNext(new CurrentPlayerIsBlocking(this.Player, false));

                PubSub.Publish<PlayerMoved>(new PlayerMoved(this.Player.Id, 1));

                this.Rigidbody.velocity = new Vector3(playerMove.MoveVector.x * this.Player.MoveSpeed * Time.deltaTime, this.Rigidbody.velocity.y, 0.0f);
            }
        }
        else if(playerMove.MoveVector == Vector3.zero && !this.IsJumping)
        {
            this.IsBlocking.OnNext(new CurrentPlayerIsBlocking(this.Player, false));

            this.Rigidbody.velocity = Vector3.zero;
            PubSub.Publish<PlayerMoved>(new PlayerMoved(this.Player.Id, 0));
        }
    }

    private MovementDirection GetFacingDirection()
    {
        if (this.CurrentOpponent != null)
        {
            var playerXPos = this.transform.position.x;
            var opponentXPos = this.CurrentOpponent.gameObject.transform.position.x;

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
        if(!this.canReceiveInput)
        {
            return;
        }

        if (this.CanJump)
        {
            this.Rigidbody.velocity = Vector3.zero;
            this.Rigidbody.AddForce(new Vector3(0.0f, this.Player.JumpForce, 0.0f), ForceMode.Impulse);
            this.CanJump = false;
            this.IsJumping = true;

            PubSub.Publish<PlayerJumpStart>(new PlayerJumpStart(this.Player.Id));
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

                    PubSub.Publish<PlayerJumpEnd>(new PlayerJumpEnd(this.Player.Id));
                }
                break;
        }
    }
}
