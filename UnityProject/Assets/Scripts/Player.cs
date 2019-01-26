using UnityEngine;
using UniRxEventAggregator.Events;

public class Player : PubSubMonoBehaviour
{
    [SerializeField]
    private int id;
    public int Id { get { return this.id; } }

    private void Awake()
    {
        this.gameObject.AddComponent<AnimatorController>().Init(this);
    }
}
