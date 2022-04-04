using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(MusicSystem))]
public class MusicSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var system = (MusicSystem)target;
        if (GUILayout.Button("PlayMusic"))
        {
            system.PlayMusic();
        }
    }
}
