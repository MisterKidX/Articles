using UnityEngine;
using UnityEngine.SceneManagement;

// test: scene loading in runtime after scene rename
// test: scene loading in runtime after scene move dir
// test: scene loading in runtime after changing scene build indices
// test: no scenes in build index, what errors are thrown?
// test: delete referenced scene - try to load at runtime and see inspector
// test: restarting unity editor
// test: scene reimports
// test: changes to scene guids - in this case should also save the scene path and try to run
// it the other way around `AssetDatabase.AssetPathToGUID(assetPath)`
// test: use it in an array
// test: use it in build
// test: make sure that empty scenes (null values) throw the right errors.
// try and load empty scenes (no guid) with name and index and see errors

// test [fail]: change scene name, directory and build indices then build
// try and use https://docs.unity3d.com/ScriptReference/AssetDatabase.RegisterCustomDependency.html
// try to create a scene processor and regenerate all editor scripts https://docs.unity3d.com/ScriptReference/EditorApplication.RepaintHierarchyWindow.html

// TODO: clean code
// TODO: move helper functions to single file

public class TestScenePicker : MonoBehaviour
{
    [ScenePicker]
    public string sceneFromString;
    [ScenePicker]
    public int sceneFromInt;

    public Scene SceneFromObject1;

    private void OnGUI()
    {
        if(GUILayout.Button("load string scene"))
        {
            Debug.Log("Loading: " + sceneFromString);
            SceneManager.LoadScene(sceneFromString);
        }

        if (GUILayout.Button("load int scene"))
        {
            Debug.Log("Loading: " + sceneFromInt);
            SceneManager.LoadScene(sceneFromInt);
        }

        if (GUILayout.Button("log and load scene from object"))
        {
            Debug.Log(SceneFromObject1);
            SceneManager.LoadScene(SceneFromObject1.Name);
        }
    }
}