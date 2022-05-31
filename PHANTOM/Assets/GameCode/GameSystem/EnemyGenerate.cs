using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.Common.FsNodeReaders.Watcher;
using UniRx;
using UnityEngine;

public class EnemyGenerate : MonoBehaviour
{ 
    [SerializeField] private GenerateEnemyData TEST;

    [SerializeField] private List<GenerateEnemyGroupData> enemyGenerateEnemyDatas=new List<GenerateEnemyGroupData>();


    private Queue<GenerateEnemyGroupData> GenerateEnemyDatas;

    private void Awake()
    {
        GenerateEnemyDatas = new Queue<GenerateEnemyGroupData>();
        enemyGenerateEnemyDatas.ForEach(data => { GenerateEnemyDatas.Enqueue(data);});
    }
    
    public void GenerateEnemy()
    {
        if (GenerateEnemyDatas.Count>0)
        {
            var datas= GenerateEnemyDatas.Dequeue();
            datas.groupData.ForEach(data=>Generate(data.ID,data.bornLocation));
        }
        else
        {
            Debug.LogWarning("There is no enemyData");
        }
    }

    private int index=1;
[ContextMenu("Test")]
    public void GenerateEnemyByWave()
    {
        int waveIndex = index;
        StartCoroutine(GenerateEnemyByWave(waveIndex));
        index++;
    }
    private IEnumerator GenerateEnemyByWave(int waveIndex)
    {
        var groupData= GenerateEnemyDatas.Peek();
        while (groupData.Wave == waveIndex)
        {
            GenerateEnemy();
            yield return new WaitForSeconds(5); 
            groupData = GenerateEnemyDatas.Peek();
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
    [Range(1, 8)]public int Wave;
    [Range(1, 8)]public int part;
    public List<GenerateEnemyData> groupData = new List<GenerateEnemyData>();
}