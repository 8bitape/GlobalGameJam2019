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
    private readonly string LEFT_PUNCH = "LeftPunch";

    public void Init(Player player)
    {
        this.PlayerId = player.Id;

        this.Animator = this.GetComponent<Animator>();

        if (this.Animator != null)
        {
            PubSub.GetEvent<PlayerMoved>().Where(e => e.PlayerID == this.PlayerId).Subscribe(this.PlayerMoved);
            PubSub.GetEvent<PlayerAttack>().Where(e => e.JoystickID == this.PlayerId).Subscribe(this.PlayerAttack);
        }
    }

    private void PlayerMoved(PlayerMoved playerMoved)
    {
        this.Animator.SetInteger(SPEED, playerMoved.Value);
    }

    private void PlayerAttack(PlayerAttack playerAttack)
    {
        this.Animator.SetTrigger(LEFT_PUNCH);
    }
}
