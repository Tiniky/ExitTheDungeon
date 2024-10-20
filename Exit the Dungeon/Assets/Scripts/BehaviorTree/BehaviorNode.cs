using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorNode : ScriptableObject{
    public List<BehaviorNode> Children = new List<BehaviorNode>();
    protected int CurrentChild = 0;

    public NodeStatus Status { get; protected set; }
    public string NameOfNode { get; protected set; }
    public Blackboard Blackboard { get; set; }
    private AIBehavior _agent;

    public virtual void Initialize(Blackboard blackboard, AIBehavior agent){
        Blackboard = blackboard;
        _agent = agent;
        foreach(var child in Children){
            child.Initialize(blackboard, agent);
        }
    }

    public virtual NodeStatus Execute(){
        return NodeStatus.SUCCESS;
    }

    protected bool IsAgentIDLE(){
        return _agent.GetState() == ActionState.IDLE;
    }

    protected void AgentWorking(bool isWorking){
        if(isWorking){
            _agent.DoAction();
        }else{
            _agent.FinishAction();
        }
    }
}
