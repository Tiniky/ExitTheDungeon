using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuAssetHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private GameObject _explanation;    private string _nameOfAsset;

    public void ConnectExplanation(GameObject obj, string noa){
        _explanation = obj;
        _nameOfAsset = noa;
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(Cursor.visible){
            CheckIfDescWasUpdated();
            _explanation.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        _explanation.SetActive(false);
    }

    private void CheckIfDescWasUpdated(){
        if(_nameOfAsset != null){
            ExplanationController ec = _explanation.GetComponent<ExplanationController>();
            SaveAsset asset = SaveManager.GetAsset(_nameOfAsset);

            if(asset.Unlocked){
                ec.SetDescription("SELECT");
            } else{
                switch(asset.Condition){
                    case "hr=10":
                        string hrKill = SaveManager.PlayerData["stats.hrKill"].Split(':')[1].Trim();
                        ec.SetDescription(hrKill + "/10 kills");
                        break;
                    case "es=10":
                        string esKill = SaveManager.PlayerData["stats.esKill"].Split(':')[1].Trim();
                        ec.SetDescription(esKill + "/10 kills");
                        break;
                    case "dc=10":
                        string dcKill = SaveManager.PlayerData["stats.dcKill"].Split(':')[1].Trim();
                        ec.SetDescription(dcKill + "/10 kills");
                        break;
                    case "kill=777":
                        string killCount = SaveManager.PlayerData["stats.killCount"].Split(':')[1].Trim();
                        ec.SetDescription(killCount + "/777 kills");
                        break;
                    case "dmg=10k":
                        string dmgDealt = SaveManager.PlayerData["stats.dmgDealt"].Split(':')[1].Trim();
                        ec.SetDescription(dmgDealt + "/10k dmg");
                        break;
                    case "rooms=150":
                        string roomsCleared = SaveManager.PlayerData["stats.roomsCleared"].Split(':')[1].Trim();
                        ec.SetDescription(roomsCleared + "/150 rooms");
                        break;
                    default:
                        ec.SetDescription("???");
                        break;
                }   
            }
        }
    }
}
