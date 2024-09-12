using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;

public class EditorStyles {
    private static readonly EditorStyles _current = new EditorStyles();

    public static GUIStyle RoomNodeStyle => _current.roomNodeStyle;
    public static GUIStyle SelectedNodeStyle => _current.selectedNodeStyle;
    public static GUIStyle FocusedNodeStyle => _current.focusedNodeStyle;
    private GUIStyle roomNodeStyle, selectedNodeStyle, focusedNodeStyle;

    public EditorStyles(){
        Initialze();
    }

    private void Initialze(){
        int padding = EditorManager.NodePadding;
        int border = EditorManager.NodeBorder;

        roomNodeStyle = new GUIStyle();
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeStyle.normal.textColor = Color.white;
        roomNodeStyle.fontSize = 12;
        roomNodeStyle.alignment = TextAnchor.MiddleCenter;
        roomNodeStyle.padding = new RectOffset(padding, padding, padding, padding);
        roomNodeStyle.border = new RectOffset(border, border, border, border);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
        selectedNodeStyle.normal.textColor = Color.red;
        selectedNodeStyle.fontSize = 12;
        selectedNodeStyle.alignment = TextAnchor.MiddleCenter;
        selectedNodeStyle.padding = new RectOffset(padding, padding, padding, padding);
        selectedNodeStyle.border = new RectOffset(border, border, border, border);

        focusedNodeStyle = new GUIStyle();
        focusedNodeStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
        focusedNodeStyle.normal.textColor = Color.white;
        focusedNodeStyle.padding = new RectOffset(padding, padding, padding, padding);
        focusedNodeStyle.border = new RectOffset(border, border, border, border);
    }

    public static void OptimizeStyle(float zoom) {
        _current.OptimizeStyleInstance(zoom);
    }

    private void OptimizeStyleInstance(float zoom) {
        int padding = (int)(EditorManager.NodePadding * zoom);
        int border = (int)(EditorManager.NodeBorder * zoom);
        roomNodeStyle.padding = new RectOffset(padding, padding, padding, padding);
        roomNodeStyle.border = new RectOffset(border, border, border, border);
    }
}