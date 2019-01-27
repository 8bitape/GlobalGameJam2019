using UnityEngine;
using UniRxEventAggregator.Events;
using Events;

public class Player : PubSubMonoBehaviour
{
    [SerializeField]
    private int id;
    public int Id { get { return this.id; } }

    [Space(10)]
    [SerializeField]
    private int maxHealth = 100;
    public int MaxHealth { get { return this.maxHealth; } }

    [Space(10)]
    [SerializeField]
    private float moveSpeed = 100;
    public float MoveSpeed { get { return this.moveSpeed; } }

    [SerializeField]
    private float jumpForce = 9;
    public float JumpForce { get { return this.jumpForce; } }

    [SerializeField]
    private MovementDirection startingMovementDirection = MovementDirection.None;
    public MovementDirection StartingMovementDirection { get { return this.startingMovementDirection; } }

    private Transform RightHand { get; set; }
    public Transform Chest { get; private set; }

    private PlayerHealth PlayerHealth { get; set; }
    private PlayerOpponent PlayerOpponent { get; set; }
    private PlayerMovement PlayerMovement { get; set; }
    private Body Body { get; set; }

    public void Reset()
    {
        this.PlayerHealth.Init(this);
        this.PlayerMovement.Init(this);
        this.PlayerOpponent.Init(this);
        this.Body.Init(this);
    }

    private void Awake()
    {
        this.PlayerHealth = this.gameObject.AddComponent<PlayerHealth>();
        this.PlayerHealth.Init(this);

        this.PlayerOpponent = this.gameObject.AddComponent<PlayerOpponent>();
        this.PlayerOpponent.Init(this);

        this.PlayerMovement = this.gameObject.AddComponent<PlayerMovement>();
        this.PlayerMovement.Init(this);

        this.Body = this.gameObject.AddComponent<Body>();
        this.Body.Init(this);

        this.gameObject.AddComponent<AnimatorController>().Init(this);

        this.Chest = this.gameObject.transform.Find("Actor/Armature/Root/Chest");
        this.RightHand = this.gameObject.transform.Find("Actor/Armature/Root/Chest/UpperArm.R/LowerArm.R/Hand.R");

        if (this.RightHand != null)
        {
            this.RightHand.gameObject.AddComponent<PlayerLimb>().Init(this, AttackType.LightPunch);
        }
    }

    private void Start()
    {
        PubSub.Publish(new PlayerSpawned(this));
    }
}
