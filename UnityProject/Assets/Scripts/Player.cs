using UnityEngine;
using UniRxEventAggregator.Events;
using Events;
using UniRx;

public class Player : PubSubMonoBehaviour
{
    [SerializeField]
    private int id;
    public int Id { get { return this.id; } set { this.id = value; } }

    private int CharacterId;
    public int characterId { get { return this.CharacterId; } set { this.CharacterId = value; } }

    private void Awake()
    {
        this.gameObject.AddComponent<AnimatorController>().Init(this);
        //this.gameObject.AddComponent<attack_audio>().Init(this.Id, this.characterId);
        //this.gameObject.AddComponent<fighter_audio>().Init(this.id, this.characterId);
    }
}