using System;
using System.Collections.Generic;

public class HitPoints {

    //the maximum HitPoint an Entity can have
    private int _maxHP;
    //the number of current HitPoints
    private int _hp;

    private int _previousHP;
    
    /**
    * Constructor that determins an Adventurer's HP
    * @param ClassHitDie - the DieType of the Adventurer's MainClass
    * @param ConstModif - a number based on the Adventurer's Constitution skill
    **/
    public HitPoints(DieType ClassHitDie, int ConstModif, int Level){
        int baseHP = Die.Roll(ClassHitDie, 1, true) + ConstModif;
        int lvl = 1;

        while(lvl < Level){
            baseHP += (Die.Roll(ClassHitDie) + ConstModif);
            lvl++;
        }

        this._maxHP = baseHP;
        this._hp = baseHP;
        this._previousHP = baseHP;
    }

    /**
    * Constructor that determins a Creature's HP
    * @param NumberOfRolls - the number of times the HitDie should be rolled
    * @param HitDie - the DieType that's used for the HP calculation
    * @param AdditionalHP - a number of HP that Creature gets in addition
    * @param MinHP - the number of minimum HP the Creature should have
    **/
    public HitPoints(int NumberOfRolls, DieType HitDie, int AdditionalHP, int MinHP){
        int baseHP = AdditionalHP;

        while(NumberOfRolls > 0){
            baseHP += Die.Roll(HitDie);
            NumberOfRolls--;
        }

        if(baseHP < MinHP){
            int additionalRolls = 3;

            while(additionalRolls > 0){
                baseHP += Die.Roll(HitDie);
                additionalRolls--;
            }
        }

        this._maxHP = baseHP;
        this._hp = baseHP;
    }

    /**
    * Returns the current HP value
    **/
    public int GetValue(){
        return this._hp;
    }

    /**
    * Returns the max HP value
    **/
    public int GetMax(){
        return this._maxHP;
    }

    /**
    * Calculates the damage taken
    * @param DMGvalue - the number of damage points
    **/
    public void Take(int DMGvalue){
        this._previousHP = this._hp;
        this._hp -= DMGvalue;
    }

    /**
    * Calculates the heal taken, it can't go over the maximum HP
    * @param HEALvalue - the number of heal points
    **/
    public void Add(int HEALvalue){
        this._previousHP = this._hp;
        if(this._hp + HEALvalue > this._maxHP){
            this._hp = this._maxHP;
        } else{
            this._hp += HEALvalue;
        }
    }

    /**
    * Resets the hitpoints to the max value
    **/
    public void HardReset(){
        this._hp = this._maxHP;
        this._previousHP = this._hp;
    }

    /**
    * Resets the hitpoints to the 1
    **/
    public void Reset(){
        this._hp = 1;
        this._previousHP = this._hp;
    }

    public int GetPreviousHP() {
        return _previousHP;
    }
}
