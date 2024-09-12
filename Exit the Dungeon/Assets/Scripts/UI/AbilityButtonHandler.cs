using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButtonHandler : MonoBehaviour {
    public Color activeColor, inactiveColor;
    private List<AbilityButton> _actives;

    public void SetUpActiveAbilityVisbility() {
        _actives = new List<AbilityButton>();
        Transform activeAbilities = transform;
        activeColor = new Color(0.6f, 0.6f, 0.6f, 1f);
        inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        if (activeAbilities != null){
            foreach (Transform ability in activeAbilities){
                AbilityButton abilityB = ability.GetComponent<AbilityButton>();
                if (abilityB != null) {
                    abilityB.SetVisibility(ability.name == "Ability" ? inactiveColor : activeColor);
                    _actives.Add(abilityB);
                }
            }
        }
    }
}