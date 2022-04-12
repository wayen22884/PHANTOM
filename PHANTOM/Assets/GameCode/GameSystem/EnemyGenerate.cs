using System;
using UnityEngine;

public class EnemyGenerate
{
    float CheckInterval = 2;
    Action _update;

    public EnemyGenerate()
    {
        CheckInterval = 1f;
        _update = () => { };
    }

    float clock = 0;

    public void Update()
    {
        _update();
    }

    #region Update

    float pistol = -0.25f;
    float rifle = -0.3f;
    float shootGun = -0.16f;

    void CheckAndGenerate()
    {
        if (clock < CheckInterval)
        {
            clock += Time.deltaTime;
            return;
        }
        else clock = 0f;

        Generate();
    }

    private void Generate()
    {
        CharacterID ID = EnemyProbility;
        ICharacter enemy = AllSourcePool.UseNewEnemy(ID);
        float posX = UnityEngine.Random.Range(0, 2) == 1 ? 10f : -10f;
        float posY = ID == CharacterID.Enemy ? pistol : ID == CharacterID.Rifle ? rifle : shootGun;
        enemy.Transform.position = new Vector3(posX, posY, 0f);
    }

    CharacterID EnemyProbility
    {
        get
        {return CharacterID.ShootGun;
        }
    }

    #endregion

    public void Start()
    {
        _update = CheckAndGenerate;
    }

    public void Stop()
    {
        _update = () => { };
    }
}