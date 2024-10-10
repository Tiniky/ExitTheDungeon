public class Sorcerer : MainClass{
    public Sorcerer(){
        this.Role =  PartyRole.RANGED;
        this.Type =  ClassType.SORCERER;
        this.Primary = MainSkill.INT;
        this.ClassHitDie = DieType.D6;
        this.Armor = ArmorType.UNARMORED;
        this.Melee = new Staff();
    }

    //Celestial Shield(A)
    //TYPE
}