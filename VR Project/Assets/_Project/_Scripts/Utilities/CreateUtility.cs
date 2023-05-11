using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CreateUtility 
{
    [MenuItem("GameObject/Custom/Teleport Node")]
    public static void CreateNode(MenuCommand command)
    {
        CreatePrefab("Teleport Node", true);
    }

    private static void CreatePrefab(string path, bool placeAtOrigin = false)
    {
        GameObject newObject = PrefabUtility.InstantiatePrefab(Resources.Load(path)) as GameObject;
        newObject.Place(placeAtOrigin);
    }
    
    private static void Place(this GameObject gameObject, bool placeAtOrigin = false)
    {
        SceneView lastView = SceneView.lastActiveSceneView;
        gameObject.transform.position = !lastView || placeAtOrigin ? Vector3.zero : lastView.pivot;
        
        StageUtility.PlaceGameObjectInCurrentStage(gameObject);
        GameObjectUtility.EnsureUniqueNameForSibling(gameObject);
        
        Undo.RegisterCreatedObjectUndo(gameObject, $"Create Object: {gameObject.name}");
        Selection.activeGameObject = gameObject;

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
}
