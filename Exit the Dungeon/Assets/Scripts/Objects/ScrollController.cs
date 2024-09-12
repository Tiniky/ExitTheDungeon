using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour {
    public string abilityName;
    public Animator animator;
    private GameObject _txt;
    private Adventurer _adventurer;
    
    void Start() {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        _adventurer = GameManager.Player();
    }

    private void AbsorbScroll() {
        Destroy(_txt);
        Destroy(gameObject);
        Ability ability = AbilityManager.InitializeAbility(abilityName, _adventurer);

        //check
        Debug.Log("orc: " + string.Join(", ", GameManager.Player().GetActives()));
        Debug.Log("human: " + string.Join(", ", GameManager.AllyAdventurers()[0].GetActives()));
    }

    public void AwakenCutscene() {
        GameManager.CutSceneTrigger();
        CutsceneManager.StartUp();

        abilityName = SelectAbility();
        _txt = TextUIManager.CreateScrollText(abilityName);
        StartCoroutine(SignalEndOfCutscene());
    }

    private IEnumerator SignalEndOfCutscene() {
        yield return new WaitForSeconds(2.5f);
        AbsorbScroll();
        CutsceneManager.WrapUp();
        TextUIManager.UpdateInteractableText(false);
    }

    private string SelectAbility(){
        string ability = "";

        List<string> abilityPool = new List<string>();
        abilityPool.AddRange(PrefabManager.GENERAL_ABILITIES);

        switch(_adventurer.Class){
            case ClassType.BARBARIAN:
                abilityPool.AddRange(PrefabManager.BARBARIAN_ABILITIES);
                break;
            case ClassType.CLERIC:
                abilityPool.AddRange(PrefabManager.CLERIC_ABILITIES);
                break;
            case ClassType.ROGUE:
                abilityPool.AddRange(PrefabManager.ROGUE_ABILITIES);
                break;
            case ClassType.SORCERER:
                abilityPool.AddRange(PrefabManager.SORCERER_ABILITIES);
                break;
        }

        if(abilityPool.Count > 0){
            int randomIndex = UnityEngine.Random.Range(0, abilityPool.Count);
            ability = abilityPool[randomIndex];
        }

        return ability;
    }
}
