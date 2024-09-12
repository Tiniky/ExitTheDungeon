using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Linq;

public class DungeonEditor : EditorWindow {
    private static LevelGraph _currentGraph;
    private static RoomNode _currentRoom;
    private static ConnectionNode _currentConnection;
    private static List<RoomNode> _roomNodes;
    private static List<ConnectionNode> _connectionNodes;
    private RoomNode _fromNode, _toNode;

    public static EditorState State;

    private static Vector2 _graphOffset;
    private static Vector2 _graphDrag;
    private Vector2 _ogPos;
    private bool _isDouble;
    private static bool _snapToGrid, _isSomethingInFocus, _isEditable;
    private static float _zoom;

    [MenuItem("Dungeon Level Editor", menuItem = "Window/Custom Editors - SZAKDOLGOZAT/Dungeon Level Editor")]

    // editor related things
    private static void ShowWindow() {
        var window = GetWindow<DungeonEditor>();
        window.titleContent = new GUIContent("Dungeon Level Graph");
        window.Show();
    }

    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceID, int line){
        LevelGraph graph = EditorUtility.InstanceIDToObject(instanceID) as LevelGraph;

        if(graph != null){
            ShowWindow();
            _currentGraph = graph;
            
            if(_currentGraph != null){
                InitializeEditor();
                InitializeGraph(_currentGraph);
            }
            return true;
        }
        
        return false;
    }

    private void OnEnable() {
        Selection.selectionChanged += GraphSelectedChanged;
    }

    private void OnDisable() {
        Selection.selectionChanged -= GraphSelectedChanged;

        DeselectAll();
        CleanUpConnection();
    }

    private void GraphSelectedChanged(){
        LevelGraph graph = Selection.activeObject as LevelGraph;

        if(graph != null){
            _currentGraph = graph;
            GUI.changed = true;
        }
    }

    private void OnGUI() {
        if (GUILayout.Button("Swap State", GUILayout.Width(150))) {
            _isEditable = !_isEditable;
        }

        GUILayout.Label("Current State: " + (_isEditable ? "EDIT" : "INSPECT"));

        DrawGrid(EditorManager.GridSmall, 0.2f, Color.gray);
        DrawGrid(EditorManager.GridLarge, 0.3f, Color.gray);

        ProcessEvents(Event.current);

        if(_currentGraph == null){
            ClearWindow();
        } else {
            DrawConnections();
            DrawRooms();
        }

        if(GUI.changed){
            Repaint();
        }
    }

    public static void InitializeEditor(){
        State = EditorState.IDLE;
        _graphOffset = Vector2.zero;
        _graphDrag = Vector2.zero;
        _snapToGrid = true;
        _isSomethingInFocus = false;
        _zoom = 1.5f;

        _roomNodes = new List<RoomNode>();
        _connectionNodes = new List<ConnectionNode>();
    }

    private void ClearWindow(){
        _currentGraph = null;
        _currentRoom = null;
        _roomNodes = new List<RoomNode>();
        _connectionNodes = new List<ConnectionNode>();
    }

    // graph-node related things
    public static void InitializeGraph(LevelGraph graph){
        _currentGraph = graph;

        foreach(var room in _currentGraph.Rooms){
            if(room != null){
                CreateRoomNode(room);
            }
        }

        foreach(var connection in _currentGraph.Connections){
            if(connection != null){
                CreateConnectionNode(connection);
            }
        }
    }

    private static RoomNode CreateRoomNode(Room room){
        RoomNode node = new RoomNode(room);
        _roomNodes.Add(node);
        return node;
    }

    private static ConnectionNode CreateConnectionNode(Connection connection){
        RoomNode from = _roomNodes.Single(node => node.Room == connection.From);
        RoomNode to = _roomNodes.Single(node => node.Room == connection.To);
        ConnectionNode node = new ConnectionNode(connection, from, to);
        _connectionNodes.Add(node);
        return node;
    }

    private RoomNode GetRoomNode(Vector2 pos){
        foreach(RoomNode node in _roomNodes){
            if(node.GetRect(_graphOffset, _zoom).Contains(pos)){
                return node;
            }
        }

        return null;
    }

    private ConnectionNode GetConnectionNode(Vector2 pos){
        foreach(ConnectionNode node in _connectionNodes){
            if(node.GetLineRect(_graphOffset, _zoom).Contains(pos)){
                return node;
            }
        }

        return null;
    }

    private void CreateRoom(Vector2 pos){
        Room room = ScriptableObject.CreateInstance<Room>();
        
        if(_currentGraph.Rooms.Count == 0){
            room.Initialize(RoomType.SPAWN);
        } else {
            room.Initialize();
        }

        _currentGraph.Rooms.Add(room);
        AssetDatabase.AddObjectToAsset(room, _currentGraph);
        AssetDatabase.SaveAssets();

        RoomNode node = CreateRoomNode(room);
        var normPos = WorldToGridPosition(pos) - node.GetRect(Vector2.zero, 1).center;
    
        if(_snapToGrid){
            normPos = SnapToGridPosition(normPos);
        }

        room.Position = normPos;
        SelectRoom(node);
        EditorUtility.SetDirty(_currentGraph);
    }

    private void CreateConnection(){
        if(_connectionNodes.Any(x => (x.FromNode == _fromNode && x.ToNode == _toNode) || (x.FromNode == _toNode && x.ToNode == _fromNode))){
            return;
        }

        Connection conn = ScriptableObject.CreateInstance<Connection>();
        conn.Initialize(_fromNode.Room, _toNode.Room);

        _currentGraph.Connections.Add(conn);
        AssetDatabase.AddObjectToAsset(conn, _currentGraph);
        AssetDatabase.SaveAssets();
        
        ConnectionNode node = CreateConnectionNode(conn);

        EditorUtility.SetDirty(_currentGraph);
    }

    private void DeleteRoom(RoomNode node){
        if(!IsRoomEditable(node)){
            return;
        }

        _currentGraph.Rooms.Remove(node.Room);
        _roomNodes.Remove(node);
        DestroyImmediate(node.Room, true);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(_currentGraph);
    }

    private void DeleteConnection(ConnectionNode node){
        _currentGraph.Connections.Remove(node.Connection);
        _connectionNodes.Remove(node);
        DestroyImmediate(node.Connection, true);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(_currentGraph);
    }

    private void SetRoomFocus(RoomNode node){
        if(!IsRoomEditable(node)){
            return;
        }

        node.Room.IsFocused = true;
        _isSomethingInFocus = true;
    }

    private void SetConnectionStart(RoomNode node){
        _fromNode = node;
    }

    private void SetConnectionEnd(RoomNode node){
        _toNode = node;
    }

    private bool IsRoomEditable(RoomNode node){
        if(node.Room.Type == RoomType.SPAWN){
            return false;
        }

        return !_connectionNodes.Any(x => (x.FromNode == node || x.ToNode == node));
    }

    // draw related things
    private void DrawGrid(float gridSize, float gridOpacity, Color gridColor){
        gridSize = gridSize * _zoom;

        int vertical = Mathf.CeilToInt((position.width + gridSize) / gridSize);
        int horizontal = Mathf.CeilToInt((position.height + gridSize) / gridSize);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
        var gridOffset = new Vector3((_zoom * _graphOffset.x) % gridSize, (_zoom * _graphOffset.y) % gridSize, 0);
    
        for(int i = 0; i<vertical; i++){
            Handles.DrawLine(new Vector3(gridSize * i, -gridSize, 0) + gridOffset,
            new Vector3(gridSize * i, position.height + gridSize, 0f) + gridOffset);
        }

        for(int i = 0; i<horizontal; i++){
            Handles.DrawLine(new Vector3(-gridSize, gridSize * i, 0) + gridOffset,
            new Vector3(position.width + gridSize, gridSize * i, 0f) + gridOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawRooms(){
        bool isPartOfConnection;
        foreach(RoomNode node in _roomNodes){
            isPartOfConnection = node == _fromNode || node == _toNode;
            node.Draw(_graphOffset, _zoom, isPartOfConnection);
        }
    }

    private void DrawConnections(){
        foreach(ConnectionNode node in _connectionNodes){
            node.Draw(_graphOffset, _zoom);
        }
    }
    
    private Vector2 WorldToGridPosition(Vector2 pos){
        return (pos - _graphOffset * _zoom) / _zoom;
    }

    private int RoundToNearestInt(float num, int target){
        return (int) Math.Round(num / (double) target) * target;
    }

    private Vector2 RoundToNearestVec(Vector2 vec, int target){
        return new Vector2(RoundToNearestInt(vec.x, target), RoundToNearestInt(vec.y, target));
    }

    private Vector2 SnapToGridPosition(Vector2 pos){
        return RoundToNearestVec(pos, (int)EditorManager.GridSmall);
    }

    // controls related things
    private void ProcessEvents(Event e){
        Vector2 mousePos = Vector2.zero;
        RoomNode hoverRoom;
        ConnectionNode hoverConnection;

        switch(e.type){
            case EventType.ScrollWheel:
                HandleZoom(e);
                GUI.changed = true;
                break;

            case EventType.MouseDown:
                _isDouble = (e.clickCount == 2);
                mousePos = e.mousePosition;

                if(State == EditorState.IDLE){
                    hoverRoom = GetRoomNode(e.mousePosition);
                    hoverConnection = GetConnectionNode(e.mousePosition);

                    if(e.button == 1 && hoverRoom == null && hoverConnection == null){
                        State = EditorState.DRAG_GRID;
                    } else if(e.button == 0 && hoverRoom != null){
                        State = EditorState.SELECT_ROOM;
                    }
                }
                break;

            case EventType.MouseUp:
                hoverRoom = GetRoomNode(e.mousePosition);
                hoverConnection = GetConnectionNode(e.mousePosition);
                float clickDistance = Vector2.Distance(mousePos, e.mousePosition);

                //if left click
                if(e.button == 0){
                    
                    //single click
                    if(!_isDouble){

                        //on room => select room
                        if(hoverRoom != null){
                            SelectRoom(hoverRoom);
                            GUI.changed = true;
                        }

                        //on grid => deselect all
                        else if(hoverRoom == null && hoverConnection == null){
                            DeselectAll();
                            GUI.changed = true;
                        }
                    }

                    //double click
                    else{

                        //on room => select from/to
                        if(hoverRoom != null && _isEditable){
                            if(_fromNode == null){
                                SetConnectionStart(hoverRoom);
                                GUI.changed = true;
                            } else if(_toNode == null){
                                SetConnectionEnd(hoverRoom);
                                GUI.changed = true;
                            }
                        }

                        //on connection => select connection
                        else if(hoverConnection != null){
                            SelectConnection(hoverConnection);
                            GUI.changed = true;
                        }

                        //on grid => create new room
                        else if(_isEditable){
                            CreateRoom(e.mousePosition);
                            GUI.changed = true;
                        }
                    }
                }

                //if right click
                else if(e.button == 1){

                    //single click
                    if(!_isDouble){
                        
                        //on room => room options
                        if(hoverRoom != null && hoverRoom == _currentRoom && _isEditable){
                            ShowRoomConfigMenu(hoverRoom);
                            GUI.changed = true;
                        }

                        //on connection => connection options
                        else if(hoverConnection != null && _isEditable){
                            ShowConnectionConfigMenu(hoverConnection);
                            GUI.changed = true;
                        }
                    }

                    //double click
                    else {
                        //on room => configure room type
                        if(hoverRoom != null && _isEditable){
                            SelectRoom(hoverRoom);
                            SetRoomFocus(hoverRoom);
                            GUI.changed = true;
                        }
                    }
                }

                State = EditorState.IDLE;
                break;

            case EventType.MouseDrag:
                hoverRoom = GetRoomNode(e.mousePosition);

                //grid drag
                if(State == EditorState.DRAG_GRID){
                    HandleGridDrag(e);
                    GUI.changed = true;
                }

                //room drag
                else if((State == EditorState.SELECT_ROOM && _currentRoom != null && hoverRoom != null && hoverRoom.Room.IsSelected) || State == EditorState.DRAG_ROOM){
                    if(State == EditorState.SELECT_ROOM){
                        _graphDrag = Vector2.zero;
                        _ogPos = _currentRoom.Room.Position;
                    }

                    State = EditorState.DRAG_ROOM;
                    _graphDrag += e.delta;
                    HandleRoomDrag(e);
                    GUI.changed = true;
                }
                break;

            case EventType.KeyDown:
                //connect rooms
                if(e.keyCode == KeyCode.F) {
                    ConnectRooms();
                    GUI.changed = true;
                }

                //cancel room connection setup
                else if(e.keyCode == KeyCode.C) {
                    CleanUpConnection();
                    GUI.changed = true;
                }
                
                break;

            default:
                break;
        }
    }

    private void HandleZoom(Event e){
        if(_isSomethingInFocus){
            return;
        }

        var prevZoom = _zoom;

        if(e.delta.y > 0){
            _zoom -= _zoom * 0.1f;
        } else{
            _zoom += _zoom * 0.1f;
        }

        if(_isEditable){
            _zoom = Math.Max(1.4f, _zoom);
            _zoom = Math.Min(3f, _zoom);
        } else {
            _zoom = Math.Max(0.1f, _zoom);
            _zoom = Math.Min(3f, _zoom);
        }
        var center = e.mousePosition;
        _graphOffset += -(_zoom * (center - _graphOffset * prevZoom) - center * prevZoom) / (_zoom * prevZoom) - _graphOffset;
    }

    private void SelectRoom(RoomNode node){
        DeselectAll();
        Selection.activeObject = node.Room;
        _currentRoom = node;
        _currentRoom.Room.IsSelected = true;
    }

    private void SelectConnection(ConnectionNode node){
        DeselectAll();
        Selection.activeObject = node.Connection;
        _currentConnection = node;
        _currentConnection.Connection.IsSelected = true;
    }

    private void DeselectAll(){
        _isSomethingInFocus = false;

        if(_currentRoom != null){
            _currentRoom.Room.IsSelected = false;
            _currentRoom.Room.IsFocused = false;
            _currentRoom = null;
        }

        if(_currentConnection != null){
            _currentConnection.Connection.IsSelected = false;
            _currentConnection = null;
        }
    }

    private void HandleGridDrag(Event e){
        _graphOffset += e.delta / _zoom;
    }

    private void HandleRoomDrag(Event e){
        Vector2 dragOffset = _graphDrag / _zoom;
        Vector2 newPos = _ogPos + dragOffset;
        
        if(_snapToGrid){
            newPos = SnapToGridPosition(newPos);
        }

        _currentRoom.Room.Position = newPos;
        EditorUtility.SetDirty(_currentGraph);
    }

    private void ShowRoomConfigMenu(RoomNode node){
        var menu = new GenericMenu();

        if(IsRoomEditable(node)){
            menu.AddItem(new GUIContent("Configure room"), false, () => SetRoomFocus(node));
            menu.AddItem(new GUIContent("Delete room"), false, () => DeleteRoom(node));
        }
        
        if(_fromNode == null && node.Room.Type != RoomType.NONE){
            menu.AddItem(new GUIContent("Connection start"), false, () => SetConnectionStart(node));
        } else if(_toNode == null && node != _fromNode && node.Room.Type != RoomType.NONE){
            menu.AddItem(new GUIContent("Connection end"), false, () => SetConnectionEnd(node));
        }

        menu.ShowAsContext();
    }

    private void ShowConnectionConfigMenu(ConnectionNode node){
        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("Delete connection"), false, () => DeleteConnection(node));
        menu.ShowAsContext();
    }

    private void ConnectRooms(){
        if(_fromNode != null && _toNode != null && _fromNode != _toNode && _fromNode.Room.Type != RoomType.NONE && _toNode.Room.Type != RoomType.NONE){
            CreateConnection();
        }

        CleanUpConnection();
    }

    private void CleanUpConnection(){
        _fromNode = null;
        _toNode = null;
    }
}