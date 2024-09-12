public class Orc : Race {
    
    public Orc(){
        this.RaceType = RaceType.ORC;
        this.CharSize = Size.MEDIUM;
        this.CharSpeed = new Speed(30);
        this.Darkvision = true;
        this.RaceSkills = new SkillTree(1, 0, 0, 2);
    }

    public override void SetUpRacialPassive(Adventurer entity){
        this.passive = AbilityManager.InitializeAbility("Relentless Endurance", entity);
    }
}
