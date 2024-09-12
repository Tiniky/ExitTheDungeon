using System;
using System.Collections.Generic;

public abstract class MainClass {
    private PartyRole _role;
    private ClassType _type;
    private string _classDescription;
    public DieType ClassHitDie;
    public MainSkill Primary;
    public ArmorType Armor;
    public Ability passive;
    public Ability active;
    public Weapon Melee, Ranged;
    //proficiency bonuses

    public PartyRole Role {
        get {return _role;} 
        set {_role = value;}
    }

    public ClassType Type {
        get {return _type;} 
        set {_type = value;}
    }

    public string Description {
        get {return _classDescription;} 
        set {_classDescription = value;}
    }

    public virtual void SetUpClassPassive(Adventurer entity){}
    public virtual void SetUpClassActive(Adventurer entity){}

    public void AdventurerInit(Adventurer adventurer){
        if(adventurer.Class == this.Type){
            adventurer.Role = this.Role;
            adventurer.Primary = this.Primary;
            adventurer.ClassHitDie = this.ClassHitDie;
            adventurer.Armor = this.Armor;
            adventurer.Melee = this.Melee;
            adventurer.Ranged = this.Ranged;
            SetUpClassPassive(adventurer);
            SetUpClassActive(adventurer);
        }
    }
}

