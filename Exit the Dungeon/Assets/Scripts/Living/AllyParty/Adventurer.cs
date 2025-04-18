using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : Entity {
    public bool IsPlayer { get; private set; }
    public RaceType Race;
    public ClassType Class;
    private PartyRole _role;
    private MainSkill _primary;
    private DieType _hitDie;
    private ArmorType _armorType;
    public int Level { get; private set; }

    private List<Ability> _passives;
    private List<Ability> _actives;
    private Dictionary<Ability, Advantage> _advantageRolls;
    private Dictionary<Ability, Disadvantage> _disatvantageRolls;
    
    //other
    public float temporaryAttackBonus;
    public int temporaryHealBonus;

    //test
    public bool collided;
    
    protected override void Awake(){
        base.Awake();
        Type = Type.ALLY;
        isAlive = true;
        collided = false;
    }

    public PartyRole Role {
        get{return _role;} 
        set{_role = value;}
    }

    public MainSkill Primary {
        get{return _primary;}
        set{_primary = value;}
    }

    public DieType ClassHitDie {
        get{return _hitDie;}
        set{_hitDie = value;}
    }

    public ArmorType Armor {
        get{return _armorType;}
        set{_armorType = value;}
    }

    public void Initialize(int lvl, bool isPlayer = false){
        IsPlayer = isPlayer;
        _passives = new List<Ability>();
        _actives = new List<Ability>();
        _advantageRolls = new Dictionary<Ability, Advantage>();
        _disatvantageRolls = new Dictionary<Ability, Disadvantage>();
        this.Level = lvl;

        this.SkillTree = new SkillTree();
        Debug.Log("new " + this.SkillTree.ToStringSP());

        InitializeRace();
        Debug.Log("race " + this.SkillTree.ToStringSP());
        InitializeClass();
        InitializeSkills();
        Debug.Log("after sp " + this.SkillTree.ToStringSP());
        Debug.Log("after as " + this.SkillTree.ToStringAS());

        HP = new HitPoints(this.ClassHitDie, this.SkillTree.GetCONModif(), this.Level);
        AC = new ArmorClass(this.SkillTree.GetDEXModif(), this.Armor, this.HasShield, this.SkillTree.GetCONModif());
        PP = new PassivePerception(this.SkillTree.GetINTModif(), this.SkillTree.GetLuckModif());

        this.temporaryAttackBonus = 0;
        this.temporaryHealBonus = 0;
    }

    private void InitializeRace(){
        switch(this.Race){
            case RaceType.DWARF:
                new Dwarf().AdventurerInit(this);
                break;
            case RaceType.ELF:
                new Elf().AdventurerInit(this);
                break;
            case RaceType.HUMAN:
                new Human().AdventurerInit(this);
                break;
            case RaceType.ORC:
                new Orc().AdventurerInit(this);
                break;
            default:
                break;
        }
    }

    private void InitializeClass(){
        switch(this.Class){
            case ClassType.BARBARIAN:
                new Barbarian().AdventurerInit(this);
                break;
            case ClassType.CLERIC:
                new Cleric().AdventurerInit(this);
                break;
            case ClassType.ROGUE:
                new Rogue().AdventurerInit(this);
                break;
            case ClassType.SORCERER:
                new Sorcerer().AdventurerInit(this);
                break;
            default:
                break;
        }
    }

    private void InitializeSkills(){
        this.SkillTree.MergeSkillTrees(new SkillTree(this));
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Enemy")) {
            collided = true;
        } else {
            collided = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        collided = false;
    }

    public int GetSkillValue(string skill, bool calc = true){
        switch(skill){
            case "CON":
                return SkillTree.GetCONModif(calc);
            case "DEX":
                return SkillTree.GetDEXModif(calc);
            case "INT":
                return SkillTree.GetINTModif(calc);
            case "STR":
                return SkillTree.GetSTRModif(calc);
        }


        if(skill == "LUK" && calc){
            return SkillTree.GetLuckModif();
        }

        if(skill == "STH" && calc){
            return SkillTree.GetStealthModif();
        }

        return -1;
    }

    public List<Ability> GetPassives(){
        return _passives;
    }

    public List<Ability> GetActives(){
        return _actives;
    }

    public void AdvantageAdd(RollType rollType, Ability ability, MainSkill skillType){
        _advantageRolls[ability] = new Advantage(rollType, skillType);
    }

    public void AdvantageAdd(RollType rollType, Ability ability){
        _advantageRolls[ability] = new Advantage(rollType);
    }

    public void AdvantageExpired(Ability ability){
        _advantageRolls.Remove(ability);
    }

    public void LearnAbility(Ability ability) {
        if(ability.Type == AbilityType.ACTIVE){
            _actives.Add(ability);
            if(GameManager.Phase == GamePhase.ADVENTURE){
                AbilityUIManager.RefreshAbilities(this);
            }
        } else if(ability.Type == AbilityType.PASSIVE){
            _passives.Add(ability);
            if(GameManager.Phase == GamePhase.ADVENTURE){
                PassiveUIManager.CreateUIForPassive(ability);
            }
        }
    }

    public void ModifyTemporaryDamage(float bonus, bool reset = true){
        if(reset){
            this.temporaryAttackBonus = bonus;
        } else {
            this.temporaryAttackBonus += bonus;
        }
    }

    public void ModifyTemporaryHeal(int bonus){
        this.temporaryHealBonus = bonus;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(IsPlayer && collision.gameObject.CompareTag("room")){
            GameManager.Instance.EnterRoom(collision.gameObject);
        } else if(IsPlayer && collision.gameObject.CompareTag("corridor")){
            GameManager.LeftRoom(collision.gameObject);
        }
    }

    public bool IsAlive(){
        return isAlive;
    }
}
