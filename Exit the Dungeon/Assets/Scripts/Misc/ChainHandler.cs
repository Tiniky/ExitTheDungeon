using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainHandler : MonoBehaviour {
    public AssetType assetType;
    private bool _wasChecked = false;

    void Update(){
        if(SaveManager.WasLoaded && !_wasChecked){
            SaveManager.CheckIfUnlocked(assetType, gameObject);
            _wasChecked = true;
        }
    }
}

public enum AssetType{
    Character,
    Item,
    Map
}