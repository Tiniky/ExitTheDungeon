public class Cleric : MainClass{
    public Cleric(){
        this.Role =  PartyRole.HEALER;
        this.Type =  ClassType.CLERIC;
        this.Primary = MainSkill.INT;
        this.ClassHitDie = DieType.D8;
        this.Armor = ArmorType.MEDIUM;
        this.Melee = new Hammer();
        this.Ranged = new Crossbow();
    }

    //Disiple of Life(P)
    //Cure Wounds(A)
    //Bless(A)
}