using System;
using System.Collections.Generic;

public static class Die {
    private static readonly Random _random = new Random();

    public static int Roll(DieType type, int mult = 1, bool needMaxValue = false){
        switch(type){
            case DieType.D2:
                return RollD2(needMaxValue, mult);
            case DieType.D4:
                return RollD4(needMaxValue, mult);
            case DieType.D6:
                return RollD6(needMaxValue, mult);
            case DieType.D8:
                return RollD8(needMaxValue, mult);
            case DieType.D10:
                return RollD10(needMaxValue, mult);
            case DieType.D12:
                return RollD12(needMaxValue, mult);
            case DieType.D20:
                return RollD20(needMaxValue, mult);
            default:
                return 0;
        }
    }

    private static int RollD2(bool needMaxValue, int mult = 1){
        int sum = 0;
        for(int i = 0; i < mult; i++){
            if(needMaxValue){
                sum += 2;
            }

            sum += _random.Next(1, 3);
        }

        return sum;
    }

    private static int RollD4(bool needMaxValue, int mult = 1){
        int sum = 0;
        for(int i = 0; i < mult; i++){
            if(needMaxValue){
                sum += 4;
            }

            sum += _random.Next(1, 5);
        }

        return sum;
    }

    private static int RollD6(bool needMaxValue, int mult = 1){
        int sum = 0;
        for(int i = 0; i < mult; i++){
            if(needMaxValue){
                sum += 6;
            }

            sum += _random.Next(1, 7);
        }

        return sum;
    }

    private static int RollD8(bool needMaxValue, int mult = 1){
        int sum = 0;
        for(int i = 0; i < mult; i++){
            if(needMaxValue){
                sum += 8;
            }

            sum += _random.Next(1, 9);
        }

        return sum;
    }

    private static int RollD10(bool needMaxValue, int mult = 1){
        int sum = 0;
        for(int i = 0; i < mult; i++){
            if(needMaxValue){
                sum += 10;
            }

            sum += _random.Next(1, 11);
        }

        return sum;
    }

    private static int RollD12(bool needMaxValue, int mult = 1){
        int sum = 0;
        for(int i = 0; i < mult; i++){
            if(needMaxValue){
                sum += 12;
            }

            sum += _random.Next(1, 13);
        }

        return sum;
    }

    private static int RollD20(bool needMaxValue, int mult = 1){
        int sum = 0;
        for(int i = 0; i < mult; i++){
            if(needMaxValue){
                sum += 20;
            }

            sum += _random.Next(1, 21);
        }

        return sum;
    }

    public static int RollWithAdvantage(DieType type){
        int firstRoll = Roll(type);
        int secondRoll = Roll(type);

        if(firstRoll >= secondRoll){
            return firstRoll;
        } else {
            return secondRoll;
        }
    }

    public static int RollWithDisadvantage(DieType type){
        int firstRoll = Roll(type);
        int secondRoll = Roll(type);

        if(firstRoll <= secondRoll){
            return firstRoll;
        } else {
            return secondRoll;
        }
    }

    public static int RollForInitiative(Entity entity){
        int roll = Roll(DieType.D20);
        return roll + entity.SkillTree.GetDEXModif();
    }

    public static int AttackRoll(Entity entity, bool isMelee){
        int roll = Roll(DieType.D20);
        
        if(roll == 1 || roll == 20){
            return roll;
        }
        
        if(isMelee && entity.Melee != null){
            return roll + entity.SkillTree.GetModifier(entity.Melee.AttackModifier);
        } else if(!isMelee && entity.Ranged != null) {
            return roll + entity.SkillTree.GetModifier(entity.Ranged.AttackModifier);
        }

        return 0;
    }

    public static int AbilityCheck(Entity entity, MainSkill skill){
        int roll = Roll(DieType.D20);
        return roll + entity.SkillTree.GetModifier(skill);
    }
}
