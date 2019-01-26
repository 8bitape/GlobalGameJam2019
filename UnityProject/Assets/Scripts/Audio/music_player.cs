using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRxEventAggregator.Events;
using UniRx;
using FMODUnity;
using Events;

public class music_player : PubSubMonoBehaviour 
{
    [FMODUnity.EventRef]
    public string MenuMusic = ("event:/");
    FMOD.Studio.EventInstance MenuMusicInst;

    [FMODUnity.EventRef]
    public string BattleMusic = ("event:/");
    FMOD.Studio.EventInstance BattleMusicInst;

    private void Awake()
    {
        MenuMusicInst = FMODUnity.RuntimeManager.CreateInstance (MenuMusic);
        BattleMusicInst = FMODUnity.RuntimeManager.CreateInstance (BattleMusic);

        //TODO Link to battle/menu start/end
        this.Subscribe<GameStart>(e => this.BattleMusicStart());
    }

    private void MenuMusicStart()
    {
        MenuMusicInst.start();
    }
    private void MenuMusicStop()
    {
        MenuMusicInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    private void BattleMusicStart ()
    {
        BattleMusicInst.start();
    }
   private void BattleMusicStop ()
    {
        BattleMusicInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
