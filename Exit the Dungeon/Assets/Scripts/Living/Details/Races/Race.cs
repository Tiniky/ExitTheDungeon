using System;
using System.Collections.Generic;

public class Race {
    private RaceType _raceType;
    private SkillTree _secondarySkillTree;
    private Size _size;
    private Speed _speed;
    private string _raceDescription;
    public Ability passive;
    public Ability active;
    public bool Darkvision {get; set;}

    public virtual void SetUpRacialPassive(Adventurer entity){} 
    public virtual void SetUpRacialActive(Adventurer entity){} 

    public RaceType RaceType {
        get {return _raceType;} 
        set {_raceType = value;}
    }

    public SkillTree RaceSkills {
        get {return _secondarySkillTree;} 
        set {_secondarySkillTree = value;}
    }
    
    public Size CharSize {
        get {return _size;} 
        set {_size = value;}
    }

    public Speed CharSpeed {
        get {return _speed;} 
        set {_speed = value;}
    }

    public string Description {
        get {return _raceDescription;} 
        set {_raceDescription = value;}
    }

    public void AdventurerInit(Adventurer adventurer){
        if(adventurer.Race == this.RaceType){
            adventurer.Size = this.CharSize;
            adventurer.Speed = this.CharSpeed;
            adventurer.Darkvision = this.Darkvision;
            adventurer.SkillTree.MergeSkillTrees(this.RaceSkills);
            SetUpRacialPassive(adventurer);
            SetUpRacialActive(adventurer);
        }
    }
}
