using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {
    private BasicAction _action;
    private bool _wasUsed;

    public BasicAction ActionType{
        get {return _action;} 
        set {_action = value;}
    }

    public bool WasUsed{
        get {return _wasUsed;} 
        set {_wasUsed = value;}
    }

    public virtual void Activate() {}
    public virtual void Deactivate() {}
    public virtual bool IsWaitingForTarget() {return false;}
    public virtual void SetTarget(Entity entity) {}
}
