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

    private void Awake()
    {
        this.gameObject.AddComponent<PlayerHealth>().Init(this);
        this.gameObject.AddComponent<PlayerOpponent>().Init(this);
        this.gameObject.AddComponent<PlayerMovement>().Init(this);
        this.gameObject.AddComponent<Body>().Init(this);
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
