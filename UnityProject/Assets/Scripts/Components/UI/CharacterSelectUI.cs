using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRxEventAggregator.Events;
using Events;
using TMPro;

public class CharacterSelectUI : PubSubMonoBehaviour
{
    public Color player1Color;
    public Color player2Color;

    private Canvas canvas;
    private Transform portraits;
    private int characterCount;

    private const int PLAYER_1_ID = 1;
    private const int PLAYER_2_ID = 2;

    private const int PLAYER_1_INDEX = 0;
    private const int PLAYER_2_INDEX = 1;

    private int[] playerSelections = new int[] { 0, 1 };
    private bool[] playerReady = new bool[] { false, false };

    [SerializeField]
    private InputManager.AxisSet[] AxisSets;

    [SerializeField]
    private InputManager.InputSet[] InputSets;

    private bool player1StickDown = false;
    private bool player2StickDown = false;


    private int GetPlayerIndexFromID(int playerID)
    {
        return playerID - 1;
    }

    private Color GetPlayerColor(int playerID)
    {
        return (playerID == 1 ? player1Color : player2Color);
    }

    private void Start()
    {
        this.canvas = GetComponent<Canvas>();
        this.portraits = transform.GetChild(0);
        characterCount = portraits.childCount;
        for (int i=0; i<characterCount; i++)
        {
            ClearPortraitSelection(i);
        }
        SelectPortrait(PLAYER_1_ID, playerSelections[PLAYER_1_INDEX]);
        SelectPortrait(PLAYER_2_ID, playerSelections[PLAYER_2_INDEX]);
    }

    void Update()
    {
        if (playerReady[PLAYER_1_INDEX] && playerReady[PLAYER_2_INDEX])
        {
            return;
        }

        // DEBUG Player input emulation
        if (!playerReady[PLAYER_1_INDEX])
        {
            if (Input.GetAxis(this.AxisSets[0].HorizontalAxis) < 0.0f && !this.player1StickDown)
            {
                MoveSelection(PLAYER_1_ID, -1);
                this.player1StickDown = true;
            }
            else if (Input.GetAxis(this.AxisSets[0].HorizontalAxis) > 0.0f && !this.player1StickDown)
            {
                MoveSelection(PLAYER_1_ID, 1);
                this.player1StickDown = true;
            }
            else if (Input.GetAxis(this.AxisSets[0].HorizontalAxis) == 0.0f && this.player1StickDown)
            {
                this.player1StickDown = false;
            }

            if ((Input.GetButtonDown(this.InputSets[0].LightPunch)) || (Input.GetButtonDown(this.InputSets[0].LightKick)) || (Input.GetButtonDown(this.InputSets[0].HeavyPunch)) || (Input.GetButtonDown(this.InputSets[0].HeavyKick)))
            {
                LockInSelection(PLAYER_1_ID);
            }
        }
        else
        {
            if ((Input.GetButtonDown(this.InputSets[0].LightPunch)) || (Input.GetButtonDown(this.InputSets[0].LightKick)) || (Input.GetButtonDown(this.InputSets[0].HeavyPunch)) || (Input.GetButtonDown(this.InputSets[0].HeavyKick)))
            {
                UnlockSelection(PLAYER_1_ID);
            }
        }

        if (!playerReady[PLAYER_2_INDEX])
        {
            if(Input.GetAxis(this.AxisSets[1].HorizontalAxis) < 0.0f && !this.player2StickDown)
            {
                MoveSelection(PLAYER_2_ID, -1);
                player2StickDown = true;
            }
            else if(Input.GetAxis(this.AxisSets[1].HorizontalAxis) > 0.0f && !this.player2StickDown)
            {
                MoveSelection(PLAYER_2_ID, 1);
                player2StickDown = true;
            }
            else if(Input.GetAxis(this.AxisSets[1].HorizontalAxis) == 0.0f && this.player2StickDown)
            {
                this.player2StickDown = false;
            }

            if ((Input.GetButtonDown(this.InputSets[1].LightPunch)) || (Input.GetButtonDown(this.InputSets[1].LightKick)) || (Input.GetButtonDown(this.InputSets[1].HeavyPunch)) || (Input.GetButtonDown(this.InputSets[1].HeavyKick)))
            {
                LockInSelection(PLAYER_2_ID);
            }
        }
        else
        {
            if ((Input.GetButtonDown(this.InputSets[1].LightPunch)) || (Input.GetButtonDown(this.InputSets[1].LightKick)) || (Input.GetButtonDown(this.InputSets[1].HeavyPunch)) || (Input.GetButtonDown(this.InputSets[1].HeavyKick)))
            {
                UnlockSelection(PLAYER_2_ID);
            }
        }

        if (playerReady[PLAYER_1_INDEX] && playerReady[PLAYER_2_INDEX])
        {
            StartCoroutine(EndCharacterSelect());
        }
    }

    private IEnumerator EndCharacterSelect()
    {
        yield return new WaitForSeconds(0.25f);
        PubSub.Publish<EndCharacterSelect>(
            new EndCharacterSelect(
                playerSelections[PLAYER_1_INDEX],
                playerSelections[PLAYER_2_INDEX]
            )
        );
    }

    private void LockInSelection(int playerID)
    {
        int playerIndex = GetPlayerIndexFromID(playerID);
        Transform selection = GetSelectionForPlayer(playerID);
        
        // Set the border color for the player
        Image border = selection.GetComponentInChildren<Image>();
        border.color = GetPlayerColor(playerID);

        playerReady[playerIndex] = true;
    }

    private void UnlockSelection(int playerID)
    {
        int playerIndex = GetPlayerIndexFromID(playerID);
        Transform selection = GetSelectionForPlayer(playerID);

        // Set the border color back to black
        Image border = selection.GetComponentInChildren<Image>();
        border.color = new Color(0f, 0f, 0f);

        playerReady[playerIndex] = false;
    }

    private void MoveSelection(int playerID, int delta)
    {
        int playerIndex = GetPlayerIndexFromID(playerID);
        int otherPlayerIndex = (playerIndex == 0 ? 1 : 0);

        ClearPortraitSelection(playerSelections[playerIndex]);
        playerSelections[playerIndex] = (playerSelections[playerIndex] + this.characterCount + delta) % this.characterCount;
        if (playerSelections[playerIndex] == playerSelections[otherPlayerIndex])
        {
            playerSelections[playerIndex] = (playerSelections[playerIndex] + this.characterCount + delta) % this.characterCount;
        }
        SelectPortrait(playerID, playerSelections[playerIndex]);
    }

    private Transform GetSelectionForPlayer(int playerID)
    {
        int playerIndex = GetPlayerIndexFromID(playerID);
        int selectedCharacterID = playerSelections[playerIndex];
        Transform portraitArea = this.portraits.GetChild(selectedCharacterID);
        Transform selection = portraitArea.Find("Selection");
        return selection;
    }

    // TODO: Instead of cold-setting selection/clear use an Animator to
    // trigger selection/clear/ready animations

    private void SelectPortrait(int playerID, int characterID)
    {
        Transform portraitArea = this.portraits.GetChild(characterID);
        Image portraitImage = portraitArea.Find("CharacterPortrait").GetComponent<Image>();
        Color newPortraitColor = portraitImage.color;
        newPortraitColor.a = 1f;
        portraitImage.color = newPortraitColor;

        Transform selection = portraitArea.Find("Selection");

        // Set the border color to black
        Image border = selection.GetComponentInChildren<Image>();
        border.color = new Color(0f, 0f, 0f);

        // Set the player number text color
        TextMeshProUGUI playerNumber = selection.GetComponentInChildren<TextMeshProUGUI>();
        playerNumber.color = GetPlayerColor(playerID);
        playerNumber.SetText("P" + playerID);

        selection.gameObject.SetActive(true);
    }

    private void ClearPortraitSelection(int characterID)
    {
        Transform portraitArea = this.portraits.GetChild(characterID);

        Image portraitImage = portraitArea.Find("CharacterPortrait").GetComponent<Image>();
        Color newPortraitColor = portraitImage.color;
        newPortraitColor.a = 0.60f;
        portraitImage.color = newPortraitColor;

        Transform selection = portraitArea.Find("Selection");
        selection.gameObject.SetActive(false);
    }
}
