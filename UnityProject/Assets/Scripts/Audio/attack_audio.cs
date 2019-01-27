using UnityEngine;
using FMODUnity;
using UniRxEventAggregator.Events;
using UniRx;
using Events;

public class attack_audio : PubSubMonoBehaviour 
{
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

    [Header("Block")]
    [FMODUnity.EventRef]
    public string BlockAudio = ("event:/");

    private void Awake()
    {
        PubSub.GetEvent<PlayerHit>().Subscribe(e => this.PlayAudio(e));

    }

    private void PlayAudio(PlayerHit playerAttack)
    {

        if (playerAttack.Blocked)
        {
            RuntimeManager.PlayOneShot(BlockAudio, transform.position);
        }
        else
        {
            switch (playerAttack.AttackType)
            {
                case AttackType.LightPunch:
                    RuntimeManager.PlayOneShot(LightPunchAudio, transform.position);
                    break;
                case AttackType.HeavyPunch:
                    RuntimeManager.PlayOneShot(HeavyPunchAudio, transform.position);
                    break;
                case AttackType.LightKick:
                    RuntimeManager.PlayOneShot(LightKickAudio, transform.position);
                    break;
                case AttackType.HeavyKick:
                    RuntimeManager.PlayOneShot(HeavyKickAudio, transform.position);
                    break;
            }
      
        }
    }

}
