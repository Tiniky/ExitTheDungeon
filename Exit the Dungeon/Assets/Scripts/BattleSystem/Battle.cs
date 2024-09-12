using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Battle {

    private List<Entity> battleQueue = new List<Entity>();

    public Battle(List<Entity> living){
        List<EntityRoll> rolls = new List<EntityRoll>();

        foreach(Entity entity in living){
            int roll = Die.RollForInitiative(entity);
            rolls.Add(new EntityRoll(entity, roll));

            Debug.Log(entity.EntityName + " " + roll);
        }

        GameManager.FightStarted(rolls);
        FinalizeOrder(rolls);
        FightUIManager.FightQueueSetup(battleQueue);
        ListQueue();
    }

    public List<Entity> GetQueue(){
        return battleQueue;
    }

    public Entity GetCurrent(int i){
        return battleQueue[i];
    }

    public void ListQueue(){
        foreach(Entity en in battleQueue){
            Debug.Log(en.EntityName + " " + en.SkillTree.ToStringAS());
        }
    }

    private void FinalizeOrder(List<EntityRoll> rolls){
        rolls.Sort((x, y) => y.Roll.CompareTo(x.Roll));

        bool allGood = false;
        while(!allGood){
            int matching = 0;
            for(int i = 0; i < rolls.Count - 1; i++) {
                if(rolls[i].Roll == rolls[i + 1].Roll) {
                    rolls[i].UpdateRoll();
                    rolls[i + 1].UpdateRoll();
                    matching++;
                }
            }

            rolls.Sort((x, y) => y.Roll.CompareTo(x.Roll));

            if(matching == 0){
                allGood = true;
            }
        }

        foreach(EntityRoll pair in rolls){
            battleQueue.Add(pair.Entity);
        }
    }
}
