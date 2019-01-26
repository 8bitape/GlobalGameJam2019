using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UniRxEventAggregator.Events;
using UniRx;
using Events;

public class attack_audio : PubSubMonoBehaviour 
{
    [SerializeField]
    private int PlayerID = 0;

    private int CharacterID = 0;

    [Header("Light Punch")]
    [FMODUnity.EventRef]
    public string LightPunchAudio = ("event:/");

    [Header("Heavy Punch")]
    [FMODUnity.EventRef]
    public string HeavyPunchAudio = ("event:/");

    [Header("Light Kick")]
    [FMODUnity.EventRef]
    public string LightKickAudio = ("event:/");

    [Header("Heavy Kick")]
    [FMODUnity.EventRef]
    public string HeavyKickAudio = ("event:/");

    private void Awake()
    {
        PubSub.GetEvent<PlayerHit>().Where(e => e.PlayerID != this.PlayerID).Subscribe(e => this.PlayAudio(e));
    }

    public void Init(int playerID, int characterID)
    {
        this.PlayerID = playerID;
        this.CharacterID = characterID;
    }

    private void PlayAudio(PlayerHit playerAttack)
    {
        switch(playerAttack.AttackType)
            {
            case AttackType.LightPunch:
                this.PlayAudioWithParameter(this.LightPunchAudio, this.CharacterID);
                break;
            case AttackType.HeavyPunch:
                this.PlayAudioWithParameter(this.HeavyPunchAudio, this.CharacterID);
                break;
            case AttackType.LightKick:
                this.PlayAudioWithParameter(this.LightKickAudio, this.CharacterID);
                break;
            case AttackType.HeavyKick:
                this.PlayAudioWithParameter(this.HeavyKickAudio, this.CharacterID);
                break;
        }
    }

    private void PlayAudioWithParameter(string FMODEvent, int characterID)
    {
        var eventInstance = RuntimeManager.CreateInstance(FMODEvent);
        eventInstance.setParameterValue("Character", characterID);
        eventInstance.start();
        eventInstance.release();
    }

}
