using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public interface IBaseAttr 
{
    string AttrName { get; }
    string AttrType { get; }
    int MaxHP { get; }
    int HP { get; }
    void SetHP(int value);
    int SP { get; }
    void SetSP(int value);
    int MaxSP { get; }
    int EP { get; }
    void SetEP(int value);
    int MaxEP { get; }
    bool FaceRight { get; }
    bool SetFace(float move);
    float MoveSpeed { get; }
    
    int Shield{ get; set; }
    float StiffTime { get; }
    int STR { get; }
    int DEF { get; }
    bool SetMaxHP(int MaxHP);
    bool SetMaxSP(int MaxSP);
    void ReSet();
}





