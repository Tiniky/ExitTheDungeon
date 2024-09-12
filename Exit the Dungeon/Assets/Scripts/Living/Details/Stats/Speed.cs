using System;
using System.Collections.Generic;
public class Speed {
    private int _speed, _movementALL, _movementLeft;
    private bool _wasDashUsed;

    public Speed(int Feet){
        this._wasDashUsed = false;
        this._speed = Feet;
        this._movementALL = Feet / 5;
        this._movementLeft = Feet / 5;
    }

    public int GetValue(){
        if(_wasDashUsed){
            return this._speed * 2;
        }
        return this._speed;
    }

    public int StepsAll(){
        return this._movementALL;
    }

    public int StepsLeft(bool shouldBeRoundedUp = false){
        if(_wasDashUsed){
            if(shouldBeRoundedUp){
                return this._movementLeft * 5 + this._movementALL * 5;
            }

            return this._movementLeft + this._movementALL;
        } else {
            if(shouldBeRoundedUp){
                return this._movementLeft * 5;
            }

            return this._movementLeft;
        }
    }

    public void StepsTaken(int steps){
        this._movementLeft -= steps;
    }

    public void ResetSteps(){
        this._movementLeft = this._movementALL;
    }

    public void Dash(bool state){
        _wasDashUsed = state;
    }
}
