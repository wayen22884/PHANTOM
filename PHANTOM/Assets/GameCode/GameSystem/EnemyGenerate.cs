using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerate : MonoBehaviour
{
    [SerializeField] private ShowWave showWave;
    public enum WaveStatus
    {
        BeforeStart,
        CreateEnemy,
        WaveEnd,
        End
    }

    public static WaveStatus Status = WaveStatus.BeforeStart;
    [SerializeField] private GenerateEnemyData TEST;

    [SerializeField] private List<GenerateEnemyGroupData> enemyGenerateEnemyDatas = new List<GenerateEnemyGroupData>();


    private Queue<GenerateEnemyGroupData> GenerateEnemyDatas;

    private void Awake()
    {
        GenerateEnemyDatas = new Queue<GenerateEnemyGroupData>();
        enemyGenerateEnemyDatas.ForEach(data => { GenerateEnemyDatas.Enqueue(data); });
    }

    public void GenerateEnemy()
    {
        
        if (GenerateEnemyDatas.Count > 0)
        {
            Status = WaveStatus.CreateEnemy;
            var datas = GenerateEnemyDatas.Dequeue();
            datas.groupData.ForEach(data => Generate(data.ID, data.bornLocation));
        }
        else
        {
            Status = WaveStatus.End;
            Debug.LogWarning("There is no enemyData");
        }
    }

    private int index = 1;

    public void GenerateEnemyByWave()
    {
        int waveIndex = index;
        showWave.Show(waveIndex);
        StartCoroutine(GenerateEnemyByWave(waveIndex));
        index++;
    }

    private IEnumerator GenerateEnemyByWave(int waveIndex)
    {
        var groupData = GenerateEnemyDatas.Peek();
        while (groupData?.Wave == waveIndex)
        {
            GenerateEnemy();
            yield return new WaitForSeconds(5);
            if (GenerateEnemyDatas.Count > 0)
            {
                groupData = GenerateEnemyDatas.Peek();
            }
            else
            {
                groupData = null;
            }
        }

        if (groupData!=null)
        {
            Status = WaveStatus.WaveEnd;
        }
        else
        {
            Status = WaveStatus.End;
        }
    }


    [ContextMenu("TestGenerate")]
    private void TestGenerate()
    {
        Generate(TEST.ID, TEST.bornLocation);
    }

    private void Generate(CharacterID id, Vector3 bornLocation)
    {
        ICharacter enemy = AllSourcePool.UseNewEnemy(id);
        enemy.StartInput();
        enemy.Transform.position = bornLocation;
    }
}

[Serializable]
public struct GenerateEnemyData
{
    public CharacterID ID;
    public Vector3 bornLocation;
}

[Serializable]
class GenerateEnemyGroupData
{
    [Range(1, 8)] public int Wave;
    [Range(1, 8)] public int part;
    public List<GenerateEnemyData> groupData = new List<GenerateEnemyData>();
}