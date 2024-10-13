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
    private static readonly List<float> MONSTER_WEIGHTS = new List<float> {OGRE_WEIGHT, GOBLIN_WEIGHT};
    private static readonly Dictionary<float, string> MONSTER_MANUAL = new Dictionary<float, string> {
        {OGRE_WEIGHT, "OGRE"}, 
        {GOBLIN_WEIGHT, "GOBLIN"}
    };

    public static List<string> SpawnEnemies(EnemySpawnPoint spawnPoints){
        int OgreProbability = spawnPoints.OgreProbability;
        int GoblinProbability = spawnPoints.GoblinProbability;

        List<Adventurer> party = new List<Adventurer>();
        party.Add(GameManager.Player());
        
        foreach(GameObject adventurer in GameManager.Allies()){
            party.Add(adventurer.GetComponent<Adventurer>());
        }

        int partySize = party.Count;
        int estimatedMaxMonstersNum = PartyMonsterRatio[partySize];
        int estimatedMaxWeight = 0;
        foreach(Adventurer adventurer in party){
            estimatedMaxWeight += adventurer.Level;
        }

        int randomValue = UnityEngine.Random.Range(1, 101);
        float weight = 0;
        List<string> monsters = new List<string>();

        int triedWeightIndex = randomValue <= OgreProbability ? 0 : 1;
        while(triedWeightIndex < MONSTER_WEIGHTS.Count){
            float nextWeight = MONSTER_WEIGHTS[triedWeightIndex];
            float testWeight = weight + nextWeight;

            if(testWeight < estimatedMaxWeight){
                weight = testWeight;
                monsters.Add(MONSTER_MANUAL[nextWeight]);
            } else {
                break;
            }

            if(triedWeightIndex == 0){
                triedWeightIndex++;
            }
        }

        Debug.Log("Spawned " + monsters.Count + " monsters with weights: " + string.Join(", ", monsters));
        return monsters;
    }
}
