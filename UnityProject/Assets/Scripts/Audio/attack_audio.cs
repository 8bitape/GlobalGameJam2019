using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UniRxEventAggregator.Events;
using UniRx;

public class attack_audio : PubSubMonoBehaviour 
{
    [SerializeField]
    private int PlayerID = 0;

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
        PubSub.GetEvent<PlayerAttack>().Where(e => e.JoystickID == this.PlayerID).Subscribe(e => this.PlayAudio(e));

    }

    private void PlayAudio(PlayerAttack playerAttack)
    {
        switch(playerAttack.attackType)
            {
            case PlayerAttack.AttackType.LightPunch:
                RuntimeManager.PlayOneShot(LightPunchAudio, transform.position);
                RuntimeManager.PlayOneShot(LightSwingAudio, transform.position);
                break;
            case PlayerAttack.AttackType.HeavyPunch:
                RuntimeManager.PlayOneShot(HeavyPunchAudio, transform.position);
                RuntimeManager.PlayOneShot(HeavySwingAudio, transform.position);
                break;
            case PlayerAttack.AttackType.LightKick:
                RuntimeManager.PlayOneShot(LightKickAudio, transform.position);
                RuntimeManager.PlayOneShot(LightSwingAudio, transform.position);
                break;
            case PlayerAttack.AttackType.HeavyKick:
                RuntimeManager.PlayOneShot(HeavyKickAudio, transform.position);
                RuntimeManager.PlayOneShot(HeavySwingAudio, transform.position);
                break;
        }
    }

}
