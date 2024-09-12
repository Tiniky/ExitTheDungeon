using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability {
    private string _displayName;
    private int _cd, _activeTime, _lastTimeCasted;
    private TriggerType _trigger;
    private AbilityType _type;
    private AbilityRange _range;
    private AbilityState _state;
    private AbilityEffect _effect;
    private Image _icon;

    public string DisplayName {
        get {return _displayName;} 
        set {_displayName = value;}
    }

    public int Cooldown {
        get {return _cd;} 
        set {_cd = value;}
    }

    public int ActiveRounds {
        get {return _activeTime;} 
        set {_activeTime = value;}
    }

    public int LastTimeCasted{
        get {return _lastTimeCasted;} 
        set {_lastTimeCasted = value;}
    }

    public TriggerType Trigger{
        get {return _trigger;} 
        set {_trigger = value;}
    }

    public AbilityType Type {
        get {return _type;} 
        set {_type = value;}
    }

    public AbilityRange Range{
        get {return _range;}
        set {_range = value;}
    }

    public AbilityState State {
        get {return _state;} 
        set {_state = value;}
    }

    public AbilityEffect Effect {
        get {return _effect;} 
        set {_effect = value;}
    }

    public Image Icon {
        get {return _icon;} 
        set {_icon = value;}
    }

    public virtual void Activate() {}
    public virtual void Deactivate() {}
    public virtual bool IsWaitingForTarget() {return false;}
    public virtual void SetTarget(Entity entity) {}
}
