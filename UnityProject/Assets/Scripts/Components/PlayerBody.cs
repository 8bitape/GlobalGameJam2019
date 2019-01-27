using Events;
using UniRx;
using UniRxEventAggregator.Events;
using UnityEngine;

public class Body : PubSubMonoBehaviour
{
    private Player Player { get; set; }   
    
    private bool IsBlocking { get; set; }

    private GameObject HitImpact { get; set; }

    private GameObject BlockImpact { get; set; }

    private readonly float EFFECT_DURATION = 0.25f;

    public void Init(Player player)
    {
        this.Player = player;

        this.gameObject.layer = LayerMask.NameToLayer("Body");

        this.HitImpact = Resources.Load("Prefabs/HitImpact") as GameObject;
        this.BlockImpact = Resources.Load("Prefabs/BlockImpact") as GameObject;

        PubSub.GetEvent<PlayerAttacking>().Where(e => e.Opponent == this.Player).Subscribe(this.PlayerHit);

        PubSub.GetEvent<CurrentPlayerIsBlocking>().Where(e => e.Player == this.Player).Subscribe(this.CurrentPlayerIsBlocking);
    }

    private void PlayerHit(PlayerAttacking playerAttacking)
    {
        Debug.Log(string.Format("{0} was hit by {1}", this.Player, playerAttacking.Player));

        if (this.IsBlocking && this.Player.Chest != null)
        {
            var blockImpact = GameObject.Instantiate(
                this.BlockImpact, 
                this.Player.Chest.transform.position + this.BlockImpact.transform.position, 
                Quaternion.identity);

            GameObject.Destroy(blockImpact, this.EFFECT_DURATION);
        }
        else if (this.Player.Chest != null)
        {
            var hitImpact = GameObject.Instantiate(this.HitImpact, 
                this.Player.Chest.transform.position + this.HitImpact.transform.position,
                Quaternion.identity);

            GameObject.Destroy(hitImpact, this.EFFECT_DURATION);
        }

        PubSub.Publish(new PlayerHit(this.Player.Id, playerAttacking.AttackType, this.IsBlocking));
    }

    private void CurrentPlayerIsBlocking(CurrentPlayerIsBlocking currentPlayerIsBlocking)
    {
        this.IsBlocking = currentPlayerIsBlocking.IsBlocking;
    }
}
