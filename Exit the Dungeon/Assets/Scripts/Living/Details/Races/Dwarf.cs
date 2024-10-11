public class Dwarf : Race {
    
    public Dwarf(){
        this.RaceType = RaceType.DWARF;
        this.CharSize = Size.SMALL;
        this.CharSpeed = new Speed(25);
        this.Darkvision = true;
        this.RaceSkills = new SkillTree(2, 0, 0, 0);
    }

    public override void SetUpRacialPassive(Adventurer entity){
        this.passive = AbilityManager.InitializeAbility("Mark of Warding", entity);
    } 
}
