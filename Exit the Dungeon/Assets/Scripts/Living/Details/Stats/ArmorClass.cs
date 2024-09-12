using System;
using System.Collections.Generic;

public class ArmorClass {
    private int _ac;

    //for player and party member AI
    public ArmorClass(int DexModif, ArmorType Armor, int ConstModif = 0){
        switch(Armor){
            case ArmorType.UNARMORED:
                this._ac = 10 + DexModif + ConstModif;
                break;
            case ArmorType.LIGHT:
                this._ac = 12 + DexModif;
                break;
            case ArmorType.MEDIUM:
                this._ac = 14 + (DexModif >= 2 ? 2 : DexModif);
                break;
            case ArmorType.HEAVY:
                this._ac = 18;
                break;
        }
    }

    //for enemies
    public ArmorClass(int NaturalArmor){
        this._ac = NaturalArmor;
    }

    public int GetValue(){
        return _ac;
    }
}
