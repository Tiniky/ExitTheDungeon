using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemySpawnManager {
    private static readonly Dictionary<int, int> PartyMonsterRatio = new Dictionary<int, int> {
        {1, 3},
        {2, 5},
        {3, 7},
        {4, 9},
    };
    private static readonly float GOBLIN_WEIGHT = 1.5f;
    private static readonly float OGRE_WEIGHT = 2.5f;

    public static void SpawnEnemies(){
        List<Adventurer> party = new List<Adventurer>();
        party.Add(GameManager.Player());
        
        foreach(GameObject adventurer in GameManager.Allies()){
            party.Add(adventurer.GetComponent<Adventurer>());
        }

        int partySize = party.Count;
        int estimatedMonstersNum = PartyMonsterRatio[partySize];
        int estimatedMaxWeight = 0;
        foreach(Adventurer adventurer in party){
            estimatedMaxWeight += adventurer.Level;
        }
    }
}
