#if UNITY_EDITOR

using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class SceneBaker : IProcessSceneWithReport, IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        //Debug.Log("building.");
    }

    // Note that if the scene or related content in the project is unchanged from the previous player build,
    // the scene will not be built and instead cached player build data will be used.
    // In this case the callback will not be called.
    public void OnProcessScene(UnityEngine.SceneManagement.Scene scene, BuildReport report)
    {
        //Debug.Log("yay");
    }
}

#endif