using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AL_RollDie", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Action Leaf Nodes/RollDie")]
public class RollDie : ActionLeafNode {
    public RollType rollType;

    public enum RollType{
        FLEE,
        ATTACK
    }
    
    private void Awake(){
        NameOfNode = "RollDie";
        ExecuteMethod = ExecuteRollDie;
    }

    private NodeStatus ExecuteRollDie(){
        if(IsAgentIDLE()){
            AgentWorking(true);
        
            GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
            Entity entity = current.GetComponent<Entity>();
            
            if(rollType == RollType.FLEE){
                int rolled = Die.AbilityCheck(entity, MainSkill.INT);
                Blackboard.SetValue("Roll_FLEE", rolled);
            } else if(rollType == RollType.ATTACK){
                int rolled = Die.AttackRoll(entity);
                Blackboard.SetValue("Roll_ATTACK", rolled);
            }

            AgentWorking(false);

            return NodeStatus.SUCCESS;
        } else {
            return NodeStatus.RUNNING;
        }
    }
}