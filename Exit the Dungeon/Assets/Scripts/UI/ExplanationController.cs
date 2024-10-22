using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ExplanationController : MonoBehaviour {
    private Text _description;
    private string _ogtxt;

    public void SetDescription(string desc){
        _ogtxt = desc;
        _description = transform.Find("ExplanationText").gameObject.GetComponent<Text>();
        _description.text = desc;
    }

    public void SetUpDescription(string desc, BasicAction action) {
        SetDescription(desc);
        UpdateText(action);
    }

    public void UpdateText(BasicAction action){
        string txt = _ogtxt;
        string dietxt;
        Adventurer adventurer = GameManager.Player();
        Weapon weapon = null;

        if (GameManager.Phase == GamePhase.COMBAT){
            adventurer = (Adventurer)BattleManager.CurrentFighter();
        }

        if (action == BasicAction.ATTACK){
            weapon = adventurer.Melee;
            if (weapon == null) {
                dietxt = "N/A";
            } else {
                dietxt = ConvertDieToString(weapon.DMG, weapon.DMGmult);
            }
            _description.text = ReplacePlaceholder(txt, dietxt);

        } else if (action == BasicAction.RANGED){
            weapon = adventurer.Ranged;
            if (weapon == null) {
                dietxt = "N/A";
            } else {
                dietxt = ConvertDieToString(weapon.DMG, weapon.DMGmult);
            }

            _description.text = ReplacePlaceholder(txt, dietxt);
        }
    }

    private string ReplacePlaceholder(string text, string replacement){
        return text.Replace("*", replacement);
    }

    private string ConvertDieToString(DieType dt, int mult){
        StringBuilder sb = new StringBuilder();
        sb.Append(mult.ToString());
        sb.Append(dt.ToString());
        return sb.ToString();
    }
}
