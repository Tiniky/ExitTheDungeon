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

    public override void SetUpClassActive(Adventurer entity){
        this.active = AbilityManager.InitializeAbility("Cure Wounds", entity);
    }
}