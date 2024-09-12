public class Human : Race {
    
    public Human(){
        this.RaceType = RaceType.HUMAN;
        this.CharSize = Size.MEDIUM;
        this.CharSpeed = new Speed(30);
        this.Darkvision = false;
        this.RaceSkills = new SkillTree();
    }

    public override void SetUpRacialPassive(Adventurer entity){
        this.passive = AbilityManager.InitializeAbility("Most Boring", entity);
    }
}
