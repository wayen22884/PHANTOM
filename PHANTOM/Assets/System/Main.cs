using System;
using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Main : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(this);
        GameResource.Initialize();
        GameResource.BattleSceneInitialize();

        Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(_=>WaitForInitializeDone());
    }

    private ICharacter player;
    private void Update()
    {
        AllSourcePool.Update();
    }

    private void WaitForInitializeDone()
    {
        AllSourcePool.Initialize();
        player= Factory.CreatePlayer();
        player.StartInput();
        
        
        
        
        var enemy= AllSourcePool.UseNewEnemy(CharacterID.Enemy);
        var testUI= Tool.FindChildGameObject(GameResource.Canvas.gameObject, "TestUI");
        var obj= testUI.GetComponent<Text>();
        Observable.EveryUpdate().Subscribe(_ => { obj.text = $"HP:{enemy.Attr.GetBaseAttr().HP}";});
    }
}
