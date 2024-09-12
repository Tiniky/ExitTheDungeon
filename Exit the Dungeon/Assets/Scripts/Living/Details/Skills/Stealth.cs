using System;

public class Stealth : Skill {
    public Stealth(int RolledValue, Skill Luck) {
        this.Value = (int)Math.Ceiling((double)(RolledValue + Luck.Value)/2);
        this.Rolled = -1;
    }

    public Stealth(){
        this.Value = 0;
    }
}
