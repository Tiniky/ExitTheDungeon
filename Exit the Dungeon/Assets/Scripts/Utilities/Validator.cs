using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* to validate editor and scriptable objects used by editor
*/

public static class Validator{
    /**
    * function that returns true if the string is empty
    **/
    public static bool IsEmptyString(string needsToBeChecked){
        if(needsToBeChecked == ""){
            return true;
        }

        return false;
    }

    /**
    * function that returns true if there is an error
    **/
    public static bool CheckEnumerableValues(IEnumerable enumerableObjToCheck){
        bool isThereAnyError = false;
        bool isObjEmpty = true;

        foreach(var item in enumerableObjToCheck){
            if(item != null){
                isObjEmpty = false;
            } else{
                isThereAnyError = true;
            }
        }

        if(isObjEmpty){
            isThereAnyError = true;
        }

        return isThereAnyError;
    }

    public static bool IsNull(UnityEngine.Object needsToBeChecked){
        if(needsToBeChecked == null){
            return true;
        }

        return false;
    }

    public static bool IsNull(object needsToBeChecked){
        if(needsToBeChecked == null){
            return true;
        }

        return false;
    }

    public static bool CheckPositiveValue(int needsToBeChecked, bool canBe0){
        bool isThereAnyError = false;

        if(canBe0){
            if(needsToBeChecked < 0){
                isThereAnyError = true;
            }
        } else{
            if(needsToBeChecked <= 0){
                isThereAnyError = true;
            }
        }

        return isThereAnyError;
    }
}
