using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAsset {
    public string Name;
    public bool Unlocked;
    public string Condition;

    public SaveAsset(string name, bool unlocked, string condition){
        this.Name = name;
        this.Unlocked = unlocked;
        this.Condition = condition;
    }
}
