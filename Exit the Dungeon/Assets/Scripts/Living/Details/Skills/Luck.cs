using System;

public class Luck : Skill {
    public Luck(int RolledValue) {
        int randomness = Die.Roll(DieType.D6);
        this.Value = (int)(Math.Ceiling(Math.Ceiling((double)RolledValue / 6) + randomness) / 2);
        this.Rolled = -66;
    }

    public Luck(){
        this.Value = 0;
    }
}
