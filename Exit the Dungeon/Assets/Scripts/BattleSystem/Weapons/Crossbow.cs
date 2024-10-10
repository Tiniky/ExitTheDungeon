using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : Weapon {
    public Crossbow(){
        WeaponName = "CROSSBOW";
        WeaponType = WeaponType.RANGED;
        DamageType = DamageType.PIERCING;
        DMG = DieType.D8;
        DMGmult = 1;
        AttackModifier = MainSkill.DEX;
        Icon = PrefabManager.IMG_CROSSBOW;
    }
}
