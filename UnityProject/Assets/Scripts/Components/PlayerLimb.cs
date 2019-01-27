using Events;
using UniRxEventAggregator.Events;
using UniRx;
using UnityEngine;
using System.Collections;

public class PlayerLimb : PubSubMonoBehaviour
{
    private Player Player { get; set; }

    private Player CurrentOpponent { get; set; }
    
    private MeshCollider MeshCollider { get; set; }

    private Rigidbody Rigidbody { get; set; }

    private AttackType AttackType { get; set; }

    public void Init(Player player, AttackType attackType)
    {
        this.Player = player;
        this.AttackType = attackType;    
        this.MeshCollider = this.gameObject.AddComponent<MeshCollider>();
        this.Rigidbody = this.gameObject.AddComponent<Rigidbody>();

        this.gameObject.layer = LayerMask.NameToLayer("Limb");

        this.InitMeshCollider();
        this.InitRigidBody();

        PubSub.GetEvent<PlayerAttack>().Where(e => e.JoystickID == this.Player.Id).Subscribe(this.Attack);

        PubSub.GetEvent<CurrentPlayerOpponent>().Where(e => e.Player == this.Player).Subscribe(e => this.CurrentOpponent = e.Opponent);
    }

    private void InitRigidBody()
    {
        this.Rigidbody.isKinematic = true;
        this.Rigidbody.useGravity = false;
    }

    private void InitMeshCollider()
    {
        var primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var meshFilter = primitive.GetComponent<MeshFilter>();

        if (meshFilter != null)
        {
            this.MeshCollider.sharedMesh = meshFilter.sharedMesh;
            this.MeshCollider.convex = true;
            this.MeshCollider.isTrigger = true;
            this.MeshCollider.enabled = false;
        }

        GameObject.Destroy(primitive);
    }

    private void Attack(PlayerAttack playerAttack)
    {
        this.AttackType = playerAttack.attackType;

        this.StartCoroutine(this.AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        this.MeshCollider.enabled = true;

        yield return new WaitForSeconds(0.25f);

        this.MeshCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this.Player.gameObject)
        {
            return;
        }

        Debug.Log(string.Format("{0} is attacking {1}", this.Player, this.CurrentOpponent));

        switch (this.AttackType)
        {
            case AttackType.LightPunch:
                PubSub.Publish(new PlayerAttacking(this.Player, this.CurrentOpponent, AttackType.LightPunch));
                break;

            case AttackType.HeavyPunch:
                PubSub.Publish(new PlayerAttacking(this.Player, this.CurrentOpponent, AttackType.HeavyPunch));
                break;

            case AttackType.LightKick:
                PubSub.Publish(new PlayerAttacking(this.Player, this.CurrentOpponent, AttackType.LightKick));
                break;

            case AttackType.HeavyKick:
                PubSub.Publish(new PlayerAttacking(this.Player, this.CurrentOpponent, AttackType.HeavyKick));
                break;
        }
    }
}
