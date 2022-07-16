using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ToothbrushEffect))]
public class ToothbrushInspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ToothbrushEffect effect = (ToothbrushEffect)target;

        if(GUILayout.Button("Start Effect"))
        {
            effect.StartToothbrushEffect();
        }
        else if(GUILayout.Button("Reverse Effect"))
        {
            effect.ReverseToothbrushEffect();
        }
    }
}
