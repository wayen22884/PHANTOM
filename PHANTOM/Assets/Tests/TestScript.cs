using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestScriptSimplePasses()
    {
        bool IsTrue = false;
        Assert.AreEqual(true,IsTrue);
    }
    
}
