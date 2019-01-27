using UnityEngine;
using FMODUnity;
using UniRxEventAggregator.Events;
using UniRx;
using Events;

public class fighter_audio : PubSubMonoBehaviour 
{
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

    [Header("UI Left/Right")]
    [FMODUnity.EventRef]
    public string UiLR = ("event:/");

    [Header("UI Select Punch")]
    [FMODUnity.EventRef]
    public string SelectPunch = ("event:/");


    private void Awake()
    {
        PubSub.GetEvent<PlayerAttack>().Subscribe(e => this.PlayAudio(e));
        PubSub.GetEvent<PlayerJumpStart>().Subscribe(e => this.PlayJumpStartAudio(e));
        PubSub.GetEvent<PlayerJumpEnd>().Subscribe(e => this.PlayJumpEndAudio(e));

        this.Subscribe<CharacterSelectChange>(e => this.UiLeftRight());
        this.Subscribe<CharacterSelectReadyChange>(e => this.UiSelect());
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

    private void UiLeftRight ()
    {
        RuntimeManager.PlayOneShot(UiLR, transform.position);
    }
    private void UiSelect()
    {
        RuntimeManager.PlayOneShot(SelectPunch, transform.position);
    }
}
