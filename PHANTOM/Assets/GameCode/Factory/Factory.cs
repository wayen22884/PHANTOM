using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


public enum CharacterID
{
    Enemy = 1,
    Rifle = 2,
    ShootGun = 3,
    Player = 99
}

public static class Factory
{
    public static PlayerCharacter CreatePlayer()
    {
        var GO = GameObject.Instantiate(GameResource.PlayerObj, new Vector3(0, -0.25f, 0f), new Quaternion(0f, 0f, 0f, 0f));
        var model = GameObject.Instantiate(GameResource.PlayerModel, Tool.FindChildGameObject(GO,"ModelPoint").transform);
        //PlayCharacter內塞入物體
        var player = new PlayerCharacter();
        AllSourcePool.SetPlayer(player);
        player.SetGameObject(GO,model);
        var playerAttr = new PlayerAttr();
        playerAttr.SetCharacter(player);
        //PlayCharacter內塞入PlayerAttr
        player.SetCharacterAttr(playerAttr);
        //PlayerAttr內塞入AttrStrategy
        playerAttr.SetAttrStrategy(new AttrStrategy());
        var playerBaseAttr = GameResource.PlayerBaseAttr;
        //PlayerAttr內塞入PlayerBaseAttr
        playerAttr.SetBaseAttr(playerBaseAttr);
        SetBloodBar(player.gameObject, player.Attr.GetBaseAttr(), new Color(0, 255, 0, 255), true);
        return player;
    }

    public static EnemyCharacter CreateEnemy(CharacterID EnemyType)
    {
        var GO = GameObject.Instantiate(GameResource.EnemyObj(EnemyType), new Vector3(0f, 0f, 0f),
            new Quaternion(0f, 0f, 0f, 0f));
        //EnemyCharacter內塞入物體
        EnemyCharacter character = new EnemyCharacter("Enemy", EnemyType);
        character.AddValueEventHandler(AllSourcePool.PlayerCharacter.ChangeValue);

        //將物件加入sourcepool
        GO.SetActive(false);
        AllSourcePool.AddToDeadList(character, EnemyType);
        character.SetGameObject(GO,GO);
        character.SetAI();

        var enemyAttr = new EnemyAttr();
        enemyAttr.SetEnemyCharacter(character);
        //EnemyCharacter內塞入EnemyAttr
        character.SetCharacterAttr(enemyAttr);
        //EnemyAttr內塞入AttrStrategy
        enemyAttr.SetAttrStrategy(new AttrStrategy());
        var EnemyBaseAttr = new EnemyBaseAttr();
        EnemyBaseAttr.SetMaxHP(100);
        EnemyBaseAttr.SetHP(100);
        //EnemyAttr內塞入EnemyBaseAttr
        enemyAttr.SetBaseAttr(EnemyBaseAttr);

        return character;
    }

    public static void SetBloodBar(GameObject gameObject, IBaseAttr baseAttr, Color? color = null, bool Last = false)
    {
        FloatingBar BloodBar = Last ? AllSourcePool.UseLastFloatingBar() : AllSourcePool.UseFloatingBar();
        BloodBar.SetFollowTarget(gameObject.transform.GetChild(1), baseAttr, color);
        BloodBar.gameObject.SetActive(true);
    }

    public static FloatingBar CreatFloatingBar()
    {
        var GO = GameObject.Instantiate(GameResource.BloodBar);
        GO.transform.SetParent(GameResource.FloatingBars.transform);
        GO.SetActive(false);
        var bar = new FloatingBar();
        AllSourcePool.AddToNotUsingfloatingBarList(bar);
        bar.Awake(GO);
        return bar;
    }
    public static AttackTrigger CreateAttackTrigger()
    {
        GameObject GO = GameObject.Instantiate(GameResource.AttackTrigger);
        GO.SetActive(false);
        var attackTrigger = new CircleAttackeTrigger(GO);
        AllSourcePool.AddAttackTriggerPool(attackTrigger);
        return attackTrigger;
    }
}