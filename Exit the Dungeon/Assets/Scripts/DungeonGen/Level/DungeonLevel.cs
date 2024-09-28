using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel_", menuName = "SZAKDOLGOZAT/Scriptable Objects/Dungeon/Dungeon Level")]
[System.Serializable]
public class DungeonLevel : ScriptableObject {
    public List<LevelGraph> LevelGraphOptions;
}
