using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWaypoint : ActionLeafNode {
    private readonly List<Vector3> OffsetList = new List<Vector3>(){
        new Vector3(-2f, 0, 0),
        new Vector3(-1f, 0, 0),
        new Vector3(1f, 0, 0),
        new Vector3(2f, 0, 0)
    };

    public override void Initialize(Blackboard blackboard){
        base.Initialize(blackboard);
        NameOfNode = "FindWaypoint";
        ExecuteMethod = ExecuteFindWaypoint;
    }

    private NodeStatus ExecuteFindWaypoint(){
        Vector3 pos = Blackboard.GetValue<Vector3>("SpawnPoint");
        
        float offsetX = OffsetList[Random.Range(0, OffsetList.Count)].x;
        
        Vector3 newWaypoint = new Vector3(pos.x + offsetX, pos.y ,0);
        
        Debug.Log("Spawnpoint: " + pos);
        Debug.Log("New waypoint: " + newWaypoint);
        Blackboard.SetValue("SelectedWaypoint", newWaypoint);
        
        return NodeStatus.SUCCESS;
    }
}
