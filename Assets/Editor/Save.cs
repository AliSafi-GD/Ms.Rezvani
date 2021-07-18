using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SaveTexture))]
public class Save : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SaveTexture trg = (SaveTexture)target;
        if (GUILayout.Button("Save"))
        {
            trg.SaveTexturePNG();
        }
    }
}
