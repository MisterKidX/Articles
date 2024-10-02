using UnityEditor;
using UnityEngine;

[System.Serializable]
public record Scene
{
    [SerializeField]
    [HideInInspector]
    private string _guid;

    // serialize fields to save values between runs

    [SerializeField]
    private string _path;

    public string Path
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
    public int Index => GetBuildIndex(_guid);

    public override string ToString()
    {
        return new { Name, Path, Index }.ToString();
    }

    private int GetBuildIndex(string guid)
    {
        var scenes = EditorBuildSettings.scenes;

        for (int i = 0; i < scenes.Length; i++)
            if (scenes[i].guid.ToString() == guid)
                return i;

        return -1;
    }

    private string GetSceneName(string path)
    {
        var startIndex = path.LastIndexOf('/');
        var endIndex = path.LastIndexOf('.');
        return path.Substring(startIndex + 1, endIndex - startIndex - 1);
    }
}