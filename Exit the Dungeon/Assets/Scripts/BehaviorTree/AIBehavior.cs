using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class AIBehavior : MonoBehaviour {
    public TextAsset jsonFile;
    public RootNode Tree;
    public NodeStatus Status;
    public bool isTreeExecuting, wasInitialized;

    void Start(){
        wasInitialized = false;

        if(jsonFile == null){
            Debug.LogError("JSON file not found.");
            return;
        }

        JObject treeData = JObject.Parse(jsonFile.text);
        Tree = (RootNode)BuildTree(treeData);

        if(Tree == null){
            Debug.LogError("Failed to build tree.");
            return;
        }

        Status = NodeStatus.SUCCESS;
        isTreeExecuting = false;

        Debug.Log("agent's spawn: " + gameObject.transform.position);
        Tree.SetAgent(gameObject);
        Tree.PrintTree();
        wasInitialized = true;
    }

    void Update(){
        if(!isTreeExecuting && wasInitialized){
            isTreeExecuting = true;
            StartCoroutine(ExecuteTree());
        }
    }

    private IEnumerator ExecuteTree(){
        Status = Tree.Execute();
        //Debug.Log("Tree execution completed with status: " + Status);
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        isTreeExecuting = false;
    }

    private BehaviorNode BuildTree(JToken nodeData){
        if (nodeData == null) {
            Debug.LogError("Node data is null.");
            return null;
        }

        BehaviorNode node = null;
        string nodeType = nodeData["nodeType"].ToString();
        string name = nodeData["nodeName"].ToString();
        string methodName;

        switch(nodeType){
            case "Root":
                node = new RootNode(name);
                break;
            case "Selector":
                node = new Selector(name);
                break;
            case "Sequence":
                node = new Sequence(name);
                break;
            case "RandomSelector":
                node = new RandomSelector(name);
                break;
            case "Condition":
                methodName = nodeData["methodName"]?.ToString();
                string shouldBeTrue = nodeData["shouldBeTrue"]?.ToString();
                node = new ConditionLeafNode(name, BehaviorNodeMethods.ConditionMethods[methodName], shouldBeTrue);
                break;
            case "Action":
                methodName = nodeData["methodName"]?.ToString();
                node = new ActionLeafNode(name, BehaviorNodeMethods.ActionMethods[methodName]);
                break;
        }

        if(node != null){
            if(nodeData["children"] != null){
                foreach(var child in nodeData["children"]){
                    node.Children.Add(BuildTree(child));
                }
            }
        }

        return node;
    }

    private void LoadJSON(){
        Creature entity = gameObject.GetComponent<Creature>();

        if(entity.Size == Size.SMALL || entity.Size == Size.MEDIUM){
            jsonFile = Resources.Load<TextAsset>("JSONs/BT_small");
        } else if(entity.Size == Size.LARGE){
            jsonFile = Resources.Load<TextAsset>("JSONs/BT_big");
        } else if(entity.Size == Size.HUMONGOUS){
            jsonFile = Resources.Load<TextAsset>("JSONs/BT_boss");
        }
    }
}
