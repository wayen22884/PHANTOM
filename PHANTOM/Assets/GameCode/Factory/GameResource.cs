using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.UI;

public static class GameResource 
{
    public static void Initialize()
    {
        Addressables.LoadAssetAsync<GameObject>("Prefabs/Player.prefab").Completed += (i) => {  PlayerObj= i.Result; };
        Addressables.LoadAssetAsync<GameObject>("Prefabs/PlayerModel.prefab").Completed += (i) => {  PlayerModel= i.Result; };
        Addressables.LoadAssetAsync<GameObject>("Prefabs/AttackTrigger.prefab").Completed += (i) => {  AttackTrigger= i.Result; };
        Addressables.LoadAssetAsync<GameObject>("Prefabs/Enemy.prefab").Completed += (i) => { enemy1 = i.Result; };
        //Addressables.LoadAssetAsync<GameObject>("Prefabs/EnemyModel.prefab").Completed += (i) => { enemy1Model = i.Result; };
        Addressables.LoadAssetAsync<SaveData>("EnemyData.asset").Completed += (i) => {  enemyData= i.Result; };
        // Addressables.LoadAssetAsync<GameObject>("Prefabs/Rifle.prefab").Completed += (i) => {  _rifle= i.Result; };
        // Addressables.LoadAssetAsync<GameObject>("Prefabs/ShootGun.prefab").Completed += (i) => {  _shootGun= i.Result; };
         Addressables.LoadAssetAsync<GameObject>("Prefabs/bloodBar.prefab").Completed += (i) => {  BloodBar= i.Result; };
        // Addressables.LoadAssetAsync<RuntimeAnimatorController>("Animator/Player.controller").Completed += (i) => {  _playerAnimatorController= i.Result; };
        Addressables.LoadAssetAsync<AudioMixer>("Music/MusicController.mixer").Completed += (i) => { audioMixer = i.Result; _BGMGroup = FindMusicGroup("BGM"); _EffectGroup = FindMusicGroup("Effect"); };
        Addressables.LoadAssetAsync<PlayerBaseAttr>("DataForTest/PlayerBaseData.asset").Completed += (i) => {  PlayerBaseAttr= i.Result; };
        // Addressables.LoadAssetAsync<AudioClip>("Music/Zapper.wav").Completed+= (i)=> { bulletaudio = i.Result; };
        
    }
    public static void MainMenuSceneInitialize()
    {
        CanvasSetting();
        
    }
    public static void BattleSceneInitialize()
    {
        CanvasSetting();
        FloatingBars = Tool.FindChildGameObject(Canvas, "FloatingBars");
        
        CreatePlayerAndEnemy();
    }

    private static void CreatePlayerAndEnemy()
    {
        AllSourcePool.Initialize();
        var player = Factory.CreatePlayer();
        player.StartInput();
    }

    static void CanvasSetting()
    {
        Canvas = GameObject.Find("Canvas");
        if (Canvas == null) Debug.LogError("There is no Canvas");
    }


    
    private static GameObject enemy1;
    private static GameObject _rifle;
    private static GameObject _shootGun;
    
    private static GameObject enemy1Model;
    public static GameObject Canvas { get; private set; }
    private static GameObject _wall;
    public static GameObject PlayerObj { get; private set; }
    public static GameObject PlayerModel { get; private set; }

    public static GameObject BloodBar{ get; private set; }
    public static GameObject FloatingBars{ get; private set; }
    
    
    public static GameObject EnemyObj(CharacterID EnemyType)
    {
        if (EnemyType == CharacterID.Enemy) return enemy1;
        else if (EnemyType == CharacterID.Rifle) return _rifle;
        else if (EnemyType == CharacterID.ShootGun) return _shootGun;
        else {Debug.LogError("not exist Enemy"); return null; }
    }
    public static GameObject EnemyModel(CharacterID enemyType)
    {
        if (enemyType == CharacterID.Enemy) return enemy1Model;
        else {Debug.LogError("not exist EnemyModel"); return null; }
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
    private static SaveData enemyData;
    public static SaveData SaveData(CharacterID ID)
    {
        if (ID == CharacterID.Enemy) return enemyData;
        else { Debug.LogError("The saveData is not exist"); return null; }
    }
}
