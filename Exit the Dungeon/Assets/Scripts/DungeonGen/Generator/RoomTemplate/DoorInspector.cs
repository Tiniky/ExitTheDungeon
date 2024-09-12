using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Doorway))]
public class DoorInspector : UnityEditor.Editor{

    public override void OnInspectorGUI(){
        Doorway doorway = (Doorway)target;
        SerializedObject serializedDoorway = new SerializedObject(doorway);
        SerializedProperty doorsProp = serializedDoorway.FindProperty("Doors");

        EditorGUILayout.PropertyField(doorsProp, true);
        serializedDoorway.ApplyModifiedProperties();

        if (GUI.changed) {
            EditorUtility.SetDirty(doorway);
        }
    }
}
