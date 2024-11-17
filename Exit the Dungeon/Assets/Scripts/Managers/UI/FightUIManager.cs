using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FightUIManager {
    private static GameObject _initiativeScreenPrefab, _fighterInitiativePrefab, _arrowPrefab, _turnPrefab, _infoPrefab, _fighterQueuePrefab, _arrow, _turn, _infoText;
    private static List<GameObject> _participantClones;
    private static Dictionary<Entity, GameObject> _inqueueClones;
    private static GameObject _fightRelatedStuffHolder, _initiativeStuffHolder;
    private static readonly Color originalColor = new Color32(188, 236, 225, 255);

    public static void Initialize(Canvas canvas) {
        _initiativeScreenPrefab = PrefabManager.INITIATIVE;
        _fighterInitiativePrefab = PrefabManager.FIGHT_PARTICIPANT_INITIATIVE;
        _arrowPrefab = PrefabManager.ARROW;
        _turnPrefab = PrefabManager.TURN_PASS;
        _infoPrefab = PrefabManager.FIGHT_TEXT;
        _fighterQueuePrefab = PrefabManager.FIGHT_PARTICIPANT_QUEUE;
        _participantClones = new List<GameObject>();
        _inqueueClones = new Dictionary<Entity, GameObject>();

        _initiativeStuffHolder = PrefabManager.InstantiatePrefabV2(_initiativeScreenPrefab, canvas.gameObject, false, new Vector3(0f, 0f, 0f), "Initiative");
        _fightRelatedStuffHolder = UIManager._CreateEmptyUIGameObject("Fight Info", new Vector3(750f, -50f, 0f));
        _arrow = PrefabManager.InstantiatePrefabV2(_arrowPrefab, _fightRelatedStuffHolder, false, new Vector3(-100f, 250f, 0f), "Arrow");
        _turn = PrefabManager.InstantiatePrefabV2(_turnPrefab, _fightRelatedStuffHolder, false, new Vector3(-425f, -370f, 0f), "Turn");
        _infoText = PrefabManager.InstantiatePrefabV2(_infoPrefab, _fightRelatedStuffHolder, false, new Vector3(-765f, 350f, 0f), "Info");

        InitVisibility(false);
        FightVisibility(false);
    }

    public static void InitiativeRollSetup(List<EntityRoll> rolls){
        float distance = 80f;
        bool isEven = rolls.Count % 2 == 0 ? true : false;
        int participantsOnOneSide = (int)(rolls.Count / 2);
        
        for(int i = 0; i < rolls.Count; i++){
            if(isEven){
                GameObject participant = PrefabManager.InstantiatePrefabV1(_fighterInitiativePrefab, _initiativeStuffHolder, true, new Vector3(-2 * (participantsOnOneSide - i) * distance + distance, 0f, 0f), rolls[i].Entity.EntityName);
                FillParticipant(rolls[i], participant);
                _participantClones.Add(participant);
            } else {
                GameObject participant = PrefabManager.InstantiatePrefabV1(_fighterInitiativePrefab, _initiativeStuffHolder, true, new Vector3(-2 * (participantsOnOneSide - i) * distance, 0f, 0f), rolls[i].Entity.EntityName);
                FillParticipant(rolls[i], participant);
                _participantClones.Add(participant);
            }
        }

        InitVisibility(true);

        Debug.Log("InitRoll done");
    }

    private static void FillParticipant(EntityRoll participant, GameObject participantUI){
        Image iconImage = participantUI.transform.Find("Icon").GetComponent<Image>();
        iconImage.sprite = participant.Entity.entityIcon.sprite;
        Text rollAmountText = participantUI.transform.Find("RollBorder/RollAmount").GetComponent<Text>();
        rollAmountText.text = participant.Roll.ToString();
    }

    public static void FightQueueSetup(List<Entity> fightParticipants){
        float distance = 50f;

        for(int i=0; i<fightParticipants.Count; i++){
            GameObject participant = PrefabManager.InstantiatePrefabV1(_fighterQueuePrefab, _fightRelatedStuffHolder, true, new Vector3(0f, -2 * i * distance, 0f), fightParticipants[i].EntityName);
            _inqueueClones[fightParticipants[i]] = participant;
            FillFightData(participant, fightParticipants[i], i);
        }
    }

    private static void FillFightData(GameObject participantUI, Entity entity, int i){
        Image iconImage = participantUI.transform.Find("Token").GetComponent<Image>();
        iconImage.sprite = entity.entityIcon.sprite;
        Text hpAmountText = participantUI.transform.Find("HP").GetComponent<Text>();
        hpAmountText.text = entity.HP.GetValue().ToString() + " / " + entity.HP.GetMax().ToString();
        Text movementText = participantUI.transform.Find("Movement").GetComponent<Text>();
        movementText.text = entity.Speed.StepsLeft(true).ToString() + " / " + entity.Speed.GetValue().ToString();
    }

    public static void UpdateMovementFor(Entity entity, int stepsTaken = -1){
        GameObject currentPrefab = _inqueueClones[entity];
        Text movementText = currentPrefab.transform.Find("Movement").GetComponent<Text>();
        
        if(entity.isAlive){
            Speed movement = entity.Speed;
            if(stepsTaken == -1){
                movement.ResetSteps();
            } else {
                movement.StepsTaken(stepsTaken);
            }
            
            movementText.text = movement.StepsLeft(true).ToString() + " / " + movement.GetValue().ToString();
        } else {
            movementText.text = "-";
        }
    }

    public static void UpdateHPFor(Entity entity){
        GameObject currentPrefab = _inqueueClones[entity];
        Text hpAmountText = currentPrefab.transform.Find("HP").GetComponent<Text>();
        
        int previousHP = entity.HP.GetPreviousHP();
        int currentHP = entity.HP.GetValue();

        if (currentHP < previousHP || currentHP == 1) {
            hpAmountText.color = Color.red;
        } else if (currentHP > previousHP && currentHP != 1) {
            hpAmountText.color = Color.green;
        }

        hpAmountText.text = currentHP.ToString() + " / " + entity.HP.GetMax().ToString();

        CoroutineRunner.Instance.StartIt(RevertColorAfterDelay(hpAmountText, 2f, entity.isAlive));

        if(!entity.isAlive){
            hpAmountText.text = "DEAD";
            UpdateMovementFor(entity);
        }
    }

    private static IEnumerator RevertColorAfterDelay(Text textComponent, float delay, bool shouldChangeBack) {
        yield return new WaitForSeconds(delay);
        if(shouldChangeBack){
            textComponent.color = originalColor;
        }
    }

    public static List<GameObject> GetParticipantClones(){
        return _participantClones;
    }

    public static List<GameObject> GetFightQueueClones(){
        List<GameObject> clones = new List<GameObject>();
        foreach(Entity entity in _inqueueClones.Keys){
            clones.Add(_inqueueClones[entity]);
        }
        return clones;
    }

    public static void InitVisibility(bool shouldBeVisible) {
        _initiativeStuffHolder.SetActive(shouldBeVisible);
    }

    public static void FightVisibility(bool shouldBeVisible) {
        if(shouldBeVisible && (GameManager.Phase == GamePhase.COMBAT || GameManager.Phase == GamePhase.INITIATIVE)){
            _fightRelatedStuffHolder.SetActive(true);
        } else {
            _fightRelatedStuffHolder.SetActive(false);
        }
    }

    public static void UpdateFightInfo(string msg){
        Text infoText = _infoText.GetComponent<Text>();
        infoText.text = msg;

        if(!string.IsNullOrEmpty(msg) && !msg.StartsWith("Next Up")){
            LogManager.AddMessage(msg);
        }
    }

    public static void ClearFightInfo(){
        UpdateFightInfo("");
    }

    public static void ClearFightQueue(){
        _inqueueClones.Clear();
        _arrow.GetComponent<ArrowController>().Reset();
    }
}
