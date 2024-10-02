using UnityEngine;
using UnityEngine.SceneManagement;

// test: changing scenes after scene rename
// test: changing scenes after scene move dir
// test: restarting unity editor
// test: changing scene build indices
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