using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleState {
    private static bool _wasMainActionUsed, _isAttacking;

    public static void Reset(){
        _wasMainActionUsed = false;
        _isAttacking = false;
        ActionUIManager.ResetOOAForAll();
        AbilityUIManager.ResetOOAForAll();
    }

    public static bool CanUseAction(){
        return !_wasMainActionUsed;
    }

    public static void ActionUsed(){
        _wasMainActionUsed = true;

        ActionUIManager.ShowOOAForAll();
        AbilityUIManager.ShowOOAForAll();
    }

    public static bool IsCurrentlyAttacking(){
        return _isAttacking;
    }

    public static void DeclareAttack(bool _waitingForTarget){
        _isAttacking = _waitingForTarget;
    }

    public static void LockActions(){
        ActionUIManager.ShowOOAForAll();
        AbilityUIManager.ShowOOAForAll();
    }
}