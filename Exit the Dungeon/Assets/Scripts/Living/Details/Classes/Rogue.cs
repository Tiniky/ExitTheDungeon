public class Rogue : MainClass{
    public Rogue(){
        this.Role =  PartyRole.MELEE;
        this.Type =  ClassType.ROGUE;
        this.Primary = MainSkill.DEX;
        this.ClassHitDie = DieType.D8;
        this.Armor = ArmorType.LIGHT;
        this.Melee = new Dagger();
        this.Ranged = new Bow();
    }

    public override void SetUpClassActive(Adventurer entity) {
        this.active = AbilityManager.InitializeAbility("Sneak Attack", entity);
    }
}