DAY 0 - jan 8
picked version: Unity 2021.1.7f1 and generated the project
basic setup

WEEK 1
came up with a devplan, gathered resources and did some research
created a DungeonRoomNodeGraph class for storing the level graphs
created a DungeonRoomNode class which represent a room
created a DungeonRoomNodeType and DungeonRoomNodeTypeList to keep together what type of rooms are needed
implemented a simple node editor to make the dungeon graph creation easier
- on right click create a dungeon room node
- choose type (Entrance, Chest, Puzzle, Party member, Boss, Small, Medium, Large, Corridor)
- connect rooms with left click hold

WEEK 2
created a few test level graphs with the node graph editor
made small improvements to the editor because it was uncomfortable to use
- drag rooms with left click hold
- connect rooms got swapped to right click hold
- select rooms with left click
- delete connections
- delete nodes
- added canvas grid to align the nodes and make it look nicer
- created the 5 dungeon room node graph for variations
isometric project research, planing best approach for procgen

WEEK 3
took a break from the procgen and started implementing some core classes
- static Die class that can be used for rolling with each dnd dice
- EntityDetails class for the common attributes of the living entities
- created a class for each Stat to keep the Single Resposibility Principle
- SkillTree class to store the skills (in int for now)
- abstract MainClass and abstract Race class
- created a basic class for each Class (Barbarian, Bard, Cleric, Druid, Fighter, Ranger, Rogue, Sorcerer) and each Race (Dragonborn, Dwarf, Elf, Goliath, Human, Orc, Tabaxi)

WEEKS OFF
new deadline: June 1
complete plan for thesis and the Steam version
updated dev plan

DAY 0 v2 - feb 8
- deleted Races and Classes not needed for the Thesis Edition
- cleaning up in the small classes

WEEK 1 v2
started implementing the battle system
- roll for initiative with EntityRoll class
- sort the battle queue based on the rolls
realised I'm started implementing base Ability class
cleaned up the SkillTree and created classes for each skill
cleaned up the Entity class (no more EntityDetails), created the Adventurer and Creature classes

WEEK 2 v2
implemented an AbilityHandler
- stores the usable abilities of the player
- tracks ability states and cooldowns
collected all the Adventurer's ability in on place with Initialize()
figured out the ideal pixel per units (char: 16, tile: 128, grid cell size: 3x, 1.5y, charpos: 0.35y)

WEEK 3 v2
switched to flat 2D :D
prepared demo plan
- (T1) player orc barbarian character (with Relentless Endurance and Rage) - DONE
- (T2) scene with collision (cage with party member, chest <- scroll of Extra Attack, pillar with gemstone, a hand to place the gemstone, enemies) - DONE
- (T3) player can move around with WASD - DONE
- (T4) character animation for moving around - DONE
- (T5) enemy prefab (Ogre) - DONE
- (T6) enemy idle animation - DONE
- (T7) battle trigger - DONE
- (T8) initiative roll - DONE
- (T9) player can move around with mouse click - DONE
- (T10) player movement swapped to mouse - DONE
- (T11) abilities of player (p:Relentless Endurance, a:Rage on/off) - DONE
- (T12) on party member's turn the abilites swap - DONE
- (T13) show ability cooldowns, attack rolls, AC, hit/miss, red on dmg, green on heal - DONE
- (T15) basic UI plan - DONE
- (T14) pulling the switch opens the cage - DONE
- (T16) the rescued party member appears in the UI - DONE
- (T17) the party member follows the player - DONE
- (T18) UI update for when the gem is picked up - DONE
- (T19) animation for the chest open with floating scroll - DONE
- (T20) when close to interactable press F text - DONE
- (T21) on battle trigger freeze movement - DONE
- (T22) icon of fight participants and the rolled number next to them - DONE
- (T23) the fight order appears on the right - DONE
- (T24) scene "change" after initiative roll - DONE
- (T25) an arrow iterates showing who's turn is this - DONE
- (T26) animation for mouse move - DONE
- (T27) HP UI element (responsive to dmg, health appears on top of it, no void when the slider goes down) - DONE
- (T28) gem counter UI element (only appears when first gem was picked up) - DONE
- (T29) Skills UI element - DONE 
- (T30) the party member prefab - DONE
- (T31) player sprite is before after creature depends on their position - DONE
- (T32) when the gate opens party member text box appears - DONE
- (T33) the party member gets interactable - DONE
- (T34) the door gets a reference of the prisoner - DONE
- (T35) upon entering the gem room the other doors close - DONE
- (T36) the chest room and the enemy room isn't accessable without the gem in hand - DONE
- (T37) the passive for Extra Attack appears at the top - DONE
- (T38) minimap appearing and following player movement - DONE
- (T39) animation + player move freeze for when the gate opens - DONE
- (T40) when the scroll appears it gets collected automatically and the name of the ability appears on screen - DONE
- (T41) snap fight participants to the closest tile - DONE
- (T42) hide mouse cursor - DONE
- (T43) fight UI, pass turn - DONE
- (T44) movement related stuff (click on and move to tiles only, move distance and limitations with related  highlight color) - DONE
- (T45) implement basic actions (basic attack, dash, ranged, shove) - DONE
- (T46) ExplanationText prefab for explaining UI elements (Skill, Actions, Passives, Abilites) - DONE
- (T47) inventory UI plan to check equipped stats - DONE
- (T48) inventory that appears on TAB - DONE
- (T49) pass turn - DONE
- (T50) implement death for Entity (skip from battlequeue, remove from tile, partymember - remove from healthbar group, player - death screen) - DONE

cleaned up unnecessary isometric sprites
started implementing the abilities, created class for Advantage and Disadvantage (T1)
created room template and a demo room with the necessary assets, layers and collision (T2)
player movement and animation setup (T3 + T4)
cam follows the player

WEEK 4 v2
implemented basic move to mouse (T9)
impemented the interactible class
made some modifications to the demo scene
created gem float and switch pull animation
implemented the interactable class switchcontroller
added highlight for the switch
implemented the interactable class chestcontroller
added animation and highlight for the chest

WEEK 5 v2
removed the highlight because performance issues, code clean up
gem controller set up
adventurer now counts how many gems are in the inventory
set up the Ogre class and prefab (T5)
created a prefab of all the interactable objects
created the ogre idle and move animations (T6)
set up the battle trigger
started implementing the initiative roll on trigger
implemented GameManager to initialize Adventurer
started optimizing the Adventurer, Race, MainClass, Skills, SkillTree, ... classes

WEEK 6 v2
finished up the optimization
initiative roll all set up (T8)
animation clean up so the player is set to idle when battle is triggered (T21)
started planing the mockup of the basic UI

WEEK 7 v2
finished up the basic UI mockup (T15)
implemented the health controller and the player behaviour classes
created the responsive healthbar with text that updates (T27)
created the UI element for the gem counter (T18) + (T28)
created the UI element part for the skills (T29)
created the cage door open animation (T14)
fixed the cage/door collision, you can now move behind the cage
created the human rogue character and imported the sprites

WEEK 8 v2
created the animations for the human rogue
created the prefab with collision (T30)
created the order in layer modifier function that checks who should be in front (T31)
updated the game manager to initialize all the living currently in the scene
created a DoorController who has the hostage field (T34)
created the PartyMemberBehaviour script and separated the CreatureBehaviour into a parent class and a PlayerBehaviour (T33)
set up the responsive HP UI element for the party member T(16)
added a thankstext UI element and created the FloatingText and FloatingTextController scripts (T32)
created the movement freeze when the switch is pulled also created the VoidController script for the cutscene (T39)
added the UI text that appears when near an interactable thing (T20)
imported a scroll texture and set up the scroll floating animation (T19)
created the triggers and now areas can become accessable and unaccessable (T35) + (T36)
finished up the scroll with the cutscene (T40)
imported an icon for the extra attack and updated the ScrollController (T37)

WEEK 9 v2
updated the move to mouse character animation (T26)
updated the PartyMemberBehaviour so it follows the player
set up the animations and updated the partymemberbehaviour script (T17)
added the party icons and made some adjustments to the UI
started setting up the Initiative Roll canvas object

... 3 week long break lol

WEEK 10 - apr 22
fixed movement stuttering
made the initiative roll fight participants responsive, it fills out the icon and roll
modified the roll so if it's matching the luck gets added to the number (T22)
added the combat button and a ButtonController that destroys the unnecessary UI clones
added the UI part for the battle queue and the arrow
connected the battle order with the backend (T23)
interactable grid, set up to be easily extended
hides mouse cursor when there is no fight (T42)
at the beginning of fight each participant is snapped on top of the closest tile (T41)

WEEK 11
created the ability bar, the basic actions UI part and the UI for turn pass (T43)
implemented the turn pass functionality for mouse click and Enter (T49)
extended the Settings with predefined key inputs
the arrow now iterates on passed turn (T25)
tile color indicates whose turn it is
finished all tile related things (T44)
- calculated player movement range with Manhattan distance
- on hover it shows if player can move to tile or not
- move to tile fixed
- updated UI to show movement
- player can only move the given amount of tiles and can only move on their turn
- movement resets on new turn
- the turn indicator follows the player's new location
set up the minimap (T38)
updated some UI icons
created some prefabs out of gameobjects already in scene to make things more organized
created a PrefabManager
created UIManager for creating a canvas and setting up UI properly
updated the SkillManager
created the HealthBarManager to instantiate healthbars dynamically
created the ActionUIManager to set up basic actions dynamically
added UI images to PrefabManager
created the ActionHover script that shows the label onhover
created the AbilityUIManager to set up the Icons for the abilities and connected it to the existing AbilityButtonHandler
created the PassiveUIManager for the active effects
added minimap, gem counter and cutscene accessories to the UIManager
optimized instantiating and UI holder creation with PrefabManager.InstantiatePrefab() and UIManager._CreateEmptyUIGameObject()
FightUIManager set up and connected to other scripts where there is a UI update

WEEK 12
created TextUIManager and configured the interactable text
created the CutsceneManager for dynamic voidzones
fixed covers appearing and thanks text
updated the GameManager so it instantiates the creatures
fixed the follower healthbar and interaction
passive appears correctly on start
created EnemyBehaviour for enemies
created the abilities needed for the demo set them up
ability appears correctly on start
fixed gem UI
created triggers for passive abilities
fixed scroll text and updated passive UI after scroll ability learned
all fight participants snap to tiles
large creatures occupie 2 tiles
created mockup for inventory UI
created the Action main class and the other basic action classes
swapped dodge to ranged
added button to actions so they are clickable now
removed the highlight from the tile and reworked it a little bit (no more hover, the distance is visible for the current adventurer)
configured move to tile for the party member as well
configured everything so Attack action finally works
configured the Dash action
the player can use only one action each fight turn
configured the Ranged action

WEEK 13
created Inventory UI prefab (T47)
created weapon class and some weapons
created InventoryManager and set up to initialize player data
InventoryManager now also handles groupmember data
inventory appears on Tab press (other UI elements turn invisible) (T48)
fixed extra attack infinite bug
added attack roll
fixed dash visual
implemented shove (can't be tested because Ogre is large)
created goblin prefab and merged it into the system
created camera controller so the camera is movable durin combat (arrows)
added battlestate to follow battle flow more easily
text announces who's turn it is
text announces the action taken
all actions work as they should (T45)
skillmanager now appears when inventory is open

WEEK 14
fixed inventory to be fully dynamic
created additional part for skillmanager so it also creates parts for allies
fixed healthbar prefabs

WEEK OFF xdd

WEEK 15 - jun 1
added fire to test player death
finished up Relentless Endurance
fixed healthbar spaghetti
reworked AbilityHandler
made modifications to the bottom half of the UI
added Log box, and set up basic LogManager
configured an automatic scroll for the logs
each fight action sends a message to the log box
on H hold all UI elements get hidden
camera during fight is moveable by mouse drag
added on cooldown, active and out of actions icons
set up cooldown basics for passives
set up color for HP change

WEEK 16
set up out of action for actions
actions only are available on adventurer's turn
fixed hover on empty ability slots
set up out of action, cooldown and currently active for abilites (T13)
actions and abilites can only be activated on adventurer's turn
Rage can be activated and deactivated (T11)
modified AbilityUIManager and PassiveUIManager so it swaps the loadout to match the current character (T12)
fixed gem bug

WEEK 17
implemented death for entities
the dead are skipped in battle queue
updated UI for dead - (T50)
removed tile holder if dead
modified actions so they can be deactivated if no target was selected
added mainmenu scene
created scenesmanager to load scenes properly
targeting actions can be deactivated
added json import for explanatory descriptions
created explanationcontroller
finished up explanation text prefabs

WEEK 18
explanation appears on action hover
action dmg information fills dynamically for current active character
explanation appears on ability hover
explanation appears on passive hover
explanation appears on skill hover (T46)
hover only works when cursor is visible
fixed skill visibility for party member

... few week hiatus

WEEK 19
removed Extra Attack (passive) and added Double Strike (active)
fixed ability activation
created JSON for abilites
updated scroll controller
fixed learning ability
fixed double strike appearing for other party members

WEEK 2x
started fixing the graph editor => decided to scrap it and start over
created the Editor class
planned out how would the classes interact
created the Room, RoomNode, Connection, ConnectionNode and LevelGraph classes
started setting up the basics

WEEK 2x + 1
added grid and zoom on mouse wheel scroll
planed functions:
- (T1) double left click on grid: create NONE roomnode - DONE
- (T2) left click on room: select room <= it becomes dragable - DONE
- (T3) mouse drag right click on grid: drag grid - DONE
- (T4) mouse drag left click on selected room: drag room - DONE 
- (T5) double left click on connection: select connection - DONE
- (T6) right click on connection: option to delete Connection - DONE
- (T7) right click on room: option to delete or configure room - DONE
- (T8) to connect rooms: double left click on from => double left click on to => F - DONE
- (T9) room connect alternative: right click on from + start connection => right click on to + end connection => F - DONE
- (T10) when configure room type the type names should be readable - DONE
- (T11) the first room should be the spawn room - DONE
- (T12) NONE and SPAWN should not be options - DONE
- (T13) on open the rooms already crated should be visible - DONE
- (T14) while editor is opened with a graph, double clicking anoth graph should load it - DONE
- (T15) only the name of the room should be drawn on it - DONE
- (T16) left click on grid to deselect - DONE
- (T17) double right click on room: type dropdown appears - DONE
- (T18) the spawn room and connected rooms should not be editable - DONE
- (T19) the selected room/connection info appears in the inspector - DONE
- (T20) EDIT turn ON/OFF - DONE
- (T21) if EDIT is ON zoom only 1.4 to 3 - DONE
- (T22) if EDIT is OFF zoom is available from 0.1 to 3 - DONE
- (T23) delete room - DONE
- (T24) delete connection - DONE
set up base logic for event handeling
set up grid drag - (T3)
when double click on empty grid, a new room spawns - (T1)
rooms appear on load - (T13)
rooms appear with only they name showing - (T15)
left click on grid deselects all - (T16)

WEEK 2x + 2
reworked event handeling
select room on left click - (T2)
drag selected room on left click drag (T4)
if graph is empty first room is spawn (T11)
fixed switching between graphs
selected room info appears
fixed editor delay on GUI update
created context menu for room and connection - (T6) + (T7)
experimented with the zoom - (T10)
focus is set on room on double right click - (T17)
added button to swap State between EDIT and INSPECT - (T20)
fixed zoom for EDIT and INSPECT - (T21) + (T22)
EDIT commands: all
INSPECT commands: zoom, grid drag, room select, room drag

WEEK 2x + 3
NONE and SPAWN visibility - (T12)
connection create with both methods - (T8) + (T9)
select connection - (T5)
spawn rooms + connected rooms are not configureable or deletable - (T10)
delete connection - (T24)
delete room - (T23)
fixed delete bug
created Dungeon Level graph
updated the Room Template prefab

... end of summer so new sprint ig xdd

WEEK 0
did some planning about the generator

WEEK 1
created the room template generation aka initializing new prefab for room from script
doorplanning
created the Door class and modified the Doorway class
created DoorInspector class to have fancy buttons on Doorway in the inspector
wrote developement plan
started developing a door selection
started setting up the room Settings
added validation for the template
added interactable tiles generation for the room template inspector
made some changes to the inspector to make the tileprefab and tilemap added automatically
added a validator for the template

WEEK 2
created 3-3 combat room and corridor templates
created the rest of the templates
created dev plan with deadlines
fixed github issues
created the Dungeon Generator class
started updating the GameManager
wrote DFS like algorithm for my custom graph
updated the Dungeon Generator class with more variables
handled template loading and template selection
created new class for instantiated rooms and corridors
created new class for Doorline
created flow chart for the generator
worked on the generator

WEEK end of sept
finally managed to generate correct maps with the generator
reworked the room templates

WEEK first week of oct
made some optimalizations to the dungeon generator
the dungeon level gets generated
the doors are marked WasUsed upon being selected correctly
the base dungeon also gets generated without Unity time out
||TO-DO: 
- figure out what's wrong with overlaps - DONE
- check why dead ends are a thing - DONE
- track which default templates are mostly used and select a lesser used one - DONE
- add collision to templates - DONE
- finish up templates - DONE
- figure out walls - DONE
- spawn the player in the spawn room - DONE
fixed overlap
fixed deadends
fixed default template selection
set up corridor and boss room templates
added collision for the rest of the templates
added things for the rest of the layers
modified the corner coordinates (they changed because of the walls)
made algorithm that instantiates a wall object where the door wasn't used and to fill next to corridors as well
spawned the player

made some planning:
||PROC GEN TODO
add triggers before doors that update where the player is - DONE
make the interactable tile invisible out of combat - DONE
spawn the party members to the party member spawns
instantiate the rest of the environment
create door wall prefabs - DONE
lock the door of the boss room and add gem check before it
configure gem room gimmick
spawn the min amount of monsters <= when party member rescued go through combat rooms and spawn more monsters
on combat triggers doors close
rework spanwpointhandler into smaller classes - DONE
set up spawns on the templates - DONE
verify if everything works as in the DemoScene

||BEHAVIOR TREE TODO
editor based on the level graph
possible actions: 
when idle walk around the spawn point
if combat on their turn
slecet target based on logic
decide which attack to use
check if target in reach
close gap if needed
attack
upon low health decide to run or continue to fight
!enemies can't leave room

||PRE SAVE SYTEM TODO
each level should have an item chest and a skill chest
common items could be: bonus vision, faster movement, 
fix movement speed issues
in inventory a bag for the items unlocked

||SAVE SYSTEM TODO
Main Menu: Continue (continues save file), New Game (confirmation + deletes save file), Exit
new scene showing the 4 characters <- only the first is selectable
unlocks for characters: hit the killing blow on 10 enemies with the given char
unlock for lvl up: hit the killing blow on 10 enemies with the given unlocked char
item pool showing all the items
the unavailable items are gray
on hover the available items show what they do
on hover the unavailable items show how to unlock them
at the bottom map selection
unlock maps: beat prev map boss
unlock infinite mode: unlock all characters
(infinite mode: no boss rooms, gem rooms and prison rooms, shorter maps, only one party member room, one tp room, boss on 6th 12th 18th... iterations) !NOTE: only the name and icon will be in the thesis version
secret character unlock: beat 5 bosses in a row in endless mode !NOTE: only the name and icon will be in the thesis version
item unlocks:
	- burn 10 times and die by fire at least once: fire immunity
	- use the orc passive 10 times: Stone of ressurection <- revives dead party members
item ideas: redistributes health among all party members, a magical shield reducing incoming damage, increases the accuracy and critical hit chance, something luck based !NOTE doesn't need to be implemented the name, icon and unlock is enough for the thesis version

||LOG SYTEM TODO
log when:
changing rooms
party member joins party
someone steps on fire
a lever is pulled
a chest is opened
a gem is picked up
the boss room unlocks
the boss room door was interacted without the needed gem amount
someone dies
someone gets revived
a passive triggers
a monster initiated combat

||ELSE TODO
add spotlight to characters - DONE
figure out why the walls are brighter - DONE
hover on monsters should show their HP and skill tree + any additional info + icon
on R rebuild dungeon
2-3 additional dungeon graph
the last 2 remaining party member prefabs
rework spawnpointhandler

WEEK first week of oct part2
created Dungeon class
added tracker for CurrentRoom and CurrentCorridor
added triggers to each entrance
added spotlight for player prefab and dimmed the global light
hid interactable tiles on load
fixed column collision bug
created the door prefabs
reworked spawn system
added spawns to the rooms