using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

public static class BattleManager {
    private static List<Entity> all;
    private static List<GameObject> allObj;
    private static Battle battle;
    private static GameObject _arrow;
    private static int _current, _turnCounter;
    private static Action _chosenAction;
    private static Ability _chosenAbility;
    private static bool _ongoingFight, _isFirst, _wasButtonPressed;

    public static void Initialize(){
        LogManager.AddMessage("Combat has been initiated.");
        _ongoingFight = true;
        _isFirst = true;
        _wasButtonPressed = false;
        all = new List<Entity>();
        allObj = new List<GameObject>();
        _turnCounter = 0;

        Adventurer player = GameManager.Player();
        all.Add(player);
        allObj.Add(GameManager.PlayerObj());

        foreach(GameObject ally in GameManager.Allies()){
            Adventurer partyMember = ally.GetComponent<Adventurer>();
            all.Add(partyMember);
            allObj.Add(ally);
        }

        foreach(GameObject enemy in GameManager.Enemies()){
            if(enemy.transform.position.x > player.transform.position.x){
                enemy.transform.localScale = new Vector3(-1, 1, 1);
            } else {
                enemy.transform.localScale = new Vector3(1, 1, 1);
            }

            all.Add(enemy.GetComponent<Creature>());
            allObj.Add(enemy);
        }

        GameManager.TurnOffTheLigths(allObj);
        battle = new Battle(all);
        _current = 0;
        TurnIndicatorSetup(_current);
        HandleCurrentFighter();
    }

    public static void InitializeArrow(GameObject arrow){
        _arrow = arrow;
    }

    public static void GoNext(){
        if(_isFirst){
            _isFirst = false;
        }

        ActionManager.CleanUpAfterTurn(_chosenAction);
        _chosenAction = null;
        AbilityManager.CleanUpAfterTurn(_chosenAbility);
        _chosenAbility = null;

        int next = _current + 1;
        TurnIndicatorSetup();

        if(next < all.Count){
            _current++;
            _arrow.transform.position += new Vector3(0, -100, 0);
        } else {
            _current = 0;
            _turnCounter += 1;
            _arrow.GetComponent<ArrowController>().Reset();
        }

        TurnIndicatorSetup(_current);
        Entity entity = battle.GetCurrent(_current);
        if(entity.isAlive){
            FightUIManager.UpdateMovementFor(entity);
            HandleCurrentFighter();
        } else {
            GoNext();
        }
    }

    private static void TurnIndicatorSetup(int curr = -1){
        AnnounceNextUp();
        int index = curr == -1 ? _current : curr;
        Entity nextUp = battle.GetCurrent(index);
        
        List<InteractableTile> tiles = new List<InteractableTile>();
        
        if(nextUp.Size == Size.MEDIUM || nextUp.Size == Size.SMALL){
            tiles = TileManager.Instance.StandsOn(nextUp.gameObject, 1);
        } else if(nextUp.Size == Size.LARGE) {
            tiles = TileManager.Instance.StandsOn(nextUp.gameObject, 2);
        }
        
        TileManager.Instance.Reset();

        if(curr == -1){
            foreach(InteractableTile tile in tiles){
                tile.ResetColor();
            }
        } else {
            foreach(InteractableTile tile in tiles){
                tile.IndicateTurn();
            }
        }
    }

    public static bool IsTheirTurn(Entity entity){
        if(battle.GetCurrent(_current) == entity){
            return true;
        }

        return false;
    }

    public static Entity CurrentFighter(){
        return battle.GetCurrent(_current);
    }

    public static bool IsTargetInActionRange(Entity entity) {
        if(!_ongoingFight || _chosenAction == null){
            return false;
        }

        int range;

        //rework with weapon range
        if(_chosenAction.ActionType == BasicAction.ATTACK){
            range = 1;
        } else if(_chosenAction.ActionType == BasicAction.RANGED){
            range = 10;
        } else if(_chosenAction.ActionType == BasicAction.SHOVE){
            range = 1;
        } else {
            range = 0;
        }

        return TileManager.Instance.IsTwoEntityInRange(CurrentFighter(), entity, range);
    }

    public static bool IsTargetInAbilityRange(Entity entity) {
        if(!_ongoingFight || _chosenAbility == null){
            return false;
        }

        int range;

        if(_chosenAbility.Range == AbilityRange.SELF){
            range = 0;
        } else if(_chosenAbility.Range == AbilityRange.CLOSE){
            range = 1;
        } else if(_chosenAbility.Range == AbilityRange.FAR){
            range = 10;
        } else {
            range = 0;
        }

        return TileManager.Instance.IsTwoEntityInRange(CurrentFighter(), entity, range);
    }

    public static bool IsTargetInRange(Entity target, int range){
        return TileManager.Instance.IsTwoEntityInRange(CurrentFighter(), target, range);
    }

    public static void MovementRangeOf(Entity entity){
        TileManager.Instance.Reset();

        if(entity is Adventurer){
            TileManager.Instance.ShowWalkRange(entity);
        }
    }

    public static void ActionChosen(Action action){
        _chosenAction = action;
    }

    public static void AbilityChosen(Ability ability){
        _chosenAbility = ability;
    }

    public static void SetTargetOfAction(Entity entity){
        if(BattleState.IsCurrentlyAttacking()){
            if(_chosenAction != null){
                _chosenAction.SetTarget(entity);
            } else if(_chosenAbility != null){
                _chosenAbility.SetTarget(entity);
            }
        }
    }

    public static void LightUpAllEnemy(){
        foreach(Entity entity in all){
            if(entity is Creature){
                Creature creature = (Creature)entity;
                creature.LightUp();

                Debug.Log(creature.EntityName + " lit up");
            }
        }

        Debug.Log("all enemy lighted up");
    }

    public static void LightUpAllAlly(){
        foreach(Entity entity in all){
            if(entity is Adventurer){
                Adventurer adventurer = (Adventurer)entity;
                adventurer.LightUp();
            }
        }
    }

    private static async void AnnounceNextUp(){
        if(_isFirst){
            await WaitForButtonPress();
        }

        FightUIManager.UpdateFightInfo("Next Up is " + CurrentFighter().EntityName);
        await Task.Delay(750);
        FightUIManager.ClearFightInfo();
    }

    private static async Task WaitForButtonPress() {
        var tcs = new TaskCompletionSource<bool>();

        while (!_wasButtonPressed){
            await Task.Delay(100);
        }

        tcs.SetResult(true);

        await tcs.Task;
    }

    public static void PressButton(){
        _wasButtonPressed = true;
    }

    public static bool WasButtonPressed(){
        return _wasButtonPressed;
    }

    public static int TurnCounter(){
        return _turnCounter;
    }

    private static void HandleCurrentFighter(){
        if(CurrentFighter().GetType() == typeof(Adventurer)){
            BattleState.Reset();
            AbilityUIManager.FillAbilitiesOf((Adventurer)CurrentFighter());
            PassiveUIManager.FillPassiveOf((Adventurer)CurrentFighter());
            ActionUIManager.UpdateActions();
        } else {
            BattleState.LockActions();
        }
    }

    public static void CheckForEndOfCombat(){
        List<Entity> fighters = battle.GetQueue();
        int living = 0;
        foreach(Entity entity in fighters){
            if(entity.isAlive){
                living++;
            }
        }

        if(living == GameManager.Allies().Count + 1){
            GameManager.EndCombatPhase();
        }
    }

    public static List<Entity> GetStillAlive(){
        if(battle == null){
            Debug.LogError("Battle is null.");
            return new List<Entity>();
        }

        List<Entity> fighters = battle.GetQueue();
        List<Entity> alive = new List<Entity>();

        foreach(Entity entity in fighters){
            if(entity.isAlive){
                alive.Add(entity);
            }
        }

        return alive;
    }

    public static List<Entity> GetStillAliveAdventurers(){
        return GetStillAlive().Where(entity => entity != null && entity.Type == Type.ALLY).ToList();
    }

    public static List<Entity> GetStillAliveMonsters(){
        return GetStillAlive().Where(entity => entity != null && entity.Type == Type.ENEMY).ToList();
    }

    public static List<GameObject> GetALL(){
        return allObj;
    }
}
