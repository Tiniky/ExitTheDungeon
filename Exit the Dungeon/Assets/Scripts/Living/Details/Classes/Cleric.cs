public class Cleric : MainClass{
    public Cleric(){
        this.Role =  PartyRole.HEALER;
        this.Type =  ClassType.CLERIC;
        this.Primary = MainSkill.INT;
        this.ClassHitDie = DieType.D8;
        this.Armor = ArmorType.MEDIUM;
    }

    //Disiple of Life(P)
    //Cure Wounds(A)
    //Bless(A)
}