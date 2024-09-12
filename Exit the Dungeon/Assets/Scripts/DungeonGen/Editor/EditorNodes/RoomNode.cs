using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomNode {
    public Room Room { get; private set; }
    private GUIStyle _roomNodeStyle;
    private GUIStyle _selectedNodeStyle;
    private GUIStyle _focusedNodeStyle;

    public RoomNode(Room _room){
        Room = _room;
    }

    public virtual Rect GetRect(Vector2 offset, float zoom){
        float width = EditorManager.NodeWidth * zoom;
        float height = EditorManager.NodeHeight * zoom;
        return new Rect((Room.Position.x + offset.x) * zoom, (Room.Position.y + offset.y) * zoom, width, height);
    }

    #if UNITY_EDITOR
        public virtual void Draw(Vector2 offset, float zoom, bool isPartOfConnection){
            Rect rectOfNode = GetRect(offset, zoom);
            EditorStyles.OptimizeStyle(zoom);

            if(isPartOfConnection){
                _selectedNodeStyle = new GUIStyle(EditorStyles.SelectedNodeStyle);
                _selectedNodeStyle.normal.textColor = Color.green;
                _selectedNodeStyle.fontSize = (int) (_selectedNodeStyle.fontSize * zoom);
                GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f, 0.9f);
                GUI.Box(rectOfNode, Room.GetDisplayName(), _selectedNodeStyle);

            } else if(Room.IsFocused){
                _focusedNodeStyle = new GUIStyle(EditorStyles.FocusedNodeStyle);
                _focusedNodeStyle.fontSize = (int) (_focusedNodeStyle.fontSize * zoom);
                GUILayout.BeginArea(rectOfNode, _focusedNodeStyle);
                EditorGUI.BeginChangeCheck();

                RoomType[] allRoomTypes = (RoomType[])System.Enum.GetValues(typeof(RoomType));
                List<string> selectableTypes = new List<string>();
                List<int> selectableInd = new List<int>();

                if(Room.Type == RoomType.NONE){
                    for (int i = 0; i < allRoomTypes.Length; i++) {
                        if (allRoomTypes[i] != RoomType.SPAWN) {
                            selectableTypes.Add(allRoomTypes[i].ToString());
                            selectableInd.Add(i);
                        }
                    }
                } else {
                    for (int i = 0; i < allRoomTypes.Length; i++) {
                        if (allRoomTypes[i] != RoomType.NONE && allRoomTypes[i] != RoomType.SPAWN && allRoomTypes[i] != RoomType.CORRIDOR) {
                            selectableTypes.Add(allRoomTypes[i].ToString());
                            selectableInd.Add(i);
                        }
                    }
                }

                int selected = selectableInd.IndexOf((int)Room.Type);
                int selection = EditorGUILayout.Popup("", selected, selectableTypes.ToArray());
                Room.SetRoomType(allRoomTypes[selectableInd[selection]]);
                

                if(EditorGUI.EndChangeCheck()){
                    EditorUtility.SetDirty(Room);
                }

                GUILayout.EndArea();

            } else if(Room.IsSelected){
                _selectedNodeStyle = new GUIStyle(EditorStyles.SelectedNodeStyle);
                _selectedNodeStyle.fontSize = (int) (_selectedNodeStyle.fontSize * zoom);
                GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f, 0.9f);
                GUI.Box(rectOfNode, Room.GetDisplayName(), _selectedNodeStyle);

            } else {
                _roomNodeStyle = new GUIStyle(EditorStyles.RoomNodeStyle);
                _roomNodeStyle.fontSize = (int) (_roomNodeStyle.fontSize * zoom);
                GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f, 0.9f);
                GUI.Box(rectOfNode, Room.GetDisplayName(), _roomNodeStyle);
            }
        }
    #endif
}
