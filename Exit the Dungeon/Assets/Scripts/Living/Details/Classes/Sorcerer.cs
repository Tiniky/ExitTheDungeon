public class Sorcerer : MainClass{
    public Sorcerer(){
        this.Role =  PartyRole.RANGED;
        this.Type =  ClassType.SORCERER;
        this.Primary = MainSkill.INT;
        this.ClassHitDie = DieType.D6;
        this.Armor = ArmorType.UNARMORED;
        this.Melee = new Staff();
    }

    public override void SetUpClassActive(Adventurer entity){
        this.active = AbilityManager.InitializeAbility("Fire Bolt", entity);
    }
}