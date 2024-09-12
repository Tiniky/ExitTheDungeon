using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon {
    public Bow(){
        WeaponName = "SHORTBOW";
        WeaponType = WeaponType.RANGED;
        DamageType = DamageType.PIERCING;
        DMG = DieType.D6;
        DMGmult = 1;
        AttackModifier = MainSkill.DEX;
        Icon = PrefabManager.IMG_SHORTBOW;
    }
}