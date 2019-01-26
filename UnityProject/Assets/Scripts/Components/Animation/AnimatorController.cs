using UnityEngine;
using UniRxEventAggregator;
using UniRxEventAggregator.Events;
using Events;
using UniRx;

public class AnimatorController : PubSubMonoBehaviour
{
    private int PlayerId { get; set; }
    private Animator Animator { get; set; }

    private readonly string SPEED = "Speed";

    public void Init(Player player)
    {
        this.PlayerId = player.Id;
    }

    private void Awake()
    {
        this.Animator = this.GetComponent<Animator>();

        if (this.Animator != null)
        {
            PubSub.GetEvent<PlayerMoved>().Where(e => e.PlayerID == this.PlayerId).Subscribe(this.PlayerMoved);
        }
    }

    private void PlayerMoved(PlayerMoved playerMoved)
    {
        this.Animator.SetInteger(SPEED, playerMoved.Value);
    }
}
