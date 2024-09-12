using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class LogManager {
    private static GameObject _logPrefab, _logTextPrefab, _logHolder;
    private static Transform _contentPanel;
    private static List<string> _messageList;
    private static int _msgLimit;
    private static List<Message> _visibleList;
    private static ScrollRect _scrollRect;

    public static void Initialize(Canvas canvas){
        _logPrefab = PrefabManager.LOG_UI;
        _logTextPrefab = PrefabManager.LOG_TEXT;
        _logHolder = PrefabManager.InstantiatePrefabV2(_logPrefab, canvas.gameObject, false, new Vector3(-666f, -350f, 0f), "Log");
        _contentPanel = _logHolder.transform.Find("Viewport/Content");
        _scrollRect = _logHolder.GetComponent<ScrollRect>();
        
        _msgLimit = 20;
        _messageList = new List<string>();
        _visibleList = new List<Message>();
    }

    public static void AddMessage(string text){
        GameObject textObj = PrefabManager.InstantiatePrefabV3(_logTextPrefab, _contentPanel);
        Message newMsg = new Message(textObj.GetComponent<Text>());
        newMsg.msg.text = text;

        _messageList.Add(text);
        _visibleList.Add(newMsg);
        if(_visibleList.Count > _msgLimit){
            GameManager.DestroyObj(_visibleList[0].msg.gameObject);
            _visibleList.Remove(_visibleList[0]);
        }

        ScrollToBottom();
    }

    private static void ScrollToBottom(){
        Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 0f;
    }

    public static void SetVisibility(bool shouldBeVisible){
        _logHolder.SetActive(shouldBeVisible);
    }
}

public class Message{
    public Text msg;

    public Message(Text txt) {msg = txt;}
}