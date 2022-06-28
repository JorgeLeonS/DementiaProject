using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


[CustomEditor(typeof(ActionToHave))]
public class EnemyStatsEditor : Editor
{
    // This will be the serialized clone property of EnemyStats.CharacterList
    private SerializedProperty CharactersList;

    // This will be the serialized clone property of EnemyStats.EnemyStatsItems
    private SerializedProperty EnemyStatsItems;

    // This is a little bonus from my side!
    // These Lists are extremely more powerful then the default presentation of lists!
    // you can/have to implement completely custom behavior of how to display and edit 
    // the list elements
    private ReorderableList dialogItemsList;

    // Reference to the actual EnemyStats instance this Inspector belongs to
    private ActionToHave ActionToHave;
    DialogueElement.ActionType actionToDo;


    // class field for storing available options
    //private GUIContent[] availableOptions;

    // Called when the Inspector is opened / ScriptableObject is selected
    private void OnEnable()
    {
        // Get the target as the type you are actually using
        ActionToHave = (ActionToHave)target;

        // Link in serialized fields to their according SerializedProperties
        EnemyStatsItems = serializedObject.FindProperty(nameof(ActionToHave.DialogueItems));

        // Setup and configure the dialogItemsList we will use to display the content of the EnemyStatsItems 
        // in a nicer way
        dialogItemsList = new ReorderableList(serializedObject, EnemyStatsItems)
        {
            displayAdd = true,
            displayRemove = true,
            draggable = true, // for the EnemyStats items we can allow re-ordering

            // As the header we simply want to see the usual display name of the EnemyStatsItems
            drawHeaderCallback = rect => EditorGUI.LabelField(rect, EnemyStatsItems.displayName),

            // How shall elements be displayed
            drawElementCallback = (rect, index, focused, active) =>
            {
                // get the current element's SerializedProperty
                var element = EnemyStatsItems.GetArrayElementAtIndex(index);

                // Get the nested property fields of the EnemyStatsElement class
                /*
                var character = element.FindPropertyRelative(nameof(DialogueElement.CharacterID));
                */
                
                actionToDo = (DialogueElement.ActionType)EditorGUILayout.EnumPopup("Display", actionToDo);
                    
                var text = element.FindPropertyRelative(nameof(DialogueElement.EnemyStatsText));

                //var popUpHeight = EditorGUI.GetPropertyHeight(actionToHave);

                // store the original GUI.color
                var color = GUI.color;

                /*
                // if the value is invalid tint the next field red
                if (character.intValue < 0) GUI.color = Color.red;

                // Draw the Popup so you can select from the existing character names
                character.intValue = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, popUpHeight), new GUIContent(character.displayName), character.intValue, availableOptions);
                */


                // reset the GUI.color
                GUI.color = color;
                
                //rect.y += popUpHeight;
                

                // Draw the text field
                // since we use a PropertyField it will automatically recognize that this field is tagged [TextArea]
                // and will choose the correct drawer accordingly
                var textHeight = EditorGUI.GetPropertyHeight(text);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, textHeight), text);
            },

            // Get the correct display height of elements in the list
            // according to their values
            // in this case e.g. we add an additional line as a little spacing between elements
            elementHeightCallback = index =>
            {
                var element = EnemyStatsItems.GetArrayElementAtIndex(index);

                var character = element.FindPropertyRelative(nameof(DialogueElement.CharacterID));
                var text = element.FindPropertyRelative(nameof(DialogueElement.EnemyStatsText));

                return EditorGUI.GetPropertyHeight(character) + EditorGUI.GetPropertyHeight(text) + EditorGUIUtility.singleLineHeight;
            },

            // Overwrite what shall be done when an element is added via the +
            // Reset all values to the defaults for new added elements
            // By default Unity would clone the values from the last or selected element otherwise
            onAddCallback = list =>
            {
                // This adds the new element but copies all values of the select or last element in the list
                list.serializedProperty.arraySize++;

                var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                var character = newElement.FindPropertyRelative(nameof(DialogueElement.CharacterID));
                var text = newElement.FindPropertyRelative(nameof(DialogueElement.EnemyStatsText));

                character.intValue = -1;
                text.stringValue = "";
            }
        };
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

        dialogItemsList.DoLayoutList();

        // Write back changed values into the real target
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawScriptField()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", (ActionToHave)target, typeof(ActionToHave), false);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
    }
}