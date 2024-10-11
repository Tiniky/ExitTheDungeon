using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour {
    public string itemName;
    public Animator animator;
    private GameObject _txt;
    private Adventurer _adventurer;
    private SpriteRenderer _spriteRenderer;
    
    void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _adventurer = GameManager.Player();
    }

    private void AbsorbItem() {
        Destroy(_txt);
        Destroy(gameObject);
        //Item item = ItemManager.InitializeItem(itemName, _adventurer);
    }

    public void AwakenCutscene() {
        //target maybe chest??
        //GameManager.CutSceneTrigger();
        CutsceneManager.StartUp();

        itemName = SelectItem();
        _txt = TextUIManager.CreateItemText(itemName);
        StartCoroutine(SignalEndOfCutscene());
    }

    private IEnumerator SignalEndOfCutscene() {
        yield return new WaitForSeconds(2.5f);
        AbsorbItem();
        CutsceneManager.WrapUp();
        TextUIManager.UpdateInteractableText(false);
    }

    private string SelectItem(){
        string item = "";

        List<string> itemPool = new List<string>();
        itemPool.AddRange(PrefabManager.GetUnlockedItems());

        if(itemPool.Count > 0){
            int randomIndex = UnityEngine.Random.Range(0, itemPool.Count);
            item = itemPool[randomIndex];
        }

        return item;
    }
}
