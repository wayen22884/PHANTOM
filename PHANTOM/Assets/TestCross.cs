using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class TestCross : MonoBehaviour
{
    public Vector2 point1;
    public Vector2 point2;
    public Vector2 point3;
    public Vector2 point4;

    [ContextMenu("Test")]
    public void Test()
    {
        var value= Tool.GetIntersection(point1, point2, point3, point4);
        Debug.Log(value);
    }
    
    
}