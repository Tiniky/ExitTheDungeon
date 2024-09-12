using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConnectionNode {
    public Connection Connection { get; private set; }
    public RoomNode FromNode { get; private set; }
    public RoomNode ToNode { get; private set; }

    public ConnectionNode(Connection _connection, RoomNode _from, RoomNode _to) {
        Connection = _connection;
        FromNode = _from;
        ToNode = _to;
    }

    public virtual Rect GetLineRect(Vector2 offset, float zoom){
        var width = EditorManager.LineWidth * zoom;
        var lineCenter = Vector2.Lerp(FromNode.GetRect(offset, zoom).center, ToNode.GetRect(offset, zoom).center, 0.5f);
        return new Rect(lineCenter.x - width / 2.0f, lineCenter.y - width / 2.0f, width, width);
    }

    #if UNITY_EDITOR
        public virtual void Draw(Vector2 offset, float zoom){
            Vector2 fromVec = FromNode.GetRect(offset, zoom).center;
            Vector2 toVec = ToNode.GetRect(offset, zoom).center;

            Handles.color = Color.white;
            Handles.DrawLine(fromVec, toVec);

            if(Connection.IsSelected){
                Handles.color = Color.red;
            } else {
                Handles.color = new Color(0.75f, 0.75f, 0.75f, 0.9f);
            }

            Vector2 center = (fromVec + toVec)/2;
            Vector2 dir = toVec - fromVec;
            dir.Normalize();
            dir *= 5 * zoom;
            Vector2 perp = Vector2.Perpendicular(dir);

            List<Vector3> points = new List<Vector3>{
                center - dir - perp,
                center + dir - perp,
                center + dir + perp,
                center - dir + perp
            };
            Handles.DrawAAConvexPolygon(points.ToArray());
        }
    #endif
}