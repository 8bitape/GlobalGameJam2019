using Events;
using UniRx;
using UniRxEventAggregator.Events;
using UnityEngine;

public class Body : PubSubMonoBehaviour
{
    private Player Player { get; set; }   
    
    private bool IsBlocking { get; set; }

    public void Init(Player player)
    {
        this.Player = player;

        this.gameObject.layer = LayerMask.NameToLayer("Body");

        PubSub.GetEvent<PlayerAttacking>().Where(e => e.Opponent == this.Player).Subscribe(this.PlayerHit);

        PubSub.GetEvent<CurrentPlayerIsBlocking>().Where(e => e.Player == this.Player).Subscribe(this.CurrentPlayerIsBlocking);
    }

    private void PlayerHit(PlayerAttacking playerAttacking)
    {
        Debug.Log(string.Format("{0} was hit by {1}", this.Player, playerAttacking.Player));

        PubSub.Publish(new PlayerHit(this.Player.Id, playerAttacking.AttackType, this.IsBlocking));
    }

    private void CurrentPlayerIsBlocking(CurrentPlayerIsBlocking currentPlayerIsBlocking)
    {
        this.IsBlocking = currentPlayerIsBlocking.IsBlocking;
    }
}
