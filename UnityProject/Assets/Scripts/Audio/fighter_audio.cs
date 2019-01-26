using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UniRxEventAggregator.Events;
using UniRx;
using Events;

public class fighter_audio : PubSubMonoBehaviour 
{
    [SerializeField]
    private int PlayerID = 0;

    private int CharacterID = 0;

    [Header("Light Swing")]
    [FMODUnity.EventRef]
    public string LightSwingAudio = ("event:/");

    [Header("Heavy Swing")]
    [FMODUnity.EventRef]
    public string HeavySwingAudio = ("event:/");

    [Header("Jump")]
    [FMODUnity.EventRef]
    public string Jump = ("event:/");

    [Header("Land")]
    [FMODUnity.EventRef]
    public string Land = ("event:/");


    private void Awake()
    {
        PubSub.GetEvent<PlayerAttack>().Where(e => e.JoystickID == this.PlayerID).Subscribe(e => this.PlayAudio(e));
        PubSub.GetEvent<PlayerJumpStart>().Where(e => e.PlayerID == this.PlayerID).Subscribe(e => this.PlayJumpStartAudio(e));
        PubSub.GetEvent<PlayerJumpEnd>().Where(e => e.PlayerID == this.PlayerID).Subscribe(e => this.PlayJumpEndAudio(e));
    }

    public void Init(int playerID, int characterID)
    {
        this.PlayerID = playerID;
        this.CharacterID = characterID;
    }

    private void PlayAudio(PlayerAttack playerAttack)
    {
        switch(playerAttack.attackType)
            {
            case AttackType.LightPunch:
                RuntimeManager.PlayOneShot(LightSwingAudio, transform.position);
                break;
            case AttackType.HeavyPunch:
                RuntimeManager.PlayOneShot(HeavySwingAudio, transform.position);
                break;
            case AttackType.LightKick:
                RuntimeManager.PlayOneShot(LightSwingAudio, transform.position);
                break;
            case AttackType.HeavyKick:
                RuntimeManager.PlayOneShot(HeavySwingAudio, transform.position);
                break;
        }
    }
    private void PlayJumpStartAudio (PlayerJumpStart playerJumpStart)
    {
        RuntimeManager.PlayOneShot(Jump, transform.position);
    }
    private void PlayJumpEndAudio(PlayerJumpEnd playerJumpEnd)
    {
        RuntimeManager.PlayOneShot(Land, transform.position);
    }
}
