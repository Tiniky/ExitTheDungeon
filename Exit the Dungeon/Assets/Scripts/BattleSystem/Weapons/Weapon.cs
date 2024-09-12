using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon {
    private string _weaponName;
    private WeaponType _type;
    private DamageType _damageType;
    private DieType _dmg;
    private int _dmgMult;
    private MainSkill _modifier;
    private Image _icon;

    public string WeaponName{
        get{return _weaponName;}
        set{_weaponName = value;}
    }

    public WeaponType WeaponType{
        get{return _type;}
        set{_type = value;}
    }

    public DamageType DamageType{
        get{return _damageType;}
        set{_damageType = value;}
    }

    public DieType DMG{
        get{return _dmg;}
        set{_dmg = value;}
    }

    public int DMGmult{
        get{return _dmgMult;}
        set{_dmgMult = value;}
    }

    public MainSkill AttackModifier{
        get{return _modifier;}
        set{_modifier = value;}
    }

    public Image Icon {
        get{return _icon;}
        set{_icon = value;}
    }
}
