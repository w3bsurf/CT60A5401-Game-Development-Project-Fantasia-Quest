using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Added comments and the scene id and enemy group propert
 */

[CustomEditor(typeof(Cutscene))]
// A custom editor for editing cutscenes in the Unity editor
public class CutsceneInspector : Editor
{
    private VisualElement container;
    private Action openEditor;

    public void OnEnable()
    {
        openEditor = () => CutsceneEditor.ShowWindow(target as Cutscene);
    }

    // Setups the GUI for cutscene editor
    public override VisualElement CreateInspectorGUI()
    {
        container = new VisualElement();

        SerializedProperty property = serializedObject.FindProperty("autoPlay");
        PropertyField field = new PropertyField(property);
        container.Add(field);

        SerializedProperty sceneIDProperty = serializedObject.FindProperty("sceneID");
        PropertyField sceneIDField = new PropertyField(sceneIDProperty);
        container.Add(sceneIDField);

        SerializedProperty enemyGroupProperty = serializedObject.FindProperty("enemyGroup");
        PropertyField enemyGroupField = new PropertyField(enemyGroupProperty);
        container.Add(enemyGroupField);

        Button button = new Button(openEditor) { text = "Open Editor" };
        container.Add(button);

        DisplaySceneCommands();

        return container;
    }

    // Display preview of the names of cutscene commands in cutscene gameobject inspector
    private void DisplaySceneCommands()
    {
        foreach (CutsceneCommand command in (target as Cutscene).Commands)
        {
            container.Add(new Label() { text = command.ToString() });
        }
    }
}
