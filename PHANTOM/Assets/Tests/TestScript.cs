using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestScript
{
    // A Test behaves as an ordinary method
    [Test]
    [TestCase(0f,0f,0f,1f,0f,0.5f,true)]
    [TestCase(1.8f,-1.6f,0.8f,-2.6f,1.7f,-1.7f,true)]
    public void IsOneLine(float posx1,float posy1,float posx2,float posy2,float posx3,float posy3, bool expected)
    {
        var result = Tool.IsOnLine(new Vector2(posx1,posy1), new Vector2(posx2,posy2), new Vector2(posx3,posy3));
        Assert.AreEqual(expected, result);
    }

    [Test]
    [TestCase(5,5)]
    public void Test(int x,int y)
    {
        Assert.AreEqual(x,y);
    }
}