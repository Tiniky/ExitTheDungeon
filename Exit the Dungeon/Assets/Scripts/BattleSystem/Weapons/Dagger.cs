using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon {
    public Dagger(){
        WeaponName = "DAGGER";
        WeaponType = WeaponType.MELEE;
        DamageType = DamageType.PIERCING;
        DMG = DieType.D4;
        DMGmult = 1;
        AttackModifier = MainSkill.DEX;
        Icon = PrefabManager.IMG_DAGGER;
    }
}