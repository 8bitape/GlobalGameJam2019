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
        this.Subscribe<StartCharacterSelect>(this.StartCharacterSelection);
        this.characterSelectUI = transform.GetChild(0).gameObject;
    }

    public void StartCharacterSelection(StartCharacterSelect chracterSelectEvent)
    {
        characterSelectUI.SetActive(true);
    }
}
