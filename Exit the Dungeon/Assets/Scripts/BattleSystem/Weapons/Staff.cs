using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon {
    public Staff(){
        WeaponName = "STAFF";
        WeaponType = WeaponType.MELEE;
        DamageType = DamageType.BLUDGEONING;
        DMG = DieType.D6;
        DMGmult = 1;
        AttackModifier = MainSkill.STR;
        Icon = PrefabManager.IMG_STAFF;
    }
}
