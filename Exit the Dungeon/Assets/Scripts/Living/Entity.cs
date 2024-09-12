using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour {
    private string _name;
    private HitPoints _hp;
    private ArmorClass _ac;
    private Type _type;
    private PassivePerception _pp;
    private Size _size;
    private Speed _speed;
    private Weapon _melee, _ranged;
    private bool _hasDarkVision;
    private SkillTree _skillTree;
    private CreatureBehaviour _behaviour;
    public Image entityIcon;
    public bool isAlive;

    protected virtual void Awake(){}

    public string EntityName {
        get{return _name;} 
        set{_name = value;}
    }

    public HitPoints HP {
        get{return _hp;} 
        set{_hp = value;}
    }

    public ArmorClass AC {
        get{return _ac;} 
        set{_ac = value;}
    }

    public Type Type {
        get{return _type;} 
        set{_type = value;}
    }

    public PassivePerception PP {
        get{return _pp;} 
        set{_pp = value;}
    }

    public SkillTree SkillTree {
        get{return _skillTree;} 
        set{_skillTree = value;}
    }

    public Speed Speed {
        get{return _speed;} 
        set{_speed = value;}
    }

    public Size Size {
        get{return _size;} 
        set{_size = value;}
    }

    public bool Darkvision {
        get{return _hasDarkVision;}
        set{_hasDarkVision = value;}
    }

    public CreatureBehaviour Behaviour {
        get{return _behaviour;}
        set{_behaviour = value;}
    }
    
    public Weapon Melee {
        get{return _melee;}
        set{_melee = value;}
    }

    public Weapon Ranged {
        get{return _ranged;}
        set{_ranged = value;}
    }

    public void Death() {
        isAlive = false;
        
        if(gameObject.GetComponent<PlayerBehaviour>() == null){
            gameObject.SetActive(false);

            if(GameManager.Phase == GamePhase.COMBAT){
                TileManager.instance.FreeTiles(this);
            }
        } else{
            //DEATH SCREEN
            Debug.Log("you're ded");
        }
    }
}
