using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuAssetHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private GameObject _explanation;
    private string _nameOfAsset;

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

                        string hrKill = SaveManager.PlayerData["hrKill"];
                        ec.SetDescription(hrKill + "/10 kills");
                        break;
                    case "es=10":
                        string esKill = SaveManager.PlayerData["esKill"];
                        ec.SetDescription(esKill + "/10 kills");
                        break;
                    case "dc=10":
                        string dcKill = SaveManager.PlayerData["dcKill"];
                        ec.SetDescription(dcKill + "/10 kills");
                        break;
                    case "kill=777":
                        string killCount = SaveManager.PlayerData["killCount"];
                        ec.SetDescription(killCount + "/777 kills");
                        break;
                    case "dmg=10k":
                        string dmgDealt = SaveManager.PlayerData["dmgDealt"];
                        ec.SetDescription(dmgDealt + "/10k dmg");
                        break;
                    case "rooms=150":
                        string roomsCleared = SaveManager.PlayerData["roomsCleared"];
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
