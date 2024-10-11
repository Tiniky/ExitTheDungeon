public class Elf : Race {
    
    public Elf(){
        this.RaceType = RaceType.ELF;
        this.CharSize = Size.MEDIUM;
        this.CharSpeed = new Speed(35);
        this.Darkvision = true;
        this.RaceSkills = new SkillTree(0, 2, 0, 0);
    }

    public override void SetUpRacialPassive(Adventurer entity){
        this.passive = AbilityManager.InitializeAbility("Elven Training", entity);
    }
}
