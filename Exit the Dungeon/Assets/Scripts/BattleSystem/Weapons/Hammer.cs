using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon {
    public Hammer(){
        WeaponName = "HAMMER";
        WeaponType = WeaponType.MELEE;
        DamageType = DamageType.BLUDGEONING;
        DMG = DieType.D6;
        DMGmult = 1;
        AttackModifier = MainSkill.STR;
        Icon = PrefabManager.IMG_HAMMER;
    }
}
