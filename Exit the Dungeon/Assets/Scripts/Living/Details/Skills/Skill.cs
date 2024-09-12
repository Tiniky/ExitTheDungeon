using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill {
    private int _value, _rolledValue;

    public int Value {
        get{return _value;}
        set{_value = value;}
    }

    public int Rolled{
        get{return _rolledValue;}
        set{_rolledValue = value;}
    }
}
