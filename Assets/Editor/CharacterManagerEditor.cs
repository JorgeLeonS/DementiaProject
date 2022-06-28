using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CharacterManager))]
public class CharacterManagerEditor : Editor
{
    SerializedProperty characterActions;

    ReorderableList actionsList;

    private void OnEnable()
    {
        //characterActions = serializedObject.FindProperty("characterActions");
        characterActions = serializedObject.FindProperty(nameof(CharacterManager.characterActions));

        actionsList = new ReorderableList(serializedObject, characterActions, true, true, true, true);

        // Delegate to draw the elements on the list
        actionsList.drawElementCallback = DrawListItems;
        actionsList.drawHeaderCallback = DrawHeader;
    }


    // Draws the elements on the list
    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = actionsList.serializedProperty.GetArrayElementAtIndex(index);

        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("actionType"),
            GUIContent.none);

        EditorGUI.LabelField(
            new Rect(rect.x + 120, rect.y, 100, EditorGUIUtility.singleLineHeight),
            "quantity");
    }

    //Draws the header
    void DrawHeader(Rect rect)
    {
        string name = "Actions";
        EditorGUI.LabelField(rect, name);   
    }

    public override void OnInspectorGUI()
    {
        DrawScriptField();

        // load real target values into SerializedProperties
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        if (EditorGUI.EndChangeCheck())
        {
            // Write back changed values into the real target
            serializedObject.ApplyModifiedProperties();
        }

        actionsList.DoLayoutList();

        // Write back changed values into the real target
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawScriptField()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", (CharacterManager)target, typeof(CharacterManager), false);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
    }
}