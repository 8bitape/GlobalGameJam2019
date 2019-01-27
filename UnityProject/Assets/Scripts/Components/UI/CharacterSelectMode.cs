using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRxEventAggregator.Events;
using Events;

public class CharacterSelectMode : PubSubMonoBehaviour
{
    private GameObject characterSelectUI;

    void Awake()
    {
        this.characterSelectUI = transform.GetChild(0).gameObject;

        this.Subscribe<StartCharacterSelect>(e => this.EnableUI(true));
        this.Subscribe<EndCharacterSelect>(e => this.EnableUI(false));
    }

    private void EnableUI(bool isEnabled)
    {
        characterSelectUI.SetActive(isEnabled);
    }
}
