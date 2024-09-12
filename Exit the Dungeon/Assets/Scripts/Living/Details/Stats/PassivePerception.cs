using System;
using System.Collections.Generic;

public class PassivePerception {
    private int _pp;

    public PassivePerception(int IntModif, int LuckModif){
        this._pp = 10 + (int)Math.Ceiling((double)(IntModif + LuckModif) / 2);
    }

    public PassivePerception(int value){
        this._pp = value;
    }

    public int GetValue(){
        return this._pp;
    }
}
