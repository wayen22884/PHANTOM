using System.Collections;
using UnityEngine;
[CreateAssetMenu(fileName = "SaveData",menuName ="Create SaveDate")]
public class SaveData :ScriptableObject
{
    //處理 Assets\GameCode\GameAttr\IBaseAttr.cs(60,17)暫時使用
    public string AttrName;
    public string AttrType;
    public int MaxHP;
    public int MaxSP;
    public int STR;
    public int DEF;
    public float MoveSpeed;
}
