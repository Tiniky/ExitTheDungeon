using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public abstract class Entity : MonoBehaviour {
    public string EntityName;
    private HitPoints _hp;
    private ArmorClass _ac;
    private Type _type;
    private PassivePerception _pp;
    private Size _size;
    private Speed _speed;
    private Weapon _melee, _ranged;
    private bool _hasDarkVision;
    private bool _hasShield;
    private SkillTree _skillTree;
    private CreatureBehaviour _behaviour;
    public Image entityIcon;
    public bool isAlive;
    private SpriteRenderer _renderer;
    private Color _basic;
    public Color highlight, inRange, outOfRange;

    protected virtual void Awake(){
        _renderer = GetComponent<SpriteRenderer>();
        _basic = _renderer.color;
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

    public bool HasShield {
        get{return _hasShield;}
        set{_hasShield = value;}
    }

    public void Death() {
        isAlive = false;
        LogManager.AddMessage(EntityName + " has died.");
        
        if(gameObject.GetComponent<PlayerBehaviour>() == null){
            gameObject.SetActive(false);

            if(GameManager.Phase == GamePhase.COMBAT){
                TileManager.Instance.FreeTiles(this);
            }

            BattleManager.CheckForEndOfCombat();
        } else{
            Debug.Log("you're ded");
            GameManager.Phase = GamePhase.LOSE;
            GameManager.UpdateRoomsCleared();
            SaveManager.SaveProgress();
            ScenesManager.LoadDeathScreen();
        }
    }

    private void OnMouseEnter(){
        if ((BattleManager.IsTargetInActionRange(this) || BattleManager.IsTargetInAbilityRange(this)) && BattleState.IsCurrentlyAttacking()) {
            _renderer.color = inRange;
        } else {
            _renderer.color = outOfRange;
        }

        Debug.Log("hovered in");
    }

    private void OnMouseExit(){
        _renderer.color = _basic;

        Debug.Log("hovered out");
    }

    private void OnMouseDown() {
        if((BattleManager.IsTargetInActionRange(this) || BattleManager.IsTargetInAbilityRange(this)) && Input.GetMouseButtonDown(0)){
            BattleManager.SetTargetOfAction(this);
        }
    }

    public async void LightUp() {
        _renderer.color = highlight;
        await Task.Delay(300);
        _renderer.color = _basic;
    }
}
