using System.Collections;
using System.Collections.Generic;

public class EntityRoll {
    public Entity Entity {get; set;}
    public int Roll {get; set;}

    public EntityRoll(Entity entity, int roll){
        this.Entity = entity;
        this.Roll = roll;
    }

    public void UpdateRoll(){
       Roll += Entity.SkillTree.GetLuckModif();
    }
}
