#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonLevel))]
public class DungeonLevelInspector : UnityEditor.Editor{
    private DungeonLevel lvl;
    
    private void OnEnable() {
        lvl = (DungeonLevel)target;
    }

    public override void OnInspectorGUI(){
        SerializedObject serializedLevel = new SerializedObject(lvl);
        SerializedProperty graphsProp = serializedLevel.FindProperty("LevelGraphOptions");

        EditorGUILayout.PropertyField(graphsProp, true);
        serializedLevel.ApplyModifiedProperties();

        if(GUILayout.Button("Generate Dungeon Map")){
            GenerateDungeon();
        }
    }

    private void GenerateDungeon(){
        DungeonGenerator.GenerateDungeon(lvl, true);
    }
}
#endif