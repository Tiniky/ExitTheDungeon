using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;

public class Dash : Action {
    private Entity _user;
    
    public Dash(){
        this.ActionType = BasicAction.DASH;
        this.WasUsed = false;
    }

    public override void Activate() {
        _user = BattleManager.CurrentFighter();

        if(BattleState.CanUseAction()){
            string info = GetInfo();
            Debug.Log(info);
            Announce(info);

            BattleState.ActionUsed();
            BattleManager.ActionChosen(this);
            _user.Speed.Dash(true);
            FightUIManager.UpdateMovementFor(_user);

            this.WasUsed = true;
        }
    }
    
    private async void Announce(string msg){
        FightUIManager.UpdateFightInfo(msg);
        await Task.Delay(1000);
        FightUIManager.ClearFightInfo();
    }

    private string GetInfo(){
        StringBuilder sb = new StringBuilder();
        sb.Append(_user.EntityName);
        sb.Append(" used ");
        sb.Append(ActionType.ToString());

        return sb.ToString();
    }
}
