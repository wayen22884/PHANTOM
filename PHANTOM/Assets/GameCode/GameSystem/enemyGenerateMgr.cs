using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGenerateMgr : MonoBehaviour
{
    [SerializeField] private EnemyGenerate enemyGenerate;

    public void GenerateNextEnemy()
    {
        enemyGenerate.GenerateEnemy();
    }

    public void StartBattle()
    {
        StartCoroutine(CreateAllWave());
    }

    public IEnumerator CreateAllWave()
    {
        while (EnemyGenerate.Status != EnemyGenerate.WaveStatus.End)
        {
            enemyGenerate.GenerateEnemyByWave();
            while (EnemyGenerate.Status == EnemyGenerate.WaveStatus.CreateEnemy ||
                   AllSourcePool.GetAliveEnemyListCopy().Count > 0)
            {
                yield return new WaitForSeconds(10);
            }

            Debug.LogError("WaveClear");
        }

        if (EnemyGenerate.Status == EnemyGenerate.WaveStatus.End)
        {
            BattleScene.GameEnd(true);
        }
    }
}