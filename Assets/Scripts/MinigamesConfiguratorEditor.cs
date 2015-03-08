using UnityEngine;
using System.Collections;
using UnityEditor;


namespace Game
{

/// <summary>
/// Creates custom editor for defining new mini-games or changing properties of existing ones
/// </summary>
[CustomEditor(typeof(MinigamesConfigurator))]
public class MinigamesConfiguratorEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MinigamesConfigurator minigameConfigurator = (MinigamesConfigurator)target;

        if (GUILayout.Button("Add Mini-game"))
        {
            minigameConfigurator.AddMinigame();
        }

        if (GUILayout.Button("Save all changes"))
        {
            minigameConfigurator.SaveChanges();
        }
    }

}

}