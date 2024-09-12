public class Barbarian : MainClass{
    public Barbarian(){
        this.Role =  PartyRole.TANK;
        this.Type = ClassType.BARBARIAN;
        this.Primary = MainSkill.STR;
        this.ClassHitDie = DieType.D12;
        this.Armor = ArmorType.UNARMORED;
        this.Melee = new BattleAxe();
    }

    public override void SetUpClassActive(Adventurer entity) {
        this.active = AbilityManager.InitializeAbility("Rage", entity);
    }
}
