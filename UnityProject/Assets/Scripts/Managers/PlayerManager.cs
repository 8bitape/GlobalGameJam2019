using UnityEngine;
using System.Collections;
using UniRxEventAggregator.Events;
using Events;

public class PlayerManager : PubSubMonoBehaviour
{
    private GameObject Player1Prefab { get; set; }
    private GameObject Player2Prefab { get; set; }

    private readonly Vector3 OFFSCREEN = new Vector3(100, 0, 0);
    private readonly Vector3 PLAYER_1_START_POS = new Vector3(-2, 0, 0);
    private readonly Vector3 PLAYER_2_START_POS = new Vector3(2, 0, 0);

    private void Awake()
    {
        this.Player1Prefab = GameObject.Instantiate(Resources.Load("Prefabs/Player 1") as GameObject);
        this.Player2Prefab = GameObject.Instantiate(Resources.Load("Prefabs/Player 2") as GameObject);

        this.Player1Prefab.transform.position = this.OFFSCREEN;
        this.Player2Prefab.transform.position = this.OFFSCREEN;

        this.Subscribe<FightStart>(this.SpawnPlayers);
        this.Subscribe<FightOver>(this.DespawnPlayers);
    }
    
    private void SpawnPlayers(FightStart fightStart)
    {
        this.Player1Prefab.transform.position = this.PLAYER_1_START_POS;
        this.Player2Prefab.transform.position = this.PLAYER_2_START_POS;

        PubSub.Publish(new PlayersSpawned());
    }

    private void DespawnPlayers(FightOver fightOver)
    {
        this.Player1Prefab.transform.position = this.OFFSCREEN;
        this.Player2Prefab.transform.position = this.OFFSCREEN;

        this.Player1Prefab.GetComponent<Player>().Reset();
        this.Player2Prefab.GetComponent<Player>().Reset();
    }
}
