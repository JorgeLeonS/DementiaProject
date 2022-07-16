using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DistortionEffect))]
public class DistortionEffectInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DistortionEffect distortionEffect = (DistortionEffect)target;

        if (GUILayout.Button("Start"))
        {
            distortionEffect.StartTextDistortion();
        }
        else if (GUILayout.Button("Reverse"))
        {
            distortionEffect.EndTextDistortion();
        }
    }
}
