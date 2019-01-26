using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRxEventAggregator.Events;
using Events;

public class GameManager : PubSubMonoBehaviour
{
    private GameObject PlayerPrefab;

    private void Awake()
    {
        this.PlayerPrefab = (GameObject)Resources.Load("Prefabs/Player");

        this.Subscribe<TimeRanOut>(this.TimeRanOut);
        this.Subscribe<PlayerKnockedOut>(this.PlayerKnockedOut);
        this.Subscribe<GameOver>(this.GameOver);
        this.Subscribe<EndCharacterSelect>(this.EndCharacterSelect);
    }

    private void Start()
    {
        PubSub.Publish<StartCharacterSelect>(new StartCharacterSelect());

        PubSub.Publish<EndCharacterSelect>(new EndCharacterSelect(0, 1));
    }

    private void EndCharacterSelect(EndCharacterSelect endCharacterSelect)
    {
        //var playerOneSpawnPos = new Vector3(-2.0f, 0.0f, 0.0f);
        //var playerTwoSpawnPos = new Vector3(2.0f, 0.0f, 0.0f);

        //var playerOne = (GameObject)GameObject.Instantiate(this.PlayerPrefab);
        //playerOne.GetComponent<Player>().Id = 1;
        //playerOne.GetComponent<Player>().characterId = endCharacterSelect.PlayerOneSelectedCharacter;

        //var playerTwo = (GameObject)GameObject.Instantiate(this.PlayerPrefab);
        //playerTwo.GetComponent<Player>().Id = 2;
        //playerTwo.GetComponent<Player>().characterId = endCharacterSelect.PlayerTwoSelectedCharacter;

        //PubSub.Publish<PlayerInitialised>(new PlayerInitialised(1, endCharacterSelect.PlayerOneSelectedCharacter, PlayerMovement.MovementDirection.Right, playerOneSpawnPos, "Player1"));
        //PubSub.Publish<PlayerInitialised>(new PlayerInitialised(2, endCharacterSelect.PlayerTwoSelectedCharacter, PlayerMovement.MovementDirection.Left, playerTwoSpawnPos, "Player2"));

    }

    private void TimeRanOut(TimeRanOut timeRanOut)
    {

    }

    private void PlayerKnockedOut(PlayerKnockedOut playerKnockedOut)
    {

    }

    private void GameOver(GameOver gameOver)
    { 
    
    }
}
