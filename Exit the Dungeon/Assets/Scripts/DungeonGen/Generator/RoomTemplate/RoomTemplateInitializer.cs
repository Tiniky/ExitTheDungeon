using System.IO;
using UnityEditor;
using UnityEngine;

public static class RoomTemplateInitializer {
    public static void CreateRoomTemplatePrefab(){
        #if UNITY_EDITOR
        GameObject roomTemplate = new GameObject();
        RoomInitializer initializer = roomTemplate.AddComponent<RoomInitializer>();
        initializer.Initialize();
        Object.DestroyImmediate(initializer);
        var currentPath = GetCurrentPath();
        PrefabUtility.SaveAsPrefabAsset(roomTemplate, AssetDatabase.GenerateUniqueAssetPath(currentPath + "/Room_.prefab"));
        Object.DestroyImmediate(roomTemplate);
        #endif
    }

        #if UNITY_EDITOR
        private static string GetCurrentPath(){
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if(path == ""){
                path = "Assets";
            } else if(Path.GetExtension(path) != ""){
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            return path;
        }
        #endif
    }