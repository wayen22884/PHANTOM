using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public static class SaveSystem
{

    static readonly string Screenkey = "ScreenData";
    static readonly string Volumekey = "VolumeData";
    private static readonly string Rankkey = "RankData";
    public static void Save(ScreenData data)
    {
        string datastr = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Screenkey, datastr);
        PlayerPrefs.Save();
    }

    public static void Save(Volume data)
    {
        string datastr =JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Volumekey, datastr);
        PlayerPrefs.Save();
    }
    public static void Load(out  Volume data)
    {
        if (!PlayerPrefs.HasKey((Volumekey))) data= Default.DefaultVolumeData();
        else
        {
            string loadString = PlayerPrefs.GetString(Volumekey);
            data= JsonUtility.FromJson<Volume>(loadString);
        }
        
    }
    
    public static void Load(out  ScreenData data)
    {
        if (!PlayerPrefs.HasKey((Screenkey))) data= Default.DefaultScreenData();
        else
        {
            string loadString = PlayerPrefs.GetString(Screenkey);
            data= JsonUtility.FromJson<ScreenData>(loadString);
        }
        
    }

    public static void SaveRank(List<Node> list)
    {
        RankData data=new RankData(){ranklist = list};
        string datastr = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Rankkey, datastr);
        PlayerPrefs.Save();
    }
    public static void LoadRank(out List<Node> list)
    {
        if (!PlayerPrefs.HasKey((Rankkey))) list=new List<Node>();
        else
        {
            string loadString = PlayerPrefs.GetString(Rankkey);
            list= JsonUtility.FromJson<RankData>(loadString).ranklist;
        }
    }

    
    public static Rank RankCheckAndAdd(ref List<Node>  list,Node node)
    {
        Rank result=Rank.noRank;
        LinkedList<Node> ranklist=new LinkedList<Node>();
        foreach (var item in list)
        {
            ranklist.AddLast(item);
        }
        
        Node add = node;
        if (ranklist.First == null)ranklist.AddFirst(add);
        else
        {
            bool haveadd = false;
            LinkedListNode<Node> now = ranklist.First;
            while (now!=null)
            {
                if (add.Score >= now.Value.Score)
                {
                    if (now.Previous!=null)
                    {
                        ranklist.AddAfter(now.Previous, add);

                    }
                    else ranklist.AddFirst(add);
                    haveadd = true;
                    break;
                }
                now = now.Next;
            }

            if (!haveadd)
            {
                ranklist.AddLast(add);
            }
        }


        if (ranklist.Count>3) ranklist.RemoveLast();
        
        list=new List<Node>();
        foreach (var item in ranklist)
        {
            list.Add(item);
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Score==add.Score)
            {
                result = (Rank) (i + 1);
            }
        }

        return result;

    }
    public static int RankCheck(List<Node>  list,Node node)
    {
        LinkedList<Node> ranklist=new LinkedList<Node>();
        foreach (var item in list)
        {
            ranklist.AddLast(item);
        }
        if (ranklist.First == null)
        {
            ranklist.AddFirst(node);
            return 1;
        }
        
        
        Node add = node;
        LinkedListNode<Node> now = ranklist.First;
        int rank = 0;
        while (now!=null)
        {
            rank++;
            if (add.Score >= now.Value.Score)
            {
                if (rank<=3)
                {
                    return rank;
                }
                break;
            }
            now = now.Next;
        }

        if (rank < 3)
        {
            return rank + 1;
        }
        else return 0;
    }
    

}
public enum Rank
{
        first=1,
        second=2,
        thrid=3,
        noRank=0
}
public static class Default
{
    public static ScreenData DefaultScreenData()
    {
        return new ScreenData() { width = 1280, height = 720, fullscene = false };
    }
    public static Volume DefaultVolumeData()
    {
        return new Volume(0f,0f);
    }
}

[System.Serializable]
public class RankData
{
    public List<Node> ranklist;
}
[System.Serializable]
public class Node
{
    public Node(string name,int score)
    {
        Name = name;
        Score = score;
    }
    public string Name;
    public int Score;
    public override bool Equals(object obj)
    {
        return obj is Node other &&
               Name == other.Name &&
               Score == other.Score;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
