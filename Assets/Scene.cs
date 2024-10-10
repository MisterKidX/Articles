#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
// serialize fields to save values between runs
public class Scene
#if UNITY_EDITOR
: IPreprocessBuildWithReport, IProcessSceneWithReport
#endif
{
    // GUID and Path are both editor features, that's why they are private.
    [SerializeField]
    [HideInInspector]
    private string _guid;

    [SerializeField]
    [HideInInspector]
    private string _path;

    private string Path
    {
        get
        {
#if UNITY_EDITOR
            _path = AssetDatabase.GUIDToAssetPath(_guid);
#endif
            return _path;
        }
    }

    public string Name => GetSceneName(Path);
    public int Index => GetBuildIndex(Path);

    public override string ToString()
    {
        return new { Name, Path, Index }.ToString();
    }

    private int GetBuildIndex(string path)
    {
        if (string.IsNullOrEmpty(path))
            return -1;

        return SceneUtility.GetBuildIndexByScenePath(path);
    }

    private string GetSceneName(string path)
    {
        if (string.IsNullOrEmpty(path))
            return null;

        var startIndex = path.LastIndexOf('/');
        var endIndex = path.LastIndexOf('.');
        return path.Substring(startIndex + 1, endIndex - startIndex - 1);
    }

#if UNITY_EDITOR
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        //Debug.Log(_guid + ": " + ToString());
        //_path = AssetDatabase.GUIDToAssetPath(_guid);
    }

    public void OnProcessScene(UnityEngine.SceneManagement.Scene scene, BuildReport report)
    {
        foreach (var go in scene.GetRootGameObjects())
        {
            // Debug.Log("S: " + go.GetComponentInChildren<TestScenePicker>()?.SceneFromObject1);
        }
        //Debug.Log(_guid + ": " + ToString());
        //_path = AssetDatabase.GUIDToAssetPath(_guid);
    }
#endif
}