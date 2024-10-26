using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : MonoBehaviour {
    public static GameManager Instance {get; private set;}
    private static GameObject _cam;
    private static CinemachineVirtualCamera _cvc;

    //menu things
    //public static string SelectedCharacter; //<- will be used
    public static string SelectedCharacter = "OrcBarbarian";
    
    private static int CurrentLevel = 0;

    //dungeon things
    public List<DungeonLevel> DungeonLevels;
    public static InstantiatedRoom CurrentRoom;
    public static InstantiatedCorridor CurrentCorridor;
    public static Dungeon Dungeon;

    //global thing
    public static int Gem = 0;
    public static GamePhase Phase;
    public static bool IsPaused = false;
    private static bool WasSceneChange = false;
    public delegate void KeyAction(KeyCode key);
    private static Dictionary<KeyCode, KeyAction> ActionKeys;

    //living
    private static GameObject _partyHolder;
    private static GameObject _player;
    private static Adventurer _adventurer;
    private static CreatureBehaviour _playerBehaviour;
    private static GameObject[] _allyOptions;
    private static List<GameObject> _partyMembers;
    private static List<GameObject> _rescued;
    private static Dictionary<InstantiatedRoom, GameObject> _memberHostageLocation;
    private static List<GameObject> _enemiesList;
    
    //room related
    private static Dictionary<InstantiatedRoom, Vector2> _allySpawnPoints;
    private static Dictionary<InstantiatedRoom, List<Vector2>> _enemySpawnPoints;
    private static Dictionary<InstantiatedRoom, GameObject> _doors;
    private static Dictionary<InstantiatedRoom, List<InteractableObj>> _interactables;
    private static Dictionary<InstantiatedRoom, bool> _wasCleared;

    //from save - needs to be implemented
    public static bool HasKey = false;

    //other
    private static int _keyPressCount = 0;
    private static float _firstKeyPressTime = 0f;
    private static readonly float _keyPressInterval = 2f;
    
    //testing
    public static bool IsGodModeOn = true;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        SetUpGame();
    }

    private void Update(){
        HandleState();
        HandleInputs();
        
        foreach(GameObject ally in _partyMembers){
            CheckIfPlayerBehindCreature(ally);
        }

        /*if(Phase == GamePhase.COMBAT){
            foreach(GameObject enemy in _enemies[CurrentRoom]){
                CheckIfPlayerBehindCreature(enemy);
            }
        }*/
    }

    private void SetUpGame(){
        Phase = GamePhase.INIT;
        GenerateLevel(CurrentLevel);
        InitializeGame();
        Phase = GamePhase.ADVENTURE;
        WasSceneChange = true;
    }

    public void HandleState(){
        switch(Phase){
            case GamePhase.ADVENTURE:
                if(WasSceneChange){
                    TurnOnOffMTM(false);
                    WasSceneChange = false;
                }

                CheckForRestart();

                if(!_playerBehaviour.CheckIfCreatureCanMove()){
                    StartCoroutine(RestartMovementCoroutine());
                }
                break;
            case GamePhase.COMBAT:
                if(WasSceneChange){
                    FreezePlayerMovement();
                    TurnOnOffMTM(true);
                    WasSceneChange = false;
                }

                //need to check if works properly
                foreach(var actionKey in ActionKeys){
                    if(Input.GetKeyDown(actionKey.Key)){
                        actionKey.Value(actionKey.Key);
                    }
                }

                if(Input.GetKeyDown(Settings.PASSTURN) && BattleManager.WasButtonPressed()){
                    BattleManager.GoNext();
                    Debug.Log("turn passed");
                }
                break;
            case GamePhase.CUTSCENE:
                break;
            case GamePhase.PAUSE:
                break;
            case GamePhase.WIN:
                break;
            case GamePhase.LOSE:
                break;
            default:
                break;
        }
    }

    private void GenerateLevel(int lvl){
        Debug.Log("GameManager - generate lvl");
        DungeonLevel level = DungeonLevels[lvl];
        bool success = DungeonGenerator.GenerateDungeon(level);
        Debug.Log("GameManager - generation is success = " + success);
        if(success){
            Dungeon = DungeonGenerator.GetDungeon();
        }
    }

    private void InitializeGame(){
        PrefabManager.Initialize();
        TileManager.Instance.Initialize();
        _cam = GameObject.FindGameObjectWithTag("MainCamera");
        _partyHolder = new GameObject("PARTY");
        
        InitializePlayer();
        InitializeRooms();
        InitializePartyMembers();
        InitializeEnvironment();

        ActionKeys = new Dictionary<KeyCode, KeyAction> {
            {Settings.ATTACK, ActionUIManager.ActivateActionWithKey},
            {Settings.DASH, ActionUIManager.ActivateActionWithKey},
            {Settings.RANGED, ActionUIManager.ActivateActionWithKey},
            {Settings.SHOVE, ActionUIManager.ActivateActionWithKey},
            {Settings.ABILITY1, AbilityUIManager.ActivateAbilityWithKey},
            {Settings.ABILITY2, AbilityUIManager.ActivateAbilityWithKey},
            {Settings.ABILITY3, AbilityUIManager.ActivateAbilityWithKey},
            {Settings.ABILITY4, AbilityUIManager.ActivateAbilityWithKey}
        };

        _cvc = _cam.GetComponent<CinemachineVirtualCamera>();
        _cvc.m_Follow = _player.transform;
        
        InitializeUI();
        Cursor.visible = false;
    }

    private void InitializePlayer(){
        Debug.Log("GameManager - initializing player");
        Vector3 spawnPoint = Dungeon.GetSpawnPointOfPlayer();
        GameObject charPrefab = PrefabManager.GetCharacterPrefab(SelectedCharacter, true);

        _player = Instantiate(charPrefab, spawnPoint, Quaternion.identity, _partyHolder.transform);
        
        _adventurer = _player.GetComponent<Adventurer>();
        _playerBehaviour = _player.AddComponent<PlayerBehaviour>();
        _player.AddComponent<PlayerMovement>();
        _player.AddComponent<AnimationController>();
        Light2D light = _player.GetComponent<Light2D>();
        light.enabled = true;
        //light needs to be proportionate to the character's vision
        
        //todo: store character lvl somewhere
        _adventurer.Initialize(5, true);
        _player.name = _adventurer.EntityName;
        _playerBehaviour.Initialize(_adventurer.HP.GetValue());
        _adventurer.Behaviour = _playerBehaviour;
        Debug.Log("GameManager - Adventurer's health: " + _adventurer.HP.GetValue());
    }

    private void InitializeRooms(){
        Dungeon.TurnOffInteractableTiles();
        _wasCleared = new Dictionary<InstantiatedRoom, bool>();
        _allySpawnPoints = new Dictionary<InstantiatedRoom, Vector2>();
        _enemySpawnPoints = new Dictionary<InstantiatedRoom, List<Vector2>>();

        foreach(InstantiatedRoom room in Dungeon.Rooms){
            if(room.Room.Type == RoomType.SPAWN){
                _wasCleared.Add(room, true);
            } else{
                _wasCleared.Add(room, false);
            }
        }

        Dictionary<InstantiatedRoom, Vector2> partyMemberSpawnPoints = Dungeon.GetSpawnPointsOfPartyMember();
        foreach(KeyValuePair<InstantiatedRoom, Vector2> entry in partyMemberSpawnPoints){
           _allySpawnPoints.Add(entry.Key, entry.Value);
        }
        Debug.Log("GameManager - ally spawn points: " + _allySpawnPoints.Count);

        Dictionary<InstantiatedRoom, List<Vector2>> enemySpawnPoints = Dungeon.GetSpawnPointsOfEnemies();
        foreach(KeyValuePair<InstantiatedRoom, List<Vector2>> entry in enemySpawnPoints){
            _enemySpawnPoints.Add(entry.Key, entry.Value);
        }
        Debug.Log("GameManager - enemy spawn points: " + _enemySpawnPoints.Count);
    }

    private void InitializePartyMembers(){
        _memberHostageLocation = new Dictionary<InstantiatedRoom, GameObject>();
        _partyMembers = new List<GameObject>();
        _rescued = new List<GameObject>();
        _allyOptions = PrefabManager.ALLIES.ToArray();
        Debug.Log("GameManager - ally options: " + _allyOptions.Length);
        int option = 0;

        foreach(KeyValuePair<InstantiatedRoom, Vector2> entry in _allySpawnPoints){
            Vector3 spawnPoint = new Vector3(entry.Value.x + entry.Key.CurrentPosition().x, entry.Value.y + entry.Key.CurrentPosition().y, 0);
            GameObject allyObj = Instantiate(_allyOptions[option], spawnPoint, Quaternion.identity, _partyHolder.transform);
            Debug.Log("GameManager - ally spawning at: " + spawnPoint.ToString());
            
            _memberHostageLocation.Add(entry.Key, allyObj);
            _partyMembers.Add(allyObj); 
            Adventurer allyAdventurer = allyObj.GetComponent<Adventurer>();
            allyAdventurer.Initialize(5);
            allyObj.name = allyAdventurer.EntityName;
            PartyMemberBehaviour partyMember = allyObj.AddComponent<PartyMemberBehaviour>();
            partyMember.Initialize(allyAdventurer.HP.GetValue());
            allyAdventurer.Behaviour = partyMember;
            Debug.Log("GameManager - " +allyAdventurer.EntityName + "'s health: " + allyAdventurer.HP.GetValue());
            option++;
        }
    }

    private static void InitializeEnemies(InstantiatedRoom nextRoom){
        _enemiesList = new List<GameObject>();
        List<Vector2> spawnPoints = _enemySpawnPoints[nextRoom];
        Dictionary<Vector2, bool> spawnPointsDict = new Dictionary<Vector2, bool>();
        foreach(Vector2 point in spawnPoints){
            spawnPointsDict.Add(point, false);
        }

        List<string> monsters = EnemySpawnManager.SpawnEnemies(nextRoom.RoomObj.GetComponent<EnemySpawnPoint>());
        GameObject enemyHolder = new GameObject("EnemyHolder");
        enemyHolder.transform.parent = nextRoom.RoomObj.transform;
        _enemiesList.Clear();


        foreach(string monster in monsters){
            int index = UnityEngine.Random.Range(0, spawnPointsDict.Count);
            KeyValuePair<Vector2, bool> randomEntry = spawnPointsDict.ElementAt(index);

            while(randomEntry.Value){
                index = UnityEngine.Random.Range(0, spawnPointsDict.Count);
                randomEntry = spawnPointsDict.ElementAt(index);
            }

            spawnPointsDict[randomEntry.Key] = true;
            Vector3 spawnPoint = new Vector3(randomEntry.Key.x + nextRoom.CurrentPosition().x, randomEntry.Key.y + nextRoom.CurrentPosition().y, 0);

            GameObject enemy = null;
            switch(monster){
                case "OGRE":
                    enemy = Instantiate(PrefabManager.OGRE, spawnPoint, Quaternion.identity, enemyHolder.transform);
                    break;
                case "GOBLIN":
                    enemy = Instantiate(PrefabManager.GOBLIN, spawnPoint, Quaternion.identity, enemyHolder.transform);
                    break;
            }
            Creature enemyCreature = enemy.GetComponent<Creature>();
            EnemyBehaviour creature = enemy.GetComponent<EnemyBehaviour>();
            enemyCreature.Behaviour = creature;
            _enemiesList.Add(enemy);
            Debug.Log("GameManager - Enemy's health: " + enemyCreature.HP.GetValue());
        }
    }

    private void InitializeEnvironment(){
        _doors = new Dictionary<InstantiatedRoom, GameObject>();
        List<InstantiatedRoom> roomsWithDoors = Dungeon.DoorNeeded();

        foreach(InstantiatedRoom room in roomsWithDoors){
            GameObject doorHolder = room.RoomObj.transform.Find("Environment/DoorObjects").gameObject;
            Doorway dw = room.RoomObj.GetComponent<Doorway>();
            List<Door> doors = dw.Doors.Where(d => d.WasUsed).ToList();
             GameObject doorObj = null;

            if(doors.Count > 0){
                foreach(Door door in doors){
                    Vector3 doorPos = room.DoorPositionsUsed.FirstOrDefault(d => d.Direction == door.Direction).UpdatedMiddleDoor;
                    GameObject prefab = door.DoorPrefab;
                    doorObj = Instantiate(prefab, doorPos, Quaternion.identity, doorHolder.transform);
                    AdjustDoorObjPosition(doorObj, door.Direction);
                }
            }
            
            if(room.Room.Type != RoomType.BOSS){
                doorHolder.SetActive(false);
            } else{
                doorObj.AddComponent<BossDoorController>();
            }

            _doors.Add(room, doorHolder);
        }

        _interactables = new Dictionary<InstantiatedRoom, List<InteractableObj>>();
        List<InstantiatedRoom> roomsWithInteractables = Dungeon.InteractableNeeded();
        List<ChestType> chestTypes = new List<ChestType> {ChestType.ITEM, ChestType.SCROLL};
        int index = 0;

        foreach(InstantiatedRoom room in roomsWithInteractables){
            Transform interactableHolderTransform = room.RoomObj.transform.Find("Environment/InteractableObj");
            
            if(interactableHolderTransform != null){
                GameObject interactableHolder = interactableHolderTransform.gameObject;
                InteractableHandler ih = room.RoomObj.GetComponent<InteractableHandler>();
                
                if(ih != null){
                    List<InteractableObj> interactables = new List<InteractableObj>();
                    DoorController dc = null;
                    bool roomHasSwitch = false;
                    List<Vector2> possibleSwitchPos = new List<Vector2>();
                    InteractableObj interactable = null;

                    for(int i = 0; i < ih.Types.Count; i++){
                        InteractableType type = ih.Types[i];
                        Vector3 pos = new Vector3(ih.Positions[i].x + room.CurrentPosition().x, ih.Positions[i].y + room.CurrentPosition().y, 0);

                        switch(type){
                            case InteractableType.CHEST:
                                Vector3 chestPos = new Vector3(pos.x - 0.5f, pos.y - 0.4f, 0);
                                GameObject chest = Instantiate(PrefabManager.CHEST, chestPos, Quaternion.identity, interactableHolder.transform);
                                interactable = new InteractableObj(InteractableType.CHEST, pos, chest);
                                
                                ChestType chosen = chestTypes[index];
                                index++;
                                ChestController cc = chest.GetComponent<ChestController>();
                                cc.Type = chosen;
                                cc.Content = PrefabManager.GetContent(chosen);

                                break;
                            case InteractableType.GEM:
                                interactableHolder.transform.position = new Vector3(pos.x + 0.5f, pos.y + 2.35f, 0);
                                GameObject gem = Instantiate(PrefabManager.GEM, ih.Positions[i], Quaternion.identity, interactableHolder.transform);
                                interactable = new InteractableObj(InteractableType.GEM, pos, gem);
                                break;
                            case InteractableType.CELLDOOR:
                                GameObject DoorHolder = new GameObject("DoorHolder");
                                DoorHolder.transform.parent = interactableHolder.transform;
                                DoorHolder.transform.position = new Vector3(pos.x + 0.93f, pos.y - 1.32f, pos.z);
                                GameObject cellDoor = Instantiate(PrefabManager.DOOR, ih.Positions[i], Quaternion.identity, DoorHolder.transform);
                                interactable = new InteractableObj(InteractableType.CELLDOOR, pos, cellDoor);
                                dc = cellDoor.GetComponent<DoorController>();
                                dc.hostage = _memberHostageLocation[room].GetComponent<PartyMemberBehaviour>();
                                break;
                            case InteractableType.SWITCH:
                                roomHasSwitch = true;
                                possibleSwitchPos.Add(pos);
                                break;
                        }
                    }

                    if(roomHasSwitch){
                        Vector2 chosenPos = possibleSwitchPos[UnityEngine.Random.Range(0, possibleSwitchPos.Count)];
                        Vector3 switchPos = new Vector3(chosenPos.x + 0.5f, chosenPos.y + 0.5f, 0);
                        GameObject SwitchHolder = new GameObject("SwitchHolder");
                        SwitchHolder.transform.parent = interactableHolder.transform;
                        SwitchHolder.transform.position = switchPos;
                        GameObject switchObj = Instantiate(PrefabManager.SWITCH, chosenPos, Quaternion.identity, SwitchHolder.transform);
                        interactable = new InteractableObj(InteractableType.SWITCH, switchPos, switchObj);
                        SwitchController sc = switchObj.GetComponent<SwitchController>();
                        sc.connectedDoor = dc;
                    }

                    if(interactable != null){
                        interactables.Add(interactable);
                    }
                
                    _interactables.Add(room, interactables);
                }
            }
        }
    }

    private static void AdjustDoorObjPosition(GameObject doorObj, DoorDirection dir){
        switch(dir){
            case DoorDirection.UP:
                doorObj.transform.position = new Vector3(doorObj.transform.position.x + 0.5f, doorObj.transform.position.y + 1.5f, doorObj.transform.position.z);
                break;
            case DoorDirection.DOWN:
                doorObj.transform.position = new Vector3(doorObj.transform.position.x + 0.5f, doorObj.transform.position.y - 0.5f, doorObj.transform.position.z);
                break;
            case DoorDirection.RIGHT:
                doorObj.transform.position = new Vector3(doorObj.transform.position.x + 1.25f, doorObj.transform.position.y - 1.5f, doorObj.transform.position.z);
                break;
            case DoorDirection.LEFT:
                doorObj.transform.position = new Vector3(doorObj.transform.position.x - 0.25f, doorObj.transform.position.y - 1.5f, doorObj.transform.position.z);
                break;
        }
    }

    private void InitializeUI(){
        UIManager.Initialize();
    }

    public void EnterRoom(GameObject roomobj){
        InstantiatedRoom room = Dungeon.GetRoom(roomobj);
        if(room == null || room == CurrentRoom){
            return;
        }

        if(room != CurrentRoom){
            Debug.Log("GameManager - Entered room: " + room.RoomObj.name);
            LogManager.AddMessage(GetLogMessage(room));
        }

        CurrentRoom = room;
        CurrentCorridor = null;

        if(room.Room.Type == RoomType.COMBAT || room.Room.Type == RoomType.BOSS || room.Room.Type == RoomType.PRISON){
            TileManager.Instance.LoadCurrentRoom(room);
            HandleRoomDoors(true);
            StartCoroutine(Instance.InitializeCombat());
        }
    }

    private IEnumerator InitializeCombat(){
        yield return new WaitForSeconds(1.5f);
        BattleManager.Initialize();
    }
    
    public static void LeftRoom(GameObject corrobj){
        InstantiatedCorridor crd = Dungeon.GetCorridor(corrobj);
        if(crd == null || crd == CurrentCorridor){
            return;
        }

        CurrentCorridor = crd;
        InstantiatedRoom nextRoom = crd.GetOtherRoom(CurrentRoom);
        
        if((nextRoom.Room.Type == RoomType.COMBAT || nextRoom.Room.Type == RoomType.BOSS || nextRoom.Room.Type == RoomType.PRISON) && !_wasCleared[nextRoom]){
            InitializeEnemies(nextRoom);
        }

        CurrentRoom = null;
        Debug.Log("GameManager - Entered corridor: " + CurrentCorridor.CorridorObj.name);
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
        return _enemiesList;
        //return _enemies[CurrentRoom];
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
            Debug.Log("GameManager - tab was pressed");
        }
    }

    private void CheckForRestart(){
        if(Input.GetKeyDown(KeyCode.R)){
            if(_keyPressCount == 0){
                _firstKeyPressTime = Time.time;
            }

            _keyPressCount++;

            if(_keyPressCount == 3 && (Time.time - _firstKeyPressTime <= _keyPressInterval)){
                Debug.Log("GameManager - Restarting level");
                
                Destroy(_partyHolder);
                DungeonGenerator.DestroyDungeon();
                Instance.SetUpGame();

                _keyPressCount = 0;
            } else if(Time.time - _firstKeyPressTime > _keyPressInterval){
                _keyPressCount = 1;
                _firstKeyPressTime = Time.time;
            }
        }
    }

    public static bool InFight(){
        return Phase == GamePhase.COMBAT;
    }

    public static void FightStarted(List<EntityRoll> rolls){
        Phase = GamePhase.COMBAT;
        WasSceneChange = true;
        FightUIManager.InitiativeRollSetup(rolls);
        SwapTilesVisibility(true);
        Cursor.visible = true; 
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
        return distance <= 2.5f;
    }

    public static void CutSceneTrigger(Transform target){
        FreezePlayerMovement();
        _cvc.m_Follow = target;
    }

    public static void FreezePlayerMovement(){
        if(_playerBehaviour.CheckIfCreatureCanMove()){
            _playerBehaviour.DenyMovement();
            _player.GetComponent<PlayerMovement>().enabled = false;
        }
    }

    private IEnumerator RestartMovementCoroutine() {
        yield return new WaitForSeconds(2.5f);
        _playerBehaviour.AllowMovement();
        _player.GetComponent<PlayerMovement>().enabled = true;
        _cvc.m_Follow = _player.transform;
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
        FightPositionSetup();
        FightUIManager.InitVisibility(false);
        List<GameObject> fighters = FightUIManager.GetParticipantClones();
        foreach(GameObject clone in fighters){
            Destroy(clone);
        }

        fighters.Clear();
        FightUIManager.FightVisibility(true);
    }

    public static void EndCombatPhase(){
        Phase = GamePhase.ADVENTURE;
        WasSceneChange = true;
        FightUIManager.FightVisibility(false);
        SwapTilesVisibility(false);
        TileManager.Instance.ReleaseTiles();
        HandleRoomDoors(false);
        HandleFightAftermath();
        _wasCleared[CurrentRoom] = true;
        LogManager.AddMessage("Combat is over. The room has been cleared.");

        foreach(GameObject enemy in _enemiesList){
            Destroy(enemy);
        }

        List<GameObject> uiClones = FightUIManager.GetFightQueueClones();
        foreach(GameObject clone in uiClones){
            Destroy(clone);
        }

        Cursor.visible = false;
    }

    private static void SwapTilesVisibility(bool state){
        Dungeon.SetVisibilityOfTiles(state);
    }

    public static void FightPositionSetup(){
        List<GameObject> participants = new List<GameObject>();
        participants.AddRange(BattleManager.GetALL());

        foreach(GameObject fighter in participants){
            if(fighter.GetComponent<BoxCollider2D>() != null){
                fighter.GetComponent<BoxCollider2D>().enabled = false;
            }
            
            Vector3 prevPos = fighter.transform.position;
            TileManager.Instance.SnapToClosestTile(fighter);
            Debug.Log("GameManager - " + fighter.name + " snapped from " + prevPos + " to " + fighter.transform.position);
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

    public static void HandleRoomDoors(bool state){
        GameObject doorHolder = _doors[CurrentRoom];
        doorHolder.SetActive(state);
    }

    private static string GetLogMessage(InstantiatedRoom room){
        string type = room.Room.Type.ToString();
        string info = "";

        switch(type){
            case "SPAWN":
                info = "Entered the room where it all began.";
                break;
            case "COMBAT":
                info = "Entered a room full of hostile creatures. Roll for initiative!";
                break;
            case "PRISON":
                GameObject hostage = _memberHostageLocation[room];
                if(_rescued.Contains(hostage)){
                    info = "Entered an empty prison like room.";
                } else{
                    info = "Entered a prison like room. Wait is that " + hostage.name +  "?!";
                }
                break;
            case "TREASURE":
                info = "Entered a secret room. Uuuu what's in the chest?";
                break;
            case "GEM":
                info = "Entered a room with a shiny.. floating.. gem. Nothing suspicious about it.";
                break;
            case "BOSS":
                info = "Entered the point of no return. Wait you're here already?";
                break;
        }

        return info;
    }

    public static void TurnOffTheLigths(List<GameObject> allObj){
        foreach(GameObject obj in allObj){
            Light2D light = obj.GetComponent<Light2D>();
            if(light != null){
                light.enabled = false;
            }
        }

        GameObject lightObj = GameObject.FindWithTag("light");
        if(lightObj != null){
            Light2D mainLight = lightObj.GetComponent<Light2D>();
            if(mainLight != null){
                mainLight.intensity = 1.25f;
            }
        }
    }

    public static void HandleFightAftermath(){
        GameObject lightObj = GameObject.FindWithTag("light");
        if(lightObj != null){
            Light2D mainLight = lightObj.GetComponent<Light2D>();
            if(mainLight != null){
                mainLight.intensity = 0.5f;
            }
        }

        Light2D light = _player.GetComponent<Light2D>();
        if(light != null){
            light.enabled = true;
            _player.GetComponent<BoxCollider2D>().enabled = true;
        }

        foreach(GameObject ally in _rescued){
            ally.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
