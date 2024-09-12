using System;
using System.Collections.Generic;
using System.Linq;

public class SkillTree {
    private Constitution _constitution;
    private Dexterity _dexterity;
    private Intelligence _intelligence;
    private Luck _luck;
    private Stealth _stealth;
    private Strength _strength;

    public SkillTree(int Con, int Dex, int Int, int Str) {
        this._constitution = new Constitution(Con);
        this._dexterity = new Dexterity(Dex);
        this._intelligence = new Intelligence(Int);
        this._strength = new Strength(Str);
        this._stealth = new Stealth();
        this._luck = new Luck();
    }

    public SkillTree(Adventurer adventurer){
        List<int> rolls = RollForAbilityScores();

        while(rolls.Sum() < 48){
            rolls = RollForAbilityScores();
        }

        rolls.Sort((a, b) => b.CompareTo(a));

        switch(adventurer.Primary){
            case MainSkill.STR:
                InitializeAbilityScores(rolls[1], rolls[3], rolls[2], rolls[0], rolls[0]);
                break;
            case MainSkill.DEX:
                InitializeAbilityScores(rolls[2], rolls[0], rolls[1], rolls[3], rolls[0]);
                break;
            case MainSkill.INT:
                InitializeAbilityScores(rolls[2], rolls[1], rolls[0], rolls[3], rolls[0]);
                break;
            
        }
    }

    public SkillTree(){
        this._constitution = new Constitution();
        this._dexterity = new Dexterity();
        this._intelligence = new Intelligence();
        this._strength = new Strength();
        this._stealth = new Stealth();
        this._luck = new Luck();
    }

    public int GetModifier(MainSkill skill, bool Calculated = true){
        switch(skill){
            case MainSkill.CON:
                return GetCONModif(Calculated);
            case MainSkill.DEX:
                return GetDEXModif(Calculated);
            case MainSkill.INT:
                return GetINTModif(Calculated);
            case MainSkill.STR:
                return GetSTRModif(Calculated);
            case MainSkill.STH:
                return GetStealthModif();
            case MainSkill.LUK:
                return GetLuckModif();
            default:
                return 0;
        }
    }

    public int GetCONModif(bool Calculated = true){
        if(Calculated){
            return CountAbilityScoreModifier(this._constitution.Value);
        } else {
            return this._constitution.Value;
        }
        
    }

    public int GetDEXModif(bool Calculated = true){
        if(Calculated){
            return CountAbilityScoreModifier(this._dexterity.Value);
        } else {
            return this._dexterity.Value;
        }
    }

    public int GetINTModif(bool Calculated = true){
        if(Calculated){
            return CountAbilityScoreModifier(this._intelligence.Value);
        } else {
            return this._intelligence.Value;
        }
    }

    public int GetSTRModif(bool Calculated = true){
        if(Calculated){
            return CountAbilityScoreModifier(this._strength.Value);
        } else {
            return this._strength.Value;
        }
    }

    public int GetLuckModif(){
        return this._luck.Value;
    }

    public int GetStealthModif(){
        return this._stealth.Value;
    }

    public void MergeSkillTrees(SkillTree tree){
        this._constitution.Value += tree.GetCONModif(false);
        this._dexterity.Value += tree.GetDEXModif(false);
        this._intelligence.Value += tree.GetINTModif(false);
        this._luck.Value += tree.GetLuckModif();
        this._stealth.Value += tree.GetStealthModif();
        this._strength.Value += tree.GetSTRModif(false);
    }

    private List<int> RollForAbilityScores() {
        int rollsNeeded = 4;
        List<int> AS = new List<int>();

        while(rollsNeeded > 0){
            int[] rolls = new int[4]; 
            for(int i = 0; i<4; i++){
                rolls[i] = Die.Roll(DieType.D6);
            }

            int min = rolls[0];
            int ind = 0;
            for(int i = 0; i<4; i++){
                if(rolls[i]<min){
                    min = rolls[i];
                    ind = i; 
                }
            }

            int value = 0;
            for(int i = 0; i<4; i++){
                if(i != ind){
                    value += rolls[i];
                }
            }

            AS.Add(value);
            rollsNeeded--;
        }

        return AS;
    }

    private void InitializeAbilityScores(int Con, int Dex, int Int, int Str, int Primary){
        this._constitution = new Constitution(Con);
        this._dexterity = new Dexterity(Dex);
        this._intelligence = new Intelligence(Int);
        this._strength = new Strength(Str);
        this._luck = new Luck(Primary);
        this._stealth = new Stealth(CountAbilityScoreModifier(Dex), this._luck);
    }

    private int CountAbilityScoreModifier(int SkillValue){
        if(AbilityModifier.modifierMap.ContainsKey(SkillValue)){
            return AbilityModifier.modifierMap[SkillValue];
        }

        return 0;
    }

    public string ToStringAS(){
        return string.Format("CON: {0}, DEX: {1}, INT: {2}, STR: {3}, Luck: {4}, Stealth: {5}",
            GetCONModif(), GetDEXModif(), GetINTModif(), GetSTRModif(), GetLuckModif(), GetStealthModif());
    }

    public string ToStringSP(){
        return string.Format("CON: {0}, DEX: {1}, INT: {2}, STR: {3}, Luck: {4}, Stealth: {5}",
            _constitution.Value, _dexterity.Value, _intelligence.Value, 
            _strength.Value, _luck.Value, _stealth.Value);
    }
}