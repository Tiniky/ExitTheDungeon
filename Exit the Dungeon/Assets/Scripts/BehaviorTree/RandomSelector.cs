using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : Selector {
    private bool _isShuffled = false;

    public RandomSelector(string name) : base(name){}

    public override NodeStatus Execute(){
        if(!_isShuffled){
            Shuffle(Children);
            _isShuffled = true;
        }

        NodeStatus childStatus = Children[CurrentChild].Execute();
    
        if(childStatus == NodeStatus.RUNNING){
            return NodeStatus.RUNNING;
        }

        if(childStatus == NodeStatus.SUCCESS){
            CurrentChild = 0;
            _isShuffled = false;
            return NodeStatus.SUCCESS;
        }

        CurrentChild++;
        if(CurrentChild >= Children.Count){
            CurrentChild = 0;
            _isShuffled = false;
            return NodeStatus.FAILURE;
        }

        return NodeStatus.RUNNING;
    }

    private void Shuffle(List<BehaviorNode> list){
        int n = list.Count;
        while(n > 1){
            n--;
            int k = Random.Range(0, n + 1);
            BehaviorNode value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
