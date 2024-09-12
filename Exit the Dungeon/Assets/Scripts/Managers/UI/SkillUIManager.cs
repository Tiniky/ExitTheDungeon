using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SkillUIManager {
    private static GameObject _skillPrefab, _descriptionPrefab;
    private static Dictionary<Adventurer, Dictionary<string, GameObject>> _skills;
    private static Dictionary<Adventurer, ExplanationController> _descriptions;
    private static Dictionary<int, Vector3> _positions;
    private static GameObject _skillHolder;

    public static void Initialize() {
        _skillPrefab = PrefabManager.SKILL;
        _descriptionPrefab = PrefabManager.EXPLANATION_SMALL;
        _skills = new Dictionary<Adventurer, Dictionary<string, GameObject>>();
        _descriptions = new Dictionary<Adventurer, ExplanationController>();

        _positions = new Dictionary<int, Vector3>{
            {0, new Vector3(-630f, 475f, 0f)},
            {1, new Vector3(630f, 475f, 0f)},
            {2, new Vector3(-630f, -475f, 0f)},
            {3, new Vector3(630f, -475f, 0f)}
        };
        
        _skillHolder = UIManager._CreateEmptyUIGameObject("SkillHolder", new Vector3(0f, 0f, 0f));
        FillOutSkills(GameManager.Player(), 0);
    }

    public static void FillOutSkills(Adventurer adventurer, int index){
        Dictionary<string, GameObject> tempSkills = new Dictionary<string, GameObject>();
        GameObject skillHolder = UIManager._CreateEmptyUIGameObject("SkillHolder" + index, _positions[index]);
        skillHolder.transform.SetParent(_skillHolder.transform, false);

        for(int i = 0; i < Enum.GetValues(typeof(MainSkill)).Length; i++) {
            MainSkill currentSkill = (MainSkill)Enum.GetValues(typeof(MainSkill)).GetValue(i);
            string name = currentSkill.ToString();
            
            GameObject skill = PrefabManager.InstantiatePrefabV1(_skillPrefab, skillHolder, true, new Vector3(0f, -2 * i * 40f, 0f), name);
            tempSkills[name] = skill;
            FillSkillData(skill, currentSkill, adventurer, skillHolder);
        }

        _skills[adventurer] =  tempSkills;
    }

    private static void FillSkillData(GameObject skillObj, MainSkill skill, Adventurer adventurer, GameObject holder){
        int count = _skills.Count;
        bool shouldReverse = false;
        string name = skill.ToString();
        int value = adventurer.GetSkillValue(name, true);
        int raw = adventurer.GetSkillValue(name, false);
        Vector3 pos = skillObj.transform.position;

        if (count % 2 != 0){
            shouldReverse = true;
        }

        Text skillName = skillObj.transform.Find("SkillBackground/SkillName").GetComponent<Text>();
        skillName.text = name;
        Text skillValue = skillObj.transform.Find("SkillValueBackground/SkillValue").GetComponent<Text>();
        skillValue.text = value.ToString();

        SkillHover sh = skillObj.GetComponent<SkillHover>();
        GameObject explanationText;
        if(shouldReverse){
            explanationText = PrefabManager.InstantiatePrefabV1(_descriptionPrefab, holder, false, new Vector3(pos.x + 125f, pos.y, 0f), "ExplanationOf" + name);
        } else{
            explanationText = PrefabManager.InstantiatePrefabV1(_descriptionPrefab, holder, false, new Vector3(pos.x - 125f, pos.y, 0f), "ExplanationOf" + name);
        }
        
        ExplanationController ec = explanationText.GetComponent<ExplanationController>();
        sh.ConnectExplanation(explanationText);
        
        if(raw > 0){
            ec.SetDescription(raw.ToString());
        } else{
            ec.SetDescription("??");
        }

        explanationText.SetActive(false);

        _descriptions[adventurer] = ec;
    }

    public static GameObject GetSkillUIOF(Adventurer adventurer, string skillName){
        Dictionary<string, GameObject> tempSkills = new Dictionary<string, GameObject>();
        tempSkills = _skills[adventurer];
        return tempSkills[skillName];
    }

    public static void SetVisibility(bool shouldBeVisible) {
        _skillHolder.SetActive(shouldBeVisible);
    }
}
