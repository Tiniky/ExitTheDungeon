using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAxe : Weapon {
    public BattleAxe(){
        WeaponName = "BATTLEAXE";
        WeaponType = WeaponType.MELEE;
        DamageType = DamageType.SLASHING;
        DMG = DieType.D8;
        DMGmult = 1;
        AttackModifier = MainSkill.STR;
        Icon = PrefabManager.IMG_BATTLE_AXE;
    }
}