using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Cinemachine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private static GameObject _cam;

    //dungon things
    public List<DungeonLevel> DungeonLevels;
    private int currentLevel = 0;
    public static Room CurrentRoom;

    //global thing
    public static int Gem = 0;
    public static GamePhase Phase;

    //living
    private static GameObject _player;
    private static Adventurer _adventurer;
    private static CreatureBehaviour _playerBehaviour;
    // private static Dictionary<GameObject, Dictionary<Entity, CreatureBehaviour>>()
    private static GameObject[] _allyOptions;
    private static List<GameObject> _partyMembers;
    private static List<GameObject> _rescued;
    private static List<GameObject> _enemies;
    // private static Dictionary<DungeonRoom, GameObjec>
    
    //room related
    //private static List<DungeonRoom> rooms;
    private static GameObject tileHolder;
    private static TileManager tileManager;
    //private static Dictionary<GameObject, DungeonRoom> roomsWithTiles;
    private static List<GameObject> roomsWithTiles;
    private static List<Dictionary<Vector2, InteractableTile>> tiles;
    private static List<Vector3> _allySpawnPoints;
    private static List<Vector3> _enemySpawnPoints;

    private void Awake() {
        instance = this;
        Phase = GamePhase.INIT;
    }

    private void Update(){
        HandleState();
        /*HandleInputs();

        foreach(GameObject ally in _partyMembers){
            CheckIfPlayerBehindCreature(ally);
        }

        foreach(GameObject enemy in _enemies){
            CheckIfPlayerBehindCreature(enemy);
        }

        if(Phase == GamePhase.COMBAT){
            FreezePlayerMovement();
            TurnOnOffMTM(true);

            if(Input.GetKeyDown(Settings.PASSTURN) && BattleManager.WasButtonPressed()){
                BattleManager.GoNext();
                Debug.Log("turn passed");
            }

        } else {
            TurnOnOffMTM(false);

            if(!_playerBehaviour.CheckIfCreatureCanMove()){
                StartCoroutine(RestartMovementCoroutine());
            }
        }*/
    }

    public void HandleState(){
        switch(Phase){
            case GamePhase.INIT:
                GenerateLevel(currentLevel);
                InitializeGame();
                Phase = GamePhase.ADVENTURE;
                break;
        }
    }

    private void GenerateLevel(int lvl){
        Debug.Log("generate lvl");
        DungeonLevel level = DungeonLevels[lvl];
        bool success = DungeonGenerator.GenerateDungeon(level);
        Debug.Log("success = " + success);
    }

    private void InitializeGame(){
        PrefabManager.Initialize();
        _cam = GameObject.FindGameObjectWithTag("MainCamera");
        
        InitializePlayer();
        //InitializePartyMembers();
        //InitializeRooms();
        //InitializeEnemies();

        CinemachineVirtualCamera cvc = _cam.GetComponent<CinemachineVirtualCamera>();
        cvc.m_Follow = _player.transform;
        
        //InitializeUI();
        //Cursor.visible = false;
    }

    private void InitializePlayer(){
        Vector3 spawnPoint = DungeonGenerator.GetSpawnPointOfPlayer();
        _player = Instantiate(PrefabManager.ORC_BARBARIAN, spawnPoint, Quaternion.identity);
        PrefabManager.RemovePlayerFromAllies(PrefabManager.ORC_BARBARIAN);
        _adventurer = _player.GetComponent<Adventurer>();
        _playerBehaviour = _player.GetComponent<PlayerBehaviour>();
        //starter char: orc barbarian
        _adventurer.Initialize(RaceType.ORC, ClassType.BARBARIAN, "Végzetpöröly");
        _player.name = _adventurer.EntityName;
        _playerBehaviour.Initialize(_adventurer.HP.GetValue());
        _adventurer.Behaviour = _playerBehaviour;
        Debug.Log("Adventurer's health: " + _adventurer.HP.GetValue());
    }

    private void InitializeRooms(){
        _allySpawnPoints = new List<Vector3>();
        _enemySpawnPoints = new List<Vector3>();

        //for demo purposes
        _allySpawnPoints.Add(new Vector3(-10f, 5f, 0f));
        _enemySpawnPoints.Add(new Vector3(-45f, -23.2f, 0f));
        _enemySpawnPoints.Add(new Vector3(-44f, -26f, 0f));
        _enemySpawnPoints.Add(new Vector3(-44f, -21f, 0f));

        tileHolder = GameObject.Find("SceneExtras/InteractableFloorManager");
        tileManager = tileHolder.GetComponent<TileManager>();
        tiles = new List<Dictionary<Vector2, InteractableTile>>();
        roomsWithTiles = new List<GameObject>();

        //iterate through the rooms list and check for enemy rooms
        GameObject room = new GameObject("Room_1");
        room.transform.parent = tileHolder.transform;
        tiles.Add(tileManager.GenerateInteractableGrid(room, 22, 15, -52.5f, -31.5f));
        room.SetActive(false);
        roomsWithTiles.Add(room);
    }

    private void InitializePartyMembers(){
        _partyMembers = new List<GameObject>();
        _rescued = new List<GameObject>();
        _allyOptions = PrefabManager.ALLIES.ToArray();

        for(int i = 0; i < _allyOptions.Length; i++){
            GameObject allyObj = Instantiate(_allyOptions[i], _allySpawnPoints[i], Quaternion.identity);
            _partyMembers.Add(allyObj); 
            Adventurer allyAdventurer = allyObj.GetComponent<Adventurer>();
            allyAdventurer.Initialize(RaceType.HUMAN, ClassType.ROGUE, "Gibor");
            allyObj.name = allyAdventurer.EntityName;
            PartyMemberBehaviour partyMember = allyObj.GetComponent<PartyMemberBehaviour>();
            partyMember.Initialize(allyAdventurer.HP.GetValue());
            allyAdventurer.Behaviour = partyMember;
            Debug.Log("Ally's health: " + allyAdventurer.HP.GetValue());
        }

        //for demo
        DoorController cell = GameObject.FindGameObjectWithTag("cell").GetComponent<DoorController>();
        cell.hostage = _partyMembers[0].GetComponent<PartyMemberBehaviour>();
    }

    private void InitializeEnemies(){
        _enemies = new List<GameObject>();
        //for demo purpose
        GameObject enemy = Instantiate(PrefabManager.OGRE, _enemySpawnPoints[0], Quaternion.identity);
        enemy.name = "Enemy";
        _enemies.Add(enemy);
        Creature enemyCreature = enemy.GetComponent<Creature>();
        EnemyBehaviour creature = enemy.GetComponent<EnemyBehaviour>();
        enemyCreature.Behaviour = creature;
        Debug.Log("Ogre's health: " + enemyCreature.HP.GetValue());

        GameObject goblin1 = Instantiate(PrefabManager.GOBLIN, _enemySpawnPoints[1], Quaternion.identity);
        goblin1.name = "Enemy";
        _enemies.Add(goblin1);
        Creature goblin1Creature = goblin1.GetComponent<Creature>();
        EnemyBehaviour goblin1creature = goblin1.GetComponent<EnemyBehaviour>();
        goblin1Creature.Behaviour = goblin1creature;
        Debug.Log("goblin1's health: " + goblin1Creature.HP.GetValue());

        GameObject goblin2 = Instantiate(PrefabManager.GOBLIN, _enemySpawnPoints[2], Quaternion.identity);
        goblin2.name = "Enemy";
        _enemies.Add(goblin2);
        Creature goblin2Creature = goblin2.GetComponent<Creature>();
        EnemyBehaviour goblin2creature = goblin2.GetComponent<EnemyBehaviour>();
        goblin2Creature.Behaviour = goblin2creature;
        Debug.Log("goblin2's health: " + goblin2Creature.HP.GetValue());
    }

    private void InitializeUI(){
        UIManager.Initialize();
    }

    public static GameObject PlayerObj(){
        return _player;
    }

    public static Adventurer Player(){
        return _adventurer;
    }

    public static List<GameObject> Allies(){
        return _rescued;
    }

    public static List<Adventurer> AllyAdventurers(){
        List<Adventurer> adventurers = new List<Adventurer>();
        
        foreach (GameObject member in _partyMembers) {
            Adventurer adventurer = member.GetComponent<Adventurer>();
            if (adventurer != null) {
                adventurers.Add(adventurer);
            }
            
        }
        return adventurers;
    }

    public static List<GameObject> Enemies(){
        return _enemies;
    }

    private static void HandleInputs(){
        if(Input.GetKeyDown(KeyCode.H)){
            UIManager.HideUI();
        }
        
        if(Input.GetKeyUp(KeyCode.H)){
            UIManager.ShowUI();
        }

        if(Input.GetKeyDown(Settings.INVENTORY)){
            InventoryManager.InventoryVisibility();
            Debug.Log("tab was pressed");
        }

        //needs cleanup big time
        if(Phase == GamePhase.COMBAT){
            if(Input.GetKeyDown(Settings.ATTACK)){
                ActionUIManager.ActivateActionWithKey(Settings.ATTACK);
            }

            if(Input.GetKeyDown(Settings.DASH)){
                ActionUIManager.ActivateActionWithKey(Settings.DASH);
            }

            if(Input.GetKeyDown(Settings.RANGED)){
                ActionUIManager.ActivateActionWithKey(Settings.RANGED);
            }

            if(Input.GetKeyDown(Settings.SHOVE)){
                ActionUIManager.ActivateActionWithKey(Settings.SHOVE);
            }

            if(Input.GetKeyDown(Settings.ABILITY1)){
                AbilityUIManager.ActivateAbilityWithKey(Settings.ABILITY1);
            }

            if(Input.GetKeyDown(Settings.ABILITY2)){
                AbilityUIManager.ActivateAbilityWithKey(Settings.ABILITY2);
            }

            if(Input.GetKeyDown(Settings.ABILITY3)){
                AbilityUIManager.ActivateAbilityWithKey(Settings.ABILITY3);
            }

            if(Input.GetKeyDown(Settings.ABILITY4)){
                AbilityUIManager.ActivateAbilityWithKey(Settings.ABILITY4);
            }
        }
    }

    public static bool InFight(){
        return Phase == GamePhase.COMBAT;
    }

    public static void FightStarted(List<EntityRoll> rolls){
        Phase = GamePhase.COMBAT;
        FightUIManager.InitiativeRollSetup(rolls);
        SwapTilesVisibility(true);
        Cursor.visible = true; 
    }

    public static void FightEnded(){
         Phase = GamePhase.ADVENTURE;
        SwapTilesVisibility(false);
        _player.GetComponent<CapsuleCollider2D>().enabled = true;
        Cursor.visible = false; 
    }

    public static HitPoints GetPlayerHP(){
        return _adventurer.HP;
    }

    private void CheckIfPlayerBehindCreature(GameObject creature){
        if(IsNearby(creature.transform.position)){
            Renderer playerRenderer = _player.GetComponent<Renderer>();
            Renderer creatureRenderer = creature.GetComponent<Renderer>();

            if(_player.transform.position.y > creature.transform.position.y){
                playerRenderer.sortingOrder = creatureRenderer.sortingOrder - 1;
            } else {
                playerRenderer.sortingOrder = creatureRenderer.sortingOrder + 1;
            }
        }
    }

    public static bool IsNearby(Vector3 creaturePosition){
        float distance = Vector3.Distance(_player.transform.position, creaturePosition);
        return distance <= 1.5f;
    }

    public static bool IsInFollowingRange(Vector3 creaturePosition){
        float distance = Vector3.Distance(_player.transform.position, creaturePosition);
        return distance <= 2f;
    }

    public static void CutSceneTrigger(){
        FreezePlayerMovement();
    }

    public static void FreezePlayerMovement(){
        _playerBehaviour.DenyMovement();
        _player.GetComponent<PlayerMovement>().enabled = false;
    }

    private IEnumerator RestartMovementCoroutine() {
        yield return new WaitForSeconds(2.5f);
        _playerBehaviour.AllowMovement();
        _player.GetComponent<PlayerMovement>().enabled = true;
    }

    public static void Rescue(GameObject hostage){
        _rescued.Add(hostage);
        InventoryManager.AddAllyToInventory(_rescued.Count);
        SkillUIManager.FillOutSkills(hostage.GetComponent<Adventurer>(), _rescued.Count);
    }

    public static bool WasSomeoneSaved(){
        return _rescued.Count > 0;
    }

    public static void StartCombatPhase(){
        Phase = GamePhase.COMBAT;
        FightUIManager.InitVisibility(false);
        List<GameObject> fighters = FightUIManager.GetParticipantClones();

        foreach (GameObject clone in fighters){
            Destroy(clone);
        }

        fighters.Clear();
        FightUIManager.FightVisibility(true);
    }

    public static GameObject TileHolder(){
        return tileHolder;
    }

    private static void SwapTilesVisibility(bool state){
        //iterate throught roomWithTiles and search for dungeonroom.isPlayerInit()
        roomsWithTiles[0].SetActive(state);
    }

    public static void FightPositionSetup(List<GameObject> participants){
        foreach(GameObject fighter in participants){
            if(fighter.GetComponent<BoxCollider2D>() != null){
                if(fighter.GetComponent<Entity>() is Creature){
                    fighter.GetComponent<CapsuleCollider2D>().enabled = false;
                }

                fighter.GetComponent<BoxCollider2D>().enabled = false;
            } else {
                fighter.GetComponent<CapsuleCollider2D>().enabled = false;
            }
            tileManager.SnapToClosestTile(fighter);
            Debug.Log("snapped");
        }
    }

    private static void TurnOnOffMTM(bool state){
        _player.GetComponent<MoveToTile>().enabled = state;

        foreach(GameObject ally in _partyMembers){
            ally.GetComponent<MoveToTile>().enabled = state;
        }
    }

    public static void DestroyObj(GameObject obj){
        Destroy(obj);
    }
}
