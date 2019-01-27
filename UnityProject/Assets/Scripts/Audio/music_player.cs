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

    [FMODUnity.EventRef]
    public string EndBattleBell = ("event:/");
    FMOD.Studio.EventInstance EndBattleBellInst;

    private void Awake()
    {
        MenuMusicInst = FMODUnity.RuntimeManager.CreateInstance (MenuMusic);
        BattleMusicInst = FMODUnity.RuntimeManager.CreateInstance (BattleMusic);
        EndBattleBellInst = FMODUnity.RuntimeManager.CreateInstance(EndBattleBell);

        this.Subscribe<FightStart>(e => this.BattleMusicStart());
        this.Subscribe<FightOver>(e => this.BattleMusicStop());
        this.Subscribe<FightOver>(e => this.EndBattleBellStart());
        this.Subscribe<StartCharacterSelect>(e => this.MenuMusicStart());
        this.Subscribe<EndCharacterSelect>(e => this.MenuMusicStop());
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
    private void EndBattleBellStart()
    {
        EndBattleBellInst.start();
    }
}
