﻿using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class GameResource 
{
    public static void Initialize()
    {
        Addressables.LoadAssetAsync<GameObject>("Prefabs/Player.prefab").Completed += (i) => {  PlayerObj= i.Result; };
        Addressables.LoadAssetAsync<GameObject>("Prefabs/AttackTrigger.prefab").Completed += (i) => {  AttackTrigger= i.Result; };
        Addressables.LoadAssetAsync<GameObject>("Prefabs/Enemy.prefab").Completed += (i) => { _pistol = i.Result; };
        // Addressables.LoadAssetAsync<GameObject>("Prefabs/Rifle.prefab").Completed += (i) => {  _rifle= i.Result; };
        // Addressables.LoadAssetAsync<GameObject>("Prefabs/ShootGun.prefab").Completed += (i) => {  _shootGun= i.Result; };
         Addressables.LoadAssetAsync<GameObject>("Prefabs/bloodBar.prefab").Completed += (i) => {  BloodBar= i.Result; };
        // Addressables.LoadAssetAsync<RuntimeAnimatorController>("Animator/Player.controller").Completed += (i) => {  _playerAnimatorController= i.Result; };
        // Addressables.LoadAssetAsync<RuntimeAnimatorController>("Animator/EnemyPistol.controller").Completed += (i) => {  _pistolAnimatorController= i.Result; };
        // Addressables.LoadAssetAsync<RuntimeAnimatorController>("Animator/EnemyRifle.controller").Completed += (i) => {  _rifleAnimatorController= i.Result; };
        // Addressables.LoadAssetAsync<RuntimeAnimatorController>("Animator/EnemyShootGun.controller").Completed += (i) => {  _shootGunAnimatorController= i.Result; };
        // Addressables.LoadAssetAsync<AudioMixer>("Music/MusicController.mixer").Completed += (i) => { audioMixer = i.Result; _BGMGroup = FindMusicGroup("BGM"); _EffectGroup = FindMusicGroup("Effect"); };
        Addressables.LoadAssetAsync<PlayerBaseAttr>("DataForTest/PlayerBaseData.asset").Completed += (i) => {  PlayerBaseAttr= i.Result; };
        //
        //
        // Addressables.LoadAssetAsync<AudioClip>("Music/Zapper.wav").Completed+= (i)=> { bulletaudio = i.Result; };
        // Addressables.LoadAssetAsync<AudioClip>("Music/B2.wav").Completed+= (i)=> { _mainMenuBGM = i.Result; };
        // Addressables.LoadAssetAsync<AudioClip>("Music/First-fight.wav").Completed+= (i)=> { _FirBattleBGM = i.Result; };
        // Addressables.LoadAssetAsync<AudioClip>("Music/Fight.wav").Completed += (i)=> { _SecBattleBGM = i.Result; };
        // Addressables.LoadAssetAsync<AudioClip>("Music/Transform.wav").Completed+= (i)=> { _transformBGM = i.Result; };
        // Addressables.LoadAssetAsync<AudioClip>("Music/CQC_1.wav").Completed+= (i)=> { _CQCPistolEffect = i.Result; };
        // Addressables.LoadAssetAsync<AudioClip>("Music/CQC_2.wav").Completed+= (i)=> { _CQCRifleEffect = i.Result; };
        // Addressables.LoadAssetAsync<AudioClip>("Music/CQC_3.wav").Completed += (i)=> { _CQCShootGunEffect = i.Result; };
    }
    public static void MainMenuSceneInitialize()
    {
        CanvasSetting();
    }
    public static void BattleSceneInitialize()
    {
        CanvasSetting();
        FloatingBars = Tool.FindChildGameObject(Canvas, "FloatingBars");
    }
    static void CanvasSetting()
    {
        Canvas = GameObject.Find("Canvas");
        if (Canvas == null) Debug.LogError("There is no Canvas");
    }


    
    private static GameObject _pistol;
    private static GameObject _rifle;
    private static GameObject _shootGun;
    public static GameObject Canvas { get; private set; }
    private static GameObject _wall;
    public static GameObject PlayerObj { get; private set; }

    public static GameObject BloodBar{ get; private set; }
    public static GameObject FloatingBars{ get; private set; }
    
    
    public static GameObject EnemyObj(CharacterID EnemyType)
    {
        if (EnemyType == CharacterID.Enemy) return _pistol;
        else if (EnemyType == CharacterID.Rifle) return _rifle;
        else if (EnemyType == CharacterID.ShootGun) return _shootGun;
        else {Debug.LogError("not exist Enemy"); return null; }
    }

    public static GameObject Wall => _wall;


    public static PlayerBaseAttr PlayerBaseAttr { get; private set; }

    //animator相關資料
    private static RuntimeAnimatorController _playerAnimatorController;
    private static RuntimeAnimatorController _pistolAnimatorController;
    private static RuntimeAnimatorController _rifleAnimatorController;
    private static RuntimeAnimatorController _shootGunAnimatorController;
    public static RuntimeAnimatorController PlayerAnimatorController=>_playerAnimatorController;
    public static RuntimeAnimatorController EnemyAnimatorController(CharacterID EnemyType)
    {
        if (EnemyType == CharacterID.Enemy) return _pistolAnimatorController;
        else if (EnemyType == CharacterID.Rifle) return _rifleAnimatorController;
        else if (EnemyType == CharacterID.ShootGun) return _shootGunAnimatorController;
        else { Debug.LogError("not exist Enemy"); return null; }
    }




    //音樂音效相關資料
    private static AudioClip bulletaudio;
    private static AudioClip _mainMenuBGM;
    private static AudioClip _FirBattleBGM;
    private static AudioClip _SecBattleBGM;
    private static AudioClip _transformBGM;
    private static AudioClip _CQCPistolEffect;
    private static AudioClip _CQCRifleEffect;
    private static AudioClip _CQCShootGunEffect;
    private static AudioMixer audioMixer;
    private static AudioMixerGroup _BGMGroup;
    private static AudioMixerGroup _EffectGroup;

    public static AudioClip BulletSound => bulletaudio;
    public static AudioClip MainMenuBGM => _mainMenuBGM;
    public static AudioClip FirBattleBGMBattleBGM => _FirBattleBGM;
    public static AudioClip SecBattleBGM => _SecBattleBGM;
    public static AudioClip TransformBGM =>_transformBGM;
    public static AudioMixerGroup BGMGroup => _BGMGroup;
    public static AudioMixerGroup EffectGroup => _EffectGroup;
    public static GameObject AttackTrigger { get; private set; }


    static AudioMixerGroup  FindMusicGroup(string adress)
    {
        AudioMixerGroup[] Result= audioMixer.FindMatchingGroups(adress);
        if (Result == null) { Debug.LogError($"there is no AudioMixerGroup in adress {adress}"); return null; }
        if (Result.Length > 1) { Debug.LogError($"there is more than one AudioMixerGroup in adress {adress}"); }
        return Result[0];
    }
    public static AudioClip CQCSound(CharacterID ID)
    {
        if (ID == CharacterID.Enemy) return _CQCPistolEffect;
        else if(ID == CharacterID.Rifle) return _CQCRifleEffect;
        else if(ID == CharacterID.ShootGun) return _CQCShootGunEffect;
        else { Debug.LogError("The sound is not exist"); return null; }
    }
}